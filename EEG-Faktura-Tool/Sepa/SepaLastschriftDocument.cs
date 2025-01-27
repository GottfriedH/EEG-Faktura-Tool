using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Windows.Forms;
using EEG_Faktura_Tool.Sepa.Lastschrift;

namespace EEG_Faktura_Tool.Sepa
{
    internal class SepaLastschriftDocument
    {
        DateTime _lastschriftDatum;

        SepaConfiguration _sepaConfiguration = new SepaConfiguration();

        internal class Bestand
        {
            internal int NbOfTxs;
            internal decimal CtrlSum;
            decimal BestandKontrollnummer;

            internal bool Firmenlastschrift;
            internal DateTime Einzugsdatum;
            internal SequenceType1Code LastschriftArt;
            internal List<DirectDebitTransactionInformation9> DirectDebitTransactionInformation9 = new List<DirectDebitTransactionInformation9>();

            public Bestand(bool firmenlastschrift, DateTime einzugsdatum, SequenceType1Code lastschriftArt)
            {
                Firmenlastschrift = firmenlastschrift;
                Einzugsdatum = einzugsdatum;
                LastschriftArt = lastschriftArt;
            }
        }

        Document _document;
        List<Bestand> _directDebitTransactionInformationBestand;

        public SepaLastschriftDocument(DateTime lastschriftDatum)
        {

            _lastschriftDatum = lastschriftDatum;
            _document = document();

            _directDebitTransactionInformationBestand = new List<Bestand>();
        }

        private Document document()
        {
            var document = new Document();

            document.CstmrDrctDbtInitn = customerDirectDebitInitiationV02();

            return document;
        }
        private CustomerDirectDebitInitiationV02 customerDirectDebitInitiationV02()
        {
            var customerDirectDebitInitiationV02 = new CustomerDirectDebitInitiationV02();

            customerDirectDebitInitiationV02.GrpHdr = grpHdr();

            return customerDirectDebitInitiationV02;
        }

        private GroupHeader39 grpHdr()
        {
            GroupHeader39 groupHeader39 = new GroupHeader39();

            // GroupHeader
            // Identifizierung des Kommunikationsberechtigten. Stimmen Sie Ihre Id mit dem empfangenden Finanzinstitut ab. Üblicherweise die Hauptkontonummer
            groupHeader39.InitgPty = auftraggeberPartyIdentification32();
            // Technische Referenz der Übermittlungsdatei. Eindeutigkeit über mindestens 30 Tage ist herzustellen. Beschränken Sie sich für einen sicheren Ablauf auf Buchstaben, Ziffern und Bindestrich. Eindeutigkeit erreicht man zB einfach über die Kombination des Datums mit einem Tageszähler. Max 35 Zeichen.
            groupHeader39.MsgId = "DEB" + DateTime.Now.Ticks.ToString();
            //Dateierzeugungsdatum und -zeit. Zeitpunkt in CET(CEST)-Zeit
            groupHeader39.CreDtTm = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
            groupHeader39.CtrlSumSpecified = true;

            return groupHeader39;
        }

        private PartyIdentification32 auftraggeberPartyIdentification32()
        {
            PartyIdentification32 partyIdentification32 = new PartyIdentification32();

            // PartyIdentification (Eigene Daten)
            partyIdentification32.Nm = _sepaConfiguration.Name;
            //partyIdentification32.PstlAdr = new PostalAddress6() { Ctry = _sepaConfiguration.Country, AdrLine = _sepaConfiguration.AdressLine };

            return partyIdentification32;
        }

        private PartyIdentification32 zahlungspflichtigerPartyIdentification32(string nameZahlungspflichtiger)
        {
            // ZIEL Kreditoreninformation 
            var partyIdentification32 = new PartyIdentification32
            {
                Nm = nameZahlungspflichtiger // Name des Kontoinhabers / Bezogenen. Auf 70 Zeichen begrenzt.
            };

            return partyIdentification32;
        }

        private Party6Choice creditorId()
        {
            var myOrganisationInformation = new Party6Choice();

            // CREDITOR Sender/Auftraggeber (eigene Informationen)
            // Von der Bank vergebene Identifikation
            var myOrganisationIdentificationCreditorId = new GenericPersonIdentification1[1];
            myOrganisationIdentificationCreditorId[0] = new GenericPersonIdentification1 { Id = _sepaConfiguration.CreditorId };

            myOrganisationInformation.Item = new PersonIdentification5
            {
                // Da man nicht BICOrBEI und Othr angeben darf wurde der BIC deaktiviert.
                //BICOrBEI = _parent.EigeneBankverb.BIC.ToString().TrimEnd(), // BIC des Senders
                Othr = myOrganisationIdentificationCreditorId // Andere Identifikation
            };

            return myOrganisationInformation;
        }

