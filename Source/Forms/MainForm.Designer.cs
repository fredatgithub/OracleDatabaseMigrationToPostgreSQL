namespace DatabaseMigration
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public System.Drawing.Font defaultFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

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

        private System.Windows.Forms.TextBox CreateLabelAndTextBox(string labelText, System.Windows.Forms.TextBox textBox, int x, int y, System.Windows.Forms.GroupBox parent)
        {
            if (textBox == null)
                textBox = new System.Windows.Forms.TextBox();

            textBox.Name = "txt" + labelText.Replace(":", "");
            textBox.Location = new System.Drawing.Point(x + 90, y);
            textBox.Size = new System.Drawing.Size(150, 26);
            textBox.Font = defaultFont;
            textBox.Parent = parent;

            var label = new System.Windows.Forms.Label
            {
                Text = labelText,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(85, 26),
                Name = "lbl" + labelText.Replace(":", ""),
                Parent = parent,
                Font = defaultFont
            };

            parent.Controls.Add(label);
            parent.Controls.Add(textBox);

            return textBox;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Initialisation des composants
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnexion = new System.Windows.Forms.TabPage();
            this.grpOracle = new System.Windows.Forms.GroupBox();
            this.grpPostgres = new System.Windows.Forms.GroupBox();
            
            // Initialisation des TextBox
            this.txtOracleHost = new System.Windows.Forms.TextBox();
            this.txtOraclePort = new System.Windows.Forms.TextBox();
            this.txtOracleService = new System.Windows.Forms.TextBox();
            this.txtOracleUser = new System.Windows.Forms.TextBox();
            this.txtOraclePassword = new System.Windows.Forms.TextBox();
            this.btnTestOracle = new System.Windows.Forms.Button();
            
            this.txtPgHost = new System.Windows.Forms.TextBox();
            this.txtPgPort = new System.Windows.Forms.TextBox();
            this.txtPgDatabase = new System.Windows.Forms.TextBox();
            this.txtPgUser = new System.Windows.Forms.TextBox();
            this.txtPgPassword = new System.Windows.Forms.TextBox();
            this.btnTestPg = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();

            this.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabConnexion.SuspendLayout();
            this.grpOracle.SuspendLayout();
            this.grpPostgres.SuspendLayout();

            // Configuration TabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabPages.Add(this.tabConnexion);
            this.tabControl.Font = defaultFont;

            // Configuration TabPage Connexion
            this.tabConnexion.Location = new System.Drawing.Point(4, 22);
            this.tabConnexion.Name = "tabConnexion";
            this.tabConnexion.Padding = new System.Windows.Forms.Padding(10);
            this.tabConnexion.Size = new System.Drawing.Size(792, 574);
            this.tabConnexion.Text = "Connexion";
            this.tabConnexion.UseVisualStyleBackColor = true;
            this.tabConnexion.Font = defaultFont;

            // Configuration GroupBox Oracle
            this.grpOracle.Text = "Connexion Oracle";
            this.grpOracle.Location = new System.Drawing.Point(10, 10);
            this.grpOracle.Size = new System.Drawing.Size(350, 350);
            this.grpOracle.Name = "grpOracle";
            this.grpOracle.Font = defaultFont;

            // Configuration GroupBox PostgreSQL
            this.grpPostgres.Text = "Connexion PostgreSQL";
            this.grpPostgres.Location = new System.Drawing.Point(370, 10);
            this.grpPostgres.Size = new System.Drawing.Size(350, 350);
            this.grpPostgres.Name = "grpPostgres";
            this.grpPostgres.Font = defaultFont;

            // Configuration des contrôles Oracle
            this.txtOracleHost = CreateLabelAndTextBox("Hôte:", this.txtOracleHost, 20, 30, this.grpOracle);
            this.txtOraclePort = CreateLabelAndTextBox("Port:", this.txtOraclePort, 20, 80, this.grpOracle);
            this.txtOracleService = CreateLabelAndTextBox("Service:", this.txtOracleService, 20, 130, this.grpOracle);
            this.txtOracleUser = CreateLabelAndTextBox("Utilisateur:", this.txtOracleUser, 20, 180, this.grpOracle);
            this.txtOraclePassword = CreateLabelAndTextBox("Mot de passe:", this.txtOraclePassword, 20, 230, this.grpOracle);
            
            this.txtOraclePassword.PasswordChar = '*';
            this.txtOraclePort.Text = "1521";

            this.btnTestOracle.Location = new System.Drawing.Point(90, 290);
            this.btnTestOracle.Size = new System.Drawing.Size(150, 35);
            this.btnTestOracle.Text = "Tester Oracle";
            this.btnTestOracle.Name = "btnTestOracle";
            this.btnTestOracle.Click += new System.EventHandler(this.BtnTestOracle_Click);
            this.btnTestOracle.Font = defaultFont;

            // Configuration des contrôles PostgreSQL
            this.txtPgHost = CreateLabelAndTextBox("Hôte:", this.txtPgHost, 20, 30, this.grpPostgres);
            this.txtPgPort = CreateLabelAndTextBox("Port:", this.txtPgPort, 20, 80, this.grpPostgres);
            this.txtPgDatabase = CreateLabelAndTextBox("Base:", this.txtPgDatabase, 20, 130, this.grpPostgres);
            this.txtPgUser = CreateLabelAndTextBox("Utilisateur:", this.txtPgUser, 20, 180, this.grpPostgres);
            this.txtPgPassword = CreateLabelAndTextBox("Mot de passe:", this.txtPgPassword, 20, 230, this.grpPostgres);
            
            this.txtPgPassword.PasswordChar = '*';

            this.btnTestPg.Location = new System.Drawing.Point(90, 290);
            this.btnTestPg.Size = new System.Drawing.Size(150, 35);
            this.btnTestPg.Text = "Tester PostgreSQL";
            this.btnTestPg.Name = "btnTestPg";
            this.btnTestPg.Click += new System.EventHandler(this.BtnTestPg_Click);
            this.btnTestPg.Font = defaultFont;

            // Configuration du bouton Sauvegarder
            this.btnSave.Location = new System.Drawing.Point(290, 370);
            this.btnSave.Size = new System.Drawing.Size(150, 35);
            this.btnSave.Text = "Sauvegarder";
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            this.btnSave.Font = defaultFont;

            // Ajout des contrôles aux groupes
            this.grpOracle.Controls.Add(this.btnTestOracle);
            this.grpPostgres.Controls.Add(this.btnTestPg);

            // Ajout des groupes et du bouton sauvegarder à l'onglet
            this.tabConnexion.Controls.AddRange(new System.Windows.Forms.Control[] { 
                this.grpOracle, 
                this.grpPostgres, 
                this.btnSave 
            });

            // Configuration TabPage Tables
            this.tabTables = new System.Windows.Forms.TabPage();
            this.grpOracleTables = new System.Windows.Forms.GroupBox();
            this.grpPgTables = new System.Windows.Forms.GroupBox();
            this.lstOracleTables = new System.Windows.Forms.ListBox();
            this.lstPgTables = new System.Windows.Forms.ListBox();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.progressMigration = new System.Windows.Forms.ProgressBar();

            this.tabTables.Location = new System.Drawing.Point(4, 22);
            this.tabTables.Name = "tabTables";
            this.tabTables.Padding = new System.Windows.Forms.Padding(10);
            this.tabTables.Size = new System.Drawing.Size(792, 574);
            this.tabTables.Text = "Tables";
            this.tabTables.UseVisualStyleBackColor = true;
            this.tabTables.Font = defaultFont;

            // Configuration GroupBox Oracle Tables
            this.grpOracleTables.Text = "Tables Oracle";
            this.grpOracleTables.Location = new System.Drawing.Point(10, 10);
            this.grpOracleTables.Size = new System.Drawing.Size(350, 400);
            this.grpOracleTables.Name = "grpOracleTables";
            this.grpOracleTables.Font = defaultFont;

            // Configuration GroupBox PostgreSQL Tables
            this.grpPgTables.Text = "Tables PostgreSQL";
            this.grpPgTables.Location = new System.Drawing.Point(370, 10);
            this.grpPgTables.Size = new System.Drawing.Size(350, 400);
            this.grpPgTables.Name = "grpPgTables";
            this.grpPgTables.Font = defaultFont;

            // Configuration ListBox Oracle
            this.lstOracleTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOracleTables.Name = "lstOracleTables";
            this.lstOracleTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstOracleTables.Font = defaultFont;

            // Configuration ListBox PostgreSQL
            this.lstPgTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPgTables.Name = "lstPgTables";
            this.lstPgTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstPgTables.Font = defaultFont;

            // Configuration du bouton Charger les tables
            this.btnLoadTables.Location = new System.Drawing.Point(10, 420);
            this.btnLoadTables.Size = new System.Drawing.Size(200, 35);
            this.btnLoadTables.Text = "Charger les tables";
            this.btnLoadTables.Name = "btnLoadTables";
            this.btnLoadTables.Font = defaultFont;
            this.btnLoadTables.Click += new System.EventHandler(this.BtnLoadTables_Click);

            // Configuration du bouton Migrer
            this.btnMigrate.Location = new System.Drawing.Point(520, 420);
            this.btnMigrate.Size = new System.Drawing.Size(200, 35);
            this.btnMigrate.Text = "Migrer les tables";
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Font = defaultFont;
            this.btnMigrate.Click += new System.EventHandler(this.BtnMigrate_Click);

            // Configuration de la barre de progression
            this.progressMigration.Location = new System.Drawing.Point(10, 470);
            this.progressMigration.Size = new System.Drawing.Size(710, 30);
            this.progressMigration.Name = "progressMigration";
            this.progressMigration.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            // Ajout des contrôles aux groupes
            this.grpOracleTables.Controls.Add(this.lstOracleTables);
            this.grpPgTables.Controls.Add(this.lstPgTables);

            // Ajout des contrôles à l'onglet Tables
            this.tabTables.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.grpOracleTables,
                this.grpPgTables,
                this.btnLoadTables,
                this.btnMigrate,
                this.progressMigration
            });

            // Ajouter l'onglet Tables au TabControl (après l'onglet Connexion)
            this.tabControl.TabPages.Add(this.tabTables);

            // Configuration du formulaire principal
            this.Controls.Add(this.tabControl);
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Name = "MainForm";
            this.Text = "Migration Oracle vers PostgreSQL";

            this.tabControl.ResumeLayout(false);
            this.tabConnexion.ResumeLayout(false);
            this.grpOracle.ResumeLayout(false);
            this.grpPostgres.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConnexion;
        private System.Windows.Forms.GroupBox grpOracle;
        private System.Windows.Forms.GroupBox grpPostgres;
        private System.Windows.Forms.TextBox txtOracleHost;
        private System.Windows.Forms.TextBox txtOraclePort;
        private System.Windows.Forms.TextBox txtOracleService;
        private System.Windows.Forms.TextBox txtOracleUser;
        private System.Windows.Forms.TextBox txtOraclePassword;
        private System.Windows.Forms.TextBox txtPgHost;
        private System.Windows.Forms.TextBox txtPgPort;
        private System.Windows.Forms.TextBox txtPgDatabase;
        private System.Windows.Forms.TextBox txtPgUser;
        private System.Windows.Forms.TextBox txtPgPassword;
        private System.Windows.Forms.Button btnTestOracle;
        private System.Windows.Forms.Button btnTestPg;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabPage tabTables;
        private System.Windows.Forms.ListBox lstOracleTables;
        private System.Windows.Forms.ListBox lstPgTables;
        private System.Windows.Forms.Button btnLoadTables;
        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.GroupBox grpOracleTables;
        private System.Windows.Forms.GroupBox grpPgTables;
        private System.Windows.Forms.ProgressBar progressMigration;
    }
} 