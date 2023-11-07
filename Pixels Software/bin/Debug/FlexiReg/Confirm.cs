using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlexiReg
{
    public partial class Confirm : Form
    {
        public String contraseña { get; set; }
        public Confirm(String sms, String titulo)
        {
            InitializeComponent();
            label1.Text = sms;
            this.Text = titulo;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            contraseña = txtconfirm.Text;
        }
    }
}
