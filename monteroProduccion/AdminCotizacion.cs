using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    public partial class AdminCotizacion : Form
    {
        public AdminCotizacion()
        {
            InitializeComponent();
            tabla.Columns.Add("Descripcion", "Descripcion");
            tabla.Columns.Add("Cantidad", "Cantidad");
            tabla.Columns.Add("Precio", "Precio");
            tabla.Columns.Add("Importe", "Importe");
        }
        //public void PrepararDgv(DataGridView dgv)
        //{
            
        //}
        private void Agregar_Click(object sender, EventArgs e)
        {
            if (Descripcion.Text != "" && precio.Value != 0 && cantidad.Value != 0)
            {
                decimal importe = cantidad.Value * precio.Value;
                tabla.Rows.Add(Descripcion.Text, precio.Value, cantidad.Value,importe);
            }
            else
            {
                MessageBox.Show("Completa los campos");
            }
        }
        public bool GuardarRegistrosBD(DataGridView dgv)
        {
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO prooduccion_cotiza_detalle VALUES(?descripcion,?cantidad,?precio) value ";
            //                cmd.CommandText += "(\"" + string.Concat(mifecha, " " + hora) + "\",\"" + cbx1 + "\",\"" + numericUpDown1.Value + "\",\"" + cbx2 + "\",\"" + color.Text + "\",\"" + obs_.Text + "\",\"" + serie.Text + "\",\"" + m_mot.Text + "\",\"" + ser_mot.Text + "\",\"" + destino.Text + "\",\"" + n_g.Text + "\",\"" + p1_.Text + "\",\"" + p2_.Text + "\",\"" + p3_.Text + "\",\"" + p4_.Text + "\",\"" + nVenta + "\",\"" + maestroxx.Text + "\",\"" + nombretxt.Text + "\",\"" + cap_vidtxt.Text + "\",\"" + medidastxt.Text + "\",\"" + acabadotxt.Text + "\",\"" + f_i + "\",\"" + f_f + "\",\"" + respon + "\")";
            //              con.Open();
            //                cmd.ExecuteNonQuery();

            //                con.Close();

            int i;
            bool guardo = false;
            int numreg = 0;
            con.Open();
            for (i = 0; i <= dgv.Rows.Count - 1; i++)
            {
                
                    //cmd.Parameters.Clear();
                    //cmd.Parameters.Add("@descripcion", MySqlDbType.Varchar).Value = dgv.Rows[i].Cells[0].Value.ToString();
                    //cmd.Parameters.Add("@cantidad", MySqlDbType.VarChar).Value = dgv.Rows[i].Cells[1].Value.ToString();
                    //cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = dgv.Rows[i].Cells[2].Value.ToString();
                    //numreg += cmd.ExecuteNonQuery();
                
            }
            if (numreg > 0)
            {
                guardo = true;
            }
            else
            {
                guardo = false;
            }
            con.Close();
            return guardo;
        }

        private void guardar_Click(object sender, EventArgs e)
        {
            if (tabla.Rows.Count > 0)
            {
                if (GuardarRegistrosBD(tabla))
                {
                    MessageBox.Show("Registros guardados");
                    tabla.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("No se pudieron guardar los registros");
                }
            }

        }
    }
}


