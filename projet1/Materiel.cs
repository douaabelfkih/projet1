using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projet1
{
    public partial class Materiel : Form
    {
        public Materiel()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEns_Click(object sender, EventArgs e)
        {
            Enseignant cl = new Enseignant(); cl.Show();
            this.Hide();
        }

        private void btnDEm_Click(object sender, EventArgs e)
        {
            Demande cl = new Demande(); cl.Show();
            this.Hide();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Chat cl = new Chat(); cl.Show();
            this.Hide();
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            Form1 cl = new Form1(); cl.Show();
            this.Hide();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
