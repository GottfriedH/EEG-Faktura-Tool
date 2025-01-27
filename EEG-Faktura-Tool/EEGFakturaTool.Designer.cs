namespace EEG_Faktura_Tool
{
    partial class EEGFakturaTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sEPADatenträgerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importEegFakturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sepaMandateImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sepaDateiErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einzungsdatumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tstb_Einzungsdatum = new System.Windows.Forms.ToolStripTextBox();
            this.überweisungsdatumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tstb_Ueberweisungsdatum = new System.Windows.Forms.ToolStripTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.eEGFakturaPaymentItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbxBelegeZusammenfassen = new System.Windows.Forms.CheckBox();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.lblIBAN = new System.Windows.Forms.Label();
            this.lblBIC = new System.Windows.Forms.Label();
            this.lblCreditorId = new System.Windows.Forms.Label();
            this.lblStrasse = new System.Windows.Forms.Label();
            this.lblLand = new System.Windows.Forms.Label();
            this.lblNameEEG = new System.Windows.Forms.Label();
            this.tbBIC = new System.Windows.Forms.TextBox();
            this.tbCreditorId = new System.Windows.Forms.TextBox();
            this.tbCurrency = new System.Windows.Forms.TextBox();
            this.tbIBAN = new System.Windows.Forms.TextBox();
            this.tbCountry = new System.Windows.Forms.TextBox();
            this.tbAnschriftZeile2 = new System.Windows.Forms.TextBox();
            this.tbAnschriftZeile1 = new System.Windows.Forms.TextBox();
            this.tbNameEEG = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eEGFakturaPaymentItemBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.sEPADatenträgerToolStripMenuItem,
            this.einzungsdatumToolStripMenuItem,
            this.tstb_Einzungsdatum,
            this.überweisungsdatumToolStripMenuItem,
            this.tstb_Ueberweisungsdatum});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseApplicationToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(46, 23);
            this.toolStripMenuItem1.Text = "&Datei";
            // 
            // CloseApplicationToolStripMenuItem
            // 
            this.CloseApplicationToolStripMenuItem.Name = "CloseApplicationToolStripMenuItem";
            this.CloseApplicationToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.CloseApplicationToolStripMenuItem.Text = "Programm verlassen";
            this.CloseApplicationToolStripMenuItem.Click += new System.EventHandler(this.CloseApplicationToolStripMenuItem_Click);
            // 
            // sEPADatenträgerToolStripMenuItem
            // 
            this.sEPADatenträgerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importEegFakturaToolStripMenuItem,
            this.sepaMandateImportierenToolStripMenuItem,
            this.sepaDateiErstellenToolStripMenuItem});
            this.sEPADatenträgerToolStripMenuItem.Name = "sEPADatenträgerToolStripMenuItem";
            this.sEPADatenträgerToolStripMenuItem.Size = new System.Drawing.Size(112, 23);
            this.sEPADatenträgerToolStripMenuItem.Text = "SEPA-Datenträger";
            // 
            // importEegFakturaToolStripMenuItem
            // 
            this.importEegFakturaToolStripMenuItem.Name = "importEegFakturaToolStripMenuItem";
            this.importEegFakturaToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.importEegFakturaToolStripMenuItem.Text = "&EEG-Fakutura-Datei importieren";
            this.importEegFakturaToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // sepaMandateImportierenToolStripMenuItem
            // 
            this.sepaMandateImportierenToolStripMenuItem.Enabled = false;
            this.sepaMandateImportierenToolStripMenuItem.Name = "sepaMandateImportierenToolStripMenuItem";
            this.sepaMandateImportierenToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.sepaMandateImportierenToolStripMenuItem.Text = "SEPA-&Mandate importieren";
            this.sepaMandateImportierenToolStripMenuItem.Click += new System.EventHandler(this.sepaMandateImportierenToolStripMenuItem_Click);
            // 
            // sepaDateiErstellenToolStripMenuItem
            // 
            this.sepaDateiErstellenToolStripMenuItem.Enabled = false;
            this.sepaDateiErstellenToolStripMenuItem.Name = "sepaDateiErstellenToolStripMenuItem";
            this.sepaDateiErstellenToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.sepaDateiErstellenToolStripMenuItem.Text = "SEPA-&Datenträger erstellen";
            this.sepaDateiErstellenToolStripMenuItem.Click += new System.EventHandler(this.sEPADateiErstellenToolStripMenuItem_Click);
            // 
            // einzungsdatumToolStripMenuItem
            // 
            this.einzungsdatumToolStripMenuItem.Name = "einzungsdatumToolStripMenuItem";
            this.einzungsdatumToolStripMenuItem.Size = new System.Drawing.Size(97, 23);
            this.einzungsdatumToolStripMenuItem.Text = "Einzugsdatum:";
            // 
            // tstb_Einzungsdatum
            // 
            this.tstb_Einzungsdatum.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstb_Einzungsdatum.Name = "tstb_Einzungsdatum";
            this.tstb_Einzungsdatum.Size = new System.Drawing.Size(68, 23);
            this.tstb_Einzungsdatum.Text = "Einzugsdatum:";
            // 
            // überweisungsdatumToolStripMenuItem
            // 
            this.überweisungsdatumToolStripMenuItem.Name = "überweisungsdatumToolStripMenuItem";
            this.überweisungsdatumToolStripMenuItem.Size = new System.Drawing.Size(131, 23);
            this.überweisungsdatumToolStripMenuItem.Text = "Überweisungsdatum:";
            // 
            // tstb_Ueberweisungsdatum
            // 
            this.tstb_Ueberweisungsdatum.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstb_Ueberweisungsdatum.Name = "tstb_Ueberweisungsdatum";
            this.tstb_Ueberweisungsdatum.Size = new System.Drawing.Size(68, 23);
            this.tstb_Ueberweisungsdatum.Text = "Überweisungsdatum:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(300, 16);
            this.toolStripProgressBar.Visible = false;
            // 
            // dataGridView
            // 
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.DataSource = this.eEGFakturaPaymentItemBindingSource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 27);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 72;
            this.dataGridView.Size = new System.Drawing.Size(800, 401);
            this.dataGridView.TabIndex = 2;
            // 
            // eEGFakturaPaymentItemBindingSource
            // 
            this.eEGFakturaPaymentItemBindingSource.AllowNew = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbxBelegeZusammenfassen);
            this.panel1.Controls.Add(this.lblCurrency);
            this.panel1.Controls.Add(this.lblIBAN);
            this.panel1.Controls.Add(this.lblBIC);
            this.panel1.Controls.Add(this.lblCreditorId);
            this.panel1.Controls.Add(this.lblStrasse);
            this.panel1.Controls.Add(this.lblLand);
            this.panel1.Controls.Add(this.lblNameEEG);
            this.panel1.Controls.Add(this.tbBIC);
            this.panel1.Controls.Add(this.tbCreditorId);
            this.panel1.Controls.Add(this.tbCurrency);
            this.panel1.Controls.Add(this.tbIBAN);
            this.panel1.Controls.Add(this.tbCountry);
            this.panel1.Controls.Add(this.tbAnschriftZeile2);
            this.panel1.Controls.Add(this.tbAnschriftZeile1);
            this.panel1.Controls.Add(this.tbNameEEG);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 361);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 67);
            this.panel1.TabIndex = 3;
            // 
            // cbxBelegeZusammenfassen
            // 
            this.cbxBelegeZusammenfassen.AutoSize = true;
            this.cbxBelegeZusammenfassen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxBelegeZusammenfassen.Enabled = false;
            this.cbxBelegeZusammenfassen.Location = new System.Drawing.Point(632, 23);
            this.cbxBelegeZusammenfassen.Name = "cbxBelegeZusammenfassen";
            this.cbxBelegeZusammenfassen.Size = new System.Drawing.Size(143, 17);
            this.cbxBelegeZusammenfassen.TabIndex = 2;
            this.cbxBelegeZusammenfassen.Text = "Belege zusammenfassen";
            this.cbxBelegeZusammenfassen.UseVisualStyleBackColor = true;
            // 
            // lblCurrency
            // 
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Location = new System.Drawing.Point(634, 5);
            this.lblCurrency.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(54, 13);
            this.lblCurrency.TabIndex = 1;
            this.lblCurrency.Text = "Währung:";
            // 
            // lblIBAN
            // 
            this.lblIBAN.AutoSize = true;
            this.lblIBAN.Location = new System.Drawing.Point(387, 5);
            this.lblIBAN.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIBAN.Name = "lblIBAN";
            this.lblIBAN.Size = new System.Drawing.Size(35, 13);
            this.lblIBAN.TabIndex = 1;
            this.lblIBAN.Text = "IBAN:";
            // 
            // lblBIC
            // 
            this.lblBIC.AutoSize = true;
            this.lblBIC.Location = new System.Drawing.Point(387, 24);
            this.lblBIC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBIC.Name = "lblBIC";
            this.lblBIC.Size = new System.Drawing.Size(27, 13);
            this.lblBIC.TabIndex = 1;
            this.lblBIC.Text = "BIC:";
            // 
            // lblCreditorId
            // 
            this.lblCreditorId.AutoSize = true;
            this.lblCreditorId.Location = new System.Drawing.Point(387, 43);
            this.lblCreditorId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCreditorId.Name = "lblCreditorId";
            this.lblCreditorId.Size = new System.Drawing.Size(60, 13);
            this.lblCreditorId.TabIndex = 1;
            this.lblCreditorId.Text = "Kreditor-ID:";
            // 
            // lblStrasse
            // 
            this.lblStrasse.AutoSize = true;
            this.lblStrasse.Location = new System.Drawing.Point(2, 24);
            this.lblStrasse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStrasse.Name = "lblStrasse";
            this.lblStrasse.Size = new System.Drawing.Size(41, 13);
            this.lblStrasse.TabIndex = 1;
            this.lblStrasse.Text = "Straße:";
            // 
            // lblLand
            // 
            this.lblLand.AutoSize = true;
            this.lblLand.Location = new System.Drawing.Point(2, 43);
            this.lblLand.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLand.Name = "lblLand";
            this.lblLand.Size = new System.Drawing.Size(72, 13);
            this.lblLand.TabIndex = 1;
            this.lblLand.Text = "Land/Plz/Ort:";
            // 
            // lblNameEEG
            // 
            this.lblNameEEG.AutoSize = true;
            this.lblNameEEG.Location = new System.Drawing.Point(2, 5);
            this.lblNameEEG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNameEEG.Name = "lblNameEEG";
            this.lblNameEEG.Size = new System.Drawing.Size(63, 13);
            this.lblNameEEG.TabIndex = 1;
            this.lblNameEEG.Text = "Name EEG:";
            // 
            // tbBIC
            // 
            this.tbBIC.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBIC.Location = new System.Drawing.Point(451, 24);
            this.tbBIC.Margin = new System.Windows.Forms.Padding(2);
            this.tbBIC.Name = "tbBIC";
            this.tbBIC.ReadOnly = true;
            this.tbBIC.Size = new System.Drawing.Size(179, 13);
            this.tbBIC.TabIndex = 0;
            // 
            // tbCreditorId
            // 
            this.tbCreditorId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCreditorId.Location = new System.Drawing.Point(451, 43);
            this.tbCreditorId.Margin = new System.Windows.Forms.Padding(2);
            this.tbCreditorId.Name = "tbCreditorId";
            this.tbCreditorId.ReadOnly = true;
            this.tbCreditorId.Size = new System.Drawing.Size(179, 13);
            this.tbCreditorId.TabIndex = 0;
            // 
            // tbCurrency
            // 
            this.tbCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCurrency.Location = new System.Drawing.Point(690, 5);
            this.tbCurrency.Margin = new System.Windows.Forms.Padding(2);
            this.tbCurrency.Name = "tbCurrency";
            this.tbCurrency.ReadOnly = true;
            this.tbCurrency.Size = new System.Drawing.Size(42, 13);
            this.tbCurrency.TabIndex = 0;
            // 
            // tbIBAN
            // 
            this.tbIBAN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbIBAN.Location = new System.Drawing.Point(451, 5);
            this.tbIBAN.Margin = new System.Windows.Forms.Padding(2);
            this.tbIBAN.Name = "tbIBAN";
            this.tbIBAN.ReadOnly = true;
            this.tbIBAN.Size = new System.Drawing.Size(179, 13);
            this.tbIBAN.TabIndex = 0;
            // 
            // tbCountry
            // 
            this.tbCountry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCountry.Location = new System.Drawing.Point(85, 43);
            this.tbCountry.Margin = new System.Windows.Forms.Padding(2);
            this.tbCountry.Name = "tbCountry";
            this.tbCountry.ReadOnly = true;
            this.tbCountry.Size = new System.Drawing.Size(24, 13);
            this.tbCountry.TabIndex = 0;
            // 
            // tbAnschriftZeile2
            // 
            this.tbAnschriftZeile2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbAnschriftZeile2.Location = new System.Drawing.Point(113, 43);
            this.tbAnschriftZeile2.Margin = new System.Windows.Forms.Padding(2);
            this.tbAnschriftZeile2.Name = "tbAnschriftZeile2";
            this.tbAnschriftZeile2.ReadOnly = true;
            this.tbAnschriftZeile2.Size = new System.Drawing.Size(270, 13);
            this.tbAnschriftZeile2.TabIndex = 0;
            // 
            // tbAnschriftZeile1
            // 
            this.tbAnschriftZeile1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbAnschriftZeile1.Location = new System.Drawing.Point(85, 24);
            this.tbAnschriftZeile1.Margin = new System.Windows.Forms.Padding(2);
            this.tbAnschriftZeile1.Name = "tbAnschriftZeile1";
            this.tbAnschriftZeile1.ReadOnly = true;
            this.tbAnschriftZeile1.Size = new System.Drawing.Size(298, 13);
            this.tbAnschriftZeile1.TabIndex = 0;
            // 
            // tbNameEEG
            // 
            this.tbNameEEG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbNameEEG.Location = new System.Drawing.Point(85, 5);
            this.tbNameEEG.Margin = new System.Windows.Forms.Padding(2);
            this.tbNameEEG.Name = "tbNameEEG";
            this.tbNameEEG.ReadOnly = true;
            this.tbNameEEG.Size = new System.Drawing.Size(298, 13);
            this.tbNameEEG.TabIndex = 0;
            // 
            // EEGFakturaTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EEGFakturaTool";
            this.Text = "EEG-Faktura-Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EEGFakturaTool_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eEGFakturaPaymentItemBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importEegFakturaToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource eEGFakturaPaymentItemBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn empfaengerKontoIBANDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn erstellerIBANDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripTextBox tstb_Ueberweisungsdatum;
        private System.Windows.Forms.ToolStripMenuItem sepaDateiErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox tstb_Einzungsdatum;
        private System.Windows.Forms.ToolStripMenuItem sepaMandateImportierenToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBIC;
        private System.Windows.Forms.Label lblNameEEG;
        private System.Windows.Forms.TextBox tbNameEEG;
        private System.Windows.Forms.Label lblIBAN;
        private System.Windows.Forms.Label lblCreditorId;
        private System.Windows.Forms.Label lblStrasse;
        private System.Windows.Forms.Label lblLand;
        private System.Windows.Forms.TextBox tbBIC;
        private System.Windows.Forms.TextBox tbCreditorId;
        private System.Windows.Forms.TextBox tbIBAN;
        private System.Windows.Forms.TextBox tbCountry;
        private System.Windows.Forms.TextBox tbAnschriftZeile2;
        private System.Windows.Forms.TextBox tbAnschriftZeile1;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.TextBox tbCurrency;
        private System.Windows.Forms.ToolStripMenuItem einzungsdatumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem überweisungsdatumToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbxBelegeZusammenfassen;
        private System.Windows.Forms.ToolStripMenuItem sEPADatenträgerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseApplicationToolStripMenuItem;
    }
}

