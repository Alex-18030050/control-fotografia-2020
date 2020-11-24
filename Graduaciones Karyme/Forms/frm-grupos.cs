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
    public partial class frm_grupos : Form
    {
        string iaGrupos, x,T;
        int valorid, idgrupos;
        public string username3;
        bool existe = false;
        bool status;
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
        public frm_grupos()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void Max()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select max(GR_Id)+1 as ultimo from GRUPOS";
            SqlCommand command = new SqlCommand(query, conexion);
            SqlDataReader leer = command.ExecuteReader();
            if (leer.Read())
            {
                iaGrupos = leer["ultimo"].ToString();
            }
        }

        private void Clean()
        {
            btnLupa.Visible = false;
            //btnEliminar.Visible = false;
            btnGuardar.Visible = false;
            cboxEscuelas.Enabled = true;
            cboxGrupos.Enabled = false;
            txtGrupo.Clear();
            txtGrupo.Enabled = false;
            txtJefe.Clear();
            txtJefe.Enabled = false;
            txtNumJefe.Clear();
            txtNumJefe.Enabled = false;
            txtTutor.Clear();
            txtTutor.Enabled = false;
            txtNumTutor.Clear();
            txtTurno.Clear();
            txtTurno.Enabled = false;
            TxtEstatus.SelectedIndex = 0;
            TxtEstatus.Enabled = false;
            txtNumTutor.Enabled = false;
            cboxEscuelas.Focus();
        }
        private void LlenarEscuelas()
        {
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM INSTITUCIONES_ACADEMICAS order by IA_Id";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxEscuelas.DataSource = dt;
            this.cboxEscuelas.ValueMember = "IA_Id";
            this.cboxEscuelas.DisplayMember = "IA_Nombre";
            conexion.Close();
        }

        private void ObtenerID()
        {
            x = cboxEscuelas.Text;
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from INSTITUCIONES_ACADEMICAS where IA_Nombre=@IA_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Nombre", x);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                valorid = (Convert.ToInt32(leer["IA_Id"]));
            }
            txtGrupo.Focus();
        }
        private void LlenarGrupos()
        {
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM GRUPOS WHERE GR_IdIA = @IA_Id ";
            //string query = " SELECT GR_Especialidad, GR_Estatus = CASE GR_Estatus when 'true' then 'ACTIVO' when 'false' then 'BAJA' END, GR_Turno = CASE GR_Turno when '1' then 'MATUTINO' when '2' then 'VESPERTINO' when '3' then 'SABATINO' END FROM GRUPOS where GR_Id = @IA_Id  ";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Id", valorid);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxGrupos.DataSource = dt;
            this.cboxGrupos.ValueMember = "GR_Id";
            this.cboxGrupos.DisplayMember = "GR_Especialidad";
            conexion.Close();
            existe = true;
        }

        private void ObtenerIDGrupos()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from GRUPOS where GR_IdIA=@IA_Id";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Id", valorid);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                idgrupos = (Convert.ToInt32(leer["GR_Id"]));
            }
            txtGrupo.Focus();
        }


        private void frm_grupos_Load(object sender, EventArgs e)
        {
            LlenarEscuelas();
            TxtEstatus.Enabled = true;
            TxtEstatus.SelectedIndex = 0;
            TxtEstatus.Enabled = false;
        }

        private void btnLupa_Click(object sender, EventArgs e)
        {
            cboxGrupos.Enabled = true;
            ObtenerID();
            ObtenerIDGrupos();
            LlenarGrupos();
            cboxGrupos.Focus();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario grupos academicos.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }

        private void cboxEscuelas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                cboxEscuelas.Enabled = false;
                btnLupa.Visible = true;
                txtGrupo.Enabled = true;
                txtGrupo.Focus();
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }

        private void txtGrupo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtGrupo.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtGrupo.Clear();
                    txtGrupo.Focus();
                }
                else
                {
                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    //string query = "select * from GRUPOS G INNER JOIN INSTITUCIONES_ACADEMICAS  ON G.GR_IdIA = @IA_Id ";
                    string query = "select * from GRUPOS where GR_Especialidad = @GR_Especialidad";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@GR_Especialidad", txtGrupo.Text);
                    SqlDataReader leer = comando.ExecuteReader();
                    if (leer.Read())
                    {
                        existe = true;
                        if (MessageBox.Show("Este grupo ya existe. Para actualizar seleccione del cuadro de la derecha el grupo", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            txtGrupo.Clear();
                            cboxGrupos.Focus();
                        }
                    }
                    else
                    {
                        existe = false;
                        if (MessageBox.Show("Este grupo No existe. Desea agregar?", "Adertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            txtGrupo.Enabled = false;
                            cboxGrupos.Enabled = false;
                            btnLupa.Visible = false;
                            txtJefe.Enabled = true;
                            txtJefe.Focus();
                        }
                        else
                        {
                            this.Clean();
                        }
                    }
                }
            }
            if (e.KeyChar == 27)
            {
                this.Clean();
            }
        }

        private void cboxGrupos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string objekt = cboxGrupos.Text;
                MessageBox.Show(objekt);
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                //string query = "select * from GRUPOS where GR_Especialidad=@GR_Especialidad";
                string query = "SELECT GR_Id , GR_IdIA, GR_IdAlumno ,GR_TelAlum ,GR_Especialidad,GR_Turno,GR_Tutor,GR_TelTutor, GR_Turnoo = CASE GR_Turno when '1' then 'MATUTINO' when '2' then 'VESPERTINO' when '3' then 'SABATINO' END, GR_Estatuss = CASE GR_Estatus when 'true' then 'ACTIVO' when 'false' then 'BAJA' END FROM GRUPOS where GR_Especialidad = @GR_Especialidad";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@GR_Especialidad", objekt);
                SqlDataReader leer = comando.ExecuteReader();
               // MessageBox.Show("Va leers");
                if (leer.Read())
                {
                    btnGuardar.Visible = true;
                    // btnEliminar.Visible = true;
                    btnGuardar.Enabled = true;
                    btnGuardar.Focus();
                    string accion = "Consulto un grupo escolar.";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username3, accion);

                    txtGrupo.Enabled = true;
                    txtJefe.Enabled = true;
                    txtNumJefe.Enabled = true;
                    txtTutor.Enabled = true;
                    txtNumTutor.Enabled = true;
                    txtTurno.Enabled = true;
                    TxtEstatus.Enabled = true;

                    txtGrupo.Text = (leer["GR_Especialidad"].ToString());
                    txtJefe.Text = (leer["GR_IdAlumno"].ToString());
                    txtNumJefe.Text = (leer["GR_TelAlum"].ToString());
                    txtTutor.Text = (leer["GR_Tutor"].ToString());
                    txtNumTutor.Text = (leer["Gr_TelTutor"].ToString());
                    txtTurno.Text = (leer["GR_Turnoo"].ToString());
                    TxtEstatus.Text = (leer["GR_Estatuss"].ToString());
                }
                else
                {
                    MessageBox.Show("NO LEYO, Revise los datos que introdujo.");
                }
            }
        }

        private void txtJefe_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtJefe.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtJefe.Clear();
                    txtJefe.Focus();
                }
                else
                {
                    txtJefe.Enabled = false;
                    txtNumJefe.Enabled = true;
                    txtNumJefe.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtNumJefe_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtNumJefe.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNumJefe.Clear();
                    txtNumJefe.Focus();
                }
                else
                {
                    txtNumJefe.Enabled = false;
                    txtTutor.Enabled = true;
                    txtTutor.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtTutor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtTutor.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTutor.Clear();
                    txtTutor.Focus();
                }
                else
                {
                    txtTutor.Enabled = false;
                    txtNumTutor.Enabled = true;
                    txtNumTutor.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtNumTutor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtNumTutor.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNumTutor.Clear();
                    txtNumTutor.Focus();
                }
                else
                {
                    txtNumTutor.Enabled = false;
                    txtTurno.Enabled = true;
                    txtTurno.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtTurno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtTurno.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTurno.Clear();
                    txtTurno.Focus();
                }
                else
                {
                    if (txtTurno.Text == "MATUTINO" || txtTurno.Text == "VESPERTINO" || txtTurno.Text == "SABATINO")
                    {
                        txtTurno.Enabled = false;
                        TxtEstatus.Enabled = true;
                        TxtEstatus.Focus();
                    }
                    else
                    {
                        MessageBox.Show("El dato ingresado es incorrecto. Solo puede elegir MATUTINO, VESPERTINO, o SABATINO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtTurno.Clear();
                        txtTurno.Focus();
                    }
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void TxtEstatus_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(TxtEstatus.Text))
                {
                    MessageBox.Show("Los campos no deben ir vacios", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TxtEstatus.Focus();
                }
                else
                {
                    if (TxtEstatus.Text == "ACTIVO" || TxtEstatus.Text == "BAJA")
                    {
                        TxtEstatus.Enabled = false;
                        btnGuardar.Visible = true;
                        btnGuardar.Enabled = true;
                        btnGuardar.Focus();
                    }
                    else
                    {
                        MessageBox.Show("El dato ingresado es incorrecto. Solo puede elegir ACTIVO o BAJA", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        TxtEstatus.SelectedIndex = 0;
                        TxtEstatus.Focus();
                    }
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (existe)
            {
                ActualizarGrupo();
            }
            if (!existe)
            {
                AgregarNuevoGrupo();
            }
        }

        private void AgregarNuevoGrupo()
        {
            Max();
            if (TxtEstatus.Text == "BAJA")
            {
                status = false;
            }
            else
            {
                status = true;
            }
            if (txtTurno.Text == "MATUTINO")
            {
                T = "1";
            }
            if (txtTurno.Text == "VESPERTINO")
            {
                T = "2";
            }
            if (txtTurno.Text == "SABATINO")
            {
                T = "3";
            }
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "INSERT into GRUPOS VALUES(@GR_Id, @GR_IdIA, @GR_IdAlumno,@GR_TelAlum, @GR_Especialidad, @GR_Turno, @GR_Tutor,@GR_TelTutor, @GR_Estatus)";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("GR_Id", iaGrupos);
            comando.Parameters.AddWithValue("GR_IdIA", valorid);
            comando.Parameters.AddWithValue("GR_IdAlumno", txtJefe.Text);
            comando.Parameters.AddWithValue("GR_TelAlum", txtNumJefe.Text);
            comando.Parameters.AddWithValue("@GR_Especialidad", txtGrupo.Text);
            comando.Parameters.AddWithValue("GR_Turno", T);
            comando.Parameters.AddWithValue("GR_Tutor", txtTutor.Text);
            comando.Parameters.AddWithValue("GR_TelTutor", txtNumTutor.Text);
            comando.Parameters.AddWithValue("GR_Estatus", status);
            comando.ExecuteNonQuery();
            MessageBox.Show("Grupo agregado con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string accion = "Agrego un grupo escolar nuevo";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            cboxGrupos.Enabled = true;
            LlenarGrupos();
            Clean();
        }

        private void ActualizarGrupo() 
        {
            if (TxtEstatus.Text == "BAJA")
            {
                status = false;
            }
            else
            {
                status = true;
            }
            if (txtTurno.Text == "MATUTINO")
            {
                T = "1";
            }
            if (txtTurno.Text == "VESPERTINO")
            {
                T = "2";
            }
            if (txtTurno.Text == "SABATINO")
            {
                T = "3";
            }
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "UPDATE GRUPOS SET  GR_IdIA=@GR_IdIA, GR_IdAlumno=@GR_IdAlumno, GR_TelAlum=@GR_TelAlum, GR_Especialidad=@GR_Especialidad, GR_Turno=@GR_Turno,GR_Tutor=@GR_Tutor,GR_TelTutor=@GR_TelTutor,GR_Estatus=@GR_Estatus WHERE GR_Id=@GR_Id";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@GR_Id", idgrupos);
            comando.Parameters.AddWithValue("@GR_IdIA", valorid);
            comando.Parameters.AddWithValue("@GR_IdAlumno", txtJefe.Text);
            comando.Parameters.AddWithValue("@GR_TelAlum", txtNumJefe.Text);
            comando.Parameters.AddWithValue("@GR_Especialidad", txtGrupo.Text);
            comando.Parameters.AddWithValue("@GR_Turno", T);
            comando.Parameters.AddWithValue("@GR_Tutor", txtTutor.Text);
            comando.Parameters.AddWithValue("@GR_TelTutor", txtNumTutor.Text);
            comando.Parameters.AddWithValue("@GR_Estatus", status); 
            comando.ExecuteNonQuery();
            MessageBox.Show("Grupo actualizado con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string accion = "Actualizo datos de un grupo escolar";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            LlenarGrupos();
            Clean();
        }
    }
}
