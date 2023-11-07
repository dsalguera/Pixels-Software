using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixels_Software
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dbConnection = new MySqlConnection(MySQLConnectionString);
            Mostrar();
        }

        public string TextBoxText
        {
            get { return textoPasar; }
        }

        string MySQLConnectionString = ConfigurationManager.ConnectionStrings["PixiConnection"].ConnectionString;
        MySqlConnection dbConnection;

        void Mostrar()
        {
            string query = "select id_registro, " +
                "nombre as Nombre, " +
                "correo as Correo, " +
                "telefono as Teléfono, " +
                "cedula as Cédula, " +
                "descripcion as Descripción, total as Total, archivo as Archivo, fecha_generado as 'Generado El' from registro";

            MySqlCommand command = new MySqlCommand(query, dbConnection);
            command.CommandTimeout = 60;

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = command;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);

                //Se oculta la primera columna
                dataGridView1.Columns["id_registro"].Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message + "\n\n" + e.StackTrace);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Close();
        }

        String textoPasar = "";

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textoPasar = row.Cells[1].Value.ToString();
                //form1.Show();
            }
        }

        /*void Guardar()
        {
            string query = "insert into registro(nombre, correo, telefono, cedula, descripcion, total, archivo)values(" +
                "'" + txtnombre.Text + "'," +
                "'" + txtcorreo.Text + "'," +
                "'" + txttelefono.Text + "'," +
                "'" + txtcedula.Text + "'," +
                "'" + txtdireccion.Text + "'," +
                "" + txttotal.Text + "," +
                "'" + txtarchivo.Text + "');";

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
            }
        }*/

    }
}
