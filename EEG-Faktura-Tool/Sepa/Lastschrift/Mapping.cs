using System;

namespace EEG_Faktura_Tool.Sepa.Lastschrift
{
    public static class Mapping
    {
        /*public static SequenceType1Code MandatLastschriftArtToSequenceType1Code(Types.MandatLastschriftArt mandatLastschriftArt)
        {
            switch (mandatLastschriftArt.EnumValue)
            {
                case Types.MandatLastschriftArt.Values.EinmaligerEinzug: return SequenceType1Code.OOFF;
                case Types.MandatLastschriftArt.Values.ErsterEinzug: return SequenceType1Code.FRST;
                case Types.MandatLastschriftArt.Values.WiederkehrenderEinzug: return SequenceType1Code.RCUR;
                case Types.MandatLastschriftArt.Values.LetzterEinzug: return SequenceType1Code.FNAL;
                default:
                    throw new ArgumentException($"Ungültige MandatLastschriftArt {mandatLastschriftArt}");
            }
        }*/
        public static string FirmenlastschriftToLocalInstrument2ChoiceItem(bool mandatFirmenlastschrift)
        {
            // (Basis-)Lastschrift [CORE]
            // Firmen - Lastschrift[B2B]
            return mandatFirmenlastschrift ? "B2B" : "CORE";
        }

        public static string EndToEndIdToPaymentIdentification1(string endToEndId)
        {
            // Kann im Kontoauszug zur eigenen Referenzierung zurückgegeben werden, daher ist Eindeutigkeit wichtig.Falls dennoch keine Referenz angegeben werden soll, mit dem Wert NOTPROVIDED zu füllen.
            return endToEndId != "" ? endToEndId : "NOTPROVIDED";
        }

        /// <summary>
        /// Ein Bestand(PmtInf (PaymentInstructionInformation4_Base)) hat eine Bestandsverarbeitung(PmtTpInf(PaymentTypeInformation20_Base))
        /// Da diese empfohlen ist und nicht die Einzelverarbeitung auf Einzelumsatzebene(PaymentTypeInformation20_Single) müssen die Daten beim Export laut dieser Struktur gruppiert und ausgegeben werden!!!!
        /// Das sind:
        /// - SvcLvl.Cd - Service Spezifikation(fix SEPA)
        /// - ReqdColltnDt - Gewünschtes Lastschriftsdatum
        /// - LclInstrm.Cd - Zahlungsart(CORE, B2B)
        /// - SeqTp - Lastschriftart(FRST, RCUR, FNAL, OOFF)
        /// </summary>
        /// <returns>Einen String für die Gruppierung der Daten</returns>
        public static string GroupPerPaymentInstructionInformation4(bool mandatFirmenlastschrift, SequenceType1Code lastschriftArt, DateTime lastschriftDatumGruppe)
        {
            return $"SEPA_{lastschriftDatumGruppe.ToString("yyyy-MM-dd")}_{FirmenlastschriftToLocalInstrument2ChoiceItem(mandatFirmenlastschrift)}_{lastschriftArt.ToString()}";
        }
    }
}
