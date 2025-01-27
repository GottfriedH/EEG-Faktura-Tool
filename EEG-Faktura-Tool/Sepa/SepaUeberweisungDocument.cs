using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace EEG_Faktura_Tool.Sepa.Ueberweisung
{
    internal class SepaUeberweisungDocument
    {
        DateTime _ueberweisungDatum;
        SepaConfiguration _sepaConfiguration = new SepaConfiguration();
        Document _document = new Document();
        List<Bestand> _customerCreditTransferInitiationBestand = new List<Bestand>();

        /// <summary>
        /// Entspricht dem SEPA-Element <PmitINf>
        /// </summary>
        internal class Bestand
        {
            /// <summary>
            /// Anzahl der Transaktionen
            /// </summary>
            internal int NbOfTxs;
            /// <summary>
            /// Summe der Einzeltransaktionen
            /// </summary>
            internal decimal CtrlSum;
            decimal BestandKontrollnummer;

            internal DateTime Ueberweisungsdatum;
            internal List<CreditTransferTransactionInformation10> CreditTransferTransaction = new List<CreditTransferTransactionInformation10>();

            public Bestand(DateTime ueberweisungsdatum)
            {
                Ueberweisungsdatum = ueberweisungsdatum;
            }
        }

        public SepaUeberweisungDocument(DateTime ueberweisungDatum)
        {
            _ueberweisungDatum = ueberweisungDatum;
            _document.CstmrCdtTrfInitn = new AT_CustomerCreditTransferInitiationV03();

            _document.CstmrCdtTrfInitn.GrpHdr = GrpHdr();
        }
        public void AddSepaUeberweisung(string verwendungszweck, string endToEndId, decimal betrag
                                       ,string ibanEmpfaenger, string nameEmpfaenger, DateTime ueberweisungdatum)
        {
            // Einzelne Transaktion (Einzelumsätze)
            var directDebitTransactionInformation = this.DirectDebitTransactionInformation(nameEmpfaenger, ibanEmpfaenger, betrag, verwendungszweck, endToEndId);

            Bestand bestand;
            bestand = _customerCreditTransferInitiationBestand.FirstOrDefault(_ => _.Ueberweisungsdatum == ueberweisungdatum);

            if (bestand == null)
            {
                bestand = new Bestand(ueberweisungdatum);
                _customerCreditTransferInitiationBestand.Add(bestand);
            }
            bestand.CreditTransferTransaction.Add(directDebitTransactionInformation);
            // Bestandsdaten aktualisieren
            bestand.NbOfTxs++;
            bestand.CtrlSum += betrag;
        }
        private CreditTransferTransactionInformation10 DirectDebitTransactionInformation(string nameEmpfaenger, string ibanEmpfaenger, decimal betrag, string verwendungszweck, string endToEndId)
        {
            var creditTransferTransaction = new CreditTransferTransactionInformation10
            {
                // EndToEndId
                // Auftraggeberreferenz. Kann im Kontoauszug zur eigenen Referenzierung zurückgegeben werden, daher ist Eindeutigkeit wichtig. Falls dennoch keine Referenz angegeben werden soll, mit dem Wert NOTPROVIDED zu füllen.
                PmtId = new PaymentIdentification1 { EndToEndId = Mapping.EndToEndIdToPaymentIdentification1(endToEndId) },

                Amt = new AmountType3Choice 
                {
                    //PmtTpInf wird NICHT gesetzt. Einzelverarbeitung. ... Von der Verwendung dieses Elements wird dringend abgeraten
                    // Einzelbetrag. Punkt als Dezimalzeichen. Keine negativen Werte
                    Item = new ActiveOrHistoricCurrencyAndAmount   // <InstdAmt>
                    {
                        Value = betrag,
                        Ccy = "EUR"
                    },
                },
                Cdtr = EmpfaengerPartyIdentification32(nameEmpfaenger),
                CdtrAcct = new CashAccount16
                {
                    Id = new AccountIdentification4Choice
                    {
                        // Kontonummer des Kontoinhabers / Bezogenen. IBAN eines Kontos im SEPA-Raum
                        Item = ibanEmpfaenger.ToString().TrimEnd().Replace(" ", "")
                    }
                },
                // Obwohl hier ein Array als Verwendungszweck übergeben werden kann darf nur genau 1 Element enthalten sein!
                // Verwendungszweck / Bezogenenreferenz;
                RmtInf = new RemittanceInformation5 { Ustrd = new string[] { verwendungszweck } }
            };

            return creditTransferTransaction;
        }

        private PartyIdentification32 EmpfaengerPartyIdentification32(string nameEmpfaenger)
        {
            var partyIdentification135 = new PartyIdentification32
            {
                Nm = nameEmpfaenger
            };

            return partyIdentification135;
        }

        public void Export(string filename)
        {
            var xmlNs = new XmlSerializerNamespaces();
            //xmlNs.Add("", "urn:ISO:pain.001.001.03:APC:STUZZA:payments:003");

            List<PaymentInstructionInformation3> customerCreditTransferInitiation = new List<PaymentInstructionInformation3>();

            int totalNbOfTxs = 0;
            decimal totalCtrlSum = decimal.Zero;

            // Bestände einfügen
            foreach (var bestand in _customerCreditTransferInitiationBestand)
            {
                if (bestand.CreditTransferTransaction.Count > 0)
                {
                    PaymentInstructionInformation3 paymentInstruction = paymentInstructionInformation(bestand.Ueberweisungsdatum);

                    // Bestand vervollständigen
                    // Bestandskontrollnummer. Eindeutigkeit über mindestens 1 Jahr ist herzustellen. (Max 35 Zeichen)
                    paymentInstruction.PmtInfId = "PIDCORE" + DateTime.Now.Ticks.ToString();

                    // Summen in Bestand
                    // Einzelumsätze.
                    paymentInstruction.CdtTrfTxInf = bestand.CreditTransferTransaction.ToArray();
                    // Anzahl der Einzeltransaktionen des Bestands.
                    paymentInstruction.NbOfTxs = bestand.NbOfTxs.ToString();
                    // Summe der Einzeltransaktionen des Bestands.
                    paymentInstruction.CtrlSum = bestand.CtrlSum;
                    paymentInstruction.CtrlSumSpecified = true;

                    totalNbOfTxs += bestand.NbOfTxs;
                    totalCtrlSum += bestand.CtrlSum;

                    customerCreditTransferInitiation.Add(paymentInstruction);
                }
            }

            // Bestände in Lastschriftnachricht eintragen
            _document.CstmrCdtTrfInitn.PmtInf = customerCreditTransferInitiation.ToArray();

            // Zähler und Summen eintragen
            _document.CstmrCdtTrfInitn.GrpHdr.NbOfTxs = totalNbOfTxs.ToString().Trim();
            _document.CstmrCdtTrfInitn.GrpHdr.CtrlSum = totalCtrlSum;

            try
            {
                using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    using (var xmlWriter = new CustomDateTimeWriter(fileStream, Encoding.UTF8))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        var serializer = new XmlSerializer(typeof(Document));
                        serializer.Serialize(xmlWriter, _document, xmlNs);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Beim Speichern der Sepa-Datei \"{filename}\" ist ein Fehler aufgetreten!\r\n{ex.Message}");
            }
        }

        private PaymentInstructionInformation3 paymentInstructionInformation(DateTime ueberweisungsdatum)
        {
            var paymentInstruction30 = new PaymentInstructionInformation3
            {
                DbtrAgt = new BranchAndFinancialInstitutionIdentification4 { FinInstnId = new FinancialInstitutionIdentification7() }, // Kontoführendes Institut des Kontoinhabers / Auftraggebers => Identifikation einer Bank im SEPA-Raum (BIC)
                PmtInfId = "PIDCORE" + DateTime.Now.Ticks.ToString(),
                PmtMtd = PaymentMethod3Code.TRF
            };

            // PaymentMethod2Code (Zahlungsmethode); Einzig verfügbarer Wert ist DD.
            paymentInstruction30.PmtMtd = PaymentMethod3Code.TRF;
            // BatchBookingIndicator (Sammel- oder Einzelbuchung); "true" heißt Bestandsbuchung gewünscht.
            paymentInstruction30.BtchBookg = true;
            paymentInstruction30.BtchBookgSpecified = true;

            paymentInstruction30.PmtTpInf = GetPaymentTypeInformation26();

            // Gewünschtes Überweisungsdatum.
            paymentInstruction30.ReqdExctnDt = _ueberweisungDatum;
            // Kontoinhaber / Auftraggeber. Wir übergeben hier jetzt auch mal einfach den Sender
            paymentInstruction30.Dbtr = AuftraggeberPartyIdentification135();
            // Creditor Account - Kontoinformationen des Kontoinhabers / Auftraggebers
            paymentInstruction30.DbtrAcct = CashAccount16();

            // CdtrSchmeId wird hier NICHT angegeben sondern je Lastschrift                                                                                
            // BIC des Auftraggebers nur ausgeben, wenn es befüllt ist
            if (_sepaConfiguration.BIC != "")
            {
                paymentInstruction30.DbtrAgt.FinInstnId.BIC = _sepaConfiguration.BIC.TrimEnd();
            }

            return paymentInstruction30;
        }
        private static PaymentTypeInformation19 GetPaymentTypeInformation26()
        {
            var paymentTypeInformation19 = new PaymentTypeInformation19 ();
            var serviceLevel8Choice = new ServiceLevel8Choice { Item = "SEPA", ItemElementName = ItemChoiceType4.Cd };
            paymentTypeInformation19.SvcLvl = serviceLevel8Choice;

            return paymentTypeInformation19;
        }
        private PartyIdentification32 AuftraggeberPartyIdentification135()
        {
            PartyIdentification32 partyIdentification32 = new PartyIdentification32();

            // PartyIdentification (Eigene Daten)
            partyIdentification32.Nm = _sepaConfiguration.Name;
            //partyIdentification135_Dbtr.PstlAdr = new PostalAddress6() { Ctry = _sepaConfiguration.Country, AdrLine = _sepaConfiguration.AdressLine };

            return partyIdentification32;
        }

        private CashAccount16 CashAccount16()
        {
            // CashAccount Kontoinformationen (eigene Bankverbindung) 
            var cashAccount16 = new CashAccount16();

            cashAccount16.Id = new AccountIdentification4Choice { Item = _sepaConfiguration.IBAN }; // IBAN eines Kontos im SEPA-Raum
            // Kontowährung des erkannten Kontos.
            cashAccount16.Ccy = _sepaConfiguration.Currency;

            return cashAccount16;
        }

        private GroupHeader32 GrpHdr()
        {
            var grpHdr = new GroupHeader32();

            grpHdr.MsgId = "CRED" + DateTime.Now.Ticks.ToString();
            grpHdr.CreDtTm = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
            grpHdr.CtrlSumSpecified = true;

            // Identifizierung des Kommunikationsberechtigten
            grpHdr.InitgPty = InitgPty();

            return grpHdr;
        }
        private PartyIdentification32 InitgPty()
        {
            PartyIdentification32 initgPty = new PartyIdentification32();

            // PartyIdentification (Eigene Daten)
            initgPty.Nm = _sepaConfiguration.Name;
            //partyIdentification32.PstlAdr = new PostalAddress6() { Ctry = _sepaConfiguration.Country, AdrLine = _sepaConfiguration.AdressLine };

            return initgPty;
        }
        private PaymentInstructionInformation3[] PmtInf()
        {
            List<PaymentInstructionInformation3> paymentInstruction3 = new List<PaymentInstructionInformation3>();

            return paymentInstruction3.ToArray();
        }
        private class CustomDateTimeWriter : XmlTextWriter
        {
            public CustomDateTimeWriter(TextWriter writer) : base(writer) { }
            public CustomDateTimeWriter(Stream stream, Encoding encoding) : base(stream, encoding) { }
            public CustomDateTimeWriter(string filename, Encoding encoding) : base(filename, encoding) { }

            string elementLocalName;

            public override void WriteStartElement(string prefix, string localName, string ns)
            {
                elementLocalName = localName;
                base.WriteStartElement(prefix, localName, ns);
            }

            public override void WriteRaw(string data)
            {
                // Da das DateTime Format im "pain008_Guide; 02; Corrigendum" beim Element "CreDtTm" mit "YYYY-MM-DDThh:mm:ss" vorgegeben ist muss es hier übersteuert werden 
                if (elementLocalName == "CreDtTm")
                {
                    DateTime dt;
                    if (DateTime.TryParse(data, out dt))
                    {
                        base.WriteRaw(dt.ToString("yyyy-MM-ddThh:mm:ss"));
                        return;
                    }
                }

                base.WriteRaw(data);
                return;
            }
        }
    }
}
