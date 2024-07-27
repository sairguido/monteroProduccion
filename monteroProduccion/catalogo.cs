using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //libreria para poder leer el txt
using MySql.Data.MySqlClient;
using System.Net;
using System.Timers;

namespace monteroProduccion
{
    public partial class catalogo : Form
    {
        private static System.Timers.Timer aTimer;
        public static string mensaje="";
        public static Bitmap bmp = null;
        public catalogo()
        {
            InitializeComponent();
            DataGridViewImageColumn iconColumn = new DataGridViewImageColumn();
            iconColumn.Name = "AirplaneImage"; ;
            iconColumn.HeaderText = "Imagen Del Producto";
            iconColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            iconColumn.Width = 75;
            
            dataGridView1.Columns.Add("id", "Id");
            dataGridView1.Columns.Add("nombre", "Nombre");
            dataGridView1.Columns.Add("modelo", "Modelo");
            dataGridView1.Columns.Add("acabado", "Acabado");
            dataGridView1.Columns.Add("medidas", "Medidas");
            dataGridView1.Columns.Add("cap_vid", "Capacidad/Vidrio");
            dataGridView1.Columns.Add("obs", "Obs");
            dataGridView1.Columns.Add("p_soles", "Precio");
            dataGridView1.Columns.Add("i_soles", "Inicial");
            dataGridView1.Columns.Add("seis_l", "6 Letas");
            dataGridView1.Columns.Add("doce_l", "12 Letras");
            dataGridView1.Columns.Add(iconColumn);//AGREGAR IMAGEN EN EL DATAGRID
            dataGridView1.RowTemplate.Height = 120;
            dataGridView1.Columns["id"].Visible = false;
            cargar_tabla_catalogo(dataGridView1, "");
            NuevoRegistroCatalogo.datagrid = dataGridView1;
            EditarRegistroCatalogo.datagrid = dataGridView1;
            }
  
            public static void cargar_tabla_catalogo(DataGridView dg, string nomvv){
            SetTimer();

            dg.Rows.Clear();
                MySqlConnection con = conex.con();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM produccion_cat WHERE modelo LIKE '%" + nomvv + "%'";
                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    String nombre = reader.GetString(1), modelo = reader.GetString(2), medidas = reader.GetString(3);
                    String cap_vid = reader.GetString(4), obs = reader.GetString(5);
                    String p_soles = reader.GetString(6), i_soles = reader.GetString(7), seis_letras = reader.GetString(8);
                    String doce_letras = reader.GetString(9), img = reader.GetString(10), acabado = reader.GetString(12);


                //string urlx = "-https://www.monteroperu.com/monteroperu.com/facturas/18-10-2018-05-05-27.jpg";

                //WebClient wc = new WebClient();
                //byte[] bytes = wc.DownloadData(urlx);

                //MemoryStream ms = new MemoryStream(bytes);
                //Image imgx = Image.FromStream(ms);

                //dg.Rows.Add(id, nombre, modelo, medidas, acabado, cap_vid, obs, p_soles, i_soles, seis_letras, doce_letras, imgx);


                String path = Application.StartupPath + "\\Data\\AirPlaneData\\" + img;
                if (!File.Exists(path))
                {    //INICIALIZAR EL WEB CLIENTE
                    using (WebClient webClient = new WebClient())
                    {
                        //COMPROVAR SI EXISTE LA IMAGEN EN EL SERVIDOR
                        if (File.Exists("https://www.monteroperu.com/monteroperu.com/facturas/" + img))
                        {
                            byte[] data = webClient.DownloadData("https://www.monteroperu.com/monteroperu.com/facturas/" + img);
                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                using (var yourImage = Image.FromStream(mem))
                                {
                                    // If you want it as Jpeg
                                    //-https://www.andrewhoefling.com/Home/post/basic-image-manipulation-in-c-sharp
                                    try
                                    {
                                        if (yourImage != null)
                                        { //IMAGEN DESCARGADA CON EXITO
                                            yourImage.Save(path);
                                            mensaje = "NUEVA IMAGEN, DESCARGADO.." + img;
                                            bmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + img);
                                          }
                                        else
                                        { //NO SE PUDO DESCARGAR LA IMAGEN
                                            bmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\defecto.jpg");
                                        }
                                        dg.Rows.Add(id, nombre, modelo, acabado, medidas, cap_vid, obs, p_soles, "S/ ." + i_soles + ".00", seis_letras, doce_letras, bmp);

                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show("Hubo un problema al guardar el archivo. Compruebe los permisos de archivo.");
                                    }
                                }
                            }

                        }
                        else
                        {//NO SE ENCONTRO LA IMAGEN EN EL SERVIDOR
                            bmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\defecto.jpg");
                            dg.Rows.Add(id, nombre, modelo, acabado, medidas, cap_vid, obs, p_soles, "S/ ." + i_soles + ".00", seis_letras, doce_letras, bmp);
                          }

                     }
       
                }
                else
                {
                    bmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + img);
                    dg.Rows.Add(id, nombre, modelo,acabado, medidas, cap_vid, obs, "S/." + p_soles + ".00", "S/." + i_soles + ".00", "S/." + seis_letras + ".00", "S/." + doce_letras + ".00", bmp);

                }

            }
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
            cargando.b =im;
            cargando.Rmensaje = mensaje;
            cargando frm = new cargando();
            frm.ShowDialog();
            
            //modo de acceder deun metodo estatico a uno nno estatico
            //logo cls1 = new logo();
            //cls1.reportarProgreso(100, im);
        }
    
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoRegistroCatalogo frm = new NuevoRegistroCatalogo();
            frm.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cargar_tabla_catalogo(dataGridView1, textBox1.Text);
        }
       
        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
     int c_id = int.Parse(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
            EditarRegistroCatalogo.contact_id = c_id;
            EditarRegistroCatalogo ec = new EditarRegistroCatalogo();
            ec.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

        }

       
    }
}
