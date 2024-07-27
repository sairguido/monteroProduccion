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
    public partial class splash : Form
    {
        int progress = 0;
        public splash()
        {
            InitializeComponent();
        }
        private void splash_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 50;
        }
       

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            progress += 2;
            if (progress >= 100)
            {
                timer1.Enabled = false;
                timer1.Stop();
                this.Hide();
                loginFomr abrir_ = new loginFomr();
                abrir_.Show();
            }
            progress_.Value = progress;
            conteo.Text = progress.ToString();
        }
    }
}
