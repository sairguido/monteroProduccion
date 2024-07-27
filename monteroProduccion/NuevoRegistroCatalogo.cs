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
using System.IO; //libreria para poder leer el txt
using System.Net;

namespace monteroProduccion
{
    public partial class NuevoRegistroCatalogo : Form
    {
        public string nombrearchivo;
        public string extencion;
        public static DataGridView datagrid;
       
        public NuevoRegistroCatalogo()
        {
            InitializeComponent();
            LlenarComboBoxTipo();
            LlenarComboBoxAcabado();
            urlTextBox.Enabled = false;
        }
        private void LlenarComboBoxAcabado()
        {
            using (StreamReader lector = new StreamReader(Application.StartupPath + "\\Data\\AirPlaneData\\acabado.txt"))
            {
                while (lector.Peek() >= 0)
                {
                    comboBox3.Items.Add(lector.ReadLine().Split(':')[0]);
                }
            }
        }
        private void LlenarComboBoxTipo()
        {
            MySqlConnection con = conex.con();
            //=====================================================================
            string query = "select id_tipo,descripcion from produccion_tipo";
            MySqlCommand cmd;

            cmd = new MySqlCommand(query, con);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable("produccion_tipo");
            da.Fill(table);
            comboBox2.ValueMember = "id_tipo";
            comboBox2.DisplayMember = "descripcion";
            comboBox2.DataSource = table;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //For any other formats
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                urlTextBox.Text = openFileDialog1.FileName;
                //extraer solo en nombre del arcivo
                FileInfo info = new FileInfo(urlTextBox.Text);
                extencion = Path.GetExtension(urlTextBox.Text);
                nombrearchivo = info.Name;
                //nombre.Text = extencion;
                //FindForm de extraer nombre del archivo
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (urlTextBox.Text == "") {
                MessageBox.Show("TIENES QUE SELECCIONAR UNA IMAGEN"); return;
            }

            if (!funciones.Existe(modelo.Text))
            {
                if (nombre.Text != "" && modelo.Text != "" && medidas.Text != "" && obs.Text != "" && comboBox3.Text != "")
                {
                    Guardar.Enabled = false;
                    MySqlConnection con = conex.con();
                    MySqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "INSERT INTO produccion_cat (nombre,modelo,medidas,cap_vid,obs,p_soles,i_soles,seis_letras,doce_letras,img,tipo) value ";
                    cmd.CommandText += "(\"" + nombre.Text + "\",\"" + modelo.Text + "\",\"" + medidas.Text + "\",\"" + capacidad_vidrio.Text + "\",\"" + obs.Text + "\",\"" + numericUpDown1.Value + "\",\"" + numericUpDown2.Value + "\",\"" + numericUpDown3.Value + "\",\"" + numericUpDown4.Value + "\",\"" + nombre.Text + extencion + "\",\"" + comboBox2.SelectedValue + "\",\"" + comboBox3.Text + "\")";
                    con.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("SE REGISTRO CORRECTAMENTE!   " + extencion);
                    con.Close();
                    //------------------------------------------SUBIR IMAGEN----------------
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://www.monteroperu.com/" + nombre.Text + extencion);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential("facturas@monteroperu.com", "monterogrupomontero");
                    request.UsePassive = true;
                    request.UseBinary = true;
                    request.KeepAlive = true;
                    FileStream stream = File.OpenRead(urlTextBox.Text);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Close();
                    Stream reqStream = request.GetRequestStream();
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Flush();
                    reqStream.Close();
                    MessageBox.Show("IMAGEN SUBIDA CON EXITO");
                    Guardar.Enabled = true;
                    //-------------------------------------------------------------------------------------
                    nombre.Text = modelo.Text = medidas.Text = obs.Text = numericUpDown1.Text = "";
                    catalogo.cargar_tabla_catalogo(datagrid, "");
                }
                else
                {
                    MessageBox.Show("Debes introducir Datos Obligatotios!");
                }
            }
            else
            {
                MessageBox.Show("el modelo Ya Existe...");
            }

                   
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
