namespace DatabaseMigration
{
    partial class MainForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnexion = new System.Windows.Forms.TabPage();
            this.btnTestConnections = new System.Windows.Forms.Button();
            
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.TabIndex = 0;
            
            // 
            // tabConnexion
            // 
            this.tabConnexion.Location = new System.Drawing.Point(4, 22);
            this.tabConnexion.Name = "tabConnexion";
            this.tabConnexion.Padding = new System.Windows.Forms.Padding(10);
            this.tabConnexion.Size = new System.Drawing.Size(792, 574);
            this.tabConnexion.TabIndex = 0;
            this.tabConnexion.Text = "Connexion";
            this.tabConnexion.UseVisualStyleBackColor = true;
            
            // 
            // btnTestConnections
            // 
            this.btnTestConnections.Location = new System.Drawing.Point(20, 20);
            this.btnTestConnections.Name = "btnTestConnections";
            this.btnTestConnections.Size = new System.Drawing.Size(150, 30);
            this.btnTestConnections.TabIndex = 0;
            this.btnTestConnections.Text = "Tester les connexions";
            this.btnTestConnections.UseVisualStyleBackColor = true;
            this.btnTestConnections.Click += new System.EventHandler(this.BtnTestConnections_Click);
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Migration Oracle vers PostgreSQL";
            
            this.tabConnexion.Controls.Add(this.btnTestConnections);
            this.tabControl.Controls.Add(this.tabConnexion);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConnexion;
        private System.Windows.Forms.Button btnTestConnections;
    }
} 