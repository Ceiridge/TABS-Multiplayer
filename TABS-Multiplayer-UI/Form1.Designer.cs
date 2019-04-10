namespace TABS_Multiplayer_UI
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.waitPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabCtr = new System.Windows.Forms.TabControl();
            this.connectPage = new System.Windows.Forms.TabPage();
            this.managePage = new System.Windows.Forms.TabPage();
            this.waitPanel.SuspendLayout();
            this.tabCtr.SuspendLayout();
            this.SuspendLayout();
            // 
            // waitPanel
            // 
            this.waitPanel.Controls.Add(this.label1);
            this.waitPanel.Location = new System.Drawing.Point(-50, 0);
            this.waitPanel.Name = "waitPanel";
            this.waitPanel.Size = new System.Drawing.Size(1, 1);
            this.waitPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "TABS not detected yet";
            // 
            // tabCtr
            // 
            this.tabCtr.Controls.Add(this.connectPage);
            this.tabCtr.Controls.Add(this.managePage);
            this.tabCtr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCtr.Location = new System.Drawing.Point(0, 0);
            this.tabCtr.Name = "tabCtr";
            this.tabCtr.SelectedIndex = 0;
            this.tabCtr.Size = new System.Drawing.Size(702, 373);
            this.tabCtr.TabIndex = 1;
            // 
            // connectPage
            // 
            this.connectPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.connectPage.Location = new System.Drawing.Point(4, 30);
            this.connectPage.Name = "connectPage";
            this.connectPage.Padding = new System.Windows.Forms.Padding(3);
            this.connectPage.Size = new System.Drawing.Size(694, 339);
            this.connectPage.TabIndex = 0;
            this.connectPage.Text = "Connect";
            // 
            // managePage
            // 
            this.managePage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.managePage.Location = new System.Drawing.Point(4, 30);
            this.managePage.Name = "managePage";
            this.managePage.Padding = new System.Windows.Forms.Padding(3);
            this.managePage.Size = new System.Drawing.Size(694, 339);
            this.managePage.TabIndex = 1;
            this.managePage.Text = "Manage";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.ClientSize = new System.Drawing.Size(702, 373);
            this.Controls.Add(this.tabCtr);
            this.Controls.Add(this.waitPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TABS-Multiplayer UI";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.waitPanel.ResumeLayout(false);
            this.waitPanel.PerformLayout();
            this.tabCtr.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel waitPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabCtr;
        private System.Windows.Forms.TabPage connectPage;
        private System.Windows.Forms.TabPage managePage;
    }
}

