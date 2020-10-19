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
    public partial class frm_escuelas : Form
    {
        public string username3;
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        string iaSchool,id_iaCOL, id_iaLOC;
        int exists = 0;
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
        public frm_escuelas()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void Max()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select max(IA_Id)+1 as ultimo from INSTITUCIONES_ACADEMICAS";
            SqlCommand command = new SqlCommand(query, conexion);
            SqlDataReader leer = command.ExecuteReader();
            if (leer.Read())
            {
                iaSchool = leer["ultimo"].ToString();
            }
        }
        private void LlenarEscuelas()
        {
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM INSTITUCIONES_ACADEMICAS order by IA_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxEscuelas.DataSource = dt;
            this.cboxEscuelas.ValueMember = "IA_Id";
            this.cboxEscuelas.DisplayMember = "IA_Nombre";
            conexion.Close();
        }

        private void LlenarLocalidades()
        {
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM LOCALIDADES order by LOC_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxLocalidad.DataSource = dt;
            this.cboxLocalidad.ValueMember = "LOC_Id";
            this.cboxLocalidad.DisplayMember = "LOC_Nombre";
            conexion.Close();
        }

        private void LlenarColonias()
        {
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM COLONIAS order by COL_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxColonia.DataSource = dt;
            this.cboxColonia.ValueMember = "COL_Id";
            this.cboxColonia.DisplayMember = "COL_Nombre";
            conexion.Close();
        }

        private void Clean()
        {
            txtNombre.Enabled = true;
            txtNombre.Clear();
            txtRepresentante.Enabled = true;
            txtRepresentante.Clear();
            txtRepresentante.Enabled = false;
            txtTelefono.Enabled = true;
            txtTelefono.Clear();
            txtTelefono.Enabled = false;
            txtEmail.Enabled = true;
            txtEmail.Clear();
            txtEmail.Enabled = false;
            cboxColonia.Enabled = false;
            cboxLocalidad.Enabled = false;
            btnLupa.Visible = true;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnGuardar.Enabled = false;
            cboxEscuelas.Enabled = false;
            btnEditar.Visible = false;
            txtNombre.Focus();
        }
        private void ConsultaColonias()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from COLONIAS where COL_Id=@IA_IdColonia";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_IdColonia", id_iaCOL);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                cboxColonia.Text = (leer["COL_Nombre"].ToString());
            }
        }
        private void ConsultaLocalidades()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from LOCALIDADES where LOC_Id=@IA_IdLocalidad";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_IdLocalidad", id_iaLOC);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                cboxLocalidad.Text = (leer["LOC_Nombre"].ToString());
            }
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Cerro ventana de institucion academica.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }

        private void frm_escuelas_Load(object sender, EventArgs e)
        {
            LlenarEscuelas();
            LlenarColonias();
            LlenarLocalidades();
        }

        private void btnLupa_Click(object sender, EventArgs e)
        {
            cboxEscuelas.Enabled = true;
            txtNombre.Enabled = false;
            cboxEscuelas.Visible = true;
            btnEditar.Visible = true;
            btnEditar.Enabled = true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from INSTITUCIONES_ACADEMICAS where IA_Nombre=@IA_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Nombre", cboxEscuelas.Text);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                string accion = "Consulto una institucion academica.";
                Clases.cl_globales hecho = new Clases.cl_globales();
                hecho.auditoria(username3, accion);

                txtNombre.Enabled = true;
                txtRepresentante.Enabled = true;
                txtTelefono.Enabled = true;
                txtEmail.Enabled = true;
                cboxColonia.Enabled = true;
                cboxLocalidad.Enabled = true;
                txtNombre.Text = (leer["IA_Nombre"].ToString());
                txtRepresentante.Text = (leer["IA_ReNom"].ToString());
                txtTelefono.Text = (leer["IA_Telefono"].ToString());
                txtEmail.Text = (leer["IA_Email"].ToString());
                id_iaCOL = (leer["IA_IdColonia"].ToString());
                id_iaLOC = (leer["IA_IdLocalidad"].ToString());
                iaSchool = (leer["IA_Id"].ToString());
                ConsultaColonias();
                ConsultaLocalidades();
                exists = 1;
                txtRepresentante.Enabled = true;
                txtTelefono.Enabled = true;
                txtEmail.Enabled = true;
                cboxColonia.Enabled = true;
                cboxLocalidad.Enabled = true;
                cboxEscuelas.Enabled = true;
                txtNombre.Focus();
                btnEditar.Enabled = false;
                btnEditar.Visible = false;
                btnGuardar.Enabled = true;
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtNombre.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Clear();
                    txtNombre.Focus();
                }
                else
                {
                    txtNombre.Enabled = false;
                    txtRepresentante.Enabled = true;
                    txtRepresentante.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtRepresentante_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtRepresentante.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRepresentante.Clear();
                    txtRepresentante.Focus();
                }
                else
                {
                    txtRepresentante.Enabled = false;
                    txtTelefono.Enabled = true;
                    txtTelefono.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTelefono.Clear();
                    txtTelefono.Focus();
                }
                else
                {
                    txtTelefono.Enabled = false;
                    cboxLocalidad.Enabled = true;
                    cboxLocalidad.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void cboxLocalidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(cboxLocalidad.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboxLocalidad.Focus();
                }
                else
                {
                    cboxLocalidad.Enabled = false;
                    cboxColonia.Enabled = true;
                    cboxColonia.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void cboxColonia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (string.IsNullOrEmpty(cboxColonia.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboxColonia.Focus();
                }
                else
                {
                    cboxColonia.Enabled = false;
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
                if (string.IsNullOrEmpty(txtEmail.Text)||string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Clear();
                    txtEmail.Focus();
                }
                else
                {
                    txtEmail.Enabled = false;
                    btnGuardar.Visible = true;
                    btnGuardar.Enabled = true;
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void cboxEscuelas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrWhiteSpace(txtEmail.Text)|| string.IsNullOrEmpty(cboxColonia.Text)|| string.IsNullOrEmpty(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text)|| string.IsNullOrEmpty(cboxLocalidad.Text)|| string.IsNullOrEmpty(txtNombre.Text)|| string.IsNullOrEmpty(txtRepresentante.Text))
            {
                MessageBox.Show("Ningun campo debe estar vacio. Verifique los datos que introdujo.", "Error",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
            }
            else
            {
                if (exists == 1)
                {
                    Actualizar();
                }
                if (exists == 0)
                {
                    AgregarNuevo();
                }
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string accion = "Cerro ventana de institucion academica.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // TASKS: Realizar eliminacion escolar
        }

        private void AgregarNuevo() // ToDo: Hacer with only empleados 
        {
            Max();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "INSERT into INSTITUCIONES_ACADEMICAS VALUES(@IA_Id, @IA_IdLocalidad, @IA_Nombre, @IA_Telefono, @IA_Email, @IA_ReNom, @IA_IdColonia)";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("IA_Id", iaSchool); 
            comando.Parameters.AddWithValue("IA_IdLocalidad", cboxLocalidad.SelectedValue); //VERIFICAR SI COINCIDEN LOS ID
            comando.Parameters.AddWithValue("IA_Nombre", txtNombre.Text);
            comando.Parameters.AddWithValue("IA_Telefono", txtTelefono.Text);
            comando.Parameters.AddWithValue("IA_Email", txtEmail.Text);
            comando.Parameters.AddWithValue("IA_ReNom", txtRepresentante.Text);
            comando.Parameters.AddWithValue("IA_IdColonia", cboxColonia.SelectedValue);//Haber como se agrega VERIFICAR SI COINCIDEN LOS ID
            comando.ExecuteNonQuery();
            MessageBox.Show("Escuela agregada con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string accion = "Agrego una escuela nueva";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            //Arreglar para usar el usuario activo desde el menuprincipal
            LlenarEscuelas();
            Clean();
        }

        private void Actualizar() // HAACER UN UPDATE
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "UPDATE INSTITUCIONES_ACADEMICAS SET  IA_IdLocalidad=@IA_IdLocalidad, IA_Nombre=@IA_Nombre, IA_Telefono=@IA_Telefono, IA_Email=@IA_Email, IA_ReNom=@IA_ReNom, IA_IdColonia=@IA_IdColonia WHERE IA_Id=@IA_Id";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("IA_Id", iaSchool);
            comando.Parameters.AddWithValue("IA_IdLocalidad", cboxLocalidad.SelectedValue); //VERIFICAR SI COINCIDEN LOS ID
            comando.Parameters.AddWithValue("IA_Nombre", txtNombre.Text);
            comando.Parameters.AddWithValue("IA_Telefono", txtTelefono.Text);
            comando.Parameters.AddWithValue("IA_Email", txtEmail.Text);
            comando.Parameters.AddWithValue("IA_ReNom", txtRepresentante.Text);
            comando.Parameters.AddWithValue("IA_IdColonia", cboxColonia.SelectedValue);//Haber como se agrega VERIFICAR SI COINCIDEN LOS ID
            comando.ExecuteNonQuery();
            MessageBox.Show("Escuela actualizada con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string accion = "Actualizo datos de una escuela";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            //Arreglar para usar el usuario activo desde el menuprincipal
            LlenarEscuelas();
            Clean();
        }
    }
}
