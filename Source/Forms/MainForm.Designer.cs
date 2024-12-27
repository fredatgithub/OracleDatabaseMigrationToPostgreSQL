namespace DatabaseMigration
{
    public partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Création des contrôles
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnexion = new System.Windows.Forms.TabPage();
            this.tabTables = new System.Windows.Forms.TabPage();
            
            this.grpOracle = new System.Windows.Forms.GroupBox();
            this.grpPostgres = new System.Windows.Forms.GroupBox();
            this.grpOracleTables = new System.Windows.Forms.GroupBox();
            this.grpPgTables = new System.Windows.Forms.GroupBox();
            
            this.txtOracleHost = new System.Windows.Forms.TextBox();
            this.txtOraclePort = new System.Windows.Forms.TextBox();
            this.txtOracleService = new System.Windows.Forms.TextBox();
            this.txtOracleUser = new System.Windows.Forms.TextBox();
            this.txtOraclePassword = new System.Windows.Forms.TextBox();
            
            this.txtPgHost = new System.Windows.Forms.TextBox();
            this.txtPgPort = new System.Windows.Forms.TextBox();
            this.txtPgDatabase = new System.Windows.Forms.TextBox();
            this.txtPgUser = new System.Windows.Forms.TextBox();
            this.txtPgPassword = new System.Windows.Forms.TextBox();
            
            this.btnTestOracle = new System.Windows.Forms.Button();
            this.btnTestPg = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.btnMigrate = new System.Windows.Forms.Button();
            
            this.lstOracleTables = new System.Windows.Forms.ListBox();
            this.lstPgTables = new System.Windows.Forms.ListBox();
            this.progressMigration = new System.Windows.Forms.ProgressBar();

            // Configuration TabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(900, 650);

            // Configuration des onglets
            this.tabConnexion.Text = "Connexion";
            this.tabTables.Text = "Tables";

            // Configuration des GroupBox
            this.grpOracle.Text = "Connexion Oracle";
            this.grpOracle.Location = new System.Drawing.Point(10, 10);
            this.grpOracle.Size = new System.Drawing.Size(350, 350);

            this.grpPostgres.Text = "Connexion PostgreSQL";
            this.grpPostgres.Location = new System.Drawing.Point(370, 10);
            this.grpPostgres.Size = new System.Drawing.Size(350, 350);

            // Configuration des boutons
            this.btnTestOracle.Text = "Tester Oracle";
            this.btnTestOracle.Location = new System.Drawing.Point(90, 290);
            this.btnTestOracle.Size = new System.Drawing.Size(150, 35);
            this.btnTestOracle.Click += new System.EventHandler(this.BtnTestOracle_Click);

            this.btnTestPg.Text = "Tester PostgreSQL";
            this.btnTestPg.Location = new System.Drawing.Point(90, 290);
            this.btnTestPg.Size = new System.Drawing.Size(150, 35);
            this.btnTestPg.Click += new System.EventHandler(this.BtnTestPg_Click);

            this.btnSave.Text = "Sauvegarder";
            this.btnSave.Location = new System.Drawing.Point(290, 370);
            this.btnSave.Size = new System.Drawing.Size(150, 35);
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // Structure du formulaire
            this.tabControl.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
                this.tabConnexion,
                this.tabTables
            });

            this.tabConnexion.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.grpOracle,
                this.grpPostgres,
                this.btnSave
            });

            // Configuration du formulaire
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Migration Oracle vers PostgreSQL";

            this.ResumeLayout(false);
        }

        #region Windows Form Designer generated code
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConnexion;
        private System.Windows.Forms.TabPage tabTables;
        private System.Windows.Forms.GroupBox grpOracle;
        private System.Windows.Forms.GroupBox grpPostgres;
        private System.Windows.Forms.GroupBox grpOracleTables;
        private System.Windows.Forms.GroupBox grpPgTables;
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
        private System.Windows.Forms.ListBox lstOracleTables;
        private System.Windows.Forms.ListBox lstPgTables;
        private System.Windows.Forms.Button btnLoadTables;
        private System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.ProgressBar progressMigration;
        #endregion
    }
} 