using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FlexiReg
{
    public partial class Pass : Form
    {
        public Pass()
        {
            InitializeComponent();
            LeerPass();
        }

        String dirPass = Directory.GetCurrentDirectory().ToString() + "\\Pass.xml";
        XmlDocument doc = new XmlDocument();

        string text = null;

        void LeerPass()
        {
            doc.Load(dirPass);
            
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                text = node.InnerText; //or loop through its children as well
                textBox2.Text = text;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.Show();
        }

        void GuardarPass()
        {
            XmlWriter writer = XmlWriter.Create(dirPass);
            writer.WriteStartDocument();
            writer.WriteStartElement("Contra");

            //root2
            writer.WriteElementString("Pass", txtnueva.Text.ToString());

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            writer.Close();
        }

        private void Pass_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtnueva.Text == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("¡Necesita rellenar todos los campos!", "¡No Válido!");
            }
            else
            {
                if (textBox2.Text == textBox1.Text)
                {
                    //
                    GuardarPass();
                    MessageBox.Show("¡Se guardo la contraseña!", "¡Exito!");
                    textBox1.Text = "";
                    txtnueva.Text = "";
                    LeerPass();
                }
                else
                {
                    MessageBox.Show("¡Las contraseñas no coinciden!", "¡No Válido!");
                }
            }
            
        }
    }
}
