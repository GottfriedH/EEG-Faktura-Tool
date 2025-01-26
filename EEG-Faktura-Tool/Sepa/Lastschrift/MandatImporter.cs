using EEG_Faktura_Sepa_Tool.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace EEG_Faktura_Sepa_Tool.Sepa.Lastschrift
{
    internal class MandatImporter
    {
        Excel.Application application = null;
        Excel.Workbook workbook = null;
        Excel.Worksheet sheet = null;

        private List<SepaMandat> _sepaMandatItems;

        public List<SepaMandat> ImportSepaMandate(Func<int, bool> progress)
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
                sheet = workbook.ActiveSheet;
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

            _sepaMandatItems = new List<SepaMandat>();

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

                if (ExcelHelper.GetStringValue(row.Cells[1, 1]) != "")
                {
                    SepaMandat sepaMandatItem = AddSepaMandatItem(row);
                    _sepaMandatItems.Add(sepaMandatItem);
                    Console.WriteLine($"{rowNumber}: {sepaMandatItem.Mitgliedsnummer} - {sepaMandatItem.Kontoeigner}: {sepaMandatItem.Mandatsreferenz} {sepaMandatItem.Ausstellungsdatum:d}");
                }
            }

            Console.WriteLine($"Anzahl Sepa-Mandate: € {_sepaMandatItems.Count}");

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

            return _sepaMandatItems;
        }

        private bool CheckEegFakturaFileStructureIsValid(Excel.Range row)
        {
            string mitgliedsnummer = ExcelHelper.GetStringValue(row.Cells[1, 1]);
            string kontoEigner = ExcelHelper.GetStringValue(row.Cells[1, 2]);
            string mandatsreferenz = ExcelHelper.GetStringValue(row.Cells[1, 3]);
            string ausstellungsdatum = ExcelHelper.GetStringValue(row.Cells[1, 4]);
            string firmenlastschrift= ExcelHelper.GetStringValue(row.Cells[1, 5]);

            return mitgliedsnummer == "Mitgliedsnummer"
                   && kontoEigner == "Empfänger Kontoeigner"
                   && mandatsreferenz == "Mandatsreferenz"
                   && ausstellungsdatum == "Ausstellungsdatum"
                   && firmenlastschrift == "Firmenlastschrift";
        }

        private SepaMandat AddSepaMandatItem(Excel.Range row)
        {
            string firmenlastschrift = ExcelHelper.GetStringValue(row.Cells[1, 5]);

            SepaMandat sepaMandatItem = new SepaMandat
            {
                Mitgliedsnummer = ExcelHelper.GetStringValue(row.Cells[1, 1]),
                Kontoeigner = ExcelHelper.GetStringValue(row.Cells[1, 2]),
                Mandatsreferenz = ExcelHelper.GetStringValue(row.Cells[1, 3]),
                Ausstellungsdatum = ExcelHelper.GetDateValue(row.Cells[1, 4]),
                Firmenlastschrift = firmenlastschrift != ""
            };
            return sepaMandatItem;
        }
    }
}
