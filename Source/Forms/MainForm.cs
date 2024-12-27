using DatabaseMigration.Forms;
using System;
using System.Windows.Forms;

namespace DatabaseMigration
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void BtnTestConnections_Click(object sender, EventArgs e)
    {
      var testForm = new ConnectionTestForm();
      testForm.ShowDialog();
    }
  }
}