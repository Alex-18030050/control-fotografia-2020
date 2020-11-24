using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Graduaciones_Karyme.Forms
{
    public partial class login : Form
    {
        char[] May =  { 'Z', 'Y', 'X', 'W', 'V', 'E', 'D', 'C', 'A' ,'H'}; // ARREGLO DE STRINGS
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        string nivel;
        int intentos = 0;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
           (
           int nLeftRect,
           int nTopRect,
           int RightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse

           );


        public login()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void btnexit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Clean()
        {

            txtPsswdF.Visible = true;
            txtPsswdF.Enabled = true;
            txtPsswdF.Clear();
            txtPsswdF.Visible = false;
            txt_user.Enabled = true;
            txt_user.Clear();
            txt_user.Focus();
            lblLevel.Visible = true;
            lblAdmin.Visible = false;
            lblOper.Visible = false;
            lblNov.Visible = false;
            btnLogin.Visible = false;

        }
   
        private void txt_user_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txt_user.Text.Contains(" ") || (string.IsNullOrEmpty(txt_user.Text))) 
                {
                    this.Clean();
                    txt_user.Focus();
                    MessageBox.Show("ERROR: Los nombres de usuario no llevan espacios vacios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                   conexion.Open();
                    string query = "select * from USUARIOS where US_Login=@US_Login";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@US_Login", txt_user.Text);
                    SqlDataReader leer = comando.ExecuteReader();
                    if (leer.Read())
                    {
                      //  estatus = (leer["US_Estatus"].ToString());
                        nivel = (leer["US_Nivel"].ToString());
                        // if (estatus == "0")
                        //  {
                        //     MessageBox.Show("Este usuario esta dado de baja", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //     txtPsswdF.Clear();
                        //     txtPsswdF.Focus();
                        //  }
                        if (nivel == "1")
                        {
                            lblAdmin.Visible = true;
                            lblLevel.Visible = false;
                        }
                        if (nivel == "2")
                        {
                            lblOper.Visible = true;
                            lblLevel.Visible = false;
                        }
                        if (nivel == "3")
                        {
                            lblNov.Visible = true;
                            lblLevel.Visible = false;
                        }
                        txt_user.Enabled = false;
                        txtPsswdF.Enabled = true;
                        txtPsswdF.Visible = true;
                        txtPsswdF.Focus();
                    }
                    else
                    {
                        txt_user.Clear();
                        MessageBox.Show("Este usuario No existe", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }

        private void txtPsswdF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtPsswdF.Text.Contains(" ") || txtPsswdF.TextLength <= 8|| string.IsNullOrEmpty(txtPsswdF.Text))
                {
                    intentos++;
                    MessageBox.Show("Las contraseñas de usuario NO llevan espacios vacios y NO deben tener menos de 9 caracteres", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPsswdF.Clear();
                    txtPsswdF.Focus();
                    if (intentos >= 3)
                    {
                        MessageBox.Show("Solo se permiten 3 intentos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
                else
                {
                    Clases.cl_globales Encriptar = new Clases.cl_globales();
                   string encryptPass = Encriptar.Encrypt(txtPsswdF.Text); // Procede a encriptar
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    string query = "select * from USUARIOS where US_Password=@US_Password";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@US_Password", encryptPass);
                    SqlDataReader leer = comando.ExecuteReader();
                    if (leer.Read())
                    {
                        MessageBox.Show("Ahora tiene acceso al sistema. De clic al boton rojo.", "Acceso concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnLogin.Enabled = true;
                        btnLogin.Visible = true;
                        btnLogin.Focus();
                    }
                    else
                    {
                        intentos++;
                        MessageBox.Show("Contraseña incorrecta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPsswdF.Clear();
                        txtPsswdF.Enabled = true;
                        txtPsswdF.Focus();
                        if (intentos >= 3)
                        {
                            MessageBox.Show("Solo se permiten 3 intentos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                    }
                }
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txt_user.Enabled = true;
            string usuario = txt_user.Text;
            string accion = "Accedio al sistema";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(usuario, accion);
            txt_user.Enabled = false;
            Forms.frm_menu_principal a = new Forms.frm_menu_principal();
            a.txtEmpleadoP.Text = "Graduaciones Karyme, Version 1.0   " + "   Usuario: " + txt_user.Text;
            a.username = txt_user.Text;
            this.Hide();
            a.Show();
        }
    }
}
