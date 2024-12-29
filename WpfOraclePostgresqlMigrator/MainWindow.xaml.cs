using HelperLibrary;
using Npgsql;
using System;
using System.Configuration;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static HelperLibrary.DatabaseManager;
using Forms = System.Windows.Forms;

namespace WpfOraclePostgresqlMigrator
{
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const string WINDOW_SETTINGS_FILE = "window_settings.json";
    private bool isOracleConnectionValid = false;
    private bool isPgConnectionValid = false;

    public MainWindow()
    {
      InitializeComponent();
      LoadConnectionStrings();
      LoadCredentials();
      LoadWindowSettings();
      ValidateConnections();
      UpdateUIState();

      // Ajout des gestionnaires d'événements
      this.Closing += MainWindow_Closing;
      btnTestOracle.Click += BtnTestOracle_Click;
      btnTestPg.Click += BtnTestPg_Click;
      btnSave.Click += BtnSave_Click;
      btnLoadTables.Click += BtnLoadTables_Click;
      btnMigrate.Click += BtnMigrate_Click;

      // Nouveaux gestionnaires pour l'onglet Fonctions
      btnLoadFunctions.Click += BtnLoadFunctions_Click;
      btnMigrateFunctions.Click += BtnMigrateFunctions_Click;

      // Ajouter les gestionnaires pour l'onglet Users
      btnLoadUsers.Click += BtnLoadUsers_Click;
      btnMigrateUsers.Click += BtnMigrateUsers_Click;
    }

