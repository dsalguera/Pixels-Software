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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DateTime fechaHoy = DateTime.Now;
            fecha = "" + fechaHoy.Day + "/" + fechaHoy.Month + "/" + fechaHoy.Year;
            label4.Text = fecha;
        }

        String fecha = "", hora = "";

        private void btnRegistros_Click(object sender, EventArgs e)
        {
            new Registros().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            hora = ""+DateTime.Now.ToString("hh:mm:ss tt");
            label5.Text = hora;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Registros().Show();
            this.Hide();
        }

       
        String dir = Directory.GetCurrentDirectory().ToString() + "\\FlexiReg\\bin\\Debug\\datosCliente.xml";

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(dir))
            {
                Agregar();
            }
            else
            {
                MessageBox.Show("¡El registro no existe, se creará uno nuevo!","¡Aviso!");
                Guardar();
            }

            Limpiar();

        }

        XmlDocument doc = new XmlDocument();

        void Agregar()
        {
            doc.Load(dir);
            string strNamespace = doc.DocumentElement.NamespaceURI;
            XmlNode Cliente = doc.CreateNode(XmlNodeType.Element, "Cliente", null);

            XmlNode Nombre = doc.CreateNode(XmlNodeType.Element, "Nombre", strNamespace);
            Nombre.InnerText = txtnombre.Text;

            XmlNode Cedula = doc.CreateNode(XmlNodeType.Element, "Cedula", strNamespace);
            Cedula.InnerText = txtcedula.Text;

            XmlNode Desc = doc.CreateNode(XmlNodeType.Element, "Descripcion", strNamespace);
            Desc.InnerText = txtdesc.Text;

            XmlNode Direccion = doc.CreateNode(XmlNodeType.Element, "Direccion", strNamespace);
            Direccion.InnerText = textBox2.Text;
            
            XmlNode Telefono = doc.CreateNode(XmlNodeType.Element, "Telefono", strNamespace);
            Telefono.InnerText = textBox1.Text;

            XmlNode Monto = doc.CreateNode(XmlNodeType.Element, "Total", strNamespace);
            Monto.InnerText = txtTotal.Text;

            XmlNode Archivo = doc.CreateNode(XmlNodeType.Element, "Archivo", strNamespace);
            Archivo.InnerText = txtarchivo.Text;

            XmlNode Correo = doc.CreateNode(XmlNodeType.Element, "Correo", strNamespace);
            Correo.InnerText = txtcorreo.Text;
            
            //Se agrega el registro
            Cliente.AppendChild(Nombre);
            Cliente.AppendChild(Correo);
            Cliente.AppendChild(Telefono);

            Cliente.AppendChild(Cedula);
            
            Cliente.AppendChild(Direccion);
            Cliente.AppendChild(Desc);

            Cliente.AppendChild(Monto);
            Cliente.AppendChild(Archivo);
            
            // Se va a cola
            doc.DocumentElement.AppendChild(Cliente);
            // Se guarda
            doc.Save(dir);

            MessageBox.Show("¡Registro agregado!", "¡Exito!");

        }

        void Guardar()
         {

             XmlWriter writer = XmlWriter.Create(dir);
             writer.WriteStartDocument();
             writer.WriteStartElement("Clientes");

             writer.WriteStartElement("Cliente");

             writer.WriteElementString("Nombre", txtnombre.Text.ToString());
             writer.WriteElementString("Cedula", txtcedula.Text.ToString());
             writer.WriteElementString("Descripcion", txtdesc.Text.ToString());
             writer.WriteElementString("Direccion", txtdesc.Text.ToString());
             writer.WriteElementString("Telefono", txtdesc.Text.ToString());
             writer.WriteElementString("Total", txtTotal.Text.ToString());
             writer.WriteElementString("Archivo", txtarchivo.Text.ToString());
             writer.WriteElementString("Correo", txtcorreo.Text.ToString());
             writer.WriteElementString("Fecha", fecha);
             writer.WriteElementString("Hora", hora);

             writer.WriteEndElement();
             writer.WriteEndElement();

             writer.WriteEndDocument();
             writer.Flush();

             writer.Close();

             MessageBox.Show("Registro Guardado!", "Exito");
         }

        private void button1_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void txtarchivo_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Buscar archivo";
            //ofd.Filter = "MP3 files|*.mp3";
            ofd.InitialDirectory = @"C:\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtarchivo.Text = ""+ofd.FileName+"";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            new Pass().Show();
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtcedula_TextChanged(object sender, EventArgs e)
        {

        }

        void Limpiar()
        {
            txtnombre.Text = "";
            txtcedula.Text = "";
            txtdesc.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            txtTotal.Text = "";
            txtarchivo.Text = "";
            txtcorreo.Text = "";
        }
    }
    
}
