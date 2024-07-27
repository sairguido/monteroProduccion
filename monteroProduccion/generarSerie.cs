using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    class generarSerie
    {
        //public int t = 0;
        public static string generar(int v1, string v2)
        {  
           
        MySqlConnection con = conex.con();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT nombre FROM tecnico WHERE idTecnico=" + v1;
            con.Open();

          MySqlDataReader reader = cmd.ExecuteReader();
          String p1_ = "";

            while (reader.Read()){
                p1_ = reader.GetString(0);
            }

            con.Close();
            String v1_ = existe.extraerValor(p1_, "(", ")");
            //SACAR EL ULTIMO ID
            int nVenta = 0;
            String t = "";
            if (!existe.Existe()){
                nVenta = 1;
                string s = string.Format("{0}", nVenta);

                t = s.PadLeft(3, '0');
             }else{
                nVenta = existe.maximo() + 1;

                string s = string.Format("{0}", nVenta);
               t = s.PadLeft(3, '0');
            }
            // TERMINAR DE SCAR EL UTLIMO ID

            //sacar los dos ultimos digitos de la gecha
            DateTime fechaActual = DateTime.Today;
            string ff = string.Format("" + fechaActual.Year);
            string vv = ff.Substring(ff.Length - 2, 2);

            string serie = v2+ "-" + t + "-" + v1_+""+vv;

            return serie;


        }



       



    }
}
