using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    class existe
    {
        //public static int anno = 2019;
        public static DateTime fecha = DateTime.Now;
        public static string anno;
        
        public static bool Existe()
        {
            fecha = DateTime.Now;
            anno = Convert.ToString(fecha.Year);

            MySqlConnection con = conex.con();
            con.Open();
            MySqlCommand cmdx = con.CreateCommand();
            cmdx.CommandText = "SELECT COUNT(*) FROM produccion WHERE YEAR(fecha)=" + anno;

            int count = Convert.ToInt32(cmdx.ExecuteScalar());
            con.Close();
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
         }
        
            public static int maximo()
            {
            fecha = DateTime.Now;
            anno = Convert.ToString(fecha.Year);

            MySqlConnection con = conex.con();
            con.Open();
            MySqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandText = "SELECT MAX(corelativo) as maximo FROM produccion WHERE YEAR(fecha)="+anno;
           
            MySqlDataReader reader = cmd1.ExecuteReader();
            
            int p1 = 0;

            while (reader.Read()){
             p1 = reader.GetInt32(0);
            }
            con.Close();
            return p1;
            }


        public static String extraerValor(String cadena, String stringInicial, String stringFinal)
        {
            int terminaString = cadena.LastIndexOf(stringFinal);
            String nuevoString = cadena.Substring(0, terminaString);
            int offset = stringInicial.Length;
            int iniciaString = nuevoString.LastIndexOf(stringInicial) + offset;
            int cortar = nuevoString.Length - iniciaString;
            nuevoString = nuevoString.Substring(iniciaString, cortar);
            return nuevoString;
        }


    }
    }

