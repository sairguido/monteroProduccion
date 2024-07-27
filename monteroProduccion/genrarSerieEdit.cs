using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace monteroProduccion
{
    class genrarSerieEdit
    {

        public static string generarX(string modelo, string correlativo, string Tecnico){
        //sacar los dos ultimos digitos de la gecha
            DateTime fechaActual = DateTime.Today;
            string ff = string.Format("" + fechaActual.Year);
            string vv = ff.Substring(ff.Length - 2, 2);

            string serie = modelo + "-" + correlativo + "-" + Tecnico + "" + vv;

            return serie;
        }

    }
}
