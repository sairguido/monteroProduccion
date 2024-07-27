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
    public partial class loginFomr : Form
    {
        public loginFomr()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUsuario.Text) && !String.IsNullOrEmpty(txtContrasena.Text))
            {
                try
                {
                    login_ bd = new login_();

                    Boolean res = bd.iniciarSesion(txtUsuario.Text, txtContrasena.Text);

                    if (res)
                    {
                        FormMenu p = new FormMenu();
                        p.Show();
                        this.Hide();
                    }
                    else
                    {
                         
                        pictureBox1.Image = new Bitmap(Application.StartupPath + "\\Data\\AirPlaneData\\error.png");
                        //txt_UserName.Clear();
                        //txt_PWD.Clear();
                        MessageBox.Show("Datos Incorrectos");
                    }
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {
                MessageBox.Show("Complete los datos");
            }

        }

    
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
