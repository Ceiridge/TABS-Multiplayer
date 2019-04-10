using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TABS_Multiplayer_UI
{
    public partial class ScreenshareForm : Form
    {
        public ScreenshareForm()
        {
            InitializeComponent();
        }

        protected override bool ShowWithoutActivation => true; // Don't steal the focus

        private void ScreenshareForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ScreenshareForm_Shown(object sender, EventArgs e)
        {
            this.Visible = false; // Hide this window for now
        }

        private void ScreenshareForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // The UI can only be closed by the main window
        }
    }
}
