using System;

namespace EEG_Faktura_Sepa_Tool.EEG_Faktura
{
    public class EEGFakturaPaymentItem
    {
        public Dokumenttyp Dokumenttyp { get; set; }
        public string Nummer { get; set; }
        public DateTime Datum { get; set; }
        public string Abrechnung { get; set; }
        public string EmpfaengerName { get; set; }
        public string EmpfaengerMitgliedsnummer { get; set; }
        public string EmpfaengerAdresse1 { get; set; }
        public string EmpfaengerAdresse2 { get; set; }
        public string EmpfaengerAdresse3 { get; set; }
        public string EmpfaengerKontoeigner { get; set; }
        private string _empfaengerKontoIBAN;
        public string EmpfaengerKontoIBAN {
            get
            {
                return _empfaengerKontoIBAN;
            }
            set
            {
                _empfaengerKontoIBAN = value.Replace(" ", "").Trim();
            }
        }
        public DateTime EmpfaengerMandatsausstellung { get; set; }
        public string EmpfaengerMandatsreferenz { get; set; }
        public bool Firmenlastschrift { get; set; }
        public string ErstellerName { get; set; }
        public string ErstellerBankName { get; set; }
        private string _erstellerIBAN;
        public string ErstellerIBAN
        {
            get
            {
                return _erstellerIBAN;
            }
            set
            {
                _erstellerIBAN = value.Replace(" ", "").Trim();
            }
        }
        public Decimal UstSatz1Prozent { get; set; }
        public Decimal UstSatz1SummeEuro { get; set; }
        public Decimal UstSatz2Prozent { get; set; }
        public Decimal UstSatz2SummeEuro { get; set; }
        public Decimal RechnungsbetragNetto { get; set; }
        public Decimal RechnungsbetragBrutto { get; set; }
        public string ErstellerUID { get; set; }
        public string EmpfaengerUID { get; set; }
        public string EmpfaengerVorname { get; set; }
        public string EmpfaengerNachname { get; set; }
    }
}
