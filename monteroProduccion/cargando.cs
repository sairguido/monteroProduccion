using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Threading.Tasks;
namespace monteroProduccion
{
    public partial class cargando : Form
    {
        public static int b = 0;
        public static string Rmensaje = "";
        public cargando()
        {
            InitializeComponent();
            if (b == 0){
                this.Close();
            }
            if (Rmensaje!="") {
                label1.Text = Rmensaje;
            } else {
                label1.Text =b.ToString();
            }
            timer1.Interval = 4000;
            timer1.Start();
            
            timer1.Tick += (s, e) => {
               cerrar();
            };

        }

       private void cerrar(){
            this.Close();
        }
       
        //public cargando(Action proceso, string procesoDescription)
        //{
        //    InitializeComponent();
        //    label1.Text = procesoDescription;
        //    Proceso = proceso;
        //}

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    Task.Factory.StartNew(Proceso).ContinueWith(
        //        t=>{ this.Close(); },TaskScheduler.FromCurrentSynchronizationContext());
        //}
        private void cargando_Load(object sender, EventArgs e)
        {

        }
    }
}
