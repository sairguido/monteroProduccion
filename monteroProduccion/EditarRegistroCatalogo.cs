using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    public partial class EditarRegistroCatalogo : Form
    {
        public static int contact_id = 0;
        public static DataGridView datagrid;
        public string img = "";
        public string nombrearchivo1;
        public string extencion1;
        public EditarRegistroCatalogo()
        {
            InitializeComponent();
            urlTextBox.Enabled = false;
            ComboBoxTipoEdit();
            ComboboxAcadoEdit();
            if (contact_id == 0)
            {
                this.Close();
            }
            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM produccion_cat WHERE id_cat=" + contact_id;
            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            int id = 0;                int p1 = 0;
            String nombre = "";        int p2 = 0;
            String modelo = "";        int p3 = 0;
            String medidas = "";       int p4 = 0;      
            String capacidad = "";     int tipo = 0; 
            String obs = "", acabado = "";

            while (reader.Read())
            {
                id = reader.GetInt32(0);
                nombre = reader.GetString(1);    p1 = reader.GetInt32(6);
                modelo = reader.GetString(2);    p2 = reader.GetInt32(7);
                medidas = reader.GetString(3);   p3 = reader.GetInt32(8);
                  p4 = reader.GetInt32(9);      capacidad = reader.GetString(4); img = reader.GetString(10);
                obs = reader.GetString(5);       tipo = reader.GetInt32(11);
                 acabado = reader.GetString("acabado");

            }
            con.Close();
            nombre_edit.Text = nombre;                numericUpDown1_edit.Value = p1; comboBox1_edit.Text = acabado;
            modelo_edit.Text = modelo;                numericUpDown2_edit.Value = p2;
            medidas_edit.Text = medidas;              numericUpDown3_edit.Value = p3;
            capacidad_vidrio_edit.Text = capacidad;   numericUpDown4_edit.Value = p4;
            obs_edit.Text =obs;                       comboBox2_edit.SelectedValue = tipo;
                         //urlTextBox.Text = img;
            //asignar la imagen a picture pictureBox1_edit
            Bitmap bmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + img);
            pictureBox1_edit.Image = bmp;
        }
      
        private void ComboBoxTipoEdit(){
            MySqlConnection con = conex.con();
            //=====================================================================
            string query = "select id_tipo,descripcion from produccion_tipo";
            MySqlCommand cmd;

            cmd = new MySqlCommand(query, con);

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable("produccion_tipo");
            da.Fill(table);
            comboBox2_edit.ValueMember = "id_tipo";
            comboBox2_edit.DisplayMember = "descripcion";
            comboBox2_edit.DataSource = table;
        }
        private void ComboboxAcadoEdit()
        {
            int counter = 0; string line;
            System.IO.StreamReader file = new System.IO.StreamReader(Application.StartupPath + "\\Data\\AirPlaneData\\acabado.txt");
            while ((line = file.ReadLine()) != null)
            {
                //if (line == "Todo Acero / Con Repisa:"){
                // MessageBox.Show(line);
                // comboBox1_edit.DisplayMember = "Todo Acero / Con Repisa:"; }
                comboBox1_edit.Items.Add(line.Split(':')[0]);
                counter++;
            }
            file.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string NuevoNombre;
            ///////////ASIGNAR NUEVO NOMBRE ALA IMAGEN//////////////
            if (urlTextBox.Text != "") { NuevoNombre = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"); } else { NuevoNombre = img; }
            ////////////////////FIN///////////////////////////
            if (obs_edit.Text !="" && nombre_edit.Text != "" && modelo_edit.Text != "" && medidas_edit.Text != "" && capacidad_vidrio_edit.Text !="")
            {
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE produccion_cat SET ";
                cmd.CommandText += "nombre=\"" + nombre_edit.Text + "\",modelo=\"" + modelo_edit.Text + "\",medidas=\"" + medidas_edit.Text + "\",cap_vid=\"" + capacidad_vidrio_edit.Text + "\",obs=\"" + obs_edit.Text + "\",p_soles=\"" +numericUpDown1_edit.Value + "\",i_soles=\"" + numericUpDown2_edit.Value + "\",seis_letras=\"" + numericUpDown3_edit.Value + "\",doce_letras=\"" + numericUpDown4_edit.Value + "\",img=\"" + NuevoNombre + extencion1 + "\",tipo=\"" + comboBox2_edit.SelectedValue + "\",acabado=\"" + comboBox1_edit.Text + "\" where id_cat=" + contact_id;
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Actualizado exitosamente!");
                con.Close();
                                                
                if (urlTextBox.Text != "")
                {
                    //------------------------------------------SUBIR IMAGEN----------------
                    FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create("ftp://www.monteroperu.com/" + NuevoNombre + extencion1);
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
                    button1.Enabled = true;
                    //---------------------------finnn----------------------------------------------------------

                    //string localPath = @"G:\FTPTrialLocalPath\";
                    //string fileName = "arahimkhan.txt";

                    //FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create("-ftp://127.0.0.1/Destination/" + fileName);
                    //requestFTPUploader.Credentials = new NetworkCredential("khanrahim", "arkhan22");
                    //requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

                    //FileInfo fileInfo = new FileInfo(localPath + fileName);
                    //FileStream fileStream = fileInfo.OpenRead();

                    //int bufferLength = 2048;
                    //byte[] buffer2 = new byte[bufferLength];

                    //Stream uploadStream = requestFTPUploader.GetRequestStream();
                    //int contentLength = fileStream.Read(buffer, 0, bufferLength);

                    //while (contentLength != 0)
                    //{
                    //    uploadStream.Write(buffer2, 0, contentLength);
                    //    contentLength = fileStream.Read(buffer2, 0, bufferLength);
                    //}

                    //uploadStream.Close();
                    //fileStream.Close();

                    //requestFTPUploader = null;

                    // -https://khanrahim.wordpress.com/2010/09/03/file-download-upload-delete-in-ftp-location-using-c/
                    //eliminar mediante ftp
                    //string fileName = "arahimkhan.txt";

                    //FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create("-ftp://localhost/Source/" + fileName);
                    //requestFileDelete.Credentials = new NetworkCredential("khanrahim", "arkhan22");
                    //requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;

                    //FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();


                }

                catalogo.cargar_tabla_catalogo(datagrid, "");
                //if (urlTextBox.Text != "") {
                   
                //        string filex = Application.StartupPath + "\\Data\\AirPlaneData\\" + img;

                //        if (Directory.Exists(Path.GetDirectoryName(filex)))
                //        {
                //            File.Delete(filex);
                //        }
                   
                //}
            }
            else
            {
                MessageBox.Show("Hay Campos obligatorios!");
            }
          
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //For any other formats
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1_edit.ImageLocation = openFileDialog1.FileName;
                urlTextBox.Text = openFileDialog1.FileName;
                //extraer solo en nombre del arcivo
                FileInfo info = new FileInfo(urlTextBox.Text);
                extencion1 = Path.GetExtension(urlTextBox.Text);
                nombrearchivo1 = info.Name;
                //nombre.Text = extencion;
                //FindForm de extraer nombre del archivo
            }
        }
    }
}
 