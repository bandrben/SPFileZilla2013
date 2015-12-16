using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPFileZilla2013
{
    public partial class FormAbout : Form
    {

        public FormAbout()
        {
            InitializeComponent();
            btnOk.Focus();
            textBox1.Select(0, 0);
            pbFileZilla.Click += new EventHandler(pbFileZilla_Click);

            lblVersionNumber.Text = BandR.Consts.VERSION;
        }

        void pbFileZilla_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://filezilla-project.org/");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
