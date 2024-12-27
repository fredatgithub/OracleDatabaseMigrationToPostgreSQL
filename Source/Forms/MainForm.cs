using DatabaseMigration.Forms;
using System;
using System.Windows.Forms;

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
      Text = "Migration Oracle vers PostgreSQL";
      Size = new System.Drawing.Size(800, 600);

      // Création du TabControl
      tabControl = new TabControl
      {
        Dock = DockStyle.Fill
      };

      Controls.Add(tabControl);
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

      // Ajout des contrôles à l'onglet
      tabConnexion.Controls.Add(btnTestConnections);

      // Ajout de l'onglet au TabControl
      tabControl.TabPages.Add(tabConnexion);
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