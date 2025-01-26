using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG_Faktura_Sepa_Tool.Sepa.Lastschrift
{
    internal class SepaMandat
    {
        public string Mitgliedsnummer { get; set; }
        public string Mandatsreferenz {  get; set; }
        public string Kontoeigner { get; set; }
        /// <summary>
        /// Nachname oder Firma
        /// </summary>
        public DateTime Ausstellungsdatum { get; set; }
        public bool Firmenlastschrift{ get; set; }
    }
}
