using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    class login_{

        public static string nombreCompleto = "";
        public static string tipoUsuario = "";
        public static string idUsuario = "";

        public Boolean iniciarSesion(string usuario, string contrasena)
        {
            nombreCompleto = "";
            tipoUsuario = "";
            MySqlConnection con_ = conex.con();
            MySqlCommand cmd = con_.CreateCommand();
            String campos = "codigo,nombre,tipo,idUsuario,cargo";

            string desifrado= GetSHA1(contrasena);
            
            cmd.CommandText = "SELECT " + campos + " FROM usuario WHERE codigo= '" + usuario + "' AND con='" + desifrado + "'";
            con_.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                nombreCompleto = reader.GetString("nombre");
                tipoUsuario = reader.GetString("tipo");
                tipoUsuario = reader.GetString("idUsuario");
            }

            reader.Close();
            con_.Close();

            if (String.IsNullOrEmpty(tipoUsuario))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string GetSHA1(String cipherText)
        {
            string llave1 = "sadncy23mdsdi834nmsdncu45bnn534";
            string llave2 = "jfhy3ndjc9JRNDA9jm4ndjcog45m243";
            string con2 = Reverse(cipherText);
            string mandar = llave2 + llave1 + llave2 + con2 + cipherText + llave1 + con2 + llave1 + cipherText + llave2 + con2 + llave1 + llave2 + llave1;
                                 
            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            Byte[] textOriginal = ASCIIEncoding.Default.GetBytes(mandar);
            Byte[] hash = sha1.ComputeHash(textOriginal);
            StringBuilder cadena = new StringBuilder();
            foreach (byte i in hash)
            {
                cadena.AppendFormat("{0:x2}", i);
            }
            return cadena.ToString();
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


    }
}
