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
    public partial class TipoProd : Form
    {
        private readonly DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
        public TipoProd()
        {
            InitializeComponent();
            tablaDatos.Columns.Add("id", "Id");
            tablaDatos.Columns.Add("descripcion", "DESCRIPCION");
            cargarProduccion(tablaDatos, "");
            //agregar una columna con boton eliminar
            tablaDatos.ColumnCount = 2;
            addButtonColumn();
            tablaDatos.Columns.Add(btn);
            //desabilitar botones
            id_.Visible = false;
            botonEditar.Enabled = false;
            butonElimar.Enabled = false;
            butonCancelar.Enabled = false;
        }
        private void addButtonColumn()
        {
            btn.HeaderText = @"Acccion";
            btn.Name = "button";
            btn.Text = "Seleccionar";
            btn.UseColumnTextForButtonValue = true;
        }
        public static void cargarProduccion(DataGridView dg, string nomxx)
        {
            dg.Rows.Clear();
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM produccion_tipo WHERE  descripcion LIKE '%" + nomxx + "%'";
            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32("id_tipo");
                String desc = reader.GetString("descripcion");
            
                dg.Rows.Add(id,desc);
            }
            reader.Close();
            con.Close();
        }
        private void botonCrear_Click(object sender, EventArgs e)
        {

        }

        private void cajaBuscar_TextChanged(object sender, EventArgs e)
        {
            cargarProduccion(tablaDatos, cajaBuscar.Text);
        }

        private void tablaDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == tablaDatos.Columns["button"].Index)
            {
                int c_id = int.Parse(tablaDatos.Rows[tablaDatos.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                botonCrear.Enabled = false; butonElimar.Enabled = true; butonCancelar.Enabled = true; botonEditar.Enabled = true;
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM produccion_tipo WHERE id_tipo=" + c_id;
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                int id = 0;
                String descripcion = "";
                
                if (reader.Read())
                {
                    id = reader.GetInt32("id_tipo"); descripcion = reader.GetString("descripcion");
                }
                id_.Text = id.ToString();
                cajaDescripcion.Text = descripcion;
                con.Close();
            }
        }

        private void butonCancelar_Click(object sender, EventArgs e)
        {
            cargarProduccion(tablaDatos, "");
            cajaDescripcion.Text = "";
            id_.Text = "";
            botonCrear.Enabled = true;
            botonEditar.Enabled = false;
            butonElimar.Enabled = false;
            butonCancelar.Enabled = false;
        }

        private void botonCrear_Click_1(object sender, EventArgs e)
        {
           
            if (funciones.ExisteTipo(cajaDescripcion.Text))
            {
                MessageBox.Show("NO SE PERMITEN DATOS DUPLICADO");
            }
            else
            {
                if (cajaDescripcion.Text != "")
                {
                    MySqlConnection con = conex.con();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "INSERT INTO produccion_tipo (descripcion) value ";
                    cmd.CommandText += "(\"" + cajaDescripcion.Text + "\")";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    cajaDescripcion.Text = "";
                    MessageBox.Show("SE REGISTRO CORRECTAMENTE....!");

                    con.Close();

                    cargarProduccion(tablaDatos, "");
                    cajaDescripcion.Clear();
                }
                else
                {
                    MessageBox.Show("Debes introducir Una Descripcion!");
                }
            }
        }

        private void botonEditar_Click(object sender, EventArgs e)
        {
            if (funciones.ExisteTipo(cajaDescripcion.Text))
            {
                MessageBox.Show("NO SE PERMITEN DATOS DUPLICADO");
            }
            else
            {
                if (cajaDescripcion.Text != "" && id_.Text != "")
                {
                    MySqlConnection con = conex.con();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "UPDATE produccion_tipo SET ";
                    cmd.CommandText += "descripcion=\"" + cajaDescripcion.Text + "\" WHERE id_tipo=" + id_.Text;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Actualizado exitosamente!");
                    con.Close();

                    cargarProduccion(tablaDatos, "");

                }
                else
                {
                    MessageBox.Show("Debes introducir Una Descripcion!");
                }
            }
        }

        private void butonElimar_Click(object sender, EventArgs e)
        {
           
            if (id_.Text != "")
            {
                MySqlConnection conexion = conex.con();
                conexion.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("DELETE FROM produccion_tipo WHERE id_tipo={0}", id_.Text), conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
                
                
                botonCrear.Enabled = true;
                botonEditar.Enabled = false;
                butonElimar.Enabled = false;
                butonCancelar.Enabled = false;
                cajaDescripcion.Text = "";
                id_.Text = "";
                MessageBox.Show("Registro Eliminado!");
                cargarProduccion(tablaDatos, "");
            }
            else
            {
                MessageBox.Show("Ocurrio un Error!");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
