using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using System.Runtime.InteropServices;
using iTextSharp.text.pdf;
using Xceed.Words.NET;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Pixels_Software
{
    public partial class Form1 : Form
    {
        public static Form2 frm2;

        public Form1()
        {
            InitializeComponent();
            InsertData.TextData = "";
            InsertDinero.TextData = "";
            confirmadoClick.TextData = "";

            frm2 = new Form2();

            txtagregarDolar.Text = "0";
            textBox29.Text = "0";
            textBox30.Text = "0";
            textBox43.Text = "0";
            txtinstalacion.Text = "0";
            txtimpresion.Text = "0";
            txtcorte.Text = "0";
            txtinstalacion2.Text = "0";
            txtimpresion2.Text = "0";
            txtcorte2.Text = "0";
            Baltura.Text = "0";
            Bbase.Text = "0";
            txtmonto.Text = "0";
            Sbase.Text = "0";
            Saltura.Text = "0";
            textBox28.Text = "0";
            this.Width = 584;
            this.Height = 712;
            MaximizeBox = false;

            dataGridView1.RowHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

            try
            {
                dbConnection = new MySqlConnection(MySQLConnectionString);

                Captura_Fecha();

                label69.Text = "Fecha: " + fecha + "";
                label27.Text = "Nota: Una vez eliminada la entrada \n" +
                    "no se podrá recuperar. \n\n" +
                    "Pd: Revise antes de eliminar.";

                dataGridView3.Columns.Add("montosColumna", "Montos (¢)");
                dataGridView3.Columns.Add("nombreColumna", "(Cant) Descripción");
                dataGridView3.Columns[1].Width = 150;

                //////////

                dataGridView1.Columns.Add("ID", "ID");
                dataGridView1.Columns.Add("Cantidad", "Cant.");
                dataGridView1.Columns.Add("Descripción", "Descripción");
                dataGridView1.Columns.Add("Precio", "Precio");
                dataGridView1.Columns.Add("Imagen", "Imagen");

                dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewImageColumn img = new DataGridViewImageColumn();
                img.ImageLayout = DataGridViewImageCellLayout.Zoom;
                img.Name = "Muestra";


                this.dataGridView1.Columns.Add(img);

                ///////////////////////////////////////
                
                dataGridView2.Columns.Add("Cantidad2", "Cant.");
                dataGridView2.Columns.Add("Descripción2", "Descripción");
                dataGridView2.Columns.Add("Precio2", "Precio");

                dataGridView2.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                DataGridViewImageColumn img2 = new DataGridViewImageColumn();
                img2.ImageLayout = DataGridViewImageCellLayout.Zoom;
                img2.Name = "Muestra2";
                img2.HeaderText = "Muestra";


                this.dataGridView2.Columns.Add(img2);

                ///////////////////////////////////////

                LeerVariables();

                LeerImp();
                LeerSuma();
                LeerDecimal();

                Llenar();
                PrimeraCarga();

                LeerSoloNombres();

                LeerClientes();

                LeerInstalacion(txtinstalacion);
                LeerDiseño(txtimpresion);
                LeerCorte(txtcorte);

                LeerInstalacion(txtinstalacion2);
                LeerDiseño(txtimpresion2);
                LeerCorte(txtcorte2);

                LeerInstalacionLata();
                LeerDiseñoMag();
                LeerDiseñoPapel();

                panel3.Visible = false;

                LeerDatosPapel();
                LeerMateriales();
                LeerDatosLata();

                pictureBox18.Visible = false;
                pictureBox13.Visible = false;

                LeerUltimoG();
                LeerUltimoGPVC();

                OcultarPubMateriales();
                OcultarPubPVC();

                ocultarLata();
                ocultarMagnetico();
                ocultarDolar();

                LeerImgPVC();
                LeerImgMat();

                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.Columns["Imagen"].Visible = false;

                //dataGridView2.Columns["ID"].Visible = false;
                //dataGridView2.Columns["Imagen"].Visible = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message + " " + ex.StackTrace);
            }

        }

        AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();

        public static class InsertData
        {
            public static string TextData { get; set; }
        }

        public static class InsertDinero
        {
            public static string TextData { get; set; }
        }

        public static class confirmadoClick
        {
            public static string TextData { get; set; }
        }


        void Validar_seleccion()
        {
            if (radioBasico.Checked == true)
            {
                grupoMinimo.Enabled = false;
                grupoBasico.Enabled = true;

            }
            else if (radioMinimo.Checked == true)
            {
                grupoMinimo.Enabled = true;
                grupoBasico.Enabled = false;

            }
        }

        void InsertarClick(String nombre, double precio)
        {
            string query = "update material set clicks = clicks + 1 where nombre = '"+ nombre + "' and precio = "+ precio + "";
  
            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message);
                dbConnection.Close();
            }

            //LeerMateriales();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioBasico.Checked = true;

            frm2.Show();
            frm2.Visible = false;

            LeerSoloNombres();
        }

        private void radioBasico_CheckedChanged(object sender, EventArgs e)
        {
            Validar_seleccion();
            Baltura.Text = "";
            Bbase.Text = "";
        }

        private void radioMinimo_CheckedChanged(object sender, EventArgs e)
        {
            Validar_seleccion();
            Sbase.Text = "";
            Saltura.Text = "";
        }

        /*
            comboMedida.Items.Add("mts");
            comboMedida.Items.Add("cm²");
            comboMedida.Items.Add("cm");
            comboMedida.Items.Add("mm"); 
        */

        void Calcular_Click()
        {
            if (txtinstalacion.Text == "" || txtimpresion.Text == "" || txtcorte.Text == "")
            {
                txtinstalacion.Text = "0";
                txtimpresion.Text = "0";
                txtcorte.Text = "0";
                if (radioBasico.Checked == true)
                {
                    if (comboTipo.SelectedIndex < 0 || comboCopias.Value == 0 ||
                        Baltura.Text.Equals("") || Bbase.Text.Equals(""))
                    {
                        MessageBox.Show("Verifique todos los campos!", "No valido");
                    }
                    else
                    {
                        String valor = comboCopias.Value.ToString();
                        Calculo_Total(Baltura.Text, Bbase.Text, comboTipo.SelectedItem.ToString(), Int32.Parse(valor));
                    }
                }
                else if (radioMinimo.Checked == true)
                {
                    if (comboTipo.SelectedIndex < 0 || comboCopias.Value == 0)
                    {
                        MessageBox.Show("Verifique todos los campos!", "No valido");
                    }
                    else
                    {
                        String valor = comboCopias.Value.ToString();
                        Calculo_Saldo(Sbase.Text, Saltura.Text, comboTipo.SelectedItem.ToString(), Int32.Parse(valor));
                    }
                }
            }
            else
            {
                if (radioBasico.Checked == true)
                {
                    if (comboTipo.SelectedIndex < 0 || comboCopias.Value == 0 ||
                        Baltura.Text.Equals("") || Bbase.Text.Equals(""))
                    {
                        MessageBox.Show("Verifique todos los campos!", "No valido");
                    }
                    else
                    {
                        String valor = comboCopias.Value.ToString();
                        Calculo_Total(Baltura.Text, Bbase.Text, comboTipo.SelectedItem.ToString(), Int32.Parse(valor));
                    }
                }
                else if (radioMinimo.Checked == true)
                {
                    if (comboTipo.SelectedIndex < 0 || comboCopias.Value == 0)
                    {
                        MessageBox.Show("Verifique todos los campos!", "No valido");
                    }
                    else
                    {
                        String valor = comboCopias.Value.ToString();
                        Calculo_Saldo(Sbase.Text, Saltura.Text, comboTipo.SelectedItem.ToString(), Int32.Parse(valor));
                    }
                }
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            Calcular_Click();
        }

        double constante_dolar = 0;
        double impuesto = 0;
        double formula = 0;
        double b = 0;
        double a = 0;

        void Calculo_Total(String basex, String altura, String tipo, int copias)
        {
            try
            {
                double valor = 0;
                double b = Double.Parse(basex);
                double a = Double.Parse(altura);

                string inst = txtinstalacion.Text.ToString();
                double instalac = double.Parse(inst);

                string impres = txtimpresion.Text.ToString();
                double impresion = double.Parse(impres);

                string cort = txtcorte.Text.ToString();
                double corte = double.Parse(cort);

                for (int i = 0; i < items.Count; i++)
                {
                    if (tipo == items.ElementAt(i).nombre)
                    {
                        valor = items.ElementAt(i).precio;

                        InsertarClick(items.ElementAt(i).nombre, items.ElementAt(i).precio);
                    }
                }

                /* 
                    Se multiplica por el 13%
                */

                if (checkBox1.Checked == true)
                {
                    formula = b * a * valor * constante_dolar * impuesto;

                    txtUnitario.Text = "¢" + String.Format("{0:n}", formula);

                    formula = b * a * valor * constante_dolar * impuesto * copias;

                    //txtTotal.Text = "¢" + String.Format("{0:"+decimales+"}", formula + instalac + impresion + corte);
                    RetornarDecimales(txtTotal, formula + instalac + impresion + corte);
                }
                else
                {
                    formula = b * a * valor * constante_dolar;

                    txtUnitario.Text = "¢" + String.Format("{0:n}", formula + instalac + impresion + corte);

                    formula = b * a * valor * constante_dolar * copias;

                    RetornarDecimales(txtTotal, formula + instalac + impresion + corte);
                    //txtTotal.Text = "¢" + String.Format("{0:"+decimales+"}", formula + instalac + impresion + corte);
                }

                textBox28.Text = "" + ultimoVal;

            }
            catch (Exception)
            {
            }

        }

        int formulaT = 0;

        void Calculo_Saldo(String basex, String altura, String tipo, int copias)
        {
            try
            {
                if (txtmonto.Text == "")
                {
                    MessageBox.Show("Error!", "Ingrese la cantidad");
                }
                else
                {
                    formulaT = Int32.Parse(txtmonto.Text);
                    //

                }

                double valor = 0;

                for (int i = 0; i < items.Count; i++)
                {
                    if (tipo == items.ElementAt(i).nombre)
                    {
                        valor = items.ElementAt(i).precio;
                    }
                }

                if (radioBase.Checked == true)
                {
                    if (Saltura.Text == "")
                    {
                        MessageBox.Show("Necesita ingresar una altura!");
                    }
                    else
                    {
                        a = Double.Parse(altura);
                        Buscar_base(a, valor, copias);
                    }

                }
                else if (radioAltura.Checked == true)
                {
                    if (Sbase.Text == "")
                    {
                        MessageBox.Show("Necesita ingresar una base!");
                    }
                    else
                    {
                        b = Double.Parse(basex);
                        Buscar_altura(b, valor, copias);
                    }
                }
                else if (radioAltura.Checked == false && radioBase.Checked == false)
                {
                    MessageBox.Show("Seleccione base o altura", "No valido!");
                }
            }
            catch (Exception)
            {
            }

        }

        void Buscar_base(double altura, double valor, int copias)
        {
            if (checkBox1.Checked == true)
            {
                Sbase.Text = "" + String.Format("{0:N2}", (formulaT / (altura * valor * constante_dolar * impuesto * copias)));
            }
            else
            {
                Sbase.Text = "" + String.Format("{0:N2}", (formulaT / (altura * valor * constante_dolar * copias)));
            }

        }

        void Buscar_altura(double basex, double valor, int copias)
        {
            if (checkBox1.Checked == true)
            {
                Saltura.Text = "" + String.Format("{0:N2}", (formulaT / (basex * valor * constante_dolar * impuesto * copias)));
            }
            else
            {
                Saltura.Text = "" + String.Format("{0:N2}", (formulaT / (basex * valor * constante_dolar * copias)));
            }

        }

        private void radioBase_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBase.Checked == true)
            {
                Sbase.Enabled = false;
            }
            else
            {
                Sbase.Enabled = true;
            }
        }

        private void radioAltura_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAltura.Checked == true)
            {
                Saltura.Enabled = false;
            }
            else
            {
                Saltura.Enabled = true;
            }
        }

        private void btnCalcularPVC_Click(object sender, EventArgs e)
        {
            CalcularPVC();
        }

        void CalcularPVC()
        {
            String valor = numericUpDown1.Value.ToString();

            if (txtinstalacion2.Text == "" || txtcorte2.Text == "" ||
                txtimpresion2.Text == "" || comboBox1.SelectedIndex == 0)
            {
                txtinstalacion2.Text = "0";
                txtcorte2.Text = "0";
                txtimpresion2.Text = "0";
                try
                {
                    Calculo_PVC(PVCbase.Text, PVCaltura.Text, comboBox1.SelectedItem.ToString(), Int32.Parse(valor));

                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    if (PVCbase.Text == "" || PVCaltura.Text == "" || comboBox1.SelectedIndex == 0)
                    {
                        MessageBox.Show("Necesita rellenar todos los campos!");
                    }
                    else
                    {
                        Calculo_PVC(PVCbase.Text, PVCaltura.Text, comboBox1.SelectedItem.ToString(), Int32.Parse(valor));
                    }


                }
                catch (Exception ex)
                {

                }
            }
        }

        void Calculo_PVC(String basex, String altura, String tipo, int copias)
        {
            try
            {
                double b = Double.Parse(basex);
                double a = Double.Parse(altura);

                double colon = 0;

                if (radioEntero.Checked == true)
                {
                    colon = 1.5;
                }
                else if (radioCompleto.Checked == true)
                {
                    colon = 1.8;
                }
                else if (radioGrande.Checked == true)
                {
                    colon = 2.0;
                }
                else if (radioModerado.Checked == true)
                {
                    colon = 2.5;
                }
                else if (radioIntermedio.Checked == true)
                {
                    colon = 3.5;
                }
                else if (radioMediano.Checked == true)
                {
                    colon = 4.0;
                }
                else if (radioNormal.Checked == true)
                {
                    colon = 6.0;
                }
                else if (radioPequeño.Checked == true)
                {
                    colon = 8.0;
                }

                /* 
                    Se multiplica por el 13%
                */

                string impres = txtimpresion2.Text.ToString();
                double impresion = double.Parse(impres);

                string cort = txtcorte2.Text.ToString();
                double corte = double.Parse(cort);

                double insta = Double.Parse(txtinstalacion2.Text);

                if (checkBox2.Checked == true)
                {
                    formula = b * a * colon * impuesto;

                    textBox3.Text = "¢" + String.Format("{0:n}", formula);

                    formula = b * a * colon * impuesto * copias;

                    RetornarDecimales(textBox4, formula + insta + impresion + corte);
                    //textBox4.Text = "¢" + String.Format("{0:n}", formula + insta + impresion + corte);
                }
                else
                {
                    formula = b * a * colon;

                    textBox3.Text = "¢" + String.Format("{0:n}", formula);

                    formula = b * a * colon * copias;

                    RetornarDecimales(textBox4, formula + insta + impresion + corte);
                    //textBox4.Text = "¢" + String.Format("{0:n}", formula + insta + impresion + corte);
                }

                textBox31.Text = "" + ultimoVal2;

                pictureBox.Refresh();
                Graphics g = pictureBox.CreateGraphics();
                Pintar(b, a, g);

            }
            catch (Exception)
            {
            }

        }


        void Pintar(double basex, double altura, Graphics g)
        {

            Pen pen = new Pen(Color.Black, 3);
            SolidBrush sb = new SolidBrush(Color.White);
            Font myFont = new System.Drawing.Font("Helvetica", 10, FontStyle.Regular);

            Brush myBrush = new SolidBrush(System.Drawing.Color.Black);

            //g.DrawString("H: " + basex + " cm , V: " + altura + " cm", myFont, myBrush, 10, 10);

            double mayor = 0;

            if (basex > altura)
            {
                mayor = basex;
            }
            else if (altura > basex)
            {
                mayor = altura;
            }
            else if (basex == altura)
            {
                mayor = basex;
            }

            if (mayor > 230)
            {
                float val = (float)230 / (float)mayor;
                g.ScaleTransform(val, val);
            }


            g.DrawRectangle(pen, 5, 5, (float)basex, (float)altura);
            g.FillRectangle(sb, 5, 5, (float)basex, (float)altura);
            lbl22.Text = "H: " + basex + " cm , V: " + altura + " cm";
        }

        void PintaCuadro(double basex, double altura, Graphics g)
        {
            Pen pen = new Pen(Color.Black, 3);
            SolidBrush sb = new SolidBrush(Color.White);
            Font myFont = new System.Drawing.Font("Helvetica", 10, FontStyle.Regular);

            Brush myBrush = new SolidBrush(System.Drawing.Color.Black);

            //g.DrawString("H: " + basex + " cm , V: " + altura + " cm", myFont, myBrush, 10, 10);

            double mayor = 0;

            if (basex > altura)
            {
                mayor = basex;
            }
            else if (altura > basex)
            {
                mayor = altura;
            }
            else if (basex == altura)
            {
                mayor = basex;
            }

            if (mayor > 230)
            {
                float val = (float)230 / (float)mayor;
                g.ScaleTransform(val, val);
            }


            g.DrawRectangle(pen, 15, 15, (float)basex, (float)altura);
            g.FillRectangle(sb, 15, 15, (float)basex, (float)altura);

        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Baltura.Text = "";
                Baltura.Text = "";
                txtUnitario.Text = "";
                txtTotal.Text = "";
                comboCopias.Value = 0;
                comboTipo.Text = "  -- Seleccione --";
            }
            catch (Exception)
            {
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                PVCbase.Text = "";
                PVCaltura.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                comboCopias.Value = 0;
                comboTipo.Text = "  -- Seleccione --";
            }
            catch (Exception)
            {
            }

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void comboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calcular_Click();
        }

        XmlDocument doc = new XmlDocument();

        void LeerVariables()
        {
            double dolar = 0;
            string query = "select * from variable;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    // Compra y Venta

                    constante_dolar = reader.GetInt32(4);

                    constante_compra = reader.GetInt32(5);

                    // Cargando Variables

                    txtdolar.Text = reader.GetInt32(4).ToString();

                    textBox41.Text = reader.GetInt32(5).ToString();

                    label115.Text = "x ¢" + reader.GetInt32(5).ToString();
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }


        void Guardar()
        {

            string query = "update variable set dolar_venta = " + txtdolar.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha guardado la venta", "Guardado");
        }

        double imp = 0;

        void LeerImp()
        {

            string query = "select impuesto from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    imp = reader.GetDouble(0);
                    lblImp.Text = "" + String.Format("{0:n}%", imp);
                    impuesto = (imp / 100) + 1;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        String dirI = Directory.GetCurrentDirectory().ToString() + "\\datosImp.xml";

        void GuardarImp()
        {
            string query = "update variable set impuesto = " + txtimp.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha actualizado el impuesto.");
        }

        String dir2 = Directory.GetCurrentDirectory().ToString() + "\\datosM.xml";
        List<Materiales> items = new List<Materiales>();
        List<Papeleria> papeles = new List<Papeleria>();
        List<Clientes> itemsClientes = new List<Clientes>();

        void LeerMateriales()
        {
            comboMateriales.Items.Clear();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            combo2.Items.Clear();
            comboTipo.Items.Clear();
            lista.Items.Clear();
            lista1.Items.Clear();
            listLata.Items.Clear();
            listView2.Items.Clear();
            items.Clear();

            string query = "select nombre,precio from material order by clicks desc";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    /*items.Add(new Materiales(unEmpleado.SelectSingleNode("Nombre").InnerText,
                    Double.Parse(unEmpleado.SelectSingleNode("Precio").InnerText)));*/

                    //reader.GetString(0);
                    //reader.GetString(1);
                    items.Add(new Materiales(reader.GetString(0), reader.GetDouble(1)));

                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            for (int i = 0; i < items.Count; i++)
            {
                comboMateriales.Items.Add(items.ElementAt(i).nombre);
                combo2.Items.Add(items.ElementAt(i).nombre);
                if (items.ElementAt(i).nombre.Contains("Zinc"))
                {
                    listLata.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre + " a " + items.ElementAt(i).precio + "m");
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre + " a " + items.ElementAt(i).precio + "m");
                }
                else if (items.ElementAt(i).nombre.Contains("MAT"))
                {
                    listView2.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre + " a " + items.ElementAt(i).precio + "m");
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre + " a " + items.ElementAt(i).precio + "m");
                }
                else if (items.ElementAt(i).nombre.Contains("PEP"))
                {
                    comboBox1.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre);
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre);
                }
                else if (items.ElementAt(i).nombre.Contains("PL"))
                {
                    comboBox3.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre);
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre);
                }
                else if (items.ElementAt(i).nombre.Contains("LAM"))
                {
                    comboBox5.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre + " a ¢" + items.ElementAt(i).precio);
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre + " a ¢" + items.ElementAt(i).precio);
                }
                else
                {
                    comboTipo.Items.Add(items.ElementAt(i).nombre);
                    lista.Items.Add(" - " + items.ElementAt(i).nombre + " a $" + items.ElementAt(i).precio);
                    lista1.Items.Add(" - " + items.ElementAt(i).nombre + " a $" + items.ElementAt(i).precio);
                }

            }

        }

        void Eliminar()
        {
            string query = "delete from material where nombre = '" + combo2.SelectedItem.ToString() + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se elimino el material!", "Exito al eliminar");
            LeerMateriales();
        }

        void Modificar()
        {

            int index = comboMateriales.SelectedIndex;

            string nombre = comboMateriales.SelectedItem.ToString();
            string precio = textoNuevo.Text;

            string query = "update material set precio = " + precio + " where nombre = '" + nombre + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se realizo el cambio!", "Exito al guardar");
            LeerMateriales();

        }

        void GuardarMateriales()
        {
            foreach (Materiales material in items)
            {
                string query = "insert into material(nombre,precio)values('" + material.nombre + "'," + material.precio.ToString() + ");";

                MySqlCommand command = new MySqlCommand(query, dbConnection);
                command.CommandTimeout = 60;

                try
                {
                    dbConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                    }
                    dbConnection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error! " + e.Message + e.StackTrace);
                    dbConnection.Close();
                }

            }


            MessageBox.Show("Registro guardado!", "Exito");
            txtdisplay.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtdolar.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un valor!");
            }
            else
            {
                Guardar();
                LeerVariables();
            }
        }

        private void btnGMat_Click(object sender, EventArgs e)
        {
            //GuardarMateriales();
            //LeerMateriales();
            if (textoNuevo.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un valor!");
            }
            else
            {
                Modificar();
            }
        }


        private void comboMateriales_SelectedIndexChanged(object sender, EventArgs e)
        {
            double valor = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (comboMateriales.SelectedItem.ToString() == items.ElementAt(i).nombre)
                {
                    valor = items.ElementAt(i).precio;
                }
            }

            txtdisplay.Text = "" + valor;

        }

        private void lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int indice = lista.SelectedIndices[0];

                comboMateriales.SelectedIndex = indice;
                txtdisplay.Text = "" + items.ElementAt(indice).precio;
            }
            catch (Exception)
            { }

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtimp.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un valor!");
            }
            else
            {

                GuardarImp();
                LeerImp();
            }
        }

        private void lista1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int indice = lista1.SelectedIndices[0];
                combo2.SelectedIndex = indice;
                txteliminarPrecio.Text = "" + items.ElementAt(indice).precio;
            }
            catch (Exception)
            { }
        }

        private void combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            double valor = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (combo2.SelectedItem.ToString() == items.ElementAt(i).nombre)
                {
                    valor = items.ElementAt(i).precio;
                }
            }

            txteliminarPrecio.Text = "" + valor;
        }

        private void btnEliminarSeleccion_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        void Agregar()
        {
            String tipo = "";

            if (comboBox2.SelectedIndex > 0)
            {
                tipo = comboBox2.SelectedItem.ToString();
            }

            string nombre = "";

            if (tipo.Equals(""))
            {
                nombre = txtagregarMaterial.Text;
            }
            else
            {
                nombre = tipo + " " + txtagregarMaterial.Text;
            }

            string precio = "";

            precio = txtagregarDolar.Text;

            string query = "insert into material(nombre,precio)values('" + nombre + "'," + precio + ");";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Registro agregado!", "Exito");
            LeerMateriales();
        }

        private void btnAgregarProd_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                double plata, comision, transporte;
                plata = Double.Parse(txtPrecioLata.Text);
                comision = Double.Parse(txtcomision.Text);
                transporte = Double.Parse(txtFlete.Text);

                txtprecioF.Text = "" + (plata + comision + transporte);
                Accionar();
            }
            catch (Exception)
            {
            }

        }

        void RetornarDecimales(TextBox textBox, double val)
        {

            //int valor = Int32.Parse(Math.Round(val, decimales, MidpointRounding.AwayFromZero).ToString());

            Double valor = Math.Round((Double)val, decimales);

            //decimales
            if (checkBox7.Checked == true)
            {
                //valor = (valor + 50) / 100 * 100;
                valor = Math.Round(valor / 100d) * 100;
            }

            //MessageBox.Show(""+valor);
            //String digitos = Math.Ceiling(Math.Log10(val)).ToString();

            textBox.Text = "¢" + String.Format("{0:N" + decimales + "}", valor + Int32.Parse(textBox62.Text));

            //MessageBox.Show(digitos);

        }

        public double venta = 0;
        void Accionar()
        {
            string num = "" + numericUpDown2.Value;
            double val = Double.Parse(txtprecioF.Text) / Double.Parse(num);
            richMetal.Text = "Metro de lata: ¢" + String.Format("{0:n}", val);
            richMetal.AppendText("\nEquivalente " + 100 + " x " + (txtTamañoLata.Text) + "");

            double div = val / (double)100;
            double colones = div / (Double.Parse(txtTamañoLata.Text));
            //txtTotal.Text = "¢" + String.Format("{0:n}", Math.Round(formula, decimales));
            RetornarDecimales(txtTotal, formula);

            richMetal.AppendText("\nCada tiraje: ¢" + String.Format("{0:n}", div) + " m²");
            richMetal.AppendText("\nCentímetro cuadrado: " + String.Format("{0:n}", colones));
            venta = colones * (double)numericUpDown10.Value;
            venta = Math.Round(venta, 2);
            richMetal.AppendText("\nVenta: ¢" + String.Format("{0:n}", venta) + " cm²");

            label31.Text = "Venta: ¢" + String.Format("{0:n}", (venta)) + " cm²";

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            Accionar();
        }

        double MaximoVal = 0;
        String textos = "";

        private void listLata_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listLata.SelectedItems.Count > 0)
            {

                try
                {
                    textos = listLata.SelectedItems[0].Text;

                    foreach (Materiales mat in items)
                    {
                        if (textos == mat.nombre)
                        {
                            label39.Text = "Máx: " + mat.precio;
                            MaximoVal = mat.precio;
                            Accionar();
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GuardarDatosLata();
            LeerDatosLata();
        }

        public double RetornaValor(String nombre, double basex, double altura)
        {
            double prec = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (nombre == items.ElementAt(i).nombre)
                {
                    prec = items.ElementAt(i).precio;

                    prec = basex * altura * items.ElementAt(i).precio * constante_dolar;

                    return prec;
                }

            }
            return 0;
        }

        void Laminante()
        {

            ///////////////
            double basex = Double.Parse(textBox2.Text) / 100;
            double altura = Double.Parse(textBox1.Text) / 100;
            double precio = RetornaValor("Adhesivo-Laminado", basex, altura);

            constante_lata = venta;
            double resp = 0;
            sobrante = MaximoVal - Double.Parse(textBox2.Text);
            double alturax = Double.Parse(textBox1.Text);

            if (checkImp.Checked == true)
            {
                ////////////////////////////
                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * impuesto;
                resp = sobrante * alturax * constante_lata * impuesto;
                precio = precio * impuesto;

                double tot = res + resp + precio + instalacionLata;

                textUnitario.Text = "¢" + String.Format(("{0:n}"), tot - instalacionLata);
                guardadoLata = tot;
                RetornarDecimales(textBox6, tot * (int)numericUpDown3.Value);
                //textBox6.Text = "¢" + String.Format(("{0:n}"), tot * (int)numericUpDown3.Value);
                guardadoLata = tot;
            }
            else
            {
                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata;
                resp = sobrante * alturax * constante_lata;
                textUnitario.Text = "¢" + String.Format(("{0:n}"), res + resp + precio);
                guardadoLata = res + resp + precio + instalacionLata;

                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * (int)numericUpDown3.Value;
                resp = sobrante * alturax * constante_lata * (int)numericUpDown3.Value;
                precio = precio * (int)numericUpDown3.Value;
                RetornarDecimales(textBox6, res + resp + precio + instalacionLata);
                //textBox6.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + instalacionLata);
                guardadoLata = res + resp + precio + instalacionLata;
            }



        }

        double res = 0;
        double sobrante = 0;

        void Sinlaminante()
        {

            ///////////////
            double basex = Double.Parse(textBox2.Text) / 100;
            double altura = Double.Parse(textBox1.Text) / 100;
            double precio = RetornaValor("Adhesivo", basex, altura);

            constante_lata = venta;
            double resp = 0;
            sobrante = MaximoVal - Double.Parse(textBox2.Text);
            double alturax = Double.Parse(textBox1.Text);

            if (checkImp.Checked == true)
            {
                ////////////////////////////
                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * impuesto;
                resp = sobrante * alturax * constante_lata * impuesto;
                precio = precio * impuesto;

                double tot = res + resp + precio + instalacionLata;

                textUnitario.Text = "¢" + String.Format(("{0:n}"), tot - instalacionLata);
                guardadoLata = tot;
                RetornarDecimales(textBox6, tot * (int)numericUpDown3.Value);
                //textBox6.Text = "¢" + String.Format(("{0:n}"), tot * (int)numericUpDown3.Value);
                guardadoLata = tot;
            }
            else
            {
                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata;
                resp = sobrante * alturax * constante_lata;
                textUnitario.Text = "¢" + String.Format(("{0:n}"), res + resp + precio);
                guardadoLata = res + resp + precio + instalacionLata;

                res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * (int)numericUpDown3.Value;
                resp = sobrante * alturax * constante_lata * (int)numericUpDown3.Value;
                precio = precio * (int)numericUpDown3.Value;
                RetornarDecimales(textBox6, res + resp + precio + instalacionLata);
                //textBox6.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + instalacionLata);
                guardadoLata = res + resp + precio + instalacionLata;
            }


        }

        double constante_lata = 0;
        double guardadoLata = 0;
        double instalacionLata = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            CalcularLata();
        }

        void CalcularLata()
        {
            if (textBox33.Text == "")
            {
                textBox33.Text = "0";
            }

            try
            {
                instalacionLata = Double.Parse(textBox33.Text);

                if (Double.Parse(textBox2.Text) > MaximoVal)
                {
                    MessageBox.Show("La base esta por encima del rango!", "Error");
                }
                else
                {
                    instalacionLata = Double.Parse(textBox33.Text);

                    if (radioLam.Checked == true)
                    {
                        Laminante();

                    }
                    else if (radioSin.Checked == true)
                    {
                        Sinlaminante();

                    }
                    else
                    {
                        constante_lata = venta;
                        double resp = 0;
                        sobrante = MaximoVal - Double.Parse(textBox2.Text);
                        double basexx = Double.Parse(textBox1.Text);
                        instalacionLata = Double.Parse(textBox33.Text);

                        if (checkImp.Checked == true)
                        {
                            res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * impuesto;
                            resp = sobrante * basexx * constante_lata * impuesto;
                            textUnitario.Text = "¢" + String.Format(("{0:n}"), res + resp);
                            //guardadoLata = res + resp;

                            res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * (int)numericUpDown3.Value * impuesto;
                            resp = sobrante * basexx * constante_lata * (int)numericUpDown3.Value * impuesto;
                            RetornarDecimales(textBox6, res + resp + instalacionLata);
                            //textBox6.Text = "¢" + String.Format(("{0:n}"), res + resp + instalacionLata);
                            guardadoLata = res + resp + instalacionLata;
                        }
                        else
                        {
                            res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata;
                            resp = sobrante * basexx * constante_lata;
                            textUnitario.Text = "¢" + String.Format(("{0:n}"), res + resp);
                            guardadoLata = res + resp + instalacionLata;

                            res = Double.Parse(textBox2.Text) * Double.Parse(textBox1.Text) * constante_lata * (int)numericUpDown3.Value;
                            resp = sobrante * basexx * constante_lata * (int)numericUpDown3.Value;
                            RetornarDecimales(textBox6, res + resp + instalacionLata);
                            //textBox6.Text = "¢" + String.Format(("{0:n}"), res + resp + instalacionLata);
                            guardadoLata = res + resp + instalacionLata;
                        }

                    }


                }

            }
            catch (Exception)
            {
            }
        }

        String dir3 = Directory.GetCurrentDirectory().ToString() + "\\datosL.xml";

        void GuardarDatosLata()
        {
            string query = "update lata set precio = " + txtPrecioLata.Text.ToString() + ", comision = " + txtcomision.Text.ToString() + ", flete = " + txtFlete.Text.ToString() + ", tamano = " + txtTamañoLata.Text.ToString() + ", metros = " + numericUpDown2.Value.ToString() + ", multiplicador = " + numericUpDown10.Value.ToString() + "";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Registro Guardado!", "Exito");
        }

        void LeerDatosLata()
        {

            string query = "select * from lata";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtPrecioLata.Text = reader.GetInt32(1).ToString();
                    txtcomision.Text = reader.GetInt32(2).ToString();
                    txtFlete.Text = reader.GetInt32(3).ToString();
                    //este sale a partir de una sumatoria
                    txtprecioF.Text = (reader.GetInt32(1) + reader.GetInt32(2) + reader.GetInt32(3)).ToString();
                    txtTamañoLata.Text = reader.GetInt32(4).ToString();
                    numericUpDown2.Value = Int32.Parse(reader.GetInt32(5).ToString());
                    numericUpDown10.Value = Decimal.Parse(reader.GetDouble(6).ToString());
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = "0";
                textBox2.Text = "0";
            }
            catch (Exception)
            {
            }
        }

        String dir4 = Directory.GetCurrentDirectory().ToString() + "\\datosHjas.xml";

        void LeerDatosPapel()
        {
            listView1.Items.Clear();
            papeles.Clear();

            string query = "select * from papeleria";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    papeles.Add(
                    new Papeleria(
                    reader.GetString(1),
                    reader.GetDouble(2),
                    reader.GetDouble(3),
                    reader.GetDouble(4),
                    reader.GetString(5)
                    ));
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            for (int i = 0; i < papeles.Count; i++)
            {

                listView1.Items.Add(papeles.ElementAt(i).nombre);

                //comboTipo.Items.Add(items.ElementAt(i).nombre);
                //lista.Items.Add(" - " + items.ElementAt(i).nombre + " a $" + items.ElementAt(i).precio);
                //lista1.Items.Add(" - " + items.ElementAt(i).nombre + " a $" + items.ElementAt(i).precio);

            }

        }

        double numero_volantes = 0;

        double diseño = 0;

        double unitario, unitarioCandado = 0;

        void Cantidad()
        {
            if (checkBox5.Checked == true)
            {
                button33.Text = "Con Diseño";
                diseño = Double.Parse(textBox29.Text);
            }
            else
            {
                button33.Text = "Sin Diseño";
                diseño = 0;
            }

            int valor = (int)numericUpDown5.Value;
            unitario = (precioPapel + precioLaminacion + diseño) / valor;
            
            unitarioCandado = unitario;

            double numeroFinalLaminantes = (double)numericUpDown4.Value * precioLaminacion;

            

            RetornarDecimales(textBox8, (precioPapel * (int)numericUpDown4.Value + precioPapel * (int)numericUpDown11.Value + diseño + Double.Parse(textBox30.Text) + numeroFinalLaminantes));
            //textBox8.Text = "¢" + String.Format(("{0:n}"), (precioPapel * (int)numericUpDown4.Value + precioPapel * (int)numericUpDown11.Value + diseño + Double.Parse(textBox30.Text) + numeroFinalLaminantes));

            numero_volantes = (int)numericUpDown4.Value * (int)numericUpDown5.Value;

        }

        double precioPapel = 0;
        double papelBase = 0;
        double papelAltura = 0;
        double porCorte = 0;


        void Retro()
        {
            try
            {
                porCorte = Int32.Parse(textBox63.Text.ToString());
                int cantidadHoja = (int)numericUpDown5.Value;
                int copias = (int)numericUpDown4.Value;

                double corteTotal = porCorte * cantidadHoja * copias;

                textBox30.Text = corteTotal.ToString();
                
                /*double corteGeneral = Int32.Parse(textBox30.Text.ToString());

                corteGeneral = corteGeneral + precioLaminacion;

                int cantidad = (int)numericUpDown4.Value;
                int salecantidad = (int)numericUpDown5.Value;
                textBox63.Text = ((corteGeneral / cantidad)/salecantidad).ToString();*/

                ///////////////////////////////////////////////////////////////////////
                String text = listView1.SelectedItems[0].Text;

                foreach (Papeleria pap in papeles)
                {
                    if (text == pap.nombre)
                    {
                        precioPapel = pap.precio;
                        label41.Text = "Base: " + pap.basex;
                        label42.Text = "Altura: " + pap.altura;
                        label47.Text = "Precio: ¢" + pap.precio;
                        label44.Text = "Medida: " + pap.medida;
                        textBox9.Text = "¢" + pap.precio;
                        label40.Text = pap.nombre + " " + pap.basex + " x " + pap.altura + " " + pap.medida;
                        Cantidad();
                        pict.Refresh();
                        Graphics g = pict.CreateGraphics();
                        label72.Text = "x" + numero_volantes + " Volantes";
                        PintarPapel("x" + numero_volantes + " Volantes", pap.basex, pap.altura, g);
                    }
                }
            }
            catch
            {

            }
        }

        String apartado = "";

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Retro();
            clickCandado();
            clickCandado();

            if (listView1.SelectedItems.Count > 0)
            {
                apartado = listView1.SelectedItems[0].Text.ToString();
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            Retro();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (estaEnllavado == true)
            {
                numericUpDown11.Value = (int)numericUpDown4.Value;
                textBox7.Text = "¢" + String.Format(("{0:n}"), unitarioCandado * 2);
                textBox64.Text = "¢" + String.Format(("{0:n}"), (unitarioCandado + porCorte) * 2);
            }
            Retro();

        }

        void PintarPapel(string message, double basex, double altura, Graphics g)
        {

            Pen pen = new Pen(Color.Black, (float)0.25);
            SolidBrush sb = new SolidBrush(Color.White);
            Font myFont = new System.Drawing.Font("Helvetica", (float)1.5, FontStyle.Regular);

            Brush myBrush = new SolidBrush(System.Drawing.Color.Black);

            //g.DrawString("H: " + basex + " cm , V: " + altura + " cm", myFont, myBrush, 10, 10);

            //if (basex > altura)
            //{
            //    mayor = basex;
            //}
            //else if (altura > basex)
            //{
            //    mayor = altura;
            //}
            //else if (basex == altura)
            //{
            //    mayor = basex;
            //}

            //float val = (float)150 / (float)mayor;
            g.ScaleTransform(11, 11);

            g.DrawRectangle(pen, 1, 1, (float)basex, (float)altura);
            g.FillRectangle(sb, 1, 1, (float)basex, (float)altura);
            //g.DrawString(message, myFont, myBrush, (float)basex, (float)altura);

            /////////////////////////////////////////////////////////
            ///
            //int numHorisontal = 2;
            //int numVertical = 2;
            //int squareDim = 10;
            //int xOffset = 30;
            //int yOffset = 30;

            //for (int i = 0; i <= numVertical; i++)
            //{
            //    g.DrawLine(Pens.Black, new Point(xOffset, yOffset + i * squareDim), new Point(xOffset + 20 * squareDim, yOffset + i * squareDim));
            //}
            //for (int i = 0; i < numHorisontal; i++)
            //{
            //    g.DrawLine(Pens.Black, new Point(xOffset + i * squareDim, yOffset), new Point(xOffset + i * squareDim, yOffset + 20 * squareDim));
            //}

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Double.Parse(textBox9.Text) / Double.Parse(textBox9.Text) == 1)
            {

                string query = "update papeleria set precio = " + textBox9.Text + " where nombre = '" + listView1.SelectedItems[0].Text.ToString() + "';";

                MySqlCommand command = new MySqlCommand(query, dbConnection);
                command.CommandTimeout = 60;

                try
                {
                    dbConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                    }
                    dbConnection.Close();
                }
                catch (Exception ex)
                {

                }

                MessageBox.Show("Se realizo el cambio!", "Exito al guardar");
                LeerDatosPapel();
            }
            else
            {
                MessageBox.Show("No válido!");
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                double plata, tamano, transporte;
                plata = Double.Parse(textBox13.Text);
                transporte = Double.Parse(textBox12.Text);
                tamano = Double.Parse(textBox11.Text);
                double porc = Double.Parse(textBox18.Text);

                textBox17.Text = "" + (plata + transporte);
                AccionarLaminante();
            }
            catch (Exception)
            {
            }
        }

        double ventaM = 0;
        void AccionarLaminante()
        {
            string num = "" + numericUpDown7.Value;
            double val = Double.Parse(textBox17.Text) / (Double.Parse(num)/100);
            richTextBox1.Text = "Metro de material: ¢" + String.Format("{0:n}", val);
            richTextBox1.AppendText("\nEquivalente " + (textBox11.Text) + " x " + (textBox66.Text) + "");

            double div = val / (double)100;
            double colones = div / (Double.Parse(textBox11.Text));
            RetornarDecimales(txtTotal, decimales);
            //txtTotal.Text = "¢" + String.Format("{0:n}", Math.Round(formula, decimales));

            double porc = Double.Parse(textBox18.Text);
            String multN = "" + numericUpDown12.Value;
            double mult = Double.Parse(multN);

            richTextBox1.AppendText("\nCada tiraje: ¢" + String.Format("{0:n}", div) + " cm");
            richTextBox1.AppendText("\nCentimetro cuadrado: " + String.Format("{0:n}", colones));

            //por aqui deberia de ir el multiplicador
            ventaM = colones * mult + (colones * (porc / 100));
            ventaM = Math.Round(ventaM, 2);
            richTextBox1.AppendText("\nVenta: ¢" + String.Format("{0:n}", ventaM) + " cm²");
            label58.Text = "Venta: ¢" + String.Format("{0:n}", (ventaM)) + " cm²";
            label31.Text = "Venta: ¢" + String.Format("{0:n}", (ventaM)) + " cm²";

        }

        void GuardarDatosLaminantes(String nombre)
        {
            string query = "select id_material from material where nombre = '" + nombre + "'";

            ultimoSeleccionadoLam = nombre;
            MessageBox.Show("Guardado para: "+ultimoSeleccionadoLam);

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            int id_material = 0;
            
            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    id_material = reader.GetInt32(0);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            query = "select id_material from parametros_laminantes where id_material = " + id_material + "";

            command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            bool arrojoVal = false;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows == true)
                {
                    arrojoVal = true;
                }

                while (reader.Read())
                {
                    id_material = reader.GetInt32(0);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            if (arrojoVal == true)
            {
                query = "update parametros_laminantes set precio = " + textBox13.Text.ToString() + "," +
                " flete = " + textBox12.Text.ToString() + "," +
                " tamano = " + textBox11.Text.ToString() + "," +
                " altura = " + textBox66.Text.ToString() + "," +
                " porc_venta = " + textBox18.Text.ToString() + "," +
                " metros = " + (double)numericUpDown7.Value + "," +
                " mult = " + (double)numericUpDown12.Value + " where id_material = " + id_material + ";";

            }
            else if (arrojoVal == false)
            {
                query = "insert into parametros_laminantes (id_material, precio, flete, tamano, altura, porc_venta, metros, mult) " +
                    "values(" + id_material + "," +
                    textBox13.Text.ToString() + "," + textBox12.Text.ToString() + "," +
                    textBox11.Text.ToString() + "," + textBox66.Text.ToString() + "," + textBox18.Text.ToString() + "," +
                    (double)numericUpDown7.Value + "," + (double)numericUpDown12.Value + ");";
            }

            command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Registro Guardado!", "Exito");
        }

        String ultimoSeleccionadoLam = "";

        void LeerDatosLaminantes(String nombre)
        {
            string query = "select id_material from material where nombre = '" + nombre + "'";

            ultimoSeleccionadoLam = nombre;

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            int id_material = 0;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id_material = reader.GetInt32(0);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            query = "select * from parametros_laminantes where id_material = "+ id_material + "";

            command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox13.Text = reader.GetInt32(2).ToString();
                    textBox12.Text = reader.GetInt32(3).ToString();
                    textBox11.Text = reader.GetInt32(4).ToString();
                    textBox66.Text = reader.GetInt32(5).ToString();
                    textBox18.Text = reader.GetInt32(6).ToString();

                    textBox17.Text = (reader.GetInt32(2) + reader.GetInt32(3)).ToString();

                    numericUpDown7.Value = (decimal)reader.GetDecimal(7);
                    numericUpDown12.Value = (decimal)reader.GetDecimal(8);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            label62.Text = "=> " + (textBox18.Text) + "%";

        }

        private void button12_Click(object sender, EventArgs e)
        {
            GuardarDatosLaminantes(ultimoSeleccionadoLam);
            LeerDatosLaminantes(ultimoSeleccionadoLam);
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            AccionarLaminante();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String text = listView2.SelectedItems[0].Text;
                LeerDatosLaminantes(text);

                foreach (Materiales mat in items)
                {
                    if (text == mat.nombre)
                    {
                        label56.Text = "Máx: " + mat.precio;
                        MaximoVal = mat.precio;
                        AccionarLaminante();
                    }
                }
            }
            catch (Exception)
            { }
        }

        double constante_magnetico = 0;

        private void button11_Click(object sender, EventArgs e)
        {
            CalcularMagnetico();
        }

        void CalcularMagnetico()
        {
            try
            {
                if (Double.Parse(textBox16.Text) > MaximoVal)
                {
                    MessageBox.Show("La base esta por encima del rango!", "Error");
                }
                else
                {
                    if (radioLam2.Checked == true)
                    {
                        LaminanteM();

                    }
                    else if (radioSin2.Checked == true)
                    {
                        SinlaminanteM();

                    }
                    else
                    {
                        constante_magnetico = ventaM;
                        double resp = 0;
                        double diseño = Double.Parse(textBox35.Text.ToString());
                        sobrante = MaximoVal - Double.Parse(textBox16.Text);
                        double basexx = Double.Parse(textBox15.Text);

                        if (checkBox3.Checked == true)
                        {
                            res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * impuesto;
                            resp = sobrante * basexx * constante_magnetico * impuesto;
                            textBox14.Text = "¢" + String.Format(("{0:n}"), res + resp + diseño);

                            res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * (int)numericUpDown6.Value * impuesto;
                            resp = sobrante * basexx * constante_magnetico * (int)numericUpDown6.Value * impuesto;
                            RetornarDecimales(textBox10, res + resp + diseño);
                            //textBox10.Text = "¢" + String.Format(("{0:n}"), res + resp + diseño);
                        }
                        else
                        {
                            res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico;
                            resp = sobrante * basexx * constante_magnetico;
                            textBox14.Text = "¢" + String.Format(("{0:n}"), res + resp + diseño);

                            res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * (int)numericUpDown6.Value;
                            resp = sobrante * basexx * constante_magnetico * (int)numericUpDown6.Value;
                            RetornarDecimales(textBox10, res + resp + diseño);
                            //textBox10.Text = "¢" + String.Format(("{0:n}"), res + resp + diseño);
                        }

                    }
                }

            }
            catch (Exception)
            {
            }
        }

        void LaminanteM()
        {

            ///////////////
            double basex = Double.Parse(textBox16.Text) / 100;
            double altura = Double.Parse(textBox15.Text) / 100;
            double precio = RetornaValor("Adhesivo-Laminado", basex, altura);
            double diseño = Double.Parse(textBox35.Text.ToString());

            constante_magnetico = ventaM;
            double resp = 0;
            sobrante = MaximoVal - Double.Parse(textBox16.Text);
            double alturax = Double.Parse(textBox15.Text);

            if (checkBox3.Checked == true)
            {
                ////////////////////////////
                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * impuesto;
                resp = sobrante * alturax * constante_magnetico * impuesto;
                precio = precio * impuesto;

                double tot = res + resp + precio + diseño;

                textBox14.Text = "¢" + String.Format(("{0:n}"), tot);

                RetornarDecimales(textBox10, tot * (int)numericUpDown6.Value);
                //textBox10.Text = "¢" + String.Format(("{0:n}"), tot * (int)numericUpDown6.Value);
            }
            else
            {
                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico;
                resp = sobrante * alturax * constante_magnetico;
                textBox14.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + diseño);

                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * (int)numericUpDown6.Value;
                resp = sobrante * alturax * constante_magnetico * (int)numericUpDown6.Value;
                precio = precio * (int)numericUpDown6.Value;

                RetornarDecimales(textBox10, res + resp + precio + diseño);
                //textBox10.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + diseño);
            }


        }


        void SinlaminanteM()
        {

            ///////////////
            double basex = Double.Parse(textBox16.Text) / 100;
            double altura = Double.Parse(textBox15.Text) / 100;
            double precio = RetornaValor("Adhesivo", basex, altura);
            double diseño = Double.Parse(textBox35.Text.ToString());

            constante_magnetico = ventaM;
            double resp = 0;
            sobrante = MaximoVal - Double.Parse(textBox16.Text);
            double alturax = Double.Parse(textBox15.Text);

            if (checkBox3.Checked == true)
            {
                ////////////////////////////
                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * impuesto;
                resp = sobrante * alturax * constante_magnetico * impuesto;
                precio = precio * impuesto;

                double tot = res + resp + precio + diseño;

                textBox14.Text = "¢" + String.Format(("{0:n}"), tot);

                RetornarDecimales(textBox10, tot * (int)numericUpDown6.Value);
                //textBox10.Text = "¢" + String.Format(("{0:n}"), tot * (int)numericUpDown6.Value);
            }
            else
            {
                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico;
                resp = sobrante * alturax * constante_magnetico;
                textBox14.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + diseño);

                res = Double.Parse(textBox16.Text) * Double.Parse(textBox15.Text) * constante_magnetico * (int)numericUpDown6.Value;
                resp = sobrante * alturax * constante_magnetico * (int)numericUpDown6.Value;
                precio = precio * (int)numericUpDown6.Value;

                RetornarDecimales(textBox10, res + resp + precio + diseño);
                //textBox10.Text = "¢" + String.Format(("{0:n}"), res + resp + precio + diseño);
            }


        }

        //String dirPrograma2 = Directory.GetCurrentDirectory().ToString() + "\\FlexiReg\\bin\\Debug\\FlexiReg.exe";
        //String nuevoDireccionFichero = Directory.GetCurrentDirectory().ToString() + "\\datosCliente.xml";

        private void button13_Click(object sender, EventArgs e)
        {
            //string fileToCopy = dirCliente;
            //string newLocation = Directory.GetCurrentDirectory().ToString();

            //System.IO.File.Copy(fileToCopy, nuevoDireccionFichero, true);


            //Process p = new Process();
            //p.StartInfo.FileName = dirPrograma2;
            //p.Start();

            Form3 listaClientes = new Form3();
            listaClientes.ShowDialog();
            textBox38.Text = listaClientes.TextBoxText;
            RetroNombre();
            SumarRevision();
        }

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        String fecha = "";
        String hora = "", minutos = "", segundos = "";
        DateTime fechaHoy = DateTime.Now;

        void Captura_Fecha()
        {

            fecha = "" + fechaHoy.Day + "-" + fechaHoy.Month + "-" + fechaHoy.Year;
            int h = fechaHoy.Hour;
            int m = fechaHoy.Minute;
            int s = fechaHoy.Second;

            hora = "" + h;
            minutos = "" + m;
            segundos = "" + s;
        }

        private static Bitmap bmp;
        private static Graphics gfxScreenshot;

        private void button14_Click_1(object sender, EventArgs e)
        {
            var frm = Form1.ActiveForm;
            //Bitmap bmp = new Bitmap(frm.Width, frm.Height, PixelFormat.Format32bppArgb);
            //frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            //bmp.Save(path + "\\Cotizacion de " + textBox19.Text + " " + fecha + ".png");

            //Metodo 2
            bmp = new Bitmap(frm.Bounds.Width, frm.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap
            gfxScreenshot = Graphics.FromImage(bmp);
            // Take the screenshot from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(frm.Bounds.X, frm.Bounds.Y, 0, 0, frm.Size, CopyPixelOperation.SourceCopy);
            // Save the screenshot to the specified path that the user has chosen

            String nuevoNombre = textBox38.Text;
            nuevoNombre = nuevoNombre.Replace(":", ".");

            bmp.Save(path + "\\Cotizacion de " + nuevoNombre + " " + fecha + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            String dir = path + "\\Cotizacion de " + nuevoNombre + " " + fecha + ".jpeg";

            MessageBox.Show("Se ha guardado la captura de pantalla en " + dir, "Captura de pantalla");

            string argument = "/select, \"" + dir + "\"";

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = argument;
            Process.Start(info);

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label73_Click(object sender, EventArgs e)
        {

        }

        double total = 0;

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string texto = txtTotal.Text.ToString();
                texto = texto.Replace("¢", "");
                texto = texto.Replace(".", "");

                if (!texto.Equals("0"))
                {
                    total = Double.Parse(texto);
                    textBox43.Text = "$" + String.Format("{0:n}", (total / constante_dolar));
                }
                else
                {
                    textBox43.Text = "$0.00";
                }

            }
            catch (Exception)
            {
            }

        }

        double costoCompra = 0, fleteInst = 0, ManoO = 0, TotSum = 0, CostoTotal = 0;
        void CambiaCifra()
        {

            try
            {
                costoCompra = Double.Parse(txtCostoCompra.Text);
                fleteInst = Double.Parse(txtFleteInst.Text);
                ManoO = Double.Parse(txtMano.Text);
                TotSum = Double.Parse(label75.Text);

                CostoTotal = costoCompra + fleteInst + ManoO + TotSum;
                //txtCostoTotalR.Text = "¢" + String.Format(("{0:n}"), CostoTotal);
                txtCostoTotalR.Text = CostoTotal.ToString();
            }
            catch (Exception)
            {
            }

        }

        private void txtCostoCompra_TextChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        private void txtFleteInst_TextChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        private void txtMano_TextChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        private void txtTotalSuma_TextChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            label79.Text = textBox6.Text;
            label77.Text = "" + textBox2.Text + " x " + textBox1.Text + " cm";

            GenerarDolar(textBox6, textBox51);
            //pictureBox14
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox14.Refresh();
                Graphics g = pictureBox14.CreateGraphics();
                PintaCuadro(Double.Parse(textBox2.Text), Double.Parse(textBox1.Text), g);

                LeerDatosEstructura();
                SumaPresupuesto();
                CambiaCifra();
            }
            catch (Exception)
            {
            }
        }

        void SumaPresupuesto()
        {
            double presupuesto = CostoTotal + guardadoLata;
            //textBox25.Text = "¢" + String.Format(("{0:n}"), presupuesto);
            textBox25.Text = presupuesto.ToString();
            label81.Text = label75.Text;
        }

        void ActualizaRotulo()
        {

        }

        private void label79_Click(object sender, EventArgs e)
        {
            ActualizaRotulo();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void GuardarDatosEstructura()
        {
            string query = "update detalle_estructura set costo_compra = " + txtCostoCompra.Text.ToString() + ", " +
                "flete = " + txtFleteInst.Text.ToString() + ", " +
                "mano_obra = " + txtMano.Text.ToString() + ", costo_total = " + txtCostoTotalR.Text.ToString() + "";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Registro Guardado!", "Exito");
        }

        void Eliminar(int fila, string nombre)
        {
            /*try
            {
                doc.Load(dirLista);

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
            }*/
        }

        void LeerDatosEstructura()
        {
            string query = "select * from detalle_estructura";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    txtCostoCompra.Text = reader.GetInt32(1).ToString();
                    txtFleteInst.Text = reader.GetInt32(2).ToString();
                    txtMano.Text = reader.GetInt32(3).ToString();

                    //label75.Text = (reader.GetInt32(1) + reader.GetInt32(2) + reader.GetInt32(3)).ToString();
                    txtCostoTotalR.Text = reader.GetInt32(4).ToString();
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
            if (txtCostoCompra.Text == "" || txtFleteInst.Text == "" || txtMano.Text == "")
            {
                MessageBox.Show("¡Necesita rellenar todos los campos!", "¡No válido!");
            }
            else
            {
                GuardarDatosEstructura();
                LeerDatosEstructura();
            }


        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
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

        void AgregarNuevo()
        {
            String imagenEstructura = textBox24.Text;
            imagenEstructura = imagenEstructura.Replace("\\\\", "\\\\");
            imagenEstructura = imagenEstructura.Replace("\\", "\\\\");

            string query = "insert into estructura(cantidad,descripcion,precio,imagen)values(" +
                "" + numericUpDown8.Value.ToString() + "," +
                "'" + textBox27.Text + "'," + textBox26.Text + ",'" + imagenEstructura + "')";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("¡Registro agregado!", "¡Exito!");

        }

        String imagenMom = "";

        void ADDIMG(int fila, int columna, String dir, Bitmap bit)
        {
            dataGridView1.Rows[fila].Cells[columna].Value = dir;
            imagenMom = dir;
            pictureBox15.Image = bit;


        }


        static DataGridViewImageColumn mod = new DataGridViewImageColumn();
        static DataGridViewImageColumn elim = new DataGridViewImageColumn();

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            // ToDo: insert your own column index magic number 
            if (this.dataGridView1.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 1)
            {
                e.Value = Properties.Resources.modificar;
            }
            else if (this.dataGridView1.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 2)
            {
                e.Value = Properties.Resources.eliminar;

            }

        }

        List<Imagenes> imagenes = new List<Imagenes>();

        string idEst = "";
        string cantidadEst = "";
        string descripcionEst = "";
        string precioEst = "";
        string imagenEst = "";


        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    String nombre = row.Cells["Descripción"].Value.ToString();
                    item3 = nombre;

                    String img = row.Cells["Imagen"].Value.ToString();
                    pictureBox15.Image = new Bitmap(img);
                    richTextBox2.Clear();
                    richTextBox2.AppendText(
                        row.Cells["Cantidad"].Value.ToString() + " " + row.Cells["Descripción"].Value.ToString() + "\n" +
                        "de ¢" + row.Cells["Precio"].Value.ToString() + "\n\nTotal: "
                        + (Double.Parse(row.Cells["Cantidad"].Value.ToString()) * Double.Parse(row.Cells["Precio"].Value.ToString()))
                    );

                    idEst = row.Cells[0].Value.ToString();
                    cantidadEst = row.Cells[1].Value.ToString();
                    descripcionEst = row.Cells[2].Value.ToString();
                    precioEst = row.Cells[3].Value.ToString();
                    imagenEst = row.Cells[4].Value.ToString();

                }
                catch (Exception)
                {

                }


            }
        }

        string direccionImagenes = "\\\\Pixels\\Users\\Public\\Imagenes\\";

        private void button17_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.InitialDirectory = direccionImagenes;
            open.Filter = "Imagen (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if (open.ShowDialog() == DialogResult.OK)
            {
                Bitmap bit = new Bitmap(open.FileName);
                textBox24.Text = open.FileName;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (numericUpDown8.Value == 0 || textBox27.Text == "" || textBox26.Text == "" || textBox24.Text == "")
            {
                MessageBox.Show("¡Necesita rellenar todos los campos!", "¡No válido!");
            }
            else
            {
                AgregarNuevo();
                Llenar();
            }


        }

        double suma1 = 0;
        List<String> dirimagenes = new List<String>();
        DataSet dataSet = new DataSet();
        DataTable dt = new DataTable("MyTable");

        void Llenar()
        {
            try
            {

                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Update();
                this.dataGridView1.Refresh();

                string query = "select id_estructura as ID, cantidad as Cantidad, descripcion as Descripción, precio as Precio, imagen as Imagen from estructura";

                MySqlCommand command = new MySqlCommand(query, dbConnection);
                command.CommandTimeout = 60;

                try
                {

                    /*MySqlDataAdapter sda = new MySqlDataAdapter();
                    sda.SelectCommand = command;
                    DataTable dbdataset = new DataTable();
                    sda.Fill(dbdataset);
                    BindingSource bSource = new BindingSource();

                    bSource.DataSource = dbdataset;

                    dataGridView1.DataSource = bSource;
                    
                    sda.Update(dbdataset);*/

                    //dataGridView1.Columns["ID"].Visible = false;
                    dataGridView1.Columns["Cantidad"].Width = 10;
                    dataGridView1.Columns["Precio"].Width = 13;
                    dataGridView1.Columns["Muestra"].Width = 20;

                    dbConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //Image.FromFile(reader.GetString(3)
                        //new Bitmap(imagen)

                        String imagen = reader.GetString(4);
      
                        dataGridView1.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), new Bitmap(imagen));

                    }
                    dbConnection.Close();


                }
                catch (Exception e)
                {
                    MessageBox.Show("Error! " + e.Message);
                    dbConnection.Close();
                }
                /*Parte de los mayores a cero*/
                query = "select cantidad as Cantidad, descripcion as Descripción, precio as Precio, imagen as Imagen from estructura " +
                    "where Cantidad > 0";

                command = new MySqlCommand(query, dbConnection);
                command.CommandTimeout = 60;

                try
                {
                    /*MySqlDataAdapter sda = new MySqlDataAdapter();
                    sda.SelectCommand = command;
                    DataTable dbdataset = new DataTable();
                    sda.Fill(dbdataset);
                    BindingSource bSource = new BindingSource();

                    bSource.DataSource = dbdataset;

                    dataGridView2.DataSource = bSource;

                    sda.Update(dbdataset);

                    dataGridView2.Columns["ID"].Visible = false;
                    dataGridView2.Columns["imagen"].Visible = false;*/

                    dataGridView2.Columns["Cantidad2"].Width = 10;
                    dataGridView2.Columns["Precio2"].Width = 10;
                    dataGridView2.Columns["Descripción2"].Width = 15;
                    dataGridView2.Columns["Muestra2"].Width = 60;

                    dbConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //Image.FromFile(reader.GetString(3)
                        //new Bitmap(imagen)

                        String imagen = reader.GetString(3);

                        dataGridView2.Rows.Add(reader.GetString(0), reader.GetString(1), reader.GetString(2), new Bitmap(imagen));

                    }
                    dbConnection.Close();

                }
                catch (Exception e)
                {
                    MessageBox.Show("Error! " + e.Message);
                    dbConnection.Close();
                }

                /**/
                dataGridView1.AllowUserToAddRows = false;
                dataGridView2.AllowUserToAddRows = false;

            }
            catch (Exception)
            {

            }

            double totalFila = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    totalFila += Double.Parse(row.Cells["cantidad"].Value.ToString()) * Double.Parse(row.Cells["precio"].Value.ToString());

                    label75.Text = totalFila.ToString();

                }
                catch (Exception e)
                {
                    MessageBox.Show("" + e.Message);
                }


            }

        }

        private void txtCostoCompra_TabIndexChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        List<String> item2 = new List<String>();
        String item3 = "";

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CambiaCifra();
        }

        private void label75_TextChanged(object sender, EventArgs e)
        {
            CambiaCifra();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CambiaCifra();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            CambiaCifra();
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            CambiaCifra();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void txtagregarDolar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.' && e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }



        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0 && senderGrid.Columns[e.ColumnIndex].HeaderText == "Muestra")
            {
                OpenFileDialog open = new OpenFileDialog();

                open.InitialDirectory = direccionImagenes;
                open.Filter = "Imagen (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bit = new Bitmap(open.FileName);
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bit;

                    String dir = open.FileName;
                    ADDIMG(e.RowIndex, e.ColumnIndex-1, dir, bit);
                }

            }
        }

        String descripcion = "";

        void Modificar_Fila(int fila)
        {
            try
            {
                if (fila >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[fila];
                    String cantidad = row.Cells["Cantidad"].Value.ToString();
                    descripcion = row.Cells["Descripción"].Value.ToString();
                    item3 = descripcion;
                    String precio = row.Cells["Precio"].Value.ToString();
                    imagenMom = row.Cells["Imagen"].Value.ToString();


                    Modificar(cantidad, item3, precio, imagenMom, fila);
                    Llenar();
                    item3 = "";

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message);
            }

        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            Retro();
            GenerarDolar(textBox29, textBox60);
        }

        private void txtinstalacion_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Baltura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.' && e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            Retro();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioMediano_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PrimeraCarga()
        {
            textBox5.Text = "0";

        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            double numero = Double.Parse(textBox25.Text);
            int numeSel = (int)numericUpDown9.Value;

            textBox5.Text = "¢" + String.Format(("{0:n}"), numero * numeSel);
        }

        /* 
         Corte
             */
        void LeerCorte(TextBox text)
        {
            double cort = 0;

            string query = "select corte from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cort = reader.GetInt32(0);
                    text.Text = "" + cort;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }



        }

        void GuardarCorte(TextBox text)
        {
            string query = "update variable set corte = " + text.Text + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("¡Se ha actualizado la entrada!", "Valor Modificado");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox28.Text = txtcorte.Text;
            label26.Text = "Monto Corte:";

            GuardarCorte(txtcorte);
            UltimoGuardado(button20);

        }

        /* 
         Instalacion
             */
        void LeerInstalacion(TextBox text)
        {
            double inst = 0;

            string query = "select instalacion from variable;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inst = reader.GetDouble(0);
                    text.Text = "" + inst;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        void Guardarinstalacion(TextBox text)
        {
            string query = "update variable set instalacion = " + text.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("¡Se ha actualizado la entrada!", "Valor Modificado");
        }

        void UltimoGuardado(Button btn)
        {

            string query = "update variable set ultG = '" + btn.Text.ToString() + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            LeerUltimoG();
        }

        void UltimoGuardadoPVC(Button btn)
        {
            string query = "update variable set ultGPVC = '" + btn.Text.ToString() + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            LeerUltimoGPVC();
        }

        double ultimoVal = 0;


        private void button18_Click(object sender, EventArgs e)
        {
            textBox28.Text = txtinstalacion.Text;
            label26.Text = "Instalación:";
            Guardarinstalacion(txtinstalacion);
            UltimoGuardado(button18);

        }

        /* 
         
             
        */

        void LeerUltimoG()
        {
            string query = "select ultG from variable";
            string dis = "";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dis = reader.GetString(0);

                    if (dis == "Instalación")
                    {
                        textBox28.Text = txtinstalacion.Text;
                        label26.Text = "Instalación:";
                        ultimoVal = Double.Parse(txtinstalacion.Text.ToString());
                    }
                    else if (dis == "Diseño")
                    {
                        textBox28.Text = txtimpresion.Text;
                        label26.Text = "Monto Diseño:";
                        ultimoVal = Double.Parse(txtimpresion.Text.ToString());
                    }
                    else if (dis == "Corte")
                    {
                        textBox28.Text = txtcorte.Text;
                        label26.Text = "Monto Corte:";
                        ultimoVal = Double.Parse(txtcorte.Text.ToString());
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        double ultimoVal2 = 0;


        void LeerUltimoGPVC()
        {
            string dis = "";

            string query = "select ultGPVC from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dis = reader.GetString(0);

                    if (dis == "Instalación")
                    {
                        textBox31.Text = txtinstalacion2.Text;
                        label91.Text = "Instalación:";
                        ultimoVal2 = Double.Parse(txtinstalacion2.Text.ToString());
                    }
                    else if (dis == "Diseño")
                    {
                        textBox31.Text = txtimpresion2.Text;
                        label91.Text = "Monto Diseño:";
                        ultimoVal2 = Double.Parse(txtimpresion2.Text.ToString());
                    }
                    else if (dis == "Corte")
                    {
                        textBox31.Text = txtcorte2.Text;
                        label91.Text = "Monto Corte:";
                        ultimoVal2 = Double.Parse(txtcorte2.Text.ToString());
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        void LeerDiseño(TextBox text)
        {
            double dis = 0;

            string query = "select diseno from variable;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dis = reader.GetDouble(0);
                    text.Text = "" + dis;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        String dirDis = Directory.GetCurrentDirectory().ToString() + "\\datosDis.xml";

        void GuardarDiseño(TextBox text)
        {

            string query = "update variable set diseno = " + text.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("¡Se ha actualizado la entrada!", "Valor Modificado");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox28.Text = txtimpresion.Text;
            label26.Text = "Monto Diseño:";
            GuardarDiseño(txtimpresion);
            UltimoGuardado(button19);

        }

        private void label67_Click(object sender, EventArgs e)
        {

        }

        //string path = 
        //string[] files = File.ReadAllLines(path);
        // Guardar clientes
        //String dirCliente = Directory.GetCurrentDirectory().ToString() + "\\FlexiReg\\bin\\Debug\\datosCliente.xml";

        List<String> nombrePersona = new List<string>();

        string MySQLConnectionString = ConfigurationManager.ConnectionStrings["PixiConnection"].ConnectionString;
        MySqlConnection dbConnection;

        void AgregarCliente()
        {

            string query = "insert into registro(nombre, correo, telefono, cedula, direccion, descripcion, total, archivo, fecha_generado)values(" +
                "'" + textBox38.Text + " " + fechaHoy + "'," +
                "'" + textBox20.Text + "'," +
                "'" + textBox23.Text + "'," +
                "'" + textBox22.Text + "'," +
                "'" + textBox21.Text + "'," +
                "'" + textBox32.Text + "'," +
                "'" + textBox34.Text + "'," +
                "'" + textBox40.Text + "'," +
                "'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');";

            MySqlCommand command = new MySqlCommand(query, dbConnection);

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

            query = "select max(id_registro) as id_registro from registro;";

            command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            string ultimoId = "";
            /*Se convierte el total en numerico*/

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ultimoId = reader.GetInt32(0).ToString();
                }
                dbConnection.Close();

                AgregarLista(ultimoId, textBox34.Text, textBox19.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

        }

        void AgregarLista(String idRegistro, String total, String efectivo)
        {
            if (efectivo == "")
            {
                efectivo = "0";
            }

            total = total.Replace("¢", "");
            //total = total.Replace(".", "");
            total = total.Replace(",", "");
            total = total.Replace(" ", "");

            efectivo = efectivo.Replace("¢", "");
            //efectivo = efectivo.Replace(".", "");
            efectivo = efectivo.Replace(".", "");
            efectivo = efectivo.Replace(" ", "");

            double saldo = Double.Parse(total) - Double.Parse(efectivo);

            //MessageBox.Show("ID:"+ idRegistro + "Total: " + total + ", Efectivo: " + efectivo+"Saldo: "+ saldo);

            string query = "insert into compra(id_registro, monto, efectivo, cambio)values(" + idRegistro + "," + total + "," + efectivo + "," + saldo + ");";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

            query = "select max(id_compra) as id_compra from compra;";

            command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            string ultimaCompra = "";

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ultimaCompra = reader.GetString(0);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

            for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
                query = "insert into detalle_compra(id_compra,montos,descripcion)values(" +
                    "" + ultimaCompra + "," +
                    "'" + dataGridView3.Rows[i].Cells[0].Value + "'," +
                    "'" + dataGridView3.Rows[i].Cells[1].Value + "');";

                command = new MySqlCommand(query, dbConnection);
                command.CommandTimeout = 60;

                try
                {
                    dbConnection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                    }
                    dbConnection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                    dbConnection.Close();
                }

            }

            /*montoL = montoL.Replace("¢", "");
            montoL = montoL.Replace(".", "");
            montoL = montoL.Replace(" ", "");*/

            dataGridView3.Rows.Clear();

            label106.Text = "¢00.000";
            textBox19.Text = "";
            label96.Text = "¢00.000";

        }

        void LeerClientes()
        {
            itemsClientes.Clear();

            string query = "select id_registro, " +
                "nombre as Nombre, " +
                "correo as Correo, " +
                "telefono as Teléfono, " +
                "cedula as Cédula, " +
                "direccion as Dirección, " +
                "descripcion as Descripción, " +
                "total as Total, archivo as Archivo from registro";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    itemsClientes.Add(new Clientes(

                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        reader.GetString(8)

                        ));
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

        }

        void LeerSoloNombres()
        {
            string query = "select nombre as Nombre from registro";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox38.AutoCompleteCustomSource.Add(reader.GetString(0));
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

        }

        void RetroNombre()
        {
            String text = textBox38.Text.ToString();

            foreach (Clientes cli in itemsClientes)
            {

                if (text == cli.nombre)
                {

                    textBox20.Text = "" + cli.correo;
                    textBox22.Text = "" + cli.cedula;
                    textBox23.Text = "" + cli.telefono;
                    textBox34.Text = "" + cli.monto_total;
                    textBox21.Text = "" + cli.direccion;
                    textBox32.Text = "" + cli.descripcion;
                    textBox40.Text = "" + cli.archivo;

                    LeerDatosCliente(cli.nombre);

                }
            }

            button22.Enabled = false;
        }

        DataSet dataSetCliente = new DataSet();
        String monto = "";
        String efectivo = "";
        String cambio = "";

        void reemplazador(String palabra)
        {
            palabra = palabra.Replace("¢", "");
            palabra = palabra.Replace(".", "");
            palabra = palabra.Replace(" ", "");
        }

        void LeerDatosCliente(String nombreC)
        {
            dataGridView3.Rows.Clear();

            string query = "select dc.montos, dc.descripcion, c.monto, c.efectivo, c.cambio from registro r " +
                "inner join compra c on r.id_registro = c.id_registro " +
                "inner join detalle_compra dc on dc.id_compra = c.id_compra where r.nombre = '" + nombreC + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView3.Rows.Add(reader.GetString(0), reader.GetString(1));

                    //label106.Text = "¢" + String.Format("{0:n}", Double.Parse(monto));
                    //textBox19.Text = "" + efectivo;
                    //label96.Text = "¢" + String.Format("{0:n}", Double.Parse(cambio));
                    label106.Text = "¢" + String.Format("{0:n}", Double.Parse(reader.GetString(2)));
                    textBox19.Text = "" + reader.GetString(3);
                    label96.Text = "¢" + String.Format("{0:n}", Double.Parse(reader.GetString(4)));
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
                dbConnection.Close();
            }

        }

        private void button21_Click(object sender, EventArgs e)
        {
            RetroNombre();
            SumarRevision();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            AgregarCliente();
            LeerSoloNombres();
            RetroNombre();
            SumarRevision();

        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            Accionar();
        }

        private void richMetal_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
        
        }
        
        void Eliminar_Cliente()
        {
            try
            {
                /*doc.Load(dirCliente);

                foreach (XmlNode node in doc.SelectNodes("Clientes/Cliente"))
                {
                    if (node.SelectSingleNode("Nombre").InnerText == textBox38.Text.ToString())
                    {
                        node.ParentNode.RemoveChild(node);
                    }

                }

                doc.Save(dirCliente);
                MessageBox.Show("Se elimino el registro!", "Exito al eliminar");

                textBox38.Text = "";
                textBox20.Text = "";
                textBox21.Text = "";
                textBox22.Text = "";
                textBox23.Text = "";
                textBox34.Text = "";
                textBox32.Text = "";

                LeerClientes();
                LeerSoloNombres();*/

            }
            catch (Exception)
            {
            }
        }

        void EliminarSuLista()
        {
            try
            {
                /*doc.Load(dirListaMonto);

                XmlNode node = doc.SelectSingleNode("/ListaClientes/ListaCliente[@nombre='"+ textBox38.Text.ToString() + "']");
                if (node != null)
                {
                    // get its parent node
                    XmlNode parent = node.ParentNode;

                    // remove the child node
                    parent.RemoveChild(node);

                    doc.Save(dirListaMonto);

                }*/


                dataGridView3.Rows.Clear();
                label106.Text = "¢00.000";
                textBox19.Text = "";
                label96.Text = "¢00.000";

            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message);
            }
        }
        
        private void button23_Click(object sender, EventArgs e)
        {
            /*Eliminar_Cliente();*/
            EliminarSuLista();
            textBox38.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox40.Text = "";
            textBox32.Text = "";
            textBox23.Text = "";
            textBox22.Text = "";
            textBox34.Text = "";

            button22.Enabled = true;

        }
        
        private void button24_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            dataGridView3.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView3.DefaultCellStyle.SelectionForeColor = Color.Black;

            Form1.ActiveForm.Width = 858;
            Form1.ActiveForm.Height = 719;
        }
        
        private void button25_Click(object sender, EventArgs e)
        {
            this.Width = 584;
            this.Height = 719;
        }
        
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        void EliminarEntrada(int fila, string valor)
        {
            valor = valor.Replace("¢", "");
            valor = valor.Replace(".", "");
            valor = valor.Replace(" ", "");

            String nuevo = label106.Text;

            nuevo = nuevo.Replace("¢", "");
            nuevo = nuevo.Replace(".", "");
            nuevo = nuevo.Replace(" ", "");

            suma = Double.Parse(nuevo) - Double.Parse(valor);
            dataGridView3.Rows.Remove(dataGridView3.Rows[fila]);

            //txtUnitario.Text = "¢" + String.Format("{0:n}", formula);

            label106.Text = "¢" + String.Format("{0:n}", suma);

            textBox36.Text = "";

        }
        
        void OperacionImp(int fila, int columna, string valor, string clave)
        {
            valor = valor.Replace("¢", "");
            valor = valor.Replace(".", "");
            valor = valor.Replace(" ", "");

            double deduccion = 0;
            double nuevoMonto = 0;



            if (clave.Equals("quitar"))
            {
                deduccion = Double.Parse(valor) * (imp / 100);
                nuevoMonto = Double.Parse(valor) - deduccion;
            }
            else if (clave.Equals("poner"))
            {
                deduccion = Double.Parse(valor) * (imp / 100);
                nuevoMonto = Double.Parse(valor) + deduccion;
            }

            String elnuevoMonto = "¢" + String.Format("{0:n}", nuevoMonto);

            dataGridView3.Rows[fila].Cells[columna].Value = elnuevoMonto;

        }


        double suma = 0;
        String nombre = "";

        void Verificar(TextBox text, TextBox textBase, TextBox textAltura, String copias)
        {
            double basex = 0, alturax = 0;


            if (textBase.Equals(null))
            {
                basex = 0;
            }

            if (textAltura.Equals(null))
            {
                alturax = 0;
            }

            basex = Double.Parse(textBase.Text.ToString());
            alturax = Double.Parse(textAltura.Text.ToString());

            string basess, alturass;

            if (text == textBox4 || text == textBox6 || text == textBox10)
            {
                basex = basex / 100;
                alturax = alturax / 100;
            }

            if (basex < 1)
            {
                basex = basex * 100;

                basess = "" + basex + "cm";
            }
            else
            {
                basess = "" + basex + "m";
            }

            if (alturax < 1)
            {
                alturax = alturax * 100;

                alturass = "" + alturax + "cm";
            }
            else
            {
                alturass = "" + alturax + "m";
            }


            if (text == txtTotal)
            {
                nombre = "(" + copias + ") " + basess + " x " + alturass + " " + comboTipo.SelectedItem.ToString();
            }
            else if (text == textBox4)
            {
                nombre = "(" + copias + ") " + basess + " x " + alturass + " " + comboBox1.SelectedItem.ToString();
            }
            else if (text == textBox6)
            {
                nombre = "(" + copias + ") " + basess + " x " + alturass + " " + textos;
            }
            else if (text == txtCostoTotalR)
            {
                nombre = "(" + copias + ") " + "Total Estructura";
            }
            else if (text == textBox5)
            {
                nombre = "(" + copias + ") " + label77.Text + " " + "Lata + Estructura";
            }
            else if (text == textBox10)
            {
                nombre = "(" + copias + ") " + basess + " x " + alturass + " " + "Magnético";
            }
            else if (text == textBox8)
            {
                nombre = "(" + copias + ") " + "Papelería - " + comboBox3.SelectedItem.ToString() + "";
            }
            else if (text == textBox25)
            {
                nombre = "(" + copias + ") " + label77.Text + " " + "Lata + Estructura";
            }
            else if (text.ToString() == "*Sin especificar*")
            {
                nombre = "(" + copias + ") " + textBox39.Text;
            }
            else if (text.ToString() != "*Sin especificar*")
            {
                nombre = "(" + copias + ") " + textBox39.Text;
            }
        }

        void Sumado(TextBox text, TextBox basex, TextBox alturax, String copias)
        {
            String numero = text.Text;

            numero = numero.Replace("¢", "");
            numero = numero.Replace(".", "");
            numero = numero.Replace(" ", "");

            suma += Double.Parse(numero);

            Verificar(text, basex, alturax, copias);

            //
            String new_numero = "¢" + String.Format("{0:n}", numero);

            label106.Text = "¢" + String.Format("{0:n}", suma);
            //
            dataGridView3.Rows.Add(new_numero, nombre);
            textBox36.Text = "";
            textBox39.Text = "*Sin especificar*";
        }

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                Sumado(textBox36, textBox36, textBox36, comboCopias.Value.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void textBox36_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox36, textBox36, textBox36, comboCopias.Value.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        String texto = "";



        private void txtTotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(txtTotal, Bbase, Baltura, comboCopias.Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox4, PVCbase, PVCaltura, numericUpDown1.Value.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private void textBox6_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox6, textBox2, textBox1, numericUpDown3.Value.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private void txtCostoTotalR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(txtCostoTotalR, txtCostoCompra, txtCostoCompra, "1");
                }
                catch (Exception)
                {
                }
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox5, textBox25, textBox25, numericUpDown9.Value.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    AgregadoPapeleria(textBox8);
                }
                catch (Exception)
                {
                    MessageBox.Show("Asegurese de Revisar todos los campos.", "Campos vacíos");
                }
            }
        }

        void AgregadoPapeleria(TextBox textbox)
        {
            String numero = textbox.Text;

            numero = numero.Replace("¢", "");
            numero = numero.Replace(".", "");
            numero = numero.Replace(" ", "");

            suma += Double.Parse(numero);

            String new_numero = "¢" + String.Format("{0:n}", numero);

            label106.Text = "¢" + String.Format("{0:n}", suma);
            //
            String nombre = comboBox3.SelectedItem.ToString();
            int tiro = (int)numericUpDown4.Value;
            int retiro = (int)numericUpDown11.Value;

            string seleccion = comboBox5.SelectedItem.ToString();
            seleccion = seleccion.Replace("LAM", " |");

            if (retiro > 0)
            {
                dataGridView3.Rows.Add(new_numero, "(" + tiro + ") " + "" + nombre + ", Tiro-Retiro" + seleccion);
            }
            else
            {
                dataGridView3.Rows.Add(new_numero, "(" + tiro + ") " + "" + nombre + seleccion);
            }


            textBox36.Text = "";
            textBox39.Text = "*Sin especificar*";
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox10, textBox16, textBox15, numericUpDown6.Value.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private void textBox37_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //
            }
        }

        private void label106_TextChanged(object sender, EventArgs e)
        {
            CambioDatos();
            textBox34.Text = label106.Text;
            GenerarDolarLbl(label106, label121);

        }

        private void button28_Click(object sender, EventArgs e)
        {
            textBox31.Text = txtimpresion2.Text;
            label91.Text = "Monto Diseño:";
            GuardarDiseño(txtimpresion2);
            UltimoGuardadoPVC(button28);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            textBox31.Text = txtcorte2.Text;
            label91.Text = "Monto Corte:";
            GuardarCorte(txtcorte2);
            UltimoGuardadoPVC(button29);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            textBox31.Text = txtinstalacion2.Text;
            label91.Text = "Instalación:";
            Guardarinstalacion(txtinstalacion2);
            UltimoGuardadoPVC(button30);
        }

        String dirInstLata = Directory.GetCurrentDirectory().ToString() + "\\datosInstLata.xml";

        void LeerInstalacionLata()
        {
            double inst = 0;

            string query = "select instalacion from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inst = reader.GetInt32(0);
                    textBox33.Text = "" + inst;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }



        }

        /// papeleria /// 
        String dirDisMag = Directory.GetCurrentDirectory().ToString() + "\\datosDisMag.xml";

        void GuardarDiseñoMag()
        {
            string query = "update variable set diseno = " + textBox35.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("¡Se ha actualizado la entrada!", "Valor Modificado");
        }

        void LeerDiseñoMag()
        {
            double inst = 0;

            string query = "select diseno from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inst = reader.GetInt32(0);
                    textBox35.Text = "" + inst;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        private void button32_Click(object sender, EventArgs e)
        {
            Guardarinstalacion(textBox33);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            GuardarDiseño(textBox35);
        }

        private void textBox38_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    RetroNombre();
                    SumarRevision();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.Message);
                }
            }
        }


        String dirDisPapel = Directory.GetCurrentDirectory().ToString() + "\\datosDisPapel.xml";

        void GuardarDiseñoPapel()
        {

            string query = "update variable set diseno = " + textBox29.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }


            MessageBox.Show("¡Se ha actualizado la entrada!", "Valor Modificado");
        }

        void LeerDiseñoPapel()
        {
            double inst = 0;

            string query = "select diseno from variable;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inst = Double.Parse(reader.GetInt32(0).ToString());
                    textBox29.Text = "" + inst;
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }



        }

        private void button33_Click(object sender, EventArgs e)
        {
            GuardarDiseñoPapel();
        }

        private void textBox25_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Sumado(textBox25, textBox25, textBox25, "1");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        void CambioDatos()
        {
            if (textBox19.Text == "")
            {
                label94.Text = "Cambio :";
                label96.ForeColor = Color.Black;

                label96.Text = "¢00.000";
            }
            else
            {
                String valor = label106.Text;

                valor = valor.Replace("¢", "");
                valor = valor.Replace(".", "");
                valor = valor.Replace(" ", "");

                double ecuacion = Double.Parse(textBox19.Text) - Double.Parse(valor);

                label96.Text = "¢" + String.Format("{0:n}", ecuacion);

                if (ecuacion < 0)
                {
                    label94.Text = "Saldo :";
                    label96.ForeColor = Color.Red;
                }
                else if (ecuacion > 0)
                {
                    label94.Text = "Cambio :";
                    label96.ForeColor = Color.Green;
                }

            }

        }

        private void label109_Click(object sender, EventArgs e)
        {

        }

        int click = 0;

        bool estaEnllavado = false;

        void clickCandado()
        {

            click++;
            // Aqui el candado esta bloqueado
            estaEnllavado = true;
            
            if (numericUpDown11.Value == 0)
            {
                numericUpDown11.Value = 1;
            }

            if (estaEnllavado == true)
            {
                label46.Text = "Volante Sin Corte (x2):";
                textBox7.Text = "¢" + String.Format(("{0:n}"), unitarioCandado * 2);
                textBox64.Text = "¢" + String.Format(("{0:n}"), (unitarioCandado * 2) + porCorte);
            }

            button34.Image = Properties.Resources.lock_24px;
            if (click > 1)
            {
                click = 0;
                button34.Image = Properties.Resources.unlock_24px;

                estaEnllavado = false;
                if (estaEnllavado == false)
                {
                    label46.Text = "Volante Sin Corte:";
                    textBox7.Text = "¢" + String.Format(("{0:n}"), unitarioCandado);
                    textBox64.Text = "¢" + String.Format(("{0:n}"), (unitarioCandado + porCorte));

                    numericUpDown11.Value = 0;
                }

            }


        }

        private void button34_Click(object sender, EventArgs e)
        {
            clickCandado();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            if (estaEnllavado == true)
            {
                try
                {
                    numericUpDown4.Value = (int)numericUpDown11.Value;
                }
                catch (Exception)
                {
                }
            }
            Retro();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            /*
                Capturar la direccion del path   
             */
            //var path = textBox40.Text;

            //var pi = new ProcessStartInfo(path)
            //{
            //    Arguments = Path.GetFileName(path),
            //    UseShellExecute = true,
            //    WorkingDirectory = Path.GetDirectoryName(path),
            //    FileName = "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc.exe",
            //    Verb = "OPEN"
            //};
            //Process.Start(pi);
            try
            {
                System.Diagnostics.Process.Start(@"" + textBox40.Text + "");
            }
            catch (Exception)
            {
                MessageBox.Show("La entrada no es válida", "Error de Proceso");
            }

        }

        private void button35_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Buscar archivo";

            ofd.InitialDirectory = @"C:\";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox40.Text = "" + ofd.FileName + "";
            }
        }

        String dirCompra = Directory.GetCurrentDirectory().ToString() + "\\datosG2.xml";
        double constante_compra;

        void GuardarCompra()
        {
            string query = "update variable set dolar_compra = " + textBox41.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha guardado la compra", "Guardado");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (textBox41.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un valor!");
            }
            else
            {
                GuardarCompra();
                LeerVariables();
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {

        }

        double cambiado_colones;

        private void textBox42_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox42.Text.Equals(""))
                {
                    textBox42.Text = "0";
                }
                else
                {
                    double dolares_cambio = Double.Parse(textBox42.Text.ToString());
                    cambiado_colones = dolares_cambio * constante_compra;
                    label117.Text = "¢" + String.Format("{0:n}", cambiado_colones);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Necesita introducir un valor válido");
            }
        }

        private void textBox44_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox44.Text.Equals(""))
                {
                    textBox44.Text = "0";
                }
                else
                {
                    double introducido = Double.Parse(textBox44.Text.ToString());

                    double diferencia = cambiado_colones - introducido;
                    if (diferencia < 0)
                    {
                        label118.ForeColor = Color.Red;
                        label119.ForeColor = Color.Red;
                    }
                    else
                    {
                        label118.ForeColor = Color.Green;
                        label119.ForeColor = Color.Green;
                    }

                    label118.Text = "¢" + String.Format("{0:n}", diferencia);
                    label119.Text = "$" + String.Format("{0:n}", diferencia / constante_compra);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Necesita introducir un valor válido");
            }
        }

        void ocultarDolar()
        {
            groupBox6.Hide();
            groupBox25.Height = 321;
            groupBox25.Location = new Point(8, 8);
        }
        private void button38_Click_1(object sender, EventArgs e)
        {
            ocultarDolar();
        }

        void mostrarDolar()
        {
            groupBox6.Show();
            groupBox25.Height = 173;
            groupBox25.Location = new Point(8, 156);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            mostrarDolar();
        }

        private void button40_Click(object sender, EventArgs e)
        {

        }

        void ocultarMagnetico()
        {
            groupBox22.Hide();
            groupBox19.Hide();
            groupBox20.Location = new Point(8, 8);
            groupBox21.Location = new Point(8, 77);

            //545; 68
            groupBox20.Width = 545;
            //545; 363
            groupBox21.Width = 545;
        }

        private void button40_Click_1(object sender, EventArgs e)
        {
            ocultarMagnetico();
        }

        void mostrarMagnetico()
        {
            //261; 212
            //8; 3
            groupBox19.Show();
            groupBox19.Location = new Point(8, 3);
            groupBox20.Location = new Point(275, 3);
            groupBox20.Width = 250;

            groupBox21.Location = new Point(275, 77);
            groupBox22.Show();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            mostrarMagnetico();
        }

        void ocultarLata()
        {
            groupBox13.Hide();
            groupBox14.Hide();

            groupBox11.Location = new Point(8, 8);
            groupBox15.Location = new Point(8, 77);

            //545; 68
            groupBox11.Width = 545;
            //545; 363
            groupBox15.Width = 545;
        }

        void mostrarLata()
        {
            groupBox13.Show();
            groupBox14.Show();

            groupBox11.Width = 250;

            groupBox11.Location = new Point(275, 3);
            groupBox15.Location = new Point(275, 77);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            ocultarLata();
        }

        private void button43_Click(object sender, EventArgs e)
        {
            mostrarLata();

        }

        private void button44_Click(object sender, EventArgs e)
        {
            frm2.tabControl1.Enabled = false;
            frm2.dataGridView1.Visible = false;
            frm2.dataGridView2.Visible = false;

            frm2.Visible = true;
        }

        private void txtUnitario_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(txtUnitario, textBox45);
        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            if (textBox28.Text.Equals("0"))
            {
                textBox46.Text = "0";
            }
            else
            {
                GenerarDolar(textBox28, textBox46);
            }

        }

        public void GenerarDolar(TextBox txtcolon, TextBox txtdolar)
        {
            try
            {
                string texto = txtcolon.Text.ToString();
                texto = texto.Replace("¢", "");
                texto = texto.Replace(".", "");

                double total = Double.Parse(texto);
                txtdolar.Text = "$" + String.Format("{0:n}", (total / constante_dolar));
            }
            catch (Exception)
            {
            }
        }

        public void GenerarDolarLbl(Label txtcolon, Label txtdolar)
        {
            string texto = txtcolon.Text.ToString();
            texto = texto.Replace("¢", "");
            texto = texto.Replace(".", "");

            double total = Double.Parse(texto);
            txtdolar.Text = "$" + String.Format("{0:n}", (total / constante_dolar));
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox4, textBox47);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox3, textBox48);
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            if (textBox31.Text.Equals("0"))
            {
                textBox49.Text = "0";
            }
            else
            {
                GenerarDolar(textBox31, textBox49);
            }
        }

        private void txtCostoTotalR_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(txtCostoTotalR, textBox50);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox5, textBox54);
        }

        private void textUnitario_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textUnitario, textBox52);
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox33, textBox53);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox10, textBox55);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox14, textBox56);
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox35, textBox57);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox8, textBox58);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox7, textBox59);
        }

        private void label96_TextChanged(object sender, EventArgs e)
        {
            GenerarDolarLbl(label96, label120);
        }

        private void Baltura_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    Calcular_Click();
                }
                catch (Exception)
                {
                }
            }
        }

        private void PVCaltura_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    CalcularPVC();
                }
                catch (Exception)
                {
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularPVC();
            InsertarClick(comboBox1.SelectedItem.ToString(),0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    CalcularLata();
                }
                catch (Exception)
                {
                }
            }
        }

        private void radioLam_CheckedChanged(object sender, EventArgs e)
        {
            CalcularLata();
        }

        private void radioSin_CheckedChanged(object sender, EventArgs e)
        {
            CalcularLata();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CalcularLata();
        }

        private void radioLam2_CheckedChanged(object sender, EventArgs e)
        {
            CalcularMagnetico();
        }

        private void radioSin2_CheckedChanged(object sender, EventArgs e)
        {
            CalcularMagnetico();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CalcularMagnetico();
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    CalcularMagnetico();
                }
                catch (Exception)
                {
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Aviso(sender, e);

        }

        void Aviso(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Esta seguro que desea cerrar Pixels Software?",
                      "Cerrar Programa", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    //frm2.Guardar();
                    break;
                case DialogResult.No:
                    e.Cancel = true;
                    break;
            }
        }

        double val = 0;

        private void dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {



        }

        int columna = 0;
        int celda = 0;

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();

                    MenuItem copiar = new MenuItem("Copiar");
                    MenuItem renombrar = new MenuItem("Renombrar");
                    MenuItem eliminar = new MenuItem("Eliminar");
                    MenuItem ponerImp = new MenuItem("Poner Impuesto");
                    MenuItem quitarImp = new MenuItem("Quitar Impuesto");


                    m.MenuItems.Add(copiar);
                    m.MenuItems.Add(renombrar);
                    m.MenuItems.Add(eliminar);
                    m.MenuItems.Add(ponerImp);
                    m.MenuItems.Add(quitarImp);

                    int currentMouseOverRow = dataGridView3.HitTest(e.X, e.Y).RowIndex;

                    /*if (currentMouseOverRow >= 0)
                    {
                        m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                    }*/

                    m.Show(dataGridView3, new Point(e.X, e.Y));

                    textBox37.Text = dataGridView3[e.ColumnIndex, e.RowIndex].Value.ToString();
                    celda = e.RowIndex;
                    columna = e.ColumnIndex;

                    copiarT(textBox37.Text.ToString());



                    eliminar.Click += new System.EventHandler(this.eliminar_Click);
                    copiar.Click += new System.EventHandler(this.copiar_Click);
                    renombrar.Click += new System.EventHandler(this.renombrar_Click);
                    ponerImp.Click += new System.EventHandler(this.ponerImp_Click);
                    quitarImp.Click += new System.EventHandler(this.quitarImp_Click);

                }
            }
            catch (Exception)
            {
            }
        }


        private void eliminar_Click(object sender, System.EventArgs e)
        {
            try
            {
                EliminarEntrada(celda, textBox37.Text);
            }
            catch (Exception)
            {
            }
        }

        void copiarT(String texto)
        {
            Clipboard.SetText(texto);
        }

        void CapturaCelda()
        {

        }

        private void copiar_Click(object sender, System.EventArgs e)
        {
            copiarT(textBox37.Text.ToString());
        }

        private void renombrar_Click(object sender, System.EventArgs e)
        {
            DataGridViewCell cell = dataGridView3.Rows[celda].Cells[columna];
            dataGridView3.CurrentCell = cell;
            dataGridView3.BeginEdit(true);

        }

        private void quitarImp_Click(object sender, System.EventArgs e)
        {
            OperacionImp(celda, columna, dataGridView3.Rows[celda].Cells[columna].Value.ToString(), "quitar");
        }

        private void ponerImp_Click(object sender, System.EventArgs e)
        {
            OperacionImp(celda, columna, dataGridView3.Rows[celda].Cells[columna].Value.ToString(), "poner");
        }

        string value = "";
        private void button46_Click(object sender, EventArgs e)
        {
            if (textBox38.Text.ToString().Equals(""))
            {
                MessageBox.Show("¡Necesita definir un cliente o titulo!", "¡No válido!");
            }
            else
            {
                EnviarCaja();
            }

        }

        void EnviarCaja()
        {
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    try
                    {
                        if (!cell.Value.ToString().Contains("¢"))
                        {
                            if (dataGridView3.Rows.Count <= 2)
                            {
                                value += cell.Value.ToString();
                            }
                            else
                            {
                                value += cell.Value.ToString() + " + ";
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
            }

            InsertDinero.TextData = label106.Text;
            InsertData.TextData = textBox38.Text + ": " + value;
            confirmadoClick.TextData = "click";

            frm2.Visible = true;
            value = "";
            InsertDinero.TextData = "";
            InsertData.TextData = "";
            confirmadoClick.TextData = "";
        }

        private void button45_Click(object sender, EventArgs e)
        {
            if (textBox38.Text.ToString().Equals(""))
            {
                MessageBox.Show("¡Necesita definir un cliente o titulo!", "¡No válido!");
            }
            else
            {
                EnviarCaja();
            }
        }

        private void dataGridView3_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dataGridView3.Rows.Count <= 2)
            {
                button45.Enabled = false;
                button46.Enabled = true;

            }
            else
            {
                button45.Enabled = true;
                button46.Enabled = false;
            }

        }

        private void button47_Click(object sender, EventArgs e)
        {
            suma = 0;
            label106.Text = "¢" + String.Format("{0:n}", suma);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            AjustarImpuestoTotal(label106.Text);
        }

        void QuitarImpuestoTotal(String valor)
        {
            valor = valor.Replace("¢", "");
            valor = valor.Replace(".", "");
            valor = valor.Replace(" ", "");

            String nuevo = label106.Text;

            nuevo = nuevo.Replace("¢", "");
            nuevo = nuevo.Replace(".", "");
            nuevo = nuevo.Replace(" ", "");

            double montoImpuesto = (imp / 100);
            double montoActual = Double.Parse(valor);

            double restaImpuesto = montoActual * montoImpuesto;

            suma = montoActual - restaImpuesto;

            label106.Text = "¢" + String.Format("{0:n}", suma);

            textBox36.Text = "";
        }

        void AjustarImpuestoTotal(String valor)
        {
            valor = valor.Replace("¢", "");
            valor = valor.Replace(".", "");
            valor = valor.Replace(" ", "");

            String nuevo = label106.Text;

            nuevo = nuevo.Replace("¢", "");
            nuevo = nuevo.Replace(".", "");
            nuevo = nuevo.Replace(" ", "");



            suma = Double.Parse(valor) * impuesto;

            label106.Text = "¢" + String.Format("{0:n}", suma);

            textBox36.Text = "";

        }

        private void button51_Click(object sender, EventArgs e)
        {
            QuitarImpuestoTotal(label106.Text);
        }

        string dinero = String.Empty;
        string valor = "";

        private void button48_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Archivo PDF|*.pdf", ValidateNames = true })
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);


                    try
                    {
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();

                        String imagen = Directory.GetCurrentDirectory().ToString() + "\\pixi_logo.png";

                        iTextSharp.text.Image tif = iTextSharp.text.Image.GetInstance(new Uri(imagen));

                        tif.ScalePercent(25f);
                        tif.Alignment = Left;
                        tif.IndentationLeft = 5f;
                        tif.SpacingAfter = 25f;


                        doc.Add(tif);

                        doc.Add(new iTextSharp.text.Paragraph("\n"));
                        doc.Add(new iTextSharp.text.Paragraph("\n"));

                        iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);

                        iTextSharp.text.Paragraph P = new iTextSharp.text.Paragraph(comboBox4.SelectedItem.ToString(), font);

                        P.Alignment = 2;

                        //doc.Add(new iTextSharp.text.Paragraph(comboBox4.SelectedItem.ToString()));

                        doc.Add(P);

                        PdfPTable tableFecha = new PdfPTable(3);
                        //PdfPCell cellFecha = new PdfPCell(new iTextSharp.text.Phrase(comboBox4.SelectedItem.ToString()));
                        //cellFecha.Colspan = 3;

                        tableFecha.WidthPercentage = 20;
                        tableFecha.SpacingBefore = 50f;
                        tableFecha.SpacingAfter = 10f;
                        //tableFecha.AddCell(cellFecha);
                        tableFecha.AddCell("DIA");
                        tableFecha.AddCell("MES");
                        tableFecha.AddCell("AÑO");
                        tableFecha.AddCell("" + fechaHoy.Day);
                        tableFecha.AddCell("" + fechaHoy.Month);
                        tableFecha.AddCell("" + fechaHoy.Year);
                        tableFecha.HorizontalAlignment = 2;

                        doc.Add(tableFecha);

                        doc.Add(new iTextSharp.text.Paragraph("\n"));
                        doc.Add(new iTextSharp.text.Paragraph("\n"));


                        PdfPTable datosUser = new PdfPTable(2);
                        datosUser.WidthPercentage = 100;
                        float[] widthsdata = new float[] { 3f, 1f };
                        datosUser.SetWidths(widthsdata);


                        datosUser.AddCell("NOMBRE DE LA EMPRESA: " + textBox38.Text);
                        datosUser.AddCell("TELEFONO: " + textBox23.Text);
                        datosUser.AddCell("DIRECCION: " + textBox21.Text);
                        datosUser.AddCell("CEDULA FISICA O JURIDICA: " + textBox22.Text);
                        datosUser.AddCell("DESCRIPCIÓN: " + textBox32.Text);
                        datosUser.AddCell("CORREO: " + textBox20.Text);
                        doc.Add(datosUser);

                        doc.Add(new iTextSharp.text.Paragraph("\n"));
                        doc.Add(new iTextSharp.text.Paragraph("\n"));


                        //Aqui se crea la tabla de cantidades
                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        //PdfPCell cell = new PdfPCell(new iTextSharp.text.Phrase("Header spanning 3 columns"));

                        //cell.Colspan = 4;
                        float[] widths = new float[] { 1f, 3f, 1f, 1f };

                        //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                        //table.AddCell(cell);
                        table.SetWidths(widths);

                        table.AddCell("CANTIDAD");

                        table.AddCell("CONCEPTO O DESCRIPCIÓN");

                        table.AddCell("PRECIO UNITARIO");

                        table.AddCell("TOTAL");


                        // For de los datos

                        for (int i = 0; i < dataGridView3.RowCount - 1; i++)
                        {
                            dinero = dataGridView3.Rows[i].Cells["montosColumna"].Value.ToString();
                            valor = dataGridView3.Rows[i].Cells["nombreColumna"].Value.ToString();

                            int cantidad = Int32.Parse(GetSubstringByString("(", ")", valor));
                            double unitario = 0;

                            table.AddCell("" + cantidad);

                            //Quitar la cantidad
                            int position = valor.IndexOf(") ");


                            table.AddCell(valor.Substring(position + 1));

                            dinero = dinero.Replace("¢", "");
                            dinero = dinero.Replace(".", "");

                            unitario = Double.Parse(dinero) / cantidad;


                            table.AddCell("¢" + String.Format("{0:n}", unitario));

                            table.AddCell("¢" + String.Format("{0:n}", dinero));




                        }

                        doc.Add(table);

                        PdfPTable finales = new PdfPTable(2);
                        finales.WidthPercentage = 30;
                        finales.SpacingBefore = 100f;
                        finales.SpacingAfter = 10f;
                        finales.AddCell("SUBTOTAL");
                        finales.AddCell(label106.Text);
                        finales.AddCell("I.V.A.");
                        finales.HorizontalAlignment = 2;



                        double elimpuesto = 0;

                        if (checkBox4.Checked == true)
                        {
                            elimpuesto = imp;
                        }
                        else
                        {
                            elimpuesto = 0;
                        }

                        finales.AddCell(elimpuesto + "%");

                        String elTotal = label106.Text;

                        elTotal = elTotal.Replace("¢", "");
                        elTotal = elTotal.Replace(".", "");
                        elTotal = elTotal.Replace(" ", "");

                        double elTotalD = Double.Parse(elTotal);
                        double ponerimp = (elimpuesto / 100) * elTotalD;

                        finales.AddCell("TOTAL");

                        finales.AddCell("¢" + String.Format("{0:n}", elTotalD + ponerimp));

                        doc.Add(new iTextSharp.text.Paragraph("\n"));
                        doc.Add(new iTextSharp.text.Paragraph("\n"));

                        PdfPTable exoneracion = new PdfPTable(2);
                        exoneracion.AddCell("COD. EXONERACION");
                        exoneracion.AddCell(textBox61.Text);

                        doc.Add(finales);

                        doc.Add(new iTextSharp.text.Paragraph("\n"));

                        doc.Add(exoneracion);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        doc.Close();

                        Process.Start(sfd.FileName);
                    }
                }
        }

        public string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        List<String> precios = new List<string>();

        private void button49_Click(object sender, EventArgs e)
        {
            CreateWordDocument();
        }


        void CreateWordDocument()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Archivo Word|*.docx", ValidateNames = true })
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var doc = DocX.Create(sfd.FileName);
                    var imagen = Directory.GetCurrentDirectory().ToString() + "\\pixi_logo.png";


                    Xceed.Document.NET.Image img = doc.AddImage(imagen);

                    Xceed.Document.NET.Picture p = img.CreatePicture();
                    p.Width = 350;
                    p.Height = 50;

                    /* 
                     
                     */

                    Xceed.Document.NET.Paragraph title = doc.InsertParagraph().Append("").FontSize(18);
                    title.Alignment = Xceed.Document.NET.Alignment.left;

                    // Insert a new Paragraph into the document.
                    Xceed.Document.NET.Paragraph p1 = doc.InsertParagraph();

                    // Append content to the Paragraph
                    p1.AppendPicture(p).Append("\t" + (comboBox4.Text)).Bold().FontSize(18);
                    //p1.AppendLine();

                    ///////////

                    //Xceed.Document.NET.Paragraph par = doc.InsertParagraph(comboBox4.Text).Bold().FontSize(20);
                    p1.Alignment = Xceed.Document.NET.Alignment.center;


                    Xceed.Document.NET.Table tfecha = doc.AddTable(2, 3);
                    tfecha.Alignment = Xceed.Document.NET.Alignment.right;
                    tfecha.SetWidthsPercentage(new[] { 7f, 7f, 7f }, 500);

                    tfecha.Rows[0].Cells[0].Paragraphs.First().Append("DIA");
                    tfecha.Rows[0].Cells[1].Paragraphs.First().Append("MES");
                    tfecha.Rows[0].Cells[2].Paragraphs.First().Append("AÑO");
                    tfecha.Rows[1].Cells[0].Paragraphs.First().Append("" + fechaHoy.Day);
                    tfecha.Rows[1].Cells[1].Paragraphs.First().Append("" + fechaHoy.Month);
                    tfecha.Rows[1].Cells[2].Paragraphs.First().Append("" + fechaHoy.Year);

                    doc.InsertParagraph(Environment.NewLine);

                    /*
                     * 
                     datosUser.AddCell("NOMBRE DE LA EMPRESA: "+textBox38.Text);
                        datosUser.AddCell("TELEFONO: "+textBox23.Text);
                        datosUser.AddCell("DIRECCION: "+textBox21.Text);
                        datosUser.AddCell("CEDULA FISICA O JURIDICA: "+textBox22.Text);
                        datosUser.AddCell("DESCRIPCIÓN: " + textBox32.Text);
                        datosUser.AddCell("CORREO: "+textBox20.Text);
                        doc.Add(datosUser);
                     */

                    Xceed.Document.NET.Table tcampos = doc.AddTable(3, 2);
                    tcampos.Alignment = Xceed.Document.NET.Alignment.both;
                    tcampos.SetWidthsPercentage(new[] { 60f, 35f }, 500);

                    tcampos.Rows[0].Cells[0].Paragraphs.First().Append("NOMBRE DE LA EMPRESA: " + textBox38.Text);
                    tcampos.Rows[0].Cells[1].Paragraphs.First().Append("TELEFONO: " + textBox23.Text);
                    tcampos.Rows[1].Cells[0].Paragraphs.First().Append("DIRECCION: " + textBox21.Text);
                    tcampos.Rows[1].Cells[1].Paragraphs.First().Append("CEDULA FISICA O JURIDICA: " + textBox22.Text);
                    tcampos.Rows[2].Cells[0].Paragraphs.First().Append("DESCRIPCIÓN: " + textBox32.Text);
                    tcampos.Rows[2].Cells[1].Paragraphs.First().Append("CORREO: " + textBox20.Text);


                    Xceed.Document.NET.Table t = doc.AddTable(dataGridView3.RowCount, 4);
                    t.Alignment = Xceed.Document.NET.Alignment.both;
                    t.SetWidthsPercentage(new[] { 14f, 45f, 18f, 18f }, 500);

                    for (int i = 0; i < dataGridView3.RowCount - 1; i++)
                    {

                        if (i == 0)
                        {
                            t.Rows[0].Cells[0].Paragraphs.First().Append("CANTIDAD");
                            t.Rows[0].Cells[1].Paragraphs.First().Append("DESCRIPCION");
                            t.Rows[0].Cells[2].Paragraphs.First().Append("PRECIO UNITARIO");
                            t.Rows[0].Cells[3].Paragraphs.First().Append("TOTAL");
                        }

                        /* 
                         
                     
                        */
                        dinero = dataGridView3.Rows[i].Cells["montosColumna"].Value.ToString();
                        valor = dataGridView3.Rows[i].Cells["nombreColumna"].Value.ToString();

                        int cantidad = Int32.Parse(GetSubstringByString("(", ")", valor));
                        double unitario = 0;

                        t.Rows[i + 1].Cells[0].Paragraphs.First().Append("" + cantidad);


                        //Quitar la cantidad
                        int position = valor.IndexOf(") ");

                        t.Rows[i + 1].Cells[1].Paragraphs.First().Append(valor.Substring(position + 1));

                        dinero = dinero.Replace("¢", "");
                        dinero = dinero.Replace(".", "");

                        unitario = Double.Parse(dinero) / cantidad;

                        t.Rows[i + 1].Cells[2].Paragraphs.First().Append("¢" + String.Format("{0:n}", unitario));
                        t.Rows[i + 1].Cells[3].Paragraphs.First().Append("¢" + String.Format("{0:n}", dinero));

                    }

                    Xceed.Document.NET.Table t2 = doc.AddTable(3, 2);
                    t2.Alignment = Xceed.Document.NET.Alignment.right;
                    t2.Rows[0].Cells[0].Paragraphs.First().Append("SUBTOTAL");
                    t2.Rows[0].Cells[1].Paragraphs.First().Append("" + label106.Text);
                    t2.Rows[1].Cells[0].Paragraphs.First().Append("I.V.A.");

                    double elimpuesto = 0;

                    if (checkBox4.Checked == true)
                    {
                        elimpuesto = imp;
                    }
                    else
                    {
                        elimpuesto = 0;
                    }

                    t2.Rows[1].Cells[1].Paragraphs.First().Append(elimpuesto + "%");


                    String elTotal = label106.Text;

                    elTotal = elTotal.Replace("¢", "");
                    elTotal = elTotal.Replace(".", "");
                    elTotal = elTotal.Replace(" ", "");

                    double elTotalD = Double.Parse(elTotal);
                    double ponerimp = (elimpuesto / 100) * elTotalD;

                    t2.Rows[2].Cells[0].Paragraphs.First().Append("TOTAL");
                    t2.Rows[2].Cells[1].Paragraphs.First().Append("¢" + String.Format("{0:n}", elTotalD + ponerimp));

                    //Orden del DOCX
                    doc.InsertTable(tfecha);

                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertParagraph(Environment.NewLine);

                    doc.InsertTable(tcampos);

                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertParagraph(Environment.NewLine);

                    doc.InsertTable(t);

                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertParagraph(Environment.NewLine);

                    doc.InsertTable(t2);

                    doc.InsertParagraph(Environment.NewLine);
                    doc.InsertParagraph(Environment.NewLine);

                    Xceed.Document.NET.Table t3 = doc.AddTable(1, 2);
                    t3.SetWidthsPercentage(new[] { 30f, 60f }, 500);
                    t3.Rows[0].Cells[0].Paragraphs.First().Append("COD. EXONERACION");
                    t3.Rows[0].Cells[1].Paragraphs.First().Append(textBox61.Text);

                    doc.InsertTable(t3);
                    /////////////////


                    doc.Save();

                    Process.Start("WINWORD.EXE", sfd.FileName);
                }

        }

        private void button52_Click(object sender, EventArgs e)
        {
            clickVerMonto();
        }

        int clickM = 0;
        bool estaOculto;

        void clickVerMonto()
        {

            clickM++;
            // Aqui el candado esta bloqueado
            estaOculto = true;

            button52.Image = Properties.Resources.visible_24px;

            label105.Visible = false;
            label106.Visible = false;
            label121.Visible = false;

            label89.Visible = false;
            textBox19.Visible = false;
            label97.Visible = false;
            label94.Visible = false;
            label96.Visible = false;
            label120.Visible = false;


            if (clickM > 1)
            {
                clickM = 0;
                button52.Image = Properties.Resources.mark_view_as_hidden_24px;

                label105.Visible = true;
                label106.Visible = true;
                label121.Visible = true;

                label89.Visible = true;
                textBox19.Visible = true;
                label97.Visible = true;
                label94.Visible = true;
                label96.Visible = true;
                label120.Visible = true;

                estaOculto = false;

            }


        }


        int clickA = 0;
        bool estaGrande;

        void hacerGrande()
        {

            clickA++;
            // Aqui el candado esta bloqueado
            estaGrande = true;

            button53.Image = Properties.Resources.visible_24px;

            panel4.Visible = false;
            dataGridView3.Height = 339;


            if (clickA > 1)
            {
                clickA = 0;
                button53.Image = Properties.Resources.mark_view_as_hidden_24px;

                panel4.Visible = true;
                dataGridView3.Height = 259;

                estaGrande = false;

            }


        }

        private void button53_Click(object sender, EventArgs e)
        {
            hacerGrande();
        }

        private void button54_Click(object sender, EventArgs e)
        {
            SumarRevision();
        }

        double sumatoria = 0;
        string eldinero = "";

        private void SumarRevision()
        {
            try
            {
                for (int i = 0; i < dataGridView3.RowCount - 1; i++)
                {
                    eldinero = dataGridView3.Rows[i].Cells["montosColumna"].Value.ToString();

                    eldinero = eldinero.Replace("¢", "");
                    eldinero = eldinero.Replace(".", "");

                    sumatoria += Double.Parse(eldinero);

                    label106.Text = "¢" + String.Format("{0:n}", sumatoria);

                }
                //MessageBox.Show("" + sumatoria);
                sumatoria = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error, revise sus datos.", "Error");
            }
        }

        double precioLaminacion = 0;

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex == 0)
            {
                precioLaminacion = 0;
                label122.Text = "¢" + String.Format("{0:n}", precioLaminacion);
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items.ElementAt(i).nombre.Contains(comboBox5.SelectedItem.ToString()))
                    {
                        precioLaminacion = items.ElementAt(i).precio;
                        label122.Text = "¢" + String.Format("{0:n}", precioLaminacion);

                    }
                }
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Cantidad();
        }

        void OcultarPubMateriales()
        {
            button18.Visible = false;
            button19.Visible = false;
            button20.Visible = false;

            txtinstalacion.Visible = false;
            txtimpresion.Visible = false;
            txtcorte.Visible = false;

            pictureBox18.Visible = true;
        }

        private void button55_Click(object sender, EventArgs e)
        {
            OcultarPubMateriales();
        }

        void MostrarPubMateriales()
        {
            button18.Visible = true;
            button19.Visible = true;
            button20.Visible = true;

            txtinstalacion.Visible = true;
            txtimpresion.Visible = true;
            txtcorte.Visible = true;

            pictureBox18.Visible = false;
        }

        private void button56_Click(object sender, EventArgs e)
        {
            MostrarPubMateriales();
        }

        private void pictureBox18_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.InitialDirectory = direccionImagenes;
            open.Filter = "Imagen (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                //Bitmap bit = new Bitmap(open.FileName);
                pictureBox18.Image = Image.FromFile(open.FileName);

                GuardarImgMat(open.FileName);
                LeerImgMat();
            }
        }

        void GuardarImgMat(String dir)
        {
            dir = dir.Replace("\\\\", "\\\\");
            dir = dir.Replace("\\", "\\\\");

            string query = "update imagen set img_materiales = '" + dir + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha guardado la imagen.");
        }

        void LeerImgMat()
        {
            string query = "select img_materiales from imagen";
            string url = "";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    url = reader.GetString(0).ToString();
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            try
            {
                pictureBox18.Image = Image.FromFile(url);
            }
            catch (Exception)
            {
            }

        }

        private void pictureBox13_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.InitialDirectory = direccionImagenes;
            open.Filter = "Imagen (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if (open.ShowDialog() == DialogResult.OK)
            {
                //Bitmap bit = new Bitmap(open.FileName);
                pictureBox13.Image = Image.FromFile(open.FileName);

                GuardarImgPVC(open.FileName);
                LeerImgPVC();
            }
        }

        void GuardarImgPVC(String dir)
        {
            dir = dir.Replace("\\\\", "\\\\");
            dir = dir.Replace("\\", "\\\\");

            string query = "update imagen set img_pvc = '" + dir + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha guardado la imagen.");

        }

        void LeerImgPVC()
        {
            string query = "select img_pvc from imagen";
            string url = "";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    url = reader.GetString(0).ToString();
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();

            }

            try
            {
                pictureBox13.Image = Image.FromFile(url);
            }
            catch (Exception)
            {
            }

        }

        void OcultarPubPVC()
        {
            button28.Visible = false;
            button29.Visible = false;
            button30.Visible = false;

            txtinstalacion2.Visible = false;
            txtimpresion2.Visible = false;
            txtcorte2.Visible = false;

            pictureBox13.Visible = true;
        }

        private void button58_Click(object sender, EventArgs e)
        {
            OcultarPubPVC();
        }

        void MostrarPubPVC()
        {
            button28.Visible = true;
            button29.Visible = true;
            button30.Visible = true;

            txtinstalacion2.Visible = true;
            txtimpresion2.Visible = true;
            txtcorte2.Visible = true;

            pictureBox13.Visible = false;
        }

        private void button57_Click(object sender, EventArgs e)
        {
            MostrarPubPVC();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int decimales = trackBar1.Value;


            label104.Text = decimales.ToString();

            GuardarDecimal();
            LeerDecimal();
        }

        int decimales = 0;

        void LeerDecimal()
        {
            string query = "select decimales from variable";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string text = reader.GetInt32(0).ToString();
                    val = Int32.Parse(text);
                    trackBar1.Value = Int32.Parse(text);
                    decimales = Int32.Parse(text);
                    label104.Text = "" + String.Format("{0:n}", val);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }



        String dirDecimal = Directory.GetCurrentDirectory().ToString() + "\\decimales.xml";

        void GuardarDecimal()
        {
            string query = "update variable set decimales = " + trackBar1.Value.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Decimales actualizados.");
        }



        private void button59_Click(object sender, EventArgs e)
        {
            if (textBox62.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un valor!");
            }
            else
            {
                GuardarSuma();
                LeerSuma();
            }
        }

        void LeerSuma()
        {
            string query = "select sumaExtra from variable;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    val = Double.Parse(reader.GetInt32(0).ToString());
                    label123.Text = "" + String.Format("{0:n}", val);
                    textBox62.Text = val.ToString();
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

        }

        void GuardarSuma()
        {
            string query = "update variable set sumaExtra = " + textBox62.Text.ToString() + ";";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Se ha guardado la suma extra.");
        }

        private void button60_Click(object sender, EventArgs e)
        {
            imagenEst = imagenEst.Replace("\\\\", "\\\\");
            imagenEst = imagenEst.Replace("\\", "\\\\");

            string query = "update estructura set cantidad = " + cantidadEst + ", descripcion = '" + descripcionEst + "', precio = " + precioEst + ", " +
                "imagen = '" + imagenEst + "' where id_estructura = " + idEst + " ;";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
                dbConnection.Close();
            }

            MessageBox.Show("Los datos se han actualizado!");

            Llenar();

        }

        String dirPixi = Directory.GetCurrentDirectory().ToString() + "\\Flexigraph Software Manager.exe";

        private void button61_Click_1(object sender, EventArgs e)
        {
            //string fileToCopy = dirCliente;
            //string newLocation = Directory.GetCurrentDirectory().ToString();

            //System.IO.File.Copy(fileToCopy, nuevoDireccionFichero, true);


            Process p = new Process();
            p.StartInfo.FileName = dirPixi;
            p.Start();

            Environment.Exit(0);
        }

        private void textBox63_TextChanged(object sender, EventArgs e)
        {
            clickCandado();
            clickCandado();
            Retro();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                double plata, tamano, transporte;
                plata = Double.Parse(textBox13.Text);
                transporte = Double.Parse(textBox12.Text);
                tamano = Double.Parse(textBox11.Text);
                double porc = Double.Parse(textBox18.Text);

                textBox17.Text = "" + (plata + transporte);
                AccionarLaminante();
            }
            catch (Exception)
            {
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox16.Text = "";
            textBox15.Text = "";

            textBox35.Text = "";
            textBox14.Text = "";
            textBox10.Text = "";

            textBox55.Text = "";
            textBox56.Text = "";
            textBox57.Text = "";
        }

        private void textBox64_TextChanged(object sender, EventArgs e)
        {
            GenerarDolar(textBox64, textBox65);
        }

        private void textBox19_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                CambioDatos();
            }
            catch (Exception)
            {
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            textBox37.Text = "";
        }

        void Modificar(String cantidad, String descripcion, String precio, String dirimagen, int fila)
        {
            string query = "update estructura set cantidad = " + cantidad + ", descripcion = '" + descripcion + "'," +
                " precio = " + precio + ", imagen = '" + dirimagen + "';";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                dbConnection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + e.StackTrace);
                dbConnection.Close();
            }

            MessageBox.Show("Registro Editado!", "¡Exito!");

        }

        void Eliminar_Fila(int fila)
        {
            try
            {
                if (fila >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[fila];
                    String cantidad = row.Cells["Cantidad"].Value.ToString();
                    String descripcion = row.Cells["Descripción"].Value.ToString();
                    String precio = row.Cells["Precio"].Value.ToString();
                    String imagen = row.Cells["Imagen"].Value.ToString();

                    EliminarL(fila, descripcion);
                    item3 = null;
                    dataGridView1.Rows.RemoveAt(fila);
                    Llenar();
                    MessageBox.Show("Registro Eliminado!", "¡Exito!");
                    Resetea();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.Message);
            }
        }

        void Resetea()
        {
            pictureBox15.Image = null;
            richTextBox2.Clear();
        }

        void EliminarL(int fila, string nombre)
        {
            try
            {
                /*doc.Load(dirLista);

                XmlNodeList listaClientes = doc.SelectNodes("ListaEstructura/Lista");
                XmlNode unCliente;

                unCliente = listaClientes.Item(fila);

                XmlNodeList elemList = doc.GetElementsByTagName("Lista");

                if (unCliente.SelectSingleNode("Descripción").InnerText == item3)
                {
                    unCliente.ParentNode.RemoveChild(unCliente);
                    doc.Save(dirLista);
                }*/

                Llenar();

            }
            catch (Exception)
            {
            }
        }

    }

}

