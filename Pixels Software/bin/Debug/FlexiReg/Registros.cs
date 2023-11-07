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
    public partial class Registros : Form
    {
        public Registros()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            /*
             * 
             * 
             */
             
            try
            {
                Llenar();
                LeerPass();
            }
            catch (Exception)
            {
            }

        }

        String passObtenida = "";

        
        

        DataGridViewImageColumn img = new DataGridViewImageColumn();
        DataGridViewImageColumn img2 = new DataGridViewImageColumn();

        private void button1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Registros.ActiveForm.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        String dir = Directory.GetCurrentDirectory().ToString() + "\\datosCliente.xml";
        
        List<Cliente> items = new List<Cliente>();
        List<String> item2 = new List<String>();
        XmlDocument doc = new XmlDocument();
        
        void Llenar()
        {   try
            {

                
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Update();
                this.dataGridView1.Refresh();

                
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(dir);
                this.dataGridView1.DataSource = dataSet.Tables[0];

                img.HeaderText = "Modificar";
                img.Name = "mod";
                Image imageEdit = FlexiReg.Properties.Resources.modificar;
                img.Image = imageEdit;

                img2.HeaderText = "Eliminar";
                img2.Name = "elim";
                Image imageDelete = FlexiReg.Properties.Resources.eliminar;
                img2.Image = imageDelete;

                //btn.UseColumnTextForButtonValue = true;

                /**/
                dataGridView1.AllowUserToAddRows = false;

                Luego();
            }
            catch (Exception e)
            {

            }
            
        }

        void Luego()
        {
            dataGridView1.Columns.Insert(8,img);
            dataGridView1.Columns.Insert(9,img2);
        }

        String dir2 = Directory.GetCurrentDirectory().ToString() + "\\FlexiReg\\bin\\Debug\\datosCliente.xml";
        String dir3 = Directory.GetCurrentDirectory().ToString() + "\\pass.xml";

        String leerPass = "";

        void LeerPass()
        {
            doc.Load(dir3);
            string text = null;

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                text = node.InnerText; //or loop through its children as well
            }
            leerPass = text;
            
        }

        void Modificar(String nombre, String cedula, String descripcion, String monto, String archivo, String correo, int fila)
        {

            try
            {
                doc.Load(dir2);

                XmlNodeList listaClientes = doc.SelectNodes("Clientes/Cliente");
                XmlNode unCliente;

                unCliente = listaClientes.Item(fila);

                XmlNodeList elemList = doc.GetElementsByTagName("Cliente");
                if (unCliente.SelectSingleNode("Nombre").InnerText == item2.ElementAt(0))
                {
                    unCliente.SelectSingleNode("Nombre").InnerText = nombre;
                    unCliente.SelectSingleNode("Cedula").InnerText = cedula;
                    unCliente.SelectSingleNode("Descripcion").InnerText = descripcion;
                    unCliente.SelectSingleNode("Total").InnerText = monto;
                    unCliente.SelectSingleNode("Archivo").InnerText = archivo;
                    unCliente.SelectSingleNode("Correo").InnerText = correo;
                    doc.Save(dir2);
                }

            }
            catch (Exception)
            {
            }


        }

        void Eliminar(int fila, string nombre)
        {
            try
            {
                doc.Load(dir2);

                XmlNodeList listaClientes = doc.SelectNodes("Clientes/Cliente");
                XmlNode unCliente;

                unCliente = listaClientes.Item(fila);

                XmlNodeList elemList = doc.GetElementsByTagName("Cliente");

                if (unCliente.SelectSingleNode("Nombre").InnerText == item2.ElementAt(0))
                {
                    unCliente.ParentNode.RemoveChild(unCliente);
                    doc.Save(dir2);
                }

                Llenar();

            }
            catch (Exception)
            {
            }
        }

        public static String viejo_nombre = "";
        public string pass_recuperada = "";

       

        void Modificar_Fila(int fila)
        {
            try
            {
                if (fila >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[fila];
                    String nombre = row.Cells["Nombre"].Value.ToString();
                    String cedula = row.Cells["Cedula"].Value.ToString();
                    String descripcion = row.Cells["Descripcion"].Value.ToString();
                    String monto = row.Cells["Total"].Value.ToString();
                    String archivo = row.Cells["Archivo"].Value.ToString();
                    String correo = row.Cells["Correo"].Value.ToString();

                    using (Confirm conf = new Confirm("Nuevos Cambios:" +
                    "\n\nNombre: " + nombre + " " +
                    "\nCédula: " + cedula + " " +
                    "\nDescripción: " + descripcion + " " +
                    "\nMonto: " + monto + " " +
                    "\nArchivo: " + archivo + " " +
                    "\nCorreo: " + correo + "\n\nNota: La fecha y hora no se modifican.\n\nPara continuar, íngrese la contraseña.", "¿Confirmar Edición?"))
                    {
                        if (conf.ShowDialog() == DialogResult.OK)
                        {
                            pass_recuperada = conf.contraseña;
                            if (pass_recuperada == leerPass)
                            {
                                Modificar(nombre, cedula, descripcion, monto, archivo, correo, fila);
                                MessageBox.Show("Registro Editado!", "¡Exito!");
                                item2.Clear();
                            }
                            else
                            {
                                MessageBox.Show("¡Contraseña incorrecta!", "¡No válido!");
                                Modificar_Fila(fila);
                            }
                        }
                    }
                    


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(""+e.Message);
            }
            
        }
        
        void Eliminar_Fila(int fila)
        {
            try
            {
                if (fila >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[fila];
                    String nombre = row.Cells["Nombre"].Value.ToString();
                    String cedula = row.Cells["Cedula"].Value.ToString();
                    String descripcion = row.Cells["Descripcion"].Value.ToString();
                    String monto = row.Cells["Total"].Value.ToString();
                    String archivo = row.Cells["Archivo"].Value.ToString();
                    String correo = row.Cells["Correo"].Value.ToString();

                    using (Confirm conf = new Confirm("Se eliminará:" +
                    "\n\nNombre: " + nombre + " " +
                    "\nCédula: " + cedula + " " +
                    "\nDescripción: " + descripcion + " " +
                    "\nMonto: " + monto + " " +
                    "\nArchivo: " + archivo + " " +
                    "\nCorreo: " + correo + "" +
                    "\n\nNota: No se podrá recuperar el registro.\n\nPara continuar, íngrese la contraseña.", "¿Confirmar Eliminación?"))
                    {
                        if (conf.ShowDialog() == DialogResult.OK)
                        {
                            pass_recuperada = conf.contraseña;
                            if (pass_recuperada == leerPass)
                            {
                                Eliminar(fila, nombre);
                                MessageBox.Show("Registro Eliminado!", "¡Exito!");
                                item2.Clear();
                                dataGridView1.Rows.RemoveAt(fila);
                            }
                            else
                            {
                                MessageBox.Show("¡Contraseña incorrecta!", "¡No válido!");
                                Eliminar_Fila(fila);
                            }
                        }
                    }


                    //DialogResult dr = MessageBox.Show("Se eliminará:" +
                    //"\n\nNombre: " + nombre + " " +
                    //"\nCédula: " + cedula + " " +
                    //"\nDescripción: " + descripcion + " " +
                    //"\nMonto: " + monto + " " +
                    //"\nArchivo: " + archivo + " " +
                    //"\nCorreo: " + correo,
                    //"¿Está seguro que desea eliminar el siguiente registro?",
                    //MessageBoxButtons.YesNo,
                    //MessageBoxIcon.Question);

                    //if (dr == DialogResult.Yes)
                    //{
                    //    // Do something
                    //    Eliminar(fila, nombre);
                    //    DialogResult dr2 = MessageBox.Show("Registro Eliminado!", "¡Exito!",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Asterisk);
                    //    item2.Clear();
                    //    dataGridView1.Rows.RemoveAt(fila);

                    //}


                }
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewImageColumn &&
                e.RowIndex >= 0 && senderGrid.Columns[e.ColumnIndex].HeaderText == "Modificar")
            {
                Modificar_Fila(e.RowIndex);
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewImageColumn &&
                e.RowIndex >= 0 && senderGrid.Columns[e.ColumnIndex].HeaderText == "Eliminar")
            {
                Eliminar_Fila(e.RowIndex);
            }

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var dataTable = (DataTable)dataGridView1.DataSource;
            var dataView = dataTable.DefaultView;
            string seleccion = comboBox1.SelectedItem.ToString();
            dataView.RowFilter = string.Format(""+seleccion+" like '{0}%'", textBox1.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                String nombre = row.Cells["Nombre"].Value.ToString();
                item2.Add(nombre);
                
            }
        }


    }
}
