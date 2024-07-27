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
    public partial class verImagen : Form
    {
        public static string serie = "";
        public verImagen(){
            if (serie == ""){
                this.Close();
            }
            InitializeComponent();
            int idProducto=traeID(serie);
            MySqlConnection conzx = conex.con();
            MySqlCommand cmdzx = conzx.CreateCommand();
            cmdzx.CommandText = "SELECT img FROM produccion_cat WHERE id_cat=" + idProducto;
            conzx.Open();

            MySqlDataReader readerzx = cmdzx.ExecuteReader();
            string imag = "";

            if (readerzx.Read()){
                imag = readerzx.GetString("img");
            }
            conzx.Close();
            Bitmap jbmp = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\" + imag);
            imagen.Image = jbmp;
        }


        private static int traeID(string seriex){
           
            MySqlConnection cona = conex.con();
            MySqlCommand cmda = cona.CreateCommand();
            cmda.CommandText = "SELECT id_producto,serie FROM produccion WHERE serie='"+seriex+"'";
            cona.Open();

            MySqlDataReader readera = cmda.ExecuteReader();
            int ID = 0;
            if (readera.Read()){
                ID = int.Parse(readera.GetString("id_producto"));
            }
            cona.Close();
            return ID;
           }



    }
}
