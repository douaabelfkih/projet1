﻿using System;
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
    public partial class Demande : Form
    {
        public Demande()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmat_Click(object sender, EventArgs e)
        {
            Materiel cl = new Materiel(); cl.Show();
            this.Hide();
        }

        private void btnEns_Click(object sender, EventArgs e)
        {
            Enseignant cl = new Enseignant(); cl.Show();
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
    }
}
