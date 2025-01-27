using EEG_Faktura_Sepa_Tool.Sepa;
using EEG_Faktura_Sepa_Tool.Sepa.Lastschrift;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EEG_Faktura_Sepa_Tool
{
    public partial class EEGFakturaTool : Form
    {
        internal List<EEG_Faktura.EEGFakturaPaymentItem> _paymentItems;
        internal List<SepaMandat> _sepaMandatItems;
        SepaConfiguration _sepaConfiguration = new SepaConfiguration();

        public EEGFakturaTool()
        {
            InitializeComponent();
            
            dataGridView.AutoGenerateColumns = true;
            tstb_Einzungsdatum.Text = DateTime.Now.Date.AddDays(2).ToString("yyyy-MM-dd");
            tstb_Ueberweisungsdatum.Text = DateTime.Now.Date.AddDays(3).ToString("yyyy-MM-dd");

            tbNameEEG.Text = _sepaConfiguration.Name;
            tbCountry.Text = _sepaConfiguration.Country;
            tbAnschriftZeile1.Text = _sepaConfiguration.Adresszeile1;
            tbAnschriftZeile2.Text = _sepaConfiguration.Adresszeile2;
            tbIBAN.Text = Regex.Replace(_sepaConfiguration.IBAN, ".{4}", "$0 ");
            tbBIC.Text = _sepaConfiguration.BIC;
            tbCreditorId.Text = _sepaConfiguration.CreditorId;
            tbCurrency.Text = _sepaConfiguration.Currency;
            cbxBelegeZusammenfassen.Checked = _sepaConfiguration.BelegeZusammenfassen;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripProgressBar.Maximum = 100;
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Visible = true;
            sepaDateiErstellenToolStripMenuItem.Enabled = false;

            _paymentItems?.Clear();
            _sepaMandatItems?.Clear();
            dataGridView.Refresh();


            _paymentItems = new EegFakturaImporter().ImportEEGFakturaExport(progress);
            
            eEGFakturaPaymentItemBindingSource.DataSource = _paymentItems.OrderBy(_ => _.EmpfaengerMitgliedsnummer);
            dataGridView.Refresh();

            toolStripProgressBar.Visible = false;

            if (_sepaConfiguration.FixesLastschriftMandat == "")
            {
                sepaMandateImportierenToolStripMenuItem.Enabled = true;
            }
            else
            {
                sepaDateiErstellenToolStripMenuItem.Enabled = true;
            }
        }

        private bool progress(int percent)
        {
            toolStripProgressBar.Value = percent;

            return true;
        }

        private void sEPADateiErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void Export()
        {
            if (_paymentItems != null)
            {
                new SepaExporter(_paymentItems, DateTime.Parse(tstb_Einzungsdatum.Text), DateTime.Parse(tstb_Ueberweisungsdatum.Text)).ExportSepaFile();
            }
        }

        private void sepaMandateImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportMandate();
        }

        private void ImportMandate()
        {
            toolStripProgressBar.Maximum = 100;
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Visible = true;

            _sepaMandatItems?.Clear();

            _sepaMandatItems = new MandatImporter().ImportSepaMandate(progress);
            
            toolStripProgressBar.Visible = false;

            if (_sepaMandatItems.Count > 0)
            {

                MessageBox.Show($"Es wurde(n) {_sepaMandatItems.Count} Mandat(e) importiert.");
                if (UpdateMandatInPaymentItems())
                {
                    MessageBox.Show($"Es konnten alle Mandate zugeordnet werden.");
                    sepaDateiErstellenToolStripMenuItem.Enabled = true;
                }
                else
                {
                    MessageBox.Show($"Beim Zuordnen der Mandate trat ein Fehler auf! Bitte die Daten richtigstellen und den Vorgang nochmals starten.");
                }
            }

            dataGridView.Refresh();
        }

        private bool UpdateMandatInPaymentItems()
        {
            foreach (var paymentItem in _paymentItems)
            {
                var mandat = _sepaMandatItems.FirstOrDefault(_ => _.Mitgliedsnummer == paymentItem.EmpfaengerMitgliedsnummer);

                if (mandat == null)
                {
                    MessageBox.Show($"Für das Mitglied {paymentItem.EmpfaengerMitgliedsnummer} - {paymentItem.EmpfaengerNachname} ist kein Mandat vorhanden!");
                    
                    return false;
                }

                if (paymentItem.EmpfaengerKontoeigner != mandat.Kontoeigner)
                {
                    MessageBox.Show($"Für die Mitglied {paymentItem.EmpfaengerMitgliedsnummer} - {paymentItem.EmpfaengerNachname} stimmt der Kontoeigner nicht überein!" +
                        $"\r\nRechnungsdaten:\t{paymentItem.EmpfaengerKontoeigner}" +
                        $"\r\nMandat:\t{mandat.Kontoeigner}");
                }

                paymentItem.EmpfaengerMandatsreferenz = mandat.Mandatsreferenz;
                paymentItem.EmpfaengerMandatsausstellung = mandat.Ausstellungsdatum;
                paymentItem.Firmenlastschrift = mandat.Firmenlastschrift;
            }

            return true;
        }

        private void CloseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
