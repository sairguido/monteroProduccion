using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace monteroProduccion
{
   
    public partial class FormBuscar : Form
    {
        public static DataGridView datagrid;
        public FormBuscar()
        {
            InitializeComponent();
        }
       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;
            string anno = Convert.ToString(fecha.Year);
            ListarProduccion.cargarProduccion(datagrid, textBox1.Text, anno);
        }
    }
}