    private void LoadConnectionStrings()
    {
      try
      {
        var oracleConnStr = ConfigurationManager.ConnectionStrings["OracleConnection"]?.ConnectionString;
        var pgConnStr = ConfigurationManager.ConnectionStrings["PostgresConnection"]?.ConnectionString;

        if (!string.IsNullOrEmpty(oracleConnStr))
        {
          // Parse Oracle connection string
          var oracleBuilder = new OracleConnectionStringBuilder(oracleConnStr);
          txtOracleHost.Text = oracleBuilder.DataSource;
          // Ne pas écraser le port par défaut si aucune valeur n'est définie
          if (!string.IsNullOrEmpty(oracleBuilder.DataSource))
          {
            txtOraclePort.Text = "1521"; // Garder la valeur par défaut
          }
          txtOracleUser.Text = oracleBuilder.UserID;
          txtOraclePassword.Password = oracleBuilder.Password;
        }

        if (!string.IsNullOrEmpty(pgConnStr))
        {
          // Parse Postgres connection string
          var pgBuilder = new NpgsqlConnectionStringBuilder(pgConnStr);
          txtPgHost.Text = pgBuilder.Host;
          txtPgPort.Text = pgBuilder.Port.ToString();
          txtPgDatabase.Text = pgBuilder.Database;
          txtPgUser.Text = pgBuilder.Username;
          txtPgPassword.Password = pgBuilder.Password;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors du chargement des paramètres: {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ValidateConnections()
    {
      try
      {
        // Test connexion Oracle
        using (var conn = new OracleConnection(BuildOracleConnectionString()))
        {
          conn.Open();
          isOracleConnectionValid = true;
        }
      }
      catch
      {
        isOracleConnectionValid = false;
      }

      try
      {
        // Test connexion PostgreSQL
        using (var conn = new NpgsqlConnection(BuildPostgresConnectionString()))
        {
          conn.Open();
          isPgConnectionValid = true;
        }
      }
      catch
      {
        isPgConnectionValid = false;
      }
    }

    private void UpdateUIState()
    {
      bool areConnectionsValid = isOracleConnectionValid && isPgConnectionValid;

      // Colorer l'onglet Connexion
      tabConnexion.Background = areConnectionsValid ?
          new SolidColorBrush(Colors.LightGreen) :
          new SolidColorBrush(Colors.LightPink);

      // Activer/désactiver les onglets
      tabTables.IsEnabled = areConnectionsValid;
      tabFonctions.IsEnabled = areConnectionsValid;
      tabUsers.IsEnabled = areConnectionsValid;

      // Mettre à jour le texte de l'onglet Connexion pour indiquer le statut
      tabConnexion.Header = areConnectionsValid ?
          "Connexion OK" :
          "Connexion KO";
    }

    private void BtnTestOracle_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var connStr = BuildOracleConnectionString();
        using (var conn = new OracleConnection(connStr))
        {
          conn.Open();
          isOracleConnectionValid = true;
          MessageBox.Show("Connexion Oracle réussie!", "Succès",
              MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        isOracleConnectionValid = false;
        MessageBox.Show($"Erreur de connexion Oracle: {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        UpdateUIState();
      }
    }

    private void BtnTestPg_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        var connStr = BuildPostgresConnectionString();
        using (var conn = new NpgsqlConnection(connStr))
        {
          conn.Open();
          isPgConnectionValid = true;
          MessageBox.Show("Connexion PostgreSQL réussie!", "Succès",
              MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        isPgConnectionValid = false;
        MessageBox.Show($"Erreur de connexion PostgreSQL: {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        UpdateUIState();
      }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        SaveConnectionStrings();
        SaveCredentials();
        ValidateConnections();
        UpdateUIState();
        MessageBox.Show("Paramètres sauvegardés avec succès!", "Succès",
            MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors de la sauvegarde: {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private string BuildOracleConnectionString()
    {
      return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={txtOracleHost.Text})(PORT={txtOraclePort.Text}))" +
             $"(CONNECT_DATA=(SERVICE_NAME={txtOracleService.Text})));User Id={txtOracleUser.Text};Password={txtOraclePassword.Password}";
    }

    private string BuildPostgresConnectionString()
    {
      return $"Host={txtPgHost.Text};Port={txtPgPort.Text};Database={txtPgDatabase.Text};" +
             $"Username={txtPgUser.Text};Password={txtPgPassword.Password}";
    }

    private void SaveConnectionStrings()
    {
      var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      config.ConnectionStrings.ConnectionStrings["OracleConnection"].ConnectionString = BuildOracleConnectionString();
      config.ConnectionStrings.ConnectionStrings["PostgresConnection"].ConnectionString = BuildPostgresConnectionString();
      config.Save(ConfigurationSaveMode.Modified);
      ConfigurationManager.RefreshSection("connectionStrings");
    }

    private void SaveCredentials()
    {
      var credentials = new
      {
        Oracle = new
        {
          Host = txtOracleHost.Text,
          Port = txtOraclePort.Text,
          Service = txtOracleService.Text,
          User = txtOracleUser.Text,
          Password = txtOraclePassword.Password
        },
        Postgres = new
        {
          Host = txtPgHost.Text,
          Port = txtPgPort.Text,
          Database = txtPgDatabase.Text,
          User = txtPgUser.Text,
          Password = txtPgPassword.Password
        }
      };

      string json = JsonSerializer.Serialize(credentials);
      string encrypted = HelperLibrary.SecurityHelper.Encrypt(json);
      string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
      string credentialsPath = Path.Combine(appPath, "id.txt");
      File.WriteAllText(credentialsPath, encrypted);
    }

    private void LoadCredentials()
    {
      try
      {
        string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string credentialsPath = Path.Combine(appPath, "id.txt");

        if (File.Exists(credentialsPath))
        {
          string encrypted = File.ReadAllText(credentialsPath);
          string json = HelperLibrary.SecurityHelper.Decrypt(encrypted);
          var credentials = JsonSerializer.Deserialize<dynamic>(json);

          // Oracle
          txtOracleHost.Text = credentials.Oracle.Host.GetString();
          txtOraclePort.Text = credentials.Oracle.Port.GetString();
          txtOracleService.Text = credentials.Oracle.Service.GetString();
          txtOracleUser.Text = credentials.Oracle.User.GetString();
          txtOraclePassword.Password = credentials.Oracle.Password.GetString();

          // PostgreSQL
          txtPgHost.Text = credentials.Postgres.Host.GetString();
          txtPgPort.Text = credentials.Postgres.Port.GetString();
          txtPgDatabase.Text = credentials.Postgres.Database.GetString();
          txtPgUser.Text = credentials.Postgres.User.GetString();
          txtPgPassword.Password = credentials.Postgres.Password.GetString();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors du chargement des identifiants: {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void BtnLoadTables_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        lstOracleTables.Items.Clear();
        lstPgTables.Items.Clear();

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        // Charger les tables Oracle
        var oracleTables = dbManager.GetOracleTables();
        foreach (var table in oracleTables)
        {
          lstOracleTables.Items.Add(table);
        }

        // Charger les tables PostgreSQL
        var pgTables = dbManager.GetPostgresTables();
        foreach (var table in pgTables)
        {
          lstPgTables.Items.Add(table);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors du chargement des tables : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    private void BtnMigrate_Click(object sender, RoutedEventArgs e)
    {
      if (lstOracleTables.SelectedItems.Count == 0)
      {
        MessageBox.Show("Veuillez sélectionner au moins une table à migrer.",
            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        progressMigration.Minimum = 0;
        progressMigration.Maximum = lstOracleTables.SelectedItems.Count;
        progressMigration.Value = 0;

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        foreach (var table in lstOracleTables.SelectedItems)
        {
          var tableName = table.ToString();
          var schema = dbManager.ReadOracleData($"SELECT * FROM {tableName} WHERE 1=0");
          dbManager.CreatePostgresTable(tableName, schema);
          progressMigration.Value++;
        }

        MessageBox.Show("Migration terminée avec succès!", "Succès",
            MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors de la migration : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    private void LoadWindowSettings()
    {
      try
      {
        string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string settingsPath = Path.Combine(appPath, WINDOW_SETTINGS_FILE);

        if (File.Exists(settingsPath))
        {
          string json = File.ReadAllText(settingsPath);
          var settings = JsonSerializer.Deserialize<WindowSettings>(json);

          this.Left = settings.X;
          this.Top = settings.Y;
          this.Width = settings.Width;
          this.Height = settings.Height;

          // Vérifier que la fenêtre est visible sur un écran
          bool isVisible = false;
          foreach (var screen in Forms.Screen.AllScreens)
          {
            if (screen.WorkingArea.IntersectsWith(new Rectangle(
                (int)this.Left, (int)this.Top, (int)this.Width, (int)this.Height)))
            {
              isVisible = true;
              break;
            }
          }

          // Si la fenêtre n'est pas visible, la replacer au centre de l'écran
          if (!isVisible)
          {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
          }
        }
      }
      catch
      {
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      }
    }

    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveWindowSettings();
    }

    private void SaveWindowSettings()
    {
      try
      {
        var settings = new WindowSettings
        {
          X = this.Left,
          Y = this.Top,
          Width = this.Width,
          Height = this.Height
        };

        string json = JsonSerializer.Serialize(settings);
        string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string settingsPath = Path.Combine(appPath, WINDOW_SETTINGS_FILE);
        File.WriteAllText(settingsPath, json);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors de la sauvegarde des paramètres de fenêtre : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private class WindowSettings
    {
      public double X { get; set; }
      public double Y { get; set; }
      public double Width { get; set; }
      public double Height { get; set; }
    }

    private void BtnLoadFunctions_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        lstOracleFunctions.Items.Clear();
        lstPgFunctions.Items.Clear();

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        // Charger les fonctions Oracle
        var oracleFunctions = dbManager.GetOracleFunctions();
        foreach (var function in oracleFunctions)
        {
          lstOracleFunctions.Items.Add(function);
        }

        // Charger les fonctions PostgreSQL
        var pgFunctions = dbManager.GetPostgresFunctions();
        foreach (var function in pgFunctions)
        {
          lstPgFunctions.Items.Add(function);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors du chargement des fonctions : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    private void BtnMigrateFunctions_Click(object sender, RoutedEventArgs e)
    {
      if (lstOracleFunctions.SelectedItems.Count == 0)
      {
        MessageBox.Show("Veuillez sélectionner au moins une fonction à migrer.",
            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        progressFunctionMigration.Minimum = 0;
        progressFunctionMigration.Maximum = lstOracleFunctions.SelectedItems.Count;
        progressFunctionMigration.Value = 0;

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        foreach (var function in lstOracleFunctions.SelectedItems)
        {
          var functionName = function.ToString();
          dbManager.MigrateFunction(functionName);
          progressFunctionMigration.Value++;
        }

        MessageBox.Show("Migration des fonctions terminée avec succès!", "Succès",
            MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors de la migration des fonctions : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    private void BtnLoadUsers_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        lstOracleUsers.Items.Clear();
        lstPgUsers.Items.Clear();

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        // Charger les utilisateurs Oracle
        var oracleUsers = dbManager.GetOracleUsers();
        foreach (var user in oracleUsers)
        {
          lstOracleUsers.Items.Add(user);
        }

        // Charger les utilisateurs PostgreSQL
        var pgUsers = dbManager.GetPostgresUsers();
        foreach (var user in pgUsers)
        {
          lstPgUsers.Items.Add(user);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors du chargement des utilisateurs : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }

    private void BtnMigrateUsers_Click(object sender, RoutedEventArgs e)
    {
      if (lstOracleUsers.SelectedItems.Count == 0)
      {
        MessageBox.Show("Veuillez sélectionner au moins un utilisateur à migrer.",
            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        return;
      }

      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        progressUserMigration.Minimum = 0;
        progressUserMigration.Maximum = lstOracleUsers.SelectedItems.Count;
        progressUserMigration.Value = 0;

        var dbManager = new DatabaseManager(
            ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
            ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

        foreach (User user in lstOracleUsers.SelectedItems)
        {
          dbManager.MigrateUser(user);
          progressUserMigration.Value++;
        }

        MessageBox.Show("Migration des utilisateurs terminée avec succès!", "Succès",
            MessageBoxButton.OK, MessageBoxImage.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erreur lors de la migration des utilisateurs : {ex.Message}",
            "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        Mouse.OverrideCursor = null;
      }
    }
  }
}