        /// <summary>
        /// Ist ein Paket von Zahlungen (Bestand)
        /// </summary>
        /// <returns></returns>
        private PaymentInstructionInformation4 paymentInstructionInformation4(bool firmenlastschrift, SequenceType1Code lastschriftart, DateTime lastschriftDatum)
        {
            PaymentInstructionInformation4 paymentInstructionInformation4 = new PaymentInstructionInformation4();

            // Bestand (PmtInf - PaymentInstructionInformation4_Base)
            paymentInstructionInformation4 = new PaymentInstructionInformation4();
            paymentInstructionInformation4.CdtrAgt = new BranchAndFinancialInstitutionIdentification4 { FinInstnId = new FinancialInstitutionIdentification7() }; // Kontoführendes Institut des Kontoinhabers / Auftraggebers => Identifikation einer Bank im SEPA-Raum (BIC)

            // PaymentMethod2Code (Zahlungsmethode); Einzig verfügbarer Wert ist DD.
            paymentInstructionInformation4.PmtMtd = PaymentMethod2Code.DD;
            // BatchBookingIndicator (Sammel- oder Einzelbuchung); "true" heißt Bestandsbuchung gewünscht.
            paymentInstructionInformation4.BtchBookg = true;
            paymentInstructionInformation4.BtchBookgSpecified = true;

            paymentInstructionInformation4.PmtTpInf = paymentTypeInformation20(firmenlastschrift, lastschriftart);

            // Gewünschtes Lastschriftsdatum.
            paymentInstructionInformation4.ReqdColltnDt = _lastschriftDatum;
            // Kontoinhaber / Auftraggeber. Wir übergeben hier jetzt auch mal einfach den Sender
            paymentInstructionInformation4.Cdtr = auftraggeberPartyIdentification32();
            // Creditor Account - Kontoinformationen des Kontoinhabers / Auftraggebers
            paymentInstructionInformation4.CdtrAcct = cashAccount16();

            // CdtrSchmeId wird hier NICHT angegeben sondern je Lastschrift                                                                                
            // BIC des Auftraggebers nur ausgeben, wenn es befüllt ist
            if (_sepaConfiguration.BIC != "")
            {
                paymentInstructionInformation4.CdtrAgt.FinInstnId.BIC = _sepaConfiguration.BIC.TrimEnd();
            }

            return paymentInstructionInformation4;
        }

        private PaymentTypeInformation20 paymentTypeInformation20(bool firmenlastschrift, SequenceType1Code lastschriftart)
        {
            var paymentTypeInformation20 = new PaymentTypeInformation20();

            // PaymentTypeInformation (Bestandsverarbeitung)
            paymentTypeInformation20.SvcLvl = new ServiceLevel8Choice { Item = "SEPA", ItemElementName = ItemChoiceType4.Cd }; // Service Spezifikation (Einzig verfügbarer Wert ist SEPA)
            paymentTypeInformation20.LclInstrm = new LocalInstrument2Choice { Item = Mapping.FirmenlastschriftToLocalInstrument2ChoiceItem(firmenlastschrift), ItemElementName = ItemChoiceType5.Cd }; // Zahlungsinstrument. Unterscheidung zwischen (Basis-)Lastschrift [CORE] und Firmen-Lastschrift [B2B]. 
            paymentTypeInformation20.SeqTp = lastschriftart; // Lastschriftsart. FRST Erster Einzug einer Serie, RCUR Wiederkehrender Einzug einer Serie, FNAL Letzter Einzug einer Serie, OOFF Einmaliger Einzug
            paymentTypeInformation20.SeqTpSpecified = true;

            return paymentTypeInformation20;
        }

