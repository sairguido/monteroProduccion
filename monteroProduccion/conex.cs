using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace monteroProduccion
{
    class conex
    {
        public static MySqlConnection con()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "dbmontero";
            //-------------------------------------------
            builder.Server = "173.214.171.114";
            builder.UserID = "monteroperu_system";
            builder.Password = "app29112014app29112014";
            builder.Database = "monteroperu_system";
            MySqlConnection con = new MySqlConnection(builder.ToString());

            return con;
        }
    }
}
