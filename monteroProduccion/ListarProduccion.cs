using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    public partial class ListarProduccion : Form
    {   //MENSAJE DE CARGA
        private static System.Timers.Timer aTimer;
        public static string mensaje = "";

        public static DateTime fecha = DateTime.Now;
        public static string anno;

        private readonly DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
        private readonly DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();

        public static int cbx1 =0;
        public ListarProduccion()
        {
            InitializeComponent();
           
            LlenarComboBoxMaestro_();
            dataGridView1.Columns.Add("id", "Id");//0
            dataGridView1.Columns.Add("ft", "Fecha");//1
            dataGridView1.Columns.Add("mes", "Mes");//2
            dataGridView1.Columns.Add("maestro", "Maestro");
            dataGridView1.Columns.Add("mprod", "Nº Prod");
            dataGridView1.Columns.Add("producto", "Producto");
            dataGridView1.Columns.Add("capacidad", "Cap.");
            dataGridView1.Columns.Add("desc_ac", "Des./ Acabado");
            dataGridView1.Columns.Add("color", "Color");
            dataGridView1.Columns.Add("medidas", "Medidas");
            dataGridView1.Columns.Add("serie", "Serie");
            dataGridView1.Columns.Add("mod_motor", "Mod/ Motor");
            dataGridView1.Columns.Add("serie_motor", "Serie/ Motor");//12
            dataGridView1.Columns["id"].Visible = false;
            //agregar una columna con boton
            dataGridView1.ColumnCount = 13;
            addButtonColumn();
            dataGridView1.Columns.Add(btn);
           
            //agregar una columna con boton eliminar
            dataGridView1.ColumnCount = 14;
            addButtonColumn2();
            dataGridView1.Columns.Add(btn2);
            //----------------------------------------//
            fecha = DateTime.Now;
            anno = Convert.ToString(fecha.Year);
            PeriodoCbx.Text = anno; //SELECCIONAR EL AÑO POR DEFECTO

            cargarProduccion(dataGridView1, "",anno);
            //NuevoRegistro.datagrid = dataGridView1;
            FormBuscar.datagrid = dataGridView1;
            //EditContact.datagrid = dataGridView1;
            Nprod.datagrid = dataGridView1;
            NprodEdit.datagrid = dataGridView1;
            //Nprod.anno=anno;
        }

        private void addButtonColumn()
        {
            btn.HeaderText = @"Acccion";
            btn.Name = "button";
            btn.Text = "Editar";
            btn.UseColumnTextForButtonValue = true;
        }
        private void addButtonColumn2()
        {
            btn2.HeaderText = @"Acccion";
            btn2.Name = "button2";
            btn2.Text = "Eliminar";
            btn2.UseColumnTextForButtonValue = true;
        }
        private void PeriodoCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            cargarProduccion(dataGridView1,"", PeriodoCbx.Text);
        }
        public void LlenarComboBoxMaestro_()
        {
            MySqlConnection con = conex.con();
            string selectQuery = "SELECT idTecnico,nombre,tipo FROM tecnico WHERE tipo=1";
            con.Open();
            MySqlCommand command = new MySqlCommand(selectQuery, con);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable tablaM = new DataTable("nMaestros");
            tablaM.Columns.Add("maestro", typeof(string));
            tablaM.Columns.Add("id_maestro", typeof(int));
            tablaM.Rows.Add("FILTRAR POR MAESTRO", 0); // eñl primer id en blanco
            while (reader.Read())
            {
                DataRow row = tablaM.NewRow();
                row["maestro"] = reader.GetString("idTecnico") + " | " + reader.GetString("nombre");
                row["id_maestro"] = reader.GetString("idTecnico");
                tablaM.Rows.Add(row);
            }
            cbx.DropDownStyle = ComboBoxStyle.DropDownList;
            cbx.DisplayMember = "maestro";
            cbx.ValueMember = "id_maestro";
            cbx.DataSource = tablaM;
            con.Close();
            
        }
        private void cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbx.Text!="") {   
            Int32.TryParse(cbx.SelectedValue.ToString(), out cbx1);
            //cbx1 = cbx.SelectedValue.ToString(); //traemos el id maestro del combobox
            cargarProduccion(dataGridView1, "", PeriodoCbx.Text);
           
            }
         
        }
        public static void cargarProduccion(DataGridView dg, string nomxx,string periodo)
        {
            SetTimer();
            String integrar = "";
            if (cbx1 == 0){
                integrar = "";
            } else {
                integrar = " AND maestro='"+cbx1+"'";
            }

            dg.Rows.Clear();
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            String campos = "id_p,fecha,nombreMaestro,corelativo,nombreProducto,cap_vid,color,acabado,medidas,serie,modelo_motor,serie_motor,destino,guia";
            cmd.CommandText = "select "+campos+" from produccion WHERE serie LIKE '%" + nomxx + "%' AND YEAR(produccion.fecha)='" + periodo+"'"+integrar;
            con.Open();
            
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                String fecha = reader.GetString(1);
                String maestro = reader.GetString(2); 
                String num_pro = reader.GetString(3);
                String producto = reader.GetString(4);
                String cap_vid = reader.GetString(5);
                String color = reader.GetString(6);
                String acabado = reader.GetString(7);
                String medidas = reader.GetString(8);
                String serie = reader.GetString(9);
                String modelo_motor = reader.GetString(10);
                String serie_motor = reader.GetString(11);
                String destino = reader.GetString(12);
                String guia = reader.GetString(13);
                String fecha_ddmmyyyy = fecha.Split(' ')[0];
                String mes = funciones.Mes(fecha_ddmmyyyy.Split('/')[1]);
                String t = num_pro.PadLeft(3, '0');//ante poner 3 ceros al numero
                //string fecha_de_prueba = fecha;
                dg.Rows.Add(id, fecha_ddmmyyyy, mes, maestro, "Produccion# "+t, producto, cap_vid,acabado, color, medidas, serie, modelo_motor, serie_motor);

            }
            reader.Close();
            con.Close();

            aTimer.Stop();
            aTimer.Dispose();
            im = 0;
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }
        public static int im = 0, b = 0;
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            im++;
            cargando.b = im;
            cargando.Rmensaje = mensaje;
            cargando frm = new cargando();
            frm.ShowDialog();

            //modo de acceder deun metodo estatico a uno nno estatico
            //logo cls1 = new logo();
            //cls1.reportarProgreso(100, im);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == dataGridView1.Columns["button"].Index)
            {
                int c_id = int.Parse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                NprodEdit.contact_id = c_id;
                NprodEdit ec = new NprodEdit();
                ec.ShowDialog();
            }

           if (e.ColumnIndex == dataGridView1.Columns["button2"].Index)
            {

                //PRIMERO VALIDAMOS PARA ELIMINAR EL ULTIMO REGISTRO
                if (dataGridView1.CurrentRow == null)
                    return;

                var lastRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];

                if (dataGridView1.CurrentRow == lastRow)
                {
                    if (MessageBox.Show("Estas seguro de eliminar este registro ?", "Eliminar registro", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int c_ = int.Parse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());

                        if (Eliminar(c_) > 0)
                        {
                            MessageBox.Show("Registro Eliminado Correctamente!", "Registro Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cargarProduccion(dataGridView1, "",anno);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el Registro", "Registro No Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("NECESITAS ELIMINAR EL ULTIMO REGISTRO");
                }

             }


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex ==4)//precional cualquier dato de la columna 4
            {
             int id_P = int.Parse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString()); //seleccionamos id
              MasInfo.idProduccion = id_P;
              MasInfo llamar = new MasInfo();
              llamar.ShowDialog();        
            }


            if (e.ColumnIndex == 10)
            {
                if (!EsNumerico(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString()))
                {
                    string c_idx = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[10].Value.ToString();

                    verImagen.serie = c_idx;
                    verImagen abrir = new verImagen();
                    abrir.ShowDialog();
                    //MessageBox.Show("ESTA  NO ES UN DATO VALIDO    "+ c_idx);
                }
                else
                {
                    MessageBox.Show("ESTA  NO ES UN DATO VALIDO");
                }

            }


         }

        private void label1_Click(object sender, EventArgs e)
        {

        }

       
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nprod frm = new Nprod();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormBuscar frm = new FormBuscar();
            frm.ShowDialog();
        }
        
        public static int Eliminar(int pId)
        {
            int retorno = 0;
            MySqlConnection conexion = conex.con();
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(string.Format("DELETE FROM produccion WHERE id_p={0}", pId), conexion);

            retorno = comando.ExecuteNonQuery();
            conexion.Close();

            return retorno;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exportar exp = new Exportar();
            exp.ExportarDataGridViewExcel(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected bool EsNumerico(string val)
        {
            Regex _isNumber = new Regex(@"^d+$");
            Match m = _isNumber.Match(val);
            return m.Success;

        }
       
    }
}