        /// <summary>
        /// PmtTpInf
        /// </summary>
        /// <returns></returns>
        private PaymentTypeInformation20 paymentTypeInformation20(bool firmenlastschrift)
        {
            // PaymentTypeInformation (Bestandsverarbeitung)
            var paymentTypeInformation20 = new PaymentTypeInformation20();

            paymentTypeInformation20.SvcLvl = new ServiceLevel8Choice
            {
                // Service Spezifikation (Einzig verfügbarer Wert ist SEPA)
                Item = "SEPA",
                ItemElementName = ItemChoiceType4.Cd
            };
            paymentTypeInformation20.LclInstrm = new LocalInstrument2Choice
            {
                // Zahlungsinstrument. Unterscheidung zwischen (Basis-)Lastschrift [CORE] und Firmen-Lastschrift [B2B]. 
                Item = Mapping.FirmenlastschriftToLocalInstrument2ChoiceItem(firmenlastschrift),
                ItemElementName = ItemChoiceType5.Cd
            };

            // Lastschriftsart. FRST Erster Einzug einer Serie, RCUR Wiederkehrender Einzug einer Serie, FNAL Letzter Einzug einer Serie, OOFF Einmaliger Einzug
            paymentTypeInformation20.SeqTp = SequenceType1Code.RCUR;
            paymentTypeInformation20.SeqTpSpecified = true;

            return paymentTypeInformation20;
        }

        private MandateRelatedInformation6 mandateRelatedInformation6(string mandat, DateTime mandatSignature)
        {
            // ZIEL Mandat (MandateRelatedInformation6)
            // Mandatsbezogene Informationen
            var mandateRelatedInformation6 = new MandateRelatedInformation6
            {
                // Mandatsidentifikation
                MndtId = (_sepaConfiguration.FixesLastschriftMandat != "" ? _sepaConfiguration.FixesLastschriftMandat : mandat.TrimEnd()),
                // Unterzeichnungsdatum des Mandats.
                DtOfSgntr = mandatSignature,
                DtOfSgntrSpecified = true
            };

            return mandateRelatedInformation6;
        }

        private DirectDebitTransactionInformation9 directDebitTransactionInformation9(string nameZahlungspflichtiger, string ibanZahlungspflichtiger, decimal betrag, string mandat, DateTime mandatSignature, string verwendungszweck, string endToEndId)
        {
            var directDebitTransactionInformation9 = new DirectDebitTransactionInformation9
            {
                // EndToEndId
                // Auftraggeberreferenz. Kann im Kontoauszug zur eigenen Referenzierung zurückgegeben werden, daher ist Eindeutigkeit wichtig. Falls dennoch keine Referenz angegeben werden soll, mit dem Wert NOTPROVIDED zu füllen.
                PmtId = new PaymentIdentification1 { EndToEndId = Mapping.EndToEndIdToPaymentIdentification1(endToEndId) },

                //PmtTpInf wird NICHT gesetzt. Einzelverarbeitung. ... Von der Verwendung dieses Elements wird dringend abgeraten
                // Einzelbetrag. Punkt als Dezimalzeichen. Keine negativen Werte
                InstdAmt = new ActiveOrHistoricCurrencyAndAmount
                {
                    Value = betrag,
                    Ccy = "EUR"
                },
                // Mandatsbezogene Informationen
                DrctDbtTx = new DirectDebitTransaction6
                {
                    // Mandatsbezogene Informationen
                    MndtRltdInf = mandateRelatedInformation6(mandat, mandatSignature),
                    //Kreditorenidentifikation. Die Kreditorenidentifikation muss entweder hier angegeben werden (und gilt dann nur für diese Lastschrift) oder auf der Bestandsebene (und gilt dann für alle Lastschriften des Bestands). Gleichzeitige Angabe nicht gestattet.
                    CdtrSchmeId = new PartyIdentification32 { Id = creditorId() },
                },
                // Kontoinhaber / Bezogener
                // DbtrAgt = new BranchAndFinancialInstitutionIdentification4() { FinInstnId = new FinancialInstitutionIdentification7() },
                Dbtr = zahlungspflichtigerPartyIdentification32(nameZahlungspflichtiger),
                DbtrAgt = zahlungspflicherBank(nameZahlungspflichtiger),
                DbtrAcct = new CashAccount16
                {
                    Id = new AccountIdentification4Choice
                    {
                        // Kontonummer des Kontoinhabers / Bezogenen. IBAN eines Kontos im SEPA-Raum
                        Item = ibanZahlungspflichtiger.ToString().TrimEnd().Replace(" ", "")
                    }
                },
                // Obwohl hier ein Array als Verwendungszweck übergeben werden kann darf nur genau 1 Element enthalten sein!
                // Verwendungszweck / Bezogenenreferenz;
                RmtInf = new RemittanceInformation5 { Ustrd = new string[] { verwendungszweck } }
            };

            return directDebitTransactionInformation9;
        }

