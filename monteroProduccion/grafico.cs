using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    public partial class grafico : Form
    {
        public grafico()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ////comboBox2.Items.Clear();
            if (comboBox1.SelectedItem.ToString() != "")
            {
                chart1.Series["Series1"].LegendText = "Año : " + comboBox1.Text;
                chart1.Series["Series1"].LabelToolTip = "Año : " + comboBox1.Text;
                //chart1.Titles.Add("Salary Chart");
                chart1.Series["Series1"].XValueMember = "nombre";
                chart1.Series["Series1"].YValueMembers = "cantidad";
                chart1.DataSource = EnviarDatos("SELECT COUNT(produccion.maestro) as cantidad, tecnico.nombre FROM produccion,tecnico WHERE YEAR(produccion.fecha)=" + comboBox1.Text + " AND tecnico.idTecnico=produccion.maestro GROUP BY produccion.maestro;");

                chart2.Series["Series1"].LegendText = "Año : " + comboBox1.Text;
                chart2.Series["Series1"].XValueMember = "nombre";
                chart2.Series["Series1"].YValueMembers = "p1c";
                chart2.DataSource = EnviarDatos2("SELECT SUM(produccion.p1) as p1c, tecnico.nombre FROM produccion,tecnico WHERE YEAR(produccion.fecha)=" + comboBox1.Text + " AND tecnico.idTecnico=produccion.maestro GROUP BY produccion.maestro;");

            }
            else
            {
                //comboBox2.Items.Add("Test1");
                //comboBox2.Items.Add("Test2");
            }

        }

        public DataTable EnviarDatos(string consulta)
        {
            MySqlConnection con = conex.con();
            DataTable tabla = new DataTable();
            MySqlDataAdapter mda = new MySqlDataAdapter(consulta, con);
            mda.Fill(tabla);
            return tabla;
            //MessageBox.Show("SE REGISTRO CORRECTAMENTE!   " + tabla);
        }
        public DataTable EnviarDatos2(string consulta)
        {
            MySqlConnection con = conex.con();
            DataTable tabla = new DataTable();
            MySqlDataAdapter mda = new MySqlDataAdapter(consulta, con);
            mda.Fill(tabla);
            return tabla;
            //MessageBox.Show("SE REGISTRO CORRECTAMENTE!   " + tabla);
        }

        private void grafico_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("cantidad");

            //DataTable dt2 = new DataTable();
            //dt2.Columns.Add("ganancia");

            //for (int i = 0; i < 4; i++)
            //{
            //    DataRow dr = dt.NewRow();
            //    DataRow dr2 = dt2.NewRow();

            //    dr[0] = i.ToString();
            //    dr2[0] = (i * i).ToString();

            //    dt.Rows.Add(dr);
            //    dt2.Rows.Add(dr2);
            //}

            //if (chart1.Series.Count > 0)
            //{
            //    for (int i = chart1.Series.Count - 1; i >= 0; i--)
            //        chart1.Series[i].Dispose();
            //}

            //chart1.Series.Clear();

            //chart1.Series.Add("dt");
            //chart1.Series.Add("dt2");

            //chart1.Series[0].Points.DataBindY(dt.DefaultView);
            //chart1.Series[1].Points.DataBindY(dt2.DefaultView);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }
