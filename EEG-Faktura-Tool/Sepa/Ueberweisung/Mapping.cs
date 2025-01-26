using System;

namespace EEG_Faktura_Sepa_Tool.Sepa.Ueberweisung
{
    public static class Mapping
    {
        public static string EndToEndIdToPaymentIdentification1(string endToEndId)
        {
            // Kann im Kontoauszug zur eigenen Referenzierung zurückgegeben werden, daher ist Eindeutigkeit wichtig.Falls dennoch keine Referenz angegeben werden soll, mit dem Wert NOTPROVIDED zu füllen.
            return endToEndId != "" ? endToEndId : "NOTPROVIDED";
        }
    }
}
