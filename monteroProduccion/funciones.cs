using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    class funciones
    {
        public static bool ExisteTipo(string descr)
        {

            MySqlConnection con2n = conex.con();
            MySqlCommand cmd2n = con2n.CreateCommand();
            string query = "SELECT COUNT(*) FROM produccion_tipo WHERE descripcion='" + descr + "'";
            con2n.Open();
            MySqlCommand cmd = new MySqlCommand(query, con2n);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con2n.Close();
            if (count == 0)
                return false;
            else
                return true;

        }
        public static bool Existe(string modelo)
        {
            
            MySqlConnection con2 = conex.con();
            MySqlCommand cmd2 = con2.CreateCommand();
            string query = "SELECT COUNT(*) FROM produccion_cat WHERE modelo='"+modelo+"'";
            con2.Open();
            MySqlCommand cmd = new MySqlCommand(query, con2);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con2.Close();
            if (count == 0)
                    return false;
                else
                    return true;
            
        }

        public static string extraerTipo(int id_cat)
        {
            MySqlConnection con7 = conex.con();
            MySqlCommand cmd7 = con7.CreateCommand();
            cmd7.CommandText = "SELECT tipo FROM produccion_cat WHERE id_cat=" + id_cat;
            con7.Open();

            MySqlDataReader buscar4 = cmd7.ExecuteReader();
            String tp = "";
            if (buscar4.Read())
            {
                tp = buscar4.GetString("tipo");
            }
            con7.Close();

            return tp;
        }
        public static string modelo(string id_)
        {
            MySqlConnection con_ = conex.con();
            MySqlCommand cmd_ = con_.CreateCommand();
            cmd_.CommandText = "SELECT modelo FROM produccion_cat WHERE id_cat=" + id_;
            con_.Open();

            MySqlDataReader buscar_ = cmd_.ExecuteReader();
            String tp_ = "";
            if (buscar_.Read())
            {
                tp_ = buscar_.GetString("modelo");
            }
            con_.Close();

            return tp_;
        }

        //JALAR NOMBRE DE LOS MESE
        public static string Mes(string fecha) {

          string mes = "";

     switch (int.Parse(fecha)){

                case 01: mes ="ENERO";
                   break;

                case 02: mes ="FEBRERO";
                   break;

                case 03: mes = "MARZO";
                    break;

                case 04: mes = "ABRIL";
                    break;

                case 05: mes ="MAYO";
                    break;

                case 06: mes ="JUNIO";
                    break;

                case 07: mes ="JULIO";
                    break;

                case 08: mes ="AGOSTO";
                    break;

                case 09: mes ="SEPTIEMBRE";
                break;

                case 10: mes = "OCTUBRE";
                break;

                case 11: mes ="NOVIEMBRE";
                break;

                case 12: mes ="DICIEMBRE";
                break;

            };

        return mes;

        }
    }
}
