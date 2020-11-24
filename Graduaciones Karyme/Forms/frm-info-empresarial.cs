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
    public partial class frm_info_empresarial : Form
    {
        public string username2;
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        bool exists;
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
        public frm_info_empresarial()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario de informacion general.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
        }

        private void Clean()
        {
            txtDomicilio.Clear();
            txtEmail.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtWeb.Clear();
            TxtGerente.Clear();
            txtDomicilio.Enabled = false;
            txtEmail.Enabled = false;
            txtTelefono.Enabled = false;
            txtWeb.Enabled = false;
            TxtGerente.Enabled = false;
            txtNombre.Enabled = true;
            btnLimpiar.Visible = false;
            btnGuardar.Visible = false;
            btnEditar.Visible = true;
            txtNombre.Focus();
        }
        private void Cargar()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from DATOSGENERALES";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            //comando.Parameters.AddWithValue("@IA_Nombre", cboxEscuelas.Text);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                txtNombre.Text = (leer["DG_Nombre"].ToString());
                TxtGerente.Text = (leer["DG_Gerente"].ToString());
                txtDomicilio.Text = (leer["DG_Domicilio"].ToString());
                txtTelefono.Text = (leer["DG_Telefono"].ToString());
                txtEmail.Text = (leer["DG_Email"].ToString());
                txtWeb.Text = (leer["DG_PaginaWeb"].ToString());
                exists = true;
                btnGuardar.Enabled = true;
                btnGuardar.Focus();
            }
            else
            {
                conexion.Close();
                if (MessageBox.Show("No se han agregado datos, desea agregarlos ahora?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes) ;
                txtNombre.Enabled = true;
                txtNombre.Focus();
                exists = false;
            }
        }
        private void frm_info_empresarial_Load(object sender, EventArgs e)
        {
            Cargar();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtNombre.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNombre.Clear();
                    txtNombre.Focus();
                }
                else
                {
                    TxtGerente.Enabled = true;
                    TxtGerente.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void TxtGerente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(TxtGerente.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtGerente.Clear();
                    TxtGerente.Focus();
                }
                else
                {
                    txtDomicilio.Enabled = true;
                    txtDomicilio.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtDomicilio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtDomicilio.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDomicilio.Clear();
                    txtDomicilio.Focus();
                }
                else
                {
                    txtTelefono.Enabled = true;
                    txtTelefono.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtTelefono_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtTelefono.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTelefono.Clear();
                    txtTelefono.Focus();
                }
                else
                {
                    txtEmail.Enabled = true;
                    txtEmail.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmail.Clear();
                    txtEmail.Focus();
                }
                else
                {
                    txtWeb.Enabled = true;
                    txtWeb.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtWeb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtWeb.Text))
                {
                    MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtWeb.Clear();
                    txtWeb.Focus();
                }
                else
                {
                    btnGuardar.Visible = true;
                    btnGuardar.Enabled = true;
                    btnLimpiar.Visible = true;
                    btnGuardar.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(TxtGerente.Text) || String.IsNullOrEmpty(txtDomicilio.Text) || String.IsNullOrEmpty(txtTelefono.Text) || String.IsNullOrEmpty(txtEmail.Text) || String.IsNullOrEmpty(txtWeb.Text))
            {
                MessageBox.Show("Los campos no deben estar vacios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Focus();
            }
            else
            {
                if (exists)
                {
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    string query = " UPDATE DATOSGENERALES SET DG_Nombre=@DG_Nombre, DG_Gerente=@DG_Gerente, DG_Domicilio=@DG_Domicilio, DG_Telefono=@DG_Telefono, DG_Email=@DG_Email, DG_PaginaWeb=@DG_PaginaWeb";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@DG_Nombre", txtNombre.Text);
                    comando.Parameters.AddWithValue("@DG_Gerente", TxtGerente.Text);
                    comando.Parameters.AddWithValue("@DG_Domicilio", txtDomicilio.Text);
                    comando.Parameters.AddWithValue("@DG_Telefono", txtTelefono.Text);
                    comando.Parameters.AddWithValue("@DG_Email", txtEmail.Text);
                    comando.Parameters.AddWithValue("@DG_PaginaWeb", txtWeb.Text);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Datos agregados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conexion.Close();
                    Clean();
                    Cargar();
                    txtNombre.Enabled = false;
                    string accion = "Actualizo los datos empresariales";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                if (!exists)
                {
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    string query = " INSERT INTO DATOSGENERALES VALUES(@DG_Nombre, @DG_Gerente, @DG_Domicilio, @DG_Telefono, @DG_Email, @DG_PaginaWeb)";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@DG_Nombre", txtNombre.Text);
                    comando.Parameters.AddWithValue("@DG_Gerente", TxtGerente.Text);
                    comando.Parameters.AddWithValue("@DG_Domicilio", txtDomicilio.Text);
                    comando.Parameters.AddWithValue("@DG_Telefono", txtTelefono.Text);
                    comando.Parameters.AddWithValue("@DG_Email", txtEmail.Text);
                    comando.Parameters.AddWithValue("@DG_PaginaWeb", txtWeb.Text);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Datos agregados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conexion.Close();
                    Clean();
                    Cargar();
                    txtNombre.Enabled = false;
                    string accion = "Agrego los datos empresariales";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clean();
            txtNombre.Enabled = true;
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            txtNombre.Enabled = true;
            txtDomicilio.Enabled = true;
            txtEmail.Enabled = true;
            txtTelefono.Enabled = true;
            txtWeb.Enabled = true;
            TxtGerente.Enabled = true;
            txtNombre.Enabled = true;
            btnGuardar.Visible = true;
            btnLimpiar.Visible = true;
            txtNombre.Focus();
            btnEditar.Visible = false;
        }
    }
}
