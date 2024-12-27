namespace DatabaseMigration
{
    public partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Déclaration des Labels
        private System.Windows.Forms.Label lblOracleHost;
        private System.Windows.Forms.Label lblOraclePort;
        private System.Windows.Forms.Label lblOracleService;
        private System.Windows.Forms.Label lblOracleUser;
        private System.Windows.Forms.Label lblOraclePassword;
        private System.Windows.Forms.Label lblPgHost;
        private System.Windows.Forms.Label lblPgPort;
        private System.Windows.Forms.Label lblPgDatabase;
        private System.Windows.Forms.Label lblPgUser;
        private System.Windows.Forms.Label lblPgPassword;

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
            
            // Création des GroupBox
            this.grpOracle = new System.Windows.Forms.GroupBox();
            this.grpPostgres = new System.Windows.Forms.GroupBox();
            this.grpOracleTables = new System.Windows.Forms.GroupBox();
            this.grpPgTables = new System.Windows.Forms.GroupBox();
            
            // Création des TextBox
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
            
            // Création des Labels
            this.lblOracleHost = new System.Windows.Forms.Label();
            this.lblOraclePort = new System.Windows.Forms.Label();
            this.lblOracleService = new System.Windows.Forms.Label();
            this.lblOracleUser = new System.Windows.Forms.Label();
            this.lblOraclePassword = new System.Windows.Forms.Label();
            
            this.lblPgHost = new System.Windows.Forms.Label();
            this.lblPgPort = new System.Windows.Forms.Label();
            this.lblPgDatabase = new System.Windows.Forms.Label();
            this.lblPgUser = new System.Windows.Forms.Label();
            this.lblPgPassword = new System.Windows.Forms.Label();
            
            // Création des boutons
            this.btnTestOracle = new System.Windows.Forms.Button();
            this.btnTestPg = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.btnMigrate = new System.Windows.Forms.Button();
            
            // Configuration des Labels Oracle
            this.lblOracleHost.Text = "Hôte:";
            this.lblOracleHost.Location = new System.Drawing.Point(20, 33);
            this.lblOracleHost.Size = new System.Drawing.Size(80, 20);

            this.lblOraclePort.Text = "Port:";
            this.lblOraclePort.Location = new System.Drawing.Point(20, 73);
            this.lblOraclePort.Size = new System.Drawing.Size(80, 20);

            this.lblOracleService.Text = "Service:";
            this.lblOracleService.Location = new System.Drawing.Point(20, 113);
            this.lblOracleService.Size = new System.Drawing.Size(80, 20);

            this.lblOracleUser.Text = "Utilisateur:";
            this.lblOracleUser.Location = new System.Drawing.Point(20, 153);
            this.lblOracleUser.Size = new System.Drawing.Size(80, 20);

            this.lblOraclePassword.Text = "Mot de passe:";
            this.lblOraclePassword.Location = new System.Drawing.Point(20, 193);
            this.lblOraclePassword.Size = new System.Drawing.Size(80, 20);

            // Configuration des Labels PostgreSQL
            this.lblPgHost.Text = "Hôte:";
            this.lblPgHost.Location = new System.Drawing.Point(20, 33);
            this.lblPgHost.Size = new System.Drawing.Size(80, 20);

            this.lblPgPort.Text = "Port:";
            this.lblPgPort.Location = new System.Drawing.Point(20, 73);
            this.lblPgPort.Size = new System.Drawing.Size(80, 20);

            this.lblPgDatabase.Text = "Base:";
            this.lblPgDatabase.Location = new System.Drawing.Point(20, 113);
            this.lblPgDatabase.Size = new System.Drawing.Size(80, 20);

            this.lblPgUser.Text = "Utilisateur:";
            this.lblPgUser.Location = new System.Drawing.Point(20, 153);
            this.lblPgUser.Size = new System.Drawing.Size(80, 20);

            this.lblPgPassword.Text = "Mot de passe:";
            this.lblPgPassword.Location = new System.Drawing.Point(20, 193);
            this.lblPgPassword.Size = new System.Drawing.Size(80, 20);

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

            // Configuration des TextBox Oracle
            this.txtOracleHost.Location = new System.Drawing.Point(110, 30);
            this.txtOracleHost.Size = new System.Drawing.Size(200, 26);
            this.txtOraclePort.Location = new System.Drawing.Point(110, 70);
            this.txtOraclePort.Size = new System.Drawing.Size(200, 26);
            this.txtOracleService.Location = new System.Drawing.Point(110, 110);
            this.txtOracleService.Size = new System.Drawing.Size(200, 26);
            this.txtOracleUser.Location = new System.Drawing.Point(110, 150);
            this.txtOracleUser.Size = new System.Drawing.Size(200, 26);
            this.txtOraclePassword.Location = new System.Drawing.Point(110, 190);
            this.txtOraclePassword.Size = new System.Drawing.Size(200, 26);
            this.txtOraclePassword.PasswordChar = '*';

            // Configuration des TextBox PostgreSQL
            this.txtPgHost.Location = new System.Drawing.Point(110, 30);
            this.txtPgHost.Size = new System.Drawing.Size(200, 26);
            this.txtPgPort.Location = new System.Drawing.Point(110, 70);
            this.txtPgPort.Size = new System.Drawing.Size(200, 26);
            this.txtPgDatabase.Location = new System.Drawing.Point(110, 110);
            this.txtPgDatabase.Size = new System.Drawing.Size(200, 26);
            this.txtPgUser.Location = new System.Drawing.Point(110, 150);
            this.txtPgUser.Size = new System.Drawing.Size(200, 26);
            this.txtPgPassword.Location = new System.Drawing.Point(110, 190);
            this.txtPgPassword.Size = new System.Drawing.Size(200, 26);
            this.txtPgPassword.PasswordChar = '*';

            // Configuration des boutons
            this.btnTestOracle.Text = "Tester Oracle";
            this.btnTestOracle.Location = new System.Drawing.Point(110, 230);
            this.btnTestOracle.Size = new System.Drawing.Size(150, 35);

            this.btnTestPg.Text = "Tester PostgreSQL";
            this.btnTestPg.Location = new System.Drawing.Point(110, 230);
            this.btnTestPg.Size = new System.Drawing.Size(150, 35);

            this.btnSave.Text = "Sauvegarder";
            this.btnSave.Location = new System.Drawing.Point(290, 370);
            this.btnSave.Size = new System.Drawing.Size(150, 35);

            // Ajout des contrôles aux GroupBox
            this.grpOracle.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblOracleHost, this.txtOracleHost,
                this.lblOraclePort, this.txtOraclePort,
                this.lblOracleService, this.txtOracleService,
                this.lblOracleUser, this.txtOracleUser,
                this.lblOraclePassword, this.txtOraclePassword,
                this.btnTestOracle
            });

            this.grpPostgres.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblPgHost, this.txtPgHost,
                this.lblPgPort, this.txtPgPort,
                this.lblPgDatabase, this.txtPgDatabase,
                this.lblPgUser, this.txtPgUser,
                this.lblPgPassword, this.txtPgPassword,
                this.btnTestPg
            });

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

            // Configuration finale
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
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