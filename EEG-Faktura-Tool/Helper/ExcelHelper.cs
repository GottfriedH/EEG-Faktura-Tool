using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace EEG_Faktura_Tool.Helper
{
    internal class ExcelHelper
    {
        public static string GetStringValue(Excel.Range cell)
        {
            return cell?.Value?.ToString() ?? "";
        }
        public static DateTime GetDateValue(Excel.Range cell)
        {
            string value = "#NV";
            DateTime result;

            try
            {
                value = cell?.Value?.ToString();

                result = DateTime.Parse(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Der Wert \"{value}\" kann nicht in ein Datum umgewandelt werden!");
            }

            return result;
        }
        public static Decimal GetNumericValue(Excel.Range cell)
        {
            string value = "";
            Decimal result;

            value = cell?.Value?.ToString();

            if (value == null)
            {
                return 0m;
            }

            if (!Decimal.TryParse(value, out result))
            {
                throw new ArgumentException($"Der Wert \"{value}\" kann nicht in eine Zahl umgewandelt werden!");
            }
            return result;
        }
    }
}
