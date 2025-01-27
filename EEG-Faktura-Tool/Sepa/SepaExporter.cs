using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EEG_Faktura_Sepa_Tool.EEG_Faktura;
using EEG_Faktura_Sepa_Tool.Sepa;
using EEG_Faktura_Sepa_Tool.Sepa.Lastschrift;
using EEG_Faktura_Sepa_Tool.Sepa.Ueberweisung;

namespace EEG_Faktura_Sepa_Tool
{
    internal class SepaExporter
    {
        SepaLastschriftDocument _sepaLastschrift;
        SepaUeberweisungDocument _sepaUeberweisung;
        SepaConfiguration _sepaConfiguration = new SepaConfiguration();

        List<EEG_Faktura.EEGFakturaPaymentItem> _eegFakturaPaymentItems;
        private DateTime _einzugsdatum;
        private DateTime _ueberweisungsdatum;

        public SepaExporter(List<EEG_Faktura.EEGFakturaPaymentItem> eegFakturaPaymentItems, DateTime einzugsdatum, DateTime ueberweisungsdatum)
        {
            _eegFakturaPaymentItems = eegFakturaPaymentItems ?? throw new ArgumentException("Objekt \"EegFakturaPaymentIems\" darf nicht NULL sein!");
            _einzugsdatum = einzugsdatum;
            _ueberweisungsdatum = ueberweisungsdatum;

            _sepaLastschrift = new SepaLastschriftDocument(_einzugsdatum);
            _sepaUeberweisung = new SepaUeberweisungDocument(_ueberweisungsdatum);
        }

        public void ExportSepaFile()
        {
            if (_eegFakturaPaymentItems.Count == 0)
            {
                MessageBox.Show("Es sind keine Datensätze enthalten. Der Export wird abgebrochen!");
                return;
            }

            int totalLastschriftenCount = 0;
            int totalUeberweisungCount = 0;
            decimal totalLastschriftenAmount = decimal.Zero;
            decimal totalUeberweisungAmount = decimal.Zero;
            decimal totalAmount = decimal.Zero;

            List<String> verwendugszweck = new List<String>();

            IEnumerable<IGrouping<string, EEGFakturaPaymentItem>> fakturaItemsGruppiert;

            if (_sepaConfiguration.BelegeZusammenfassen)
            {
                fakturaItemsGruppiert = _eegFakturaPaymentItems.GroupBy(_ => _.EmpfaengerMitgliedsnummer);
            }
            else
            {
                fakturaItemsGruppiert = _eegFakturaPaymentItems.GroupBy(_ => _.Nummer);
            }

            foreach (var mitgliedsnummerGruppe in fakturaItemsGruppiert)
            {
                int paymentItemCount = 0;
                
                verwendugszweck.Clear();
                
                var firstPaymentItem = mitgliedsnummerGruppe.First();

                Console.WriteLine($"{firstPaymentItem.EmpfaengerMitgliedsnummer} - {firstPaymentItem.EmpfaengerName}");

                decimal amount = decimal.Zero;

                foreach (var paymentItem in mitgliedsnummerGruppe)
                {
                    paymentItemCount++;
                    verwendugszweck.Add($"{paymentItem.Dokumenttyp} {paymentItem.Nummer}");
                    
                    Console.WriteLine($"      {paymentItem.Dokumenttyp.ToString()} => {paymentItem.RechnungsbetragBrutto}");
                    if (paymentItem.Dokumenttyp == EEG_Faktura.Dokumenttyp.Rechnung)
                    {
                        amount += paymentItem.RechnungsbetragBrutto;
                    }
                    else if (paymentItem.Dokumenttyp == EEG_Faktura.Dokumenttyp.Gutschrift)
                    {
                        amount -= paymentItem.RechnungsbetragBrutto;
                    }
                }
                
                if (amount < 0)
                {
                    _sepaUeberweisung.AddSepaUeberweisung(string.Join(", ", verwendugszweck), GetEndToEndIt(mitgliedsnummerGruppe), amount * -1, firstPaymentItem.EmpfaengerKontoIBAN
                        , firstPaymentItem.EmpfaengerName, _ueberweisungsdatum);

                    totalUeberweisungCount++;
                    totalUeberweisungAmount += amount;
                    Console.WriteLine($"      = Überweisung {amount}");
                }
                else
                {
                    // @ToDo Parameter korrekt befüllen
                    _sepaLastschrift.AddSepaLastschrift(firstPaymentItem.Firmenlastschrift, SequenceType1Code.RCUR, firstPaymentItem.EmpfaengerMandatsreferenz
                        , firstPaymentItem.EmpfaengerMandatsausstellung, String.Join(", ", verwendugszweck), GetEndToEndIt(mitgliedsnummerGruppe)
                        , amount, firstPaymentItem.EmpfaengerKontoIBAN, firstPaymentItem.EmpfaengerName, _einzugsdatum);
                    
                    totalLastschriftenCount++;
                    totalLastschriftenAmount += amount;
                    Console.WriteLine($"      = Lastschrift {amount}");
                }

                totalAmount += amount;
            }

            string pathSepaLastschriften = Path.GetFullPath(@".\eegFakturaLastschrift.XML");
            string pathSepaUeberweisung = Path.GetFullPath(@".\eegFakturaUeberweisung.XML");

            if (totalLastschriftenCount > 0)
            {
                _sepaLastschrift.Export(pathSepaLastschriften);
            }
            if (totalUeberweisungCount > 0)
            {
                _sepaUeberweisung.Export(pathSepaUeberweisung);
            }

            MessageBox.Show($"Lastschriften:\t({totalLastschriftenCount})\t€ {totalLastschriftenAmount}" + 
                $"\r\nÜberweisungen:\t({totalUeberweisungCount})\t€ {totalUeberweisungAmount}" +
                $"\r\nDifferenz:\t\t€ {totalAmount}" +
                $"\r\n" +
                $"\r\nLastschriften: {pathSepaLastschriften}" +
                $"\r\nÜberweisungen: {pathSepaUeberweisung}", "Export abgeschlossen", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetEndToEndIt(IGrouping<string, EEGFakturaPaymentItem> mitgliedsnummerGruppe)
        {
            return String.Join("-", mitgliedsnummerGruppe.Select(_ => _.Nummer));
        }
    }
}