        private static BranchAndFinancialInstitutionIdentification4 zahlungspflicherBank(string nameZahlungspflichtiger)
        {
            return new BranchAndFinancialInstitutionIdentification4
            {
                FinInstnId = new FinancialInstitutionIdentification7
                {
                    Othr = new GenericFinancialIdentification1
                    {
                        Id= "NOTPROVIDED"
                    }
                }
            };
        }

        private CashAccount16 cashAccount16()
        {
            // CashAccount Kontoinformationen (eigene Bankverbindung) 
            var cashAccount16 = new CashAccount16();
            
            cashAccount16.Id = new AccountIdentification4Choice { Item = _sepaConfiguration.IBAN }; // IBAN eines Kontos im SEPA-Raum
            // Kontowährung des erkannten Kontos.
            cashAccount16.Ccy = _sepaConfiguration.Currency;

            return cashAccount16;
        }

        public void AddSepaLastschrift(bool firmenlastschrift, SequenceType1Code lastschriftart
            , string mandat, DateTime mandatSignature, string verwendungszweck, string endToEndId, decimal betrag
            , string ibanZahlungspflichtiger, string nameZahlungspflichtiger, DateTime einzugsdatum)
        {
            // Einzelne Transaktion (Einzelumsätze)
            var directDebitTransactionInformation = directDebitTransactionInformation9(nameZahlungspflichtiger, ibanZahlungspflichtiger, betrag, mandat, mandatSignature, verwendungszweck, endToEndId);

            Bestand bestand;
            bestand = _directDebitTransactionInformationBestand.FirstOrDefault(_ => _.Firmenlastschrift == firmenlastschrift && _.Einzugsdatum == einzugsdatum && _.LastschriftArt == lastschriftart);

            if (bestand == null)
            {
                bestand = new Bestand(firmenlastschrift, einzugsdatum, lastschriftart);
                _directDebitTransactionInformationBestand.Add(bestand);
            }
            bestand.DirectDebitTransactionInformation9.Add(directDebitTransactionInformation);
            // Bestandsdaten aktualisieren
            bestand.NbOfTxs++;
            bestand.CtrlSum += betrag;
        }

        public void Export(string filename)
        {
            var xmlNs = new XmlSerializerNamespaces();
            xmlNs.Add("", "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02");

            List<PaymentInstructionInformation4> paymentInstructionInformation4List = new List<PaymentInstructionInformation4>();

            int totalNbOfTxs = 0;
            decimal totalCtrlSum = decimal.Zero;

            // Bestände einfügen
            foreach (var bestand in _directDebitTransactionInformationBestand)
            {
                if (bestand.DirectDebitTransactionInformation9.Count > 0)
                {
                    PaymentInstructionInformation4 paymentInstructionInformation = paymentInstructionInformation4(bestand.Firmenlastschrift, bestand.LastschriftArt, bestand.Einzugsdatum);

                    // Bestand vervollständigen
                    // Bestandskontrollnummer. Eindeutigkeit über mindestens 1 Jahr ist herzustellen. (Max 35 Zeichen)
                    paymentInstructionInformation.PmtInfId = "PIDCORE" + DateTime.Now.Ticks.ToString();

                    // Summen in Bestand
                    // Einzelumsätze.
                    paymentInstructionInformation.DrctDbtTxInf = bestand.DirectDebitTransactionInformation9.ToArray();
                    // Anzahl der Einzeltransaktionen des Bestands.
                    paymentInstructionInformation.NbOfTxs = bestand.NbOfTxs.ToString();
                    // Summe der Einzeltransaktionen des Bestands.
                    paymentInstructionInformation.CtrlSum = bestand.CtrlSum;
                    paymentInstructionInformation.CtrlSumSpecified = true;

                    totalNbOfTxs += bestand.NbOfTxs;
                    totalCtrlSum += bestand.CtrlSum;

                    paymentInstructionInformation4List.Add(paymentInstructionInformation);
                }
            }

            // Bestände in Lastschriftnachricht eintragen
            _document.CstmrDrctDbtInitn.PmtInf = paymentInstructionInformation4List.ToArray();

            // Zähler und Summen eintragen
            _document.CstmrDrctDbtInitn.GrpHdr.NbOfTxs = totalNbOfTxs.ToString().Trim();
            _document.CstmrDrctDbtInitn.GrpHdr.CtrlSum = totalCtrlSum;

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
