using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace monteroProduccion
{
    public partial class Nprod : Form
    {
        private static System.Timers.Timer aTimer;
        public static string mensaje = "";

        public String modelo = "";
        public static DataGridView datagrid;

        public static DateTime fechan = DateTime.Now;
        public static string annno;
        public Nprod()
        {
            InitializeComponent();
            //habre el mensaje de cargagando
            SetTimer();

            LlenarComboBoxMaestro();
            LlenarComboBoxTipo();
            LlenarComboBoxCatalogo();
           
            p1_.Enabled = false; serie.Enabled = false; medidastxt.Enabled = false;
            p2_.Enabled = false; maestroxx.Visible = false;
            p3_.Enabled = false; nombretxt.Visible = false;
            p4_.Enabled = false; cap_vidtxt.Enabled = false; acabadotxt.Enabled = false;
            //using (var dialogo = new cargando(LlenarComboBoxTipo, "hasta que hora esta webada"))
            //{
            //    dialogo.ShowDialog(this);
            //}
            //termina de cargar todo y cierra el mensaje
            aTimer.Stop();
            aTimer.Dispose();
            im = 0;
        }
   
        private void LlenarComboBoxTipo()
        {
            MySqlConnection conX = conex.con();
            //=====================================================================
            string queryX = "SELECT id_tipo,descripcion FROM produccion_tipo";
            MySqlCommand cmdX;
            cmdX = new MySqlCommand(queryX, conX);
            MySqlDataAdapter da3 = new MySqlDataAdapter(cmdX);
            DataTable table3 = new DataTable("produccion_tipo");
            da3.Fill(table3);
            ListarTipoProducto.ValueMember = "id_tipo";
            ListarTipoProducto.DisplayMember = "descripcion";
            ListarTipoProducto.DataSource = table3;
            conX.Close();
         

        }
        private void ListarTipoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {

            LlenarComboBoxCatalogo();

        }
        private void LlenarComboBoxCatalogo()
        {
        int id_Tipo; //traemos el id del combobox tipo
        Int32.TryParse(ListarTipoProducto.SelectedValue.ToString(), out id_Tipo);
        if (ListarTipoProducto.SelectedItem.ToString() != "" && id_Tipo != 0)
        {
                MySqlConnection conc = conex.con();
                string selectQuery = "SELECT id_cat,nombre,modelo,medidas,cap_vid FROM produccion_cat WHERE tipo='" + id_Tipo + "'";
                conc.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conc);

                MySqlDataReader reader = command.ExecuteReader();
                DataTable tabla = new DataTable("nData");
                tabla.Columns.Add("titulo", typeof(string));
                tabla.Columns.Add("id_catalogo", typeof(int));

                while (reader.Read())
                {
                    DataRow row = tabla.NewRow();
                    row["titulo"] = reader.GetString("nombre") + " | " + reader.GetString("modelo") + " | " + reader.GetString("medidas") + " | " + reader.GetString("cap_vid");
                    row["id_catalogo"] = reader.GetString("id_cat");
                    tabla.Rows.Add(row);
                }
                comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox2.DisplayMember = "titulo";
                comboBox2.ValueMember = "id_catalogo";
                comboBox2.DataSource = tabla;
                conc.Close();
                aver();
            }
            else
            {
                MessageBox.Show("ERROR NO SE ENCUENTRA DATOS EN EL COMBOBOX");
            }
        }
        private void LlenarComboBoxMaestro()
        {
            MySqlConnection con = conex.con();
            //=====================================================================
            string query = "select idTecnico,nombre,tipo from tecnico WHERE tipo=1";
            MySqlCommand cmd;

            cmd = new MySqlCommand(query, con);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable("tecnico");
            da.Fill(table);
            comboBox1.ValueMember = "idTecnico";
            comboBox1.DisplayMember = "nombre";
            comboBox1.DataSource = table;
            con.Close();
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection con = conex.con();
            if (dateTimePicker1.Text != "" && numericUpDown1.Value != 0 && comboBox1.Text != "" && obs_.Text != "" && ser_mot.Text != "" && comboBox2.Text !="")
            {
                int nVenta = 0;
                if (!existe.Existe()){ //PRIMERO VEEMOS SI HAY ALGUNN DATO, SI NO HAY LE ASIGNAMOS 1
                   nVenta = 1;
                 }else{
                   nVenta = existe.maximo() + 1;
                }

                MySqlCommand cmd = con.CreateCommand();
                String cbx1 = comboBox1.SelectedValue.ToString(); //traemos el id del select
                String cbx2 = comboBox2.SelectedValue.ToString(); //traemos el id del select
                String mifecha = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                String hora = dateTimePicker2.Value.ToString("hh:mm");
                String f_i = fecha_i.Value.ToString("yyyy/MM/dd hh:mm:ss");
                String f_f = fecha_f.Value.ToString("yyyy/MM/dd hh:mm:ss");
                string respon = login_.nombreCompleto;
                cmd.CommandText = "INSERT INTO produccion (fecha,maestro,num_pro,id_producto,color,obs_,serie,modelo_motor,serie_motor,destino,guia,p1,p2,p3,p4,corelativo,nombreMaestro,nombreProducto,cap_vid,medidas,acabado,fecha_i,fecha_f,responsable) value ";
                cmd.CommandText += "(\"" + string.Concat(mifecha, " " + hora) + "\",\"" + cbx1 + "\",\"" + numericUpDown1.Value + "\",\"" + cbx2 + "\",\"" + color.Text + "\",\"" + obs_.Text + "\",\"" + serie.Text + "\",\"" + m_mot.Text + "\",\"" + ser_mot.Text + "\",\"" + destino.Text + "\",\"" + n_g.Text + "\",\"" + p1_.Text + "\",\"" + p2_.Text + "\",\"" + p3_.Text + "\",\"" + p4_.Text + "\",\"" + nVenta + "\",\"" + maestroxx.Text + "\",\"" + nombretxt.Text + "\",\"" + cap_vidtxt.Text + "\",\"" + medidastxt.Text + "\",\"" + acabadotxt.Text + "\",\"" + f_i + "\",\"" + f_f + "\",\"" + respon + "\")";
                con.Open();
                cmd.ExecuteNonQuery();
                color.Text = ser_mot.Text = destino.Text = n_g.Text = "";
                MessageBox.Show("SE REGISTRO CORRECTAMENTE....!");

                con.Close();

                fechan = DateTime.Now;
                annno = Convert.ToString(fechan.Year);
                ListarProduccion.cargarProduccion(datagrid,"",annno);
                aver();
            }
            else {
                MessageBox.Show("Debes introducir Datos Obligatotios!");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int val;
            Int32.TryParse(comboBox2.SelectedValue.ToString(), out val);
            if (comboBox2.SelectedItem.ToString() != "")
            {
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT medidas,cap_vid,modelo,p_soles,i_soles,seis_letras,doce_letras,nombre,acabado FROM produccion_cat WHERE id_cat=" + val;
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                String p1= "", cap_vid = "", medidas="";
                String p2 = "", nombre = "", acabado = "";
                String p3 = "";
                String p4 = "";

                while (reader.Read())
                {
                    modelo = reader.GetString(2);
                    p1 = reader.GetString(3); cap_vid = reader.GetString("cap_vid");
                    p2 = reader.GetString(4); medidas = reader.GetString("medidas");
                    p3 = reader.GetString(5); nombre = reader.GetString("nombre");
                    p4 = reader.GetString(6); acabado = reader.GetString("acabado");
                }
                con.Close();
                p1_.Text = p1; nombretxt.Text = nombre; 
                p2_.Text = p2; cap_vidtxt.Text = cap_vid;
                p3_.Text = p3; medidastxt.Text = medidas;
                p4_.Text = p4; acabadotxt.Text = acabado;
                aver();
            }

    }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            int idMaestro;
            Int32.TryParse(comboBox1.SelectedValue.ToString(), out idMaestro);
            if (comboBox1.SelectedItem.ToString() != "")
            {
                MySqlConnection con2 = conex.con();
                MySqlCommand cmd2 = con2.CreateCommand();
                cmd2.CommandText = "SELECT nombre  FROM tecnico WHERE idTecnico=" + idMaestro;
                con2.Open();

                MySqlDataReader buscar = cmd2.ExecuteReader();
                String maetro = "";
                if (buscar.Read())
                {
                    maetro = buscar.GetString("nombre");
                }
                con2.Close();
                maestroxx.Text = maetro;
                aver();
            }
        }
        
      private void aver()
        {
            int val1;
            Int32.TryParse(comboBox1.SelectedValue.ToString(), out val1);
            //int val2;
            //Int32.TryParse(comboBox2.SelectedValue.ToString(), out val2);
            //if (comboBox1.SelectedItem.ToString() != "")
            //{
            serie.Text = generarSerie.generar(val1, modelo);
            VerSerie.Text = serie.Text;
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
      
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







    }
}
