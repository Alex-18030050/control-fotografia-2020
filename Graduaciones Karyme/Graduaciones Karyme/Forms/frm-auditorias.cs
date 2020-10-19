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
{// toDO: Solo agrega usuarios nuevos, aun no actualiza o elimina y falta relacionar a los usuarios con los empleados
    public partial class frm_auditorias : Form
    {
        public string username2;
        char[] May = { 'Z', 'Y', 'X', 'W', 'V', 'E', 'D', 'C', 'A', 'H' };
        int nivel;
        int intentos;
        int exists = 0;
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
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

        public frm_auditorias()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void Clean()
        {
            gboxNiveles.Enabled = false;
            gboxNiveles.Visible = false;
            txtUsuarioReg.Enabled = true;
            txtUsuarioReg.Clear();
            txtPasswordReg.Enabled = true;
            txtPasswordReg.Clear();
            txtPasswordReg.Enabled = false;
            txtrepetir.Enabled = true;
            txtrepetir.Clear();
            txtrepetir.Enabled = false;
            txtPasswordReg.Enabled = false;
            btnGuardarReg.Enabled = false;
            txtUsuarioReg.Enabled = true;
            txtUsuarioReg.Focus();
        }
        private void Gbox(string usuario, string password)
        {
            if (exists == 0)
            {
                if (rbAdministrador.Checked)
                {
                    nivel = 1;
                    Agregar(usuario, password, nivel);
                }
                if (rbOperador.Checked)
                {
                    nivel = 2;
                    Agregar(usuario, password, nivel);
                }
                if (rbInvitado.Checked)
                {
                    nivel = 3;
                    Agregar(usuario, password, nivel);
                }
            }
            if (exists == 1)
            {
                if (rbAdministrador.Checked)
                {
                    nivel = 1;
                    Actualizar(usuario, password, nivel);
                }
                if (rbOperador.Checked)
                {
                    nivel = 2;
                    Actualizar(usuario, password, nivel);
                }
                if (rbInvitado.Checked)
                {
                    nivel = 3;
                    Actualizar(usuario, password, nivel);
                }
            }
        }
        private void Agregar(string user, string pass, int level) // ToDo: Hacer with only empleados 
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "INSERT into USUARIOS VALUES(@US_Login, @US_Password, @US_Nivel)";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("US_Login", user);
            comando.Parameters.AddWithValue("US_Password", pass);
            comando.Parameters.AddWithValue("US_Nivel", level);
            comando.ExecuteNonQuery();
            MessageBox.Show("Usuario agregado con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string accion = "Agrego un usuario nuevo";
            Clases.cl_globales hecho = new Clases.cl_globales();
           hecho.auditoria(username2, accion);
            Clean();
        }
        private void Actualizar(string user, string pass, int level)
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "UPDATE USUARIOS SET US_Password=@US_Password, US_Nivel=@US_Nivel WHERE US_Login=@US_Login";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@US_Login", user);
            comando.Parameters.AddWithValue("@US_Password", pass);
            comando.Parameters.AddWithValue("@US_Nivel", level);
            comando.ExecuteNonQuery();
            MessageBox.Show("Usuario actualizado con exito", "Actualizacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string usuario = txtUsuarioReg.Text;
            string accion = "Actualizo un usuario";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(usuario, accion);
            Clean();
        }

        private void btnLimpiarReg_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void txtUsuarioReg_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (txtUsuarioReg.Text.Contains(" ") || string.IsNullOrEmpty(txtUsuarioReg.Text))
                {
                    MessageBox.Show("Los nombres de usuario no deben llevar espacios vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuarioReg.Clear();
                    txtUsuarioReg.Focus();
                }
                else
                {
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    string query = "select * from USUARIOS where US_Login=@US_Login ";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@US_Login", txtUsuarioReg.Text);
                    SqlDataReader leer = comando.ExecuteReader();
                    if (leer.Read())
                    {
                        string accion = "Consulto un usuario existente.";
                        Clases.cl_globales hecho = new Clases.cl_globales();
                        hecho.auditoria(username2, accion);
                        exists = 1;
                        if (MessageBox.Show("Este Usuario ya existe. ¿Desea actulizar la contraseña y el nivel?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            txtUsuarioReg.Enabled = false;
                            txtPasswordReg.Enabled = true;
                            txtUsuarioReg.Text = (leer["US_Login"].ToString());
                            MessageBox.Show("Esta contraseña no debe ser menor a 9 caracteres.", "Ingrese una nueva contraseña.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtPasswordReg.Focus();
                        }
                        else
                        {
                            Clean();
                        }
                    }
                    else
                    {
                        exists = 0;
                        if (MessageBox.Show("Este Usuario No existe. Desea agregar?", "Adertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            txtUsuarioReg.Enabled = false;
                            txtPasswordReg.Enabled = true;
                            txtPasswordReg.Focus();
                        }
                        else
                        {
                            this.Clean();
                        }
                    }
                    if (e.KeyChar == 27)
                    {
                        this.Clean();
                    }
                }
            }
        }

        private void txtPasswordReg_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
            {
                if (txtPasswordReg.Text.Contains(" ") || txtPasswordReg.TextLength <= 8 || string.IsNullOrEmpty(txtPasswordReg.Text)) //ToDo: Agregar restriccion a los simbolos
                {
                    MessageBox.Show("Las contraseñas de usuario no llevan espacios vacios ni pueden tener menos de 9 caracteres", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPasswordReg.Clear();
                    txtPasswordReg.Focus();
                    intentos++;
                    if (intentos >= 3)
                    {
                        string accion = "El formulario cerro por exceso de intentos fallidos.";
                        Clases.cl_globales hecho = new Clases.cl_globales();
                        hecho.auditoria(username2, accion);
                        MessageBox.Show("Demasiados intentos.", "El formulario se cerrara", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    txtrepetir.Enabled = true;
                    txtrepetir.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }
        private void txtrepetir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if (txtrepetir.Text == txtPasswordReg.Text)
                {
                    gboxNiveles.Visible = true;
                    gboxNiveles.Enabled = true;
                    btnGuardarReg.Enabled = true;
                    txtPasswordReg.Enabled = false;
                    gboxNiveles.Focus();
                }
                else
                {
                    MessageBox.Show("Las contraeñas no coinciden", "Verifique su contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtrepetir.Clear();
                    txtrepetir.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }

        private void btnGuardarReg_Click(object sender, EventArgs e)
        {
            Clases.cl_globales Encriptar = new Clases.cl_globales();
            string encryptPass = Encriptar.Encrypt(txtPasswordReg.Text);
            Gbox(txtUsuarioReg.Text, encryptPass);
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Salio de ventana de auditorias del sistema.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
        }
    }
}
