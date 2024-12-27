using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using System.Configuration;
using DatabaseMigration.Forms;

namespace DatabaseMigration
{
  public partial class MainForm : Form
  {
    private TabControl tabControl;
    private TabPage tabConnexion;
    
    public MainForm()
    {
      InitializeComponent();
      SetupTabControl();
    }

    private void InitializeComponent()
    {
      this.Text = "Migration Oracle vers PostgreSQL";
      this.Size = new System.Drawing.Size(800, 600);

      // Création du TabControl
      tabControl = new TabControl
      {
        Dock = DockStyle.Fill
      };

      this.Controls.Add(tabControl);
    }

    private void SetupTabControl()
    {
      // Création de l'onglet Connexion
      tabConnexion = new TabPage
      {
        Text = "Connexion",
        Padding = new Padding(10)
      };

      // Ajout du bouton de test de connexion
      Button btnTestConnections = new Button
      {
        Text = "Tester les connexions",
        Location = new System.Drawing.Point(20, 20),
        Size = new System.Drawing.Size(150, 30)
      };
      btnTestConnections.Click += BtnTestConnections_Click;

      Controls.Add(btnTestConnections);
    }

    private void BtnTestConnections_Click(object sender, EventArgs e)
    {
      var testForm = new ConnectionTestForm();
      testForm.ShowDialog();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {

    }
  }
}