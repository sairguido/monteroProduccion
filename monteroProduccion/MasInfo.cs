using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    public partial class MasInfo : Form
    {
        public static int idProduccion = 0;
        public static String fecha = "", f_i = "", f_f = "", mod_motor = "", ser_motor = "", serie = "", maestro = ""; 
        public static String num_pro = "", nombreProducto = "", obs_ = "", cap_vid = "", acabdo = "", color = "", medidas = "";
        public static String destino = "", guia = "", id_prod = "", nomIMG="", responsable="";
       
        public MasInfo()
        {
            InitializeComponent();
            if (idProduccion == 0)
            {
                this.Close();
            }

            MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            String campos ="id_p,fecha,serie,nombreMaestro,num_pro,nombreProducto,cap_vid,acabado,color,medidas,modelo_motor,serie_motor,obs_,destino,guia,fecha_i,fecha_f,id_producto,responsable";
            cmd.CommandText = "SELECT "+campos+" FROM produccion WHERE id_p=" + idProduccion;
            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            int id = 0;
            if (reader.Read())
            {
                id = reader.GetInt32("id_p"); fecha = reader.GetString("fecha"); serie = reader.GetString("serie");
                maestro = reader.GetString("nombreMaestro"); num_pro = reader.GetString("num_pro"); nombreProducto= reader.GetString("nombreProducto");
                cap_vid = reader.GetString("cap_vid"); acabdo= reader.GetString("acabado");  color = reader.GetString("color");
                medidas = reader.GetString("medidas"); mod_motor = reader.GetString("modelo_motor"); ser_motor = reader.GetString("serie_motor");
                obs_ = reader.GetString("obs_"); destino = reader.GetString("destino"); guia = reader.GetString("guia");
                f_i = reader.GetString("fecha_i"); f_f = reader.GetString("fecha_f"); id_prod = reader.GetString("id_producto");
                responsable = reader.GetString("responsable");
            }
            con.Close();
            a.Text = serie; b.Text = fecha; c.Text = maestro; d.Text = num_pro; e.Text = nombreProducto; f.Text = cap_vid;
            g.Text = acabdo; h.Text = color; i.Text = medidas; j.Text = mod_motor; k.Text = ser_motor; l.Text = obs_;
            m.Text = destino; n.Text = guia; o.Text = f_i; p.Text = f_f; q.Text = responsable;
            nomIMG = traeNombreIMG(id_prod);
            Bitmap jbmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + nomIMG);
            imagen_.Image = jbmp;
        }
        private static string traeNombreIMG(string id__)
        {

            MySqlConnection cona = conex.con();
            MySqlCommand cmda = cona.CreateCommand();
            cmda.CommandText = "SELECT img FROM produccion_cat WHERE id_cat='" + id__ + "'";
            cona.Open();

            MySqlDataReader readera = cmda.ExecuteReader();
            string imagen = "";
            if (readera.Read())
            {
                imagen = readera.GetString("img");
            }
            cona.Close();
            return imagen;
        }
        private void MasInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir_Informacion();
        }
        
        public void Imprimir_Informacion()
        {
            PrintDocument document = new System.Drawing.Printing.PrintDocument();
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.ClientSize = new System.Drawing.Size(500, 400);
            ppd.Location = new System.Drawing.Point(0, 0);
            document.PrintPage += new PrintPageEventHandler(Datos_Cliente);
            ppd.Document = document;
            ppd.ShowDialog();

            //imprir directamente sin vista previa

            //PrintDocument formulario = new PrintDocument();
            //formulario.PrintPage += new PrintPageEventHandler(Datos_Cliente);
            //PrintDialog printDialog1 = new PrintDialog();
            //printDialog1.Document = formulario;
            //DialogResult result = printDialog1.ShowDialog();
            //if (result == DialogResult.OK)
            //{
            //    formulario.Print();
            //}
        }
        //Britannic Bold; 12pt; style=Italic
        
        private Font fuente = new Font("Britannic Bold", 16); private Font fuente2 = new Font("Britannic Bold", 25);
        private void Datos_Cliente(object obj, PrintPageEventArgs ev)
        {
            float pos_x = 100; //MARGEN DERECHA IZQUIERDA
            float pos_y = 200; //MARGEN ARRIVA ABAJO
            Bitmap jbmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\b.png");
            ev.Graphics.DrawImage(jbmp, 500, 40, 280, 100);// logoev.Graphics.DrawImage(pictureBox1.Image, DERECHA, ABAJO, ANCHO, ALTO);

            //Lo que vamos a imprimir
            ev.Graphics.DrawString("Fecha: "+fecha, fuente, Brushes.Black, pos_x, 50, new
           StringFormat());//producto


            //Lo que vamos a imprimir
            ev.Graphics.DrawString(nombreProducto, fuente2, Brushes.Black, pos_x, 170, new
           StringFormat());//producto


            // lineas de codigo son las que definen los datos 
            ev.Graphics.DrawString("MAESTRO:", fuente, Brushes.Black, pos_x, pos_y+45, new
            StringFormat());
            ev.Graphics.DrawString("SERIE:", fuente, Brushes.Black, pos_x, pos_y + 90, new
            StringFormat());
            ev.Graphics.DrawString("MODELO MOTOR:", fuente, Brushes.Black, pos_x, pos_y + 135, new
            StringFormat());
            ev.Graphics.DrawString("SERIE MOTOR:", fuente, Brushes.Black, pos_x, pos_y + 180, new
          StringFormat());
            ev.Graphics.DrawString("DESC/ACABADO:", fuente, Brushes.Black, pos_x, pos_y + 225, new
          StringFormat());
            ev.Graphics.DrawString("COLOR:", fuente, Brushes.Black, pos_x, pos_y + 270, new
          StringFormat());
            ev.Graphics.DrawString("CAP/VID:", fuente, Brushes.Black, pos_x, pos_y + 315, new
         StringFormat());
            ev.Graphics.DrawString("MEDIDAS:", fuente, Brushes.Black, pos_x, pos_y + 360, new
       StringFormat());
            ev.Graphics.DrawString("RESPONSABLE :", fuente, Brushes.Black, pos_x, pos_y + 860, new
      StringFormat());
            //Estas ultimas 3 lineas de codigo son las que capturamos en nuestro formulario
            ev.Graphics.DrawString(maestro, fuente, Brushes.Black, pos_x + 100, pos_y+45, new
            StringFormat());
            ev.Graphics.DrawString(serie, fuente, Brushes.Black, pos_x + 80, pos_y + 90, new
            StringFormat());
            ev.Graphics.DrawString(mod_motor, fuente, Brushes.Black, pos_x + 180, pos_y + 135, new
            StringFormat());
            ev.Graphics.DrawString(ser_motor, fuente, Brushes.Black, pos_x + 150, pos_y + 180, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
            StringFormat());
            ev.Graphics.DrawString(acabdo, fuente, Brushes.Black, pos_x + 160, pos_y + 225, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
            StringFormat());
            ev.Graphics.DrawString(color, fuente, Brushes.Black, pos_x+80, pos_y + 270, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
            StringFormat());
            ev.Graphics.DrawString(cap_vid, fuente, Brushes.Black, pos_x + 85, pos_y + 315, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
            StringFormat());
            ev.Graphics.DrawString(medidas, fuente, Brushes.Black, pos_x + 95, pos_y + 360, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
           StringFormat());
            ev.Graphics.DrawString(responsable, fuente, Brushes.Black, pos_x + 180, pos_y + 860, new // pos_x+75 POSICION DERECHA, IZQUIERDA - 45 POSICIN ARIBA ABAJO
          StringFormat());
            if (check.Checked == true)
            {
                //imagen del producto
                Bitmap imagen = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + nomIMG);
                // Image img = Image.FromFile("logo.bmp");
                Rectangle logo = new Rectangle(300, 600, 300, 300);
                ev.Graphics.DrawImage(imagen, logo);
            }
         

        }


    }
}
