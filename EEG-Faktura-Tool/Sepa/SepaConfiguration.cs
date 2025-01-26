using EEG_Faktura_Sepa_Tool.Properties;
using System;
using System.Configuration;

namespace EEG_Faktura_Sepa_Tool.Sepa
{
    public class SepaConfiguration
    {
        public string BIC
        {
            get
            {
                string result = Settings.Default.BIC ?? throw new ArgumentException("Der BIC ist in den Settings nicht hinterlegt!");
                return result.TrimEnd().Replace(" ", "");
            }
        }
        public string CreditorId
        {
            get
            {
                string result = Settings.Default.CreditorId ?? throw new ArgumentException("Die CreditorId ist in den Settings nicht hinterlegt!");
                return result.TrimEnd().Replace(" ", "");
            }
        }
        public String Name
        {
            get
            {
                string result = Settings.Default.Name ?? throw new ArgumentException("Der Name ist in den Settings nicht hinterlegt!");

                result = result.TrimEnd();

                if (result.Length > 70)
                {
                    result = result.Substring(0, 70);
                }
                return result;
            }
        }
        public String IBAN
        {
            get
            {
                string result = Settings.Default.IBAN ?? throw new ArgumentException("Der IBAN ist in den Settings nicht hinterlegt!");

                result = result.TrimEnd().Replace(" ", "");

                return result;
            }
        }

        public string Currency
        {
            get
            {
                string result = Settings.Default.Currency ?? throw new ArgumentException("Der Währung (Currency) ist in den Settings nicht hinterlegt!");

                return result;
            }
        }
        public string FixesLastschriftMandat
        {
            get
            {
                string result = Settings.Default.FixesLastschriftMandat ?? "";

                return result.TrimEnd();
            }
        }
        public bool BelegeZusammenfassen
        {
            get
            {
                bool result = Settings.Default.BelegeZusammenfassen;

                return result;
            }
        }

        public string Country
        {
            get
            {
                string result = Settings.Default.Country ?? throw new ArgumentException("Das Land (Country) ist in den Settings nicht hinterlegt!");

                return result.TrimEnd();
            }
        }
        public string Adresszeile1 => Settings.Default.AdresseZeile1;
        public string Adresszeile2 => Settings.Default.AdresseZeile2;
        public string[] AdressLine
        {
            get
            {
                string adresLine1 = Settings.Default.AdresseZeile1;
                string adresLine2 = Settings.Default.AdresseZeile2;

                return new string[] { adresLine1, adresLine2 };
            }
        }
    }
}
