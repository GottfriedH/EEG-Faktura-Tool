using EEG_Faktura_Sepa_Tool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace EEG_Faktura_Sepa_Tool
{
    internal class EegFakturaImporter
    {
        Excel.Application application = null;
        Excel.Workbook workbook = null;
        Excel.Worksheet sheet = null;

        private List<EEG_Faktura.EEGFakturaPaymentItem> _eegFakturaPaymentItems;

        public List<EEG_Faktura.EEGFakturaPaymentItem> ImportEEGFakturaExport(Func<int, bool> progress)
        {
            string filename;

            var dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "Excel-Dateien (*.xlsx)|*.xlsx";

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                filename = dialog.FileName;
            }
            else
            {
                return null;
            }

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Open(filename);
                sheet = workbook.Sheets["Liste"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beim Öffnen der Datei {filename} ist ein Fehler aufgetreten!\r\nFehlermeldung: {ex.Message}", "Fehler");
                return null;
            }

            int rowNumber = 0;

            Excel.Range last = sheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

            int rows = last.Row;
            Console.Write($"Datensätze: {rows}");

            _eegFakturaPaymentItems = new List<EEG_Faktura.EEGFakturaPaymentItem>();

            for (int i = 1; i <= rows; i++)
            {
                rowNumber++;

                if (progress != null)
                {
                    decimal percent = (decimal)rowNumber / (decimal)rows * 100;

                    progress((int)percent);
                }

                Excel.Range row = sheet.Rows[i];

                // Überschriftenzeile prüfen und dann überspringen
                if (i == 1)
                {
                    if (!CheckEegFakturaFileStructureIsValid(row))
                    {
                        throw new ArgumentException("Der Aufbau der Datei entspricht nicht dem erwarteten Schema!");
                    }
                    
                    continue;
                }

                EEG_Faktura.EEGFakturaPaymentItem paymentItem = AddEegFakturaPaymentItem(row);
                _eegFakturaPaymentItems.Add(paymentItem);
                Console.WriteLine($"{rowNumber}: {paymentItem.EmpfaengerName} - {paymentItem.RechnungsbetragBrutto}");
            }

            Console.WriteLine($"Summe Gutschriften: € {_eegFakturaPaymentItems.Where(_ => _.Dokumenttyp == EEG_Faktura.Dokumenttyp.Gutschrift).Sum(_ => _.RechnungsbetragBrutto)}");
            Console.WriteLine($"Summe Rechnungen: € {_eegFakturaPaymentItems.Where(_ => _.Dokumenttyp == EEG_Faktura.Dokumenttyp.Rechnung).Sum(_ => _.RechnungsbetragBrutto)}");

            if (workbook != null)
            {
                if (sheet != null)
                    Marshal.ReleaseComObject(sheet);

                //close and release
                if (workbook != null)
                {
                    workbook.Close();
                    Marshal.ReleaseComObject(workbook);
                }

                //quit and release
                if (application != null)
                {
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                }
            }

            return _eegFakturaPaymentItems;
        }

        private bool CheckEegFakturaFileStructureIsValid(Excel.Range row)
        {
            return ExcelHelper.GetStringValue(row.Cells[1, 1]) == "Dokumenttyp"
                   && ExcelHelper.GetStringValue(row.Cells[1, 2]) == "Nummer"
                   && ExcelHelper.GetStringValue(row.Cells[1, 3]) == "Datum"
                   && ExcelHelper.GetStringValue(row.Cells[1, 4]) == "Abrechnung"
                   && ExcelHelper.GetStringValue(row.Cells[1, 5]) == "Empfänger Name"
                   && ExcelHelper.GetStringValue(row.Cells[1, 6]) == "Empfänger Mitgliedsnummer"
                   && ExcelHelper.GetStringValue(row.Cells[1, 7]) == "Empfänger Adresse 1"
                   && ExcelHelper.GetStringValue(row.Cells[1, 8]) == "Empfänger Adresse 2"
                   && ExcelHelper.GetStringValue(row.Cells[1, 9]) == "Empfänger Adresse 3"
                   && ExcelHelper.GetStringValue(row.Cells[1, 10]) == "Empfänger Kontoeigner"
                   && ExcelHelper.GetStringValue(row.Cells[1, 11]) == "Empfänger Konto IBAN"
                   && ExcelHelper.GetStringValue(row.Cells[1, 12]) == "Empfänger Mandatsausstellung"
                   && ExcelHelper.GetStringValue(row.Cells[1, 13]) == "Empfänger Mandatsreferenz"
                   && ExcelHelper.GetStringValue(row.Cells[1, 14]) == "Ersteller Name"
                   && ExcelHelper.GetStringValue(row.Cells[1, 15]) == "Ersteller BankName"
                   && ExcelHelper.GetStringValue(row.Cells[1, 16]) == "Ersteller IBAN"
                   && ExcelHelper.GetStringValue(row.Cells[1, 17]) == "UST Satz 1 (%)"
                   && ExcelHelper.GetStringValue(row.Cells[1, 18]) == "UST Satz 1 Summe (Euro)"
                   && ExcelHelper.GetStringValue(row.Cells[1, 19]) == "UST Satz 2 (%)"
                   && ExcelHelper.GetStringValue(row.Cells[1, 20]) == "UST Satz 2 Summe (Euro)"
                   && ExcelHelper.GetStringValue(row.Cells[1, 21]) == "Rechnungsbetrag Netto"
                   && ExcelHelper.GetStringValue(row.Cells[1, 22]) == "Rechnungsbetrag Brutto"
                   && ExcelHelper.GetStringValue(row.Cells[1, 23]) == "Ersteller UID"
                   && ExcelHelper.GetStringValue(row.Cells[1, 24]) == "Empfänger UID"
                   && ExcelHelper.GetStringValue(row.Cells[1, 25]) == "Empfänger Vorame"
                   && ExcelHelper.GetStringValue(row.Cells[1, 26]) == "Empfänger Nachname";
        }

        private EEG_Faktura.EEGFakturaPaymentItem AddEegFakturaPaymentItem(Excel.Range row)
        {
            EEG_Faktura.EEGFakturaPaymentItem paymentItem = new EEG_Faktura.EEGFakturaPaymentItem
            {
                Nummer = ExcelHelper.GetStringValue(row.Cells[1, 2]),
                Datum = ExcelHelper.GetDateValue(row.Cells[1, 3]),
                Abrechnung = ExcelHelper.GetStringValue(row.Cells[1, 4]),
                EmpfaengerName = ExcelHelper.GetStringValue(row.Cells[1, 5]),
                EmpfaengerMitgliedsnummer = ExcelHelper.GetStringValue(row.Cells[1, 6]),
                EmpfaengerAdresse1 = ExcelHelper.GetStringValue(row.Cells[1, 7]),
                EmpfaengerAdresse2 = ExcelHelper.GetStringValue(row.Cells[1, 8]),
                EmpfaengerAdresse3 = ExcelHelper.GetStringValue(row.Cells[1, 9]),
                EmpfaengerKontoeigner = ExcelHelper.GetStringValue(row.Cells[1, 10]),
                EmpfaengerKontoIBAN = ExcelHelper.GetStringValue(row.Cells[1, 11]),
                //EmpfaengerMandatsausstellung = ExcelHelper.GetDateValue(row.Cells[1, 12]),
                //EmpfaengerMandatsreferenz = ExcelHelper.GetStringValue(row.Cells[1, 13]),
                ErstellerName = ExcelHelper.GetStringValue(row.Cells[1, 14]),
                ErstellerBankName = ExcelHelper.GetStringValue(row.Cells[1, 15]),
                ErstellerIBAN = ExcelHelper.GetStringValue(row.Cells[1, 16]),
                UstSatz1Prozent = ExcelHelper.GetNumericValue(row.Cells[1, 17]),
                UstSatz1SummeEuro = ExcelHelper.GetNumericValue(row.Cells[1, 18]),
                UstSatz2Prozent = ExcelHelper.GetNumericValue(row.Cells[1, 19]),
                UstSatz2SummeEuro = ExcelHelper.GetNumericValue(row.Cells[1, 20]),
                RechnungsbetragNetto = ExcelHelper.GetNumericValue(row.Cells[1, 21]),
                RechnungsbetragBrutto = ExcelHelper.GetNumericValue(row.Cells[1, 22]),
                ErstellerUID = ExcelHelper.GetStringValue(row.Cells[1, 23]),
                EmpfaengerUID = ExcelHelper.GetStringValue(row.Cells[1, 24]),
                EmpfaengerVorname = ExcelHelper.GetStringValue(row.Cells[1, 25]),
                EmpfaengerNachname = ExcelHelper.GetStringValue(row.Cells[1, 26])
            };

            EEG_Faktura.Dokumenttyp documentType;

            Enum.TryParse(ExcelHelper.GetStringValue(row.Cells[1, 1]), out documentType);
            paymentItem.Dokumenttyp = documentType;

            return paymentItem;
        }
    }
}
