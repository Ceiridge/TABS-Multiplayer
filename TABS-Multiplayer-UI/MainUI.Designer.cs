namespace TABS_Multiplayer_UI
{
    partial class MainUI
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
            this.components = new System.ComponentModel.Container();
            this.waitPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabCtr = new System.Windows.Forms.TabControl();
            this.connectPage = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.managePage = new System.Windows.Forms.TabPage();
            this.tcpWaiter = new System.Windows.Forms.Timer(this.components);
            this.waitPanel.SuspendLayout();
            this.tabCtr.SuspendLayout();
            this.connectPage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.tabCtr.Size = new System.Drawing.Size(702, 282);
            this.tabCtr.TabIndex = 1;
            // 
            // connectPage
            // 
            this.connectPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.connectPage.Controls.Add(this.groupBox2);
            this.connectPage.Controls.Add(this.groupBox1);
            this.connectPage.Location = new System.Drawing.Point(4, 30);
            this.connectPage.Name = "connectPage";
            this.connectPage.Padding = new System.Windows.Forms.Padding(3);
            this.connectPage.Size = new System.Drawing.Size(694, 248);
            this.connectPage.TabIndex = 0;
            this.connectPage.Text = "Connect";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(8, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(678, 122);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Host Server";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(56, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(563, 45);
            this.button2.TabIndex = 1;
            this.button2.Text = "Connect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(6, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(666, 29);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Server IP";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 95);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Host Server";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(6, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(666, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "Host";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.hostBtn_Click);
            // 
            // managePage
            // 
            this.managePage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.managePage.Location = new System.Drawing.Point(4, 30);
            this.managePage.Name = "managePage";
            this.managePage.Padding = new System.Windows.Forms.Padding(3);
            this.managePage.Size = new System.Drawing.Size(694, 248);
            this.managePage.TabIndex = 1;
            this.managePage.Text = "Manage";
            // 
            // tcpWaiter
            // 
            this.tcpWaiter.Enabled = true;
            this.tcpWaiter.Tick += new System.EventHandler(this.tcpWaiter_Tick);
            // 
            // MainUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.ClientSize = new System.Drawing.Size(702, 282);
            this.Controls.Add(this.tabCtr);
            this.Controls.Add(this.waitPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "MainUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TABS-Multiplayer UI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.waitPanel.ResumeLayout(false);
            this.waitPanel.PerformLayout();
            this.tabCtr.ResumeLayout(false);
            this.connectPage.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel waitPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabCtr;
        private System.Windows.Forms.TabPage connectPage;
        private System.Windows.Forms.TabPage managePage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer tcpWaiter;
    }
}

