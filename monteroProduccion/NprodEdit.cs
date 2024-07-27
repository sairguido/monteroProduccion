using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monteroProduccion
{
public partial class NprodEdit : Form
    {
        public static DataGridView datagrid;
        public String modelo = "";
        public string correla = "";
        public String inicialesTecnico="";
        public static int contact_id = 0;

        public static DateTime fechan = DateTime.Now;
        public static string annno;
        public NprodEdit()
        {
            InitializeComponent();
           
            if (contact_id == 0){
                this.Close();
            }
            ComboboxMaestro();
            ComboboxTipo();
            ComboboxCatalogo();
            
            serie__.Enabled = false; p1_e.Enabled = false; p2_e.Enabled = false; p3_e.Enabled = false; p4_e.Enabled = false;
            maestroxx_.Visible = false; nombretxt_.Visible = false; cap_vidtxt_.Enabled = false; medidastxt_.Enabled = false; acabadotxt_.Enabled = false;
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM produccion WHERE id_p=" + contact_id;
            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            int id = 0, correlativo = 0, p4 = 0, p1 = 0, p2 = 0, p3 = 0;
            String maestro = "", destino = "", fecha_hora = "", f_i="", f_f = "", serie_motor = "", serie = "",  nombreM = "";
            String num_pro = "", guia = "", color = "", obs_ = "", modelo_motor = "", id_producto = "", aca = "", cap_vid = "", me = "";
            String nombrePr = "";
            if (reader.Read())
            {
              id = reader.GetInt32(0);          serie = reader.GetString(7);        p3 = reader.GetInt32("p3");
              fecha_hora = reader.GetString(1); modelo_motor = reader.GetString(8); p4 = reader.GetInt32("p4");
              maestro = reader.GetString("maestro");    serie_motor = reader.GetString(9);  correlativo = reader.GetInt32(16);
              num_pro = reader.GetString(3);    destino = reader.GetString(10);     p2 = reader.GetInt32("p2");
              id_producto = reader.GetString(4);guia = reader.GetString(11);        obs_ = reader.GetString(6);  
              color = reader.GetString(5);      p1 = reader.GetInt32("p1");   comboBox1.SelectedValue = maestro;
              nombreM = reader.GetString("nombreMaestro"); f_i = reader.GetString("fecha_i"); f_f = reader.GetString("fecha_f");
              aca = reader.GetString("acabado"); aca = reader.GetString("acabado"); cap_vid = reader.GetString("cap_vid"); me = reader.GetString("medidas");
              comboBox3.SelectedValue = funciones.extraerTipo(int.Parse(id_producto)); cbxProducto.SelectedValue = id_producto;
              modelo = funciones.modelo(reader.GetString("id_producto")); nombrePr = reader.GetString("nombreProducto"); //recuperar el modelo en primera instancia 17-12-18
            }
            con.Close();
            //solo la parte del medio de la serie 212-(009)-m23
            correla = existe.extraerValor(serie, "-", "-");//extrae el correlativo
            //--------------------------------------------------
            color_.Text = color;  serie__.Text = serie; n_g_.Text = guia; acabadotxt_.Text = aca; numericUpDown1.Value = int.Parse(num_pro);
            m_mot_.Text = modelo_motor; ser_mot_.Text = serie_motor; destino_.Text = destino; obs__.Text = obs_;
            //precios informacion del producto
            p1_e.Text = Convert.ToString(p1); p2_e.Text = Convert.ToString(p2); p3_e.Text = Convert.ToString(p3); p4_e.Text = Convert.ToString(p4);
            maestroxx_.Text = nombreM; cap_vidtxt_.Text = cap_vid; medidastxt_.Text = me; nombretxt_.Text = nombrePr;
            //separa la fecha de la hora para asignar alas cajas de texto
            dateTimePicker1.Text = fecha_hora.Split(' ')[0];
            dateTimePicker2.Text = fecha_hora.Split(' ')[1];
            f_i_.Text = f_i; f_f_.Text = f_f;
        }

    
        private void ComboboxTipo()
        {
            MySqlConnection concX = conex.con();
            string selectQueryX = "SELECT id_tipo,descripcion FROM produccion_tipo";
            concX.Open();
            MySqlCommand commandX = new MySqlCommand(selectQueryX, concX);

            MySqlDataReader readerX = commandX.ExecuteReader();
            // Creamos el origen de datos
            DataTable tablaX = new DataTable("nDatos");
            tablaX.Columns.Add("descripcion", typeof(string));
            tablaX.Columns.Add("id_tipo", typeof(int));

            while (readerX.Read())
            {
                DataRow rowX = tablaX.NewRow();
                rowX["descripcion"] = readerX.GetString("descripcion");
                rowX["id_tipo"] = readerX.GetString("id_tipo");
                tablaX.Rows.Add(rowX);
            }
            // Configuramos el ComboBox
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DataSource = tablaX;
            comboBox3.DisplayMember = "descripcion";
            comboBox3.ValueMember = "id_tipo";
            concX.Close();

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxCatalogo();
        }
        private void ComboboxCatalogo()
        {
            int id_Tipox; //traemos el id del combobox tipo
            Int32.TryParse(comboBox3.SelectedValue.ToString(), out id_Tipox);
            if (comboBox3.SelectedItem.ToString() != "" && id_Tipox != 0)
            {
                MySqlConnection conc = conex.con();
                string selectQuery = "SELECT id_cat,nombre,modelo FROM produccion_cat WHERE tipo='"+id_Tipox+"'";
                conc.Open();
                MySqlCommand command = new MySqlCommand(selectQuery, conc);

                MySqlDataReader reader = command.ExecuteReader();
                DataTable tabla = new DataTable("nData");
                tabla.Columns.Add("titulo", typeof(string));
                tabla.Columns.Add("id_catalogo", typeof(int));
               
                while (reader.Read())
                {
                    DataRow row = tabla.NewRow();
                    row["titulo"] = reader.GetString("nombre") + " // " + reader.GetString("modelo");
                    row["id_catalogo"] = reader.GetString("id_cat");
                    tabla.Rows.Add(row);
                   
                }
                cbxProducto.DropDownStyle = ComboBoxStyle.DropDownList;
                cbxProducto.DataSource = tabla;
                cbxProducto.DisplayMember = "titulo";
                cbxProducto.ValueMember = "id_catalogo";

                conc.Close();
                
            }
        }
        private void ComboboxMaestro()
        {
            MySqlConnection con = conex.con();
            string selectQuery = "SELECT nombre,apellido,idTecnico FROM tecnico WHERE tipo=1";
            con.Open();
            MySqlCommand command = new MySqlCommand(selectQuery, con);

            MySqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable("TestData");
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Value", typeof(int));

            while (reader.Read()){
                DataRow row = table.NewRow();
                row["Title"] = reader.GetString("nombre") + " // " + reader.GetString("apellido");
                row["Value"] = reader.GetString("idTecnico");
                table.Rows.Add(row);
            }
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "Title";
            comboBox1.ValueMember = "Value";
            con.Close();
        }

     private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){
            int id_Tecnico; //traemos el id del combobox de los tecnico
            Int32.TryParse(comboBox1.SelectedValue.ToString(), out id_Tecnico);
            if (comboBox1.SelectedItem.ToString() != "" && id_Tecnico !=0){ // inportante para que no genere conflicto antes de querer editar la serie
           
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT nombre FROM tecnico WHERE idTecnico=" + id_Tecnico;
            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            String p1_m = "";
            while (reader.Read()){
                p1_m = reader.GetString("nombre");
            }
                maestroxx_.Text = p1_m;
                con.Close();           
            inicialesTecnico = existe.extraerValor(p1_m, "(", ")");
            enviar();
            }
        }
        private void cbxProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int val;
            Int32.TryParse(cbxProducto.SelectedValue.ToString(), out val);
            if (cbxProducto.SelectedItem.ToString() != ""){
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT nombre,medidas,cap_vid,modelo,modelo,p_soles,i_soles,seis_letras,doce_letras,acabado FROM produccion_cat WHERE id_cat=" + val;
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                String p1 = "", p2 = "", p3 = "", p4 = "",cap_vid = "", medidas = "", nombr="", aca = "";

                while (reader.Read()){
             p4 = reader.GetString("p_soles"); modelo = reader.GetString("modelo"); p1 = reader.GetString("i_soles");
             p2 = reader.GetString("seis_letras"); p3 = reader.GetString("doce_letras"); cap_vid = reader.GetString("cap_vid");
                    medidas = reader.GetString("medidas"); nombr = reader.GetString("nombre"); aca = reader.GetString("acabado");
                }
                nombretxt_.Text = nombr; cap_vidtxt_.Text = cap_vid; acabadotxt_.Text = aca;
                medidastxt_.Text = medidas;
                con.Close();
                p1_e.Text = p1; p2_e.Text = p2; p3_e.Text = p3; p4_e.Text = p4;
                enviar();
            }
        }
       
   private void enviar(){
            //int val2;
            //Int32.TryParse(comboBox2.SelectedValue.ToString(), out val2);
            if (cbxProducto.ValueMember.ToString() != "")
            {

                serie__.Text = genrarSerieEdit.generarX(modelo, correla, inicialesTecnico);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (color_.Text != "" && serie__.Text != "" && m_mot_.Text != "" && ser_mot_.Text != "" && destino_.Text != "" && n_g_.Text != "" && obs__.Text != "" && p1_e.Text != "" && acabadotxt_.Text != "" && cbxProducto.Text !="")
            {
                String cbx1 = comboBox1.SelectedValue.ToString(); //traemos el id maestro del combobox
                String cbx2 = cbxProducto.SelectedValue.ToString(); //traemos el id producto del combobox
                String mifecha = dateTimePicker1.Value.ToString("yyyy/MM/dd");
                String hora = dateTimePicker2.Value.ToString("hh:mm");
                String f_i = f_i_.Value.ToString("yyyy/MM/dd hh:mm:ss");
                String f_f = f_f_.Value.ToString("yyyy/MM/dd hh:mm:ss");
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE produccion SET ";
                cmd.CommandText += "fecha=\"" + string.Concat(mifecha, " " + hora) + "\", maestro=\"" + cbx1 + "\",num_pro=\"" + numericUpDown1.Value + "\",id_producto=\"" + cbx2 + "\",color=\"" + color_.Text + "\",obs_=\"" + obs__.Text + "\",serie=\"" + serie__.Text + "\",modelo_motor=\"" + m_mot_.Text + "\",serie_motor=\"" + ser_mot_.Text + "\",destino=\"" + destino_.Text + "\",guia=\"" + n_g_.Text + "\",p1=\"" + p1_e.Text + "\",p2=\"" + p2_e.Text + "\",p3=\"" + p3_e.Text + "\",p4=\"" + p4_e.Text + "\",nombreMaestro=\"" + maestroxx_.Text + "\",nombreProducto=\"" + nombretxt_.Text + "\",cap_vid=\"" + cap_vidtxt_.Text + "\",medidas=\"" + medidastxt_.Text + "\",acabado=\"" + acabadotxt_.Text + "\",fecha_i=\"" + f_i + "\",fecha_f=\"" + f_f + "\" WHERE id_p=" + contact_id;
                con.Open();
                cmd.ExecuteNonQuery();
                // name_txt.Text = lastname_txt.Text = address_txt.Text = phone_txt.Text = email_txt.Text = "";
                MessageBox.Show("Actualizado exitosamente!");
                con.Close();

                fechan = DateTime.Now;
                annno = Convert.ToString(fechan.Year);
                ListarProduccion.cargarProduccion(datagrid, "", annno);

            }
            else
            {
                MessageBox.Show("Debes introducir Ciertos Datos Obligatoriamente!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
