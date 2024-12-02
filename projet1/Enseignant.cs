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
    public partial class Enseignant : Form
    {
        public Enseignant()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            // Code pour gérer l'événement de clic du bouton
        }

        private void btnexit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmat_Click(object sender, EventArgs e)
        {
            Materiel cl = new Materiel(); cl.Show();
            this.Hide();
        }

        private void btndem_Click(object sender, EventArgs e)
        {
            Demande cl = new Demande(); cl.Show();
            this.Hide();
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Chat cl = new Chat(); cl.Show();
            this.Hide();
        }

        private void btnRe_Click(object sender, EventArgs e)
        {
            Form1 cl = new Form1(); cl.Show();
            this.Hide();
        }
    }
}
