using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Graduaciones_Karyme.Forms
{
    public partial class frm_informes : Form
    {
        int id;
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        public string username2;
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
        public frm_informes()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            LlenarEscuelas();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario de informes.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
        }

        //---------------------------------------------------------------------------ALUMNOS-----------------------------------------------------------------------------//

        private void LlenarEscuelas()
        {
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM INSTITUCIONES_ACADEMICAS where IA_Estatus = 1 order by IA_Nombre";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxEscuelas.Items.Add(dt.Rows[i]["IA_Nombre"].ToString());
                }
                cboxEscuelas.SelectedIndex = 0;
                conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Llenar escuelas error");
            }
        }
        private void AgarrarID()
        {
            try
            {
                string x = cboxEscuelas.SelectedItem.ToString();
                //MessageBox.Show(x);
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "Select * from INSTITUCIONES_ACADEMICAS A inner join GRUPOS G on A.IA_Id = G.GR_IdIA WHERE A.IA_Nombre = @IA_Nombre";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@IA_Nombre", x);
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    id = Convert.ToInt32(leer["GR_IdIA"]);
                }
                conexion.Close();
               // MessageBox.Show(id.ToString());
                LlenarGrupos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Agarrar ID ERROR");
            }

            //MessageBox.Show("Usuario agregado con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //string accion = "Agrego un usuario nuevo";
            //Clases.cl_globales hecho = new Clases.cl_globales();
            // hecho.auditoria
        }
        private void LlenarGrupos()  // OKAY
        {
            try
            {
                cboxgrupo.Items.Clear();
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM GRUPOS where GR_Estatus = 1 and GR_IdIA =' " + id.ToString() + "  ' order by GR_Especialidad";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxgrupo.Items.Add(dt.Rows[i]["GR_Especialidad"].ToString());
                }
                cboxgrupo.SelectedIndex = 0;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRUPOS ERROR");
            }
            //cboxgrupo.Enabled = false;
        }
        public void LlenarGrid() //Okay?
        {
            try
            {
                string grupo = cboxgrupo.SelectedItem.ToString();
                //MessageBox.Show(grupo, id.ToString());
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT IA.IA_Nombre as 'ESCUELA', G.GR_Especialidad as 'GRUPO',A.AL_Nombre as 'NOMBRE(S)', A.AL_ApellidoPat as 'APELLIDO PATERNO', A.AL_ApellidoMat as 'APELLIDO MATERNO', A.AL_Complexion as 'FISICO',ND.ND_Descripcion as 'DESCRIPCON', ND_PrecioTotal as 'TOTAL',ND.ND_Anticipo as 'ANTICIPO', (Select ND.ND_PrecioTotal - ND.ND_Anticipo) as 'DEBE' FROM ALUMNOS A inner join GRUPOS G ON AL_Id = G.GR_Id inner join  NOTA_DETALLE ND ON AL_Id = ND.ND_IdAlumno inner join INSTITUCIONES_ACADEMICAS IA On G.GR_IdIA = IA.IA_Id where GR_Especialidad = '" + grupo + "' and G.GR_IdIA = '" + id.ToString() + "'  order by G.GR_Especialidad";
                //MessageBox.Show(query);
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                dgAlumnos.DataSource = dt;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRID ERROR");
            }
        }

        private void cboxEscuelas_Click(object sender, EventArgs e)
        {
            //AgarrarID();
        }

        private void btnOkAlum_Click(object sender, EventArgs e)
        {
            LlenarGrid();
        }

        private void btnImprimirAlum_Click(object sender, EventArgs e)
        {
            // STill falta para ver reporte
            MessageBox.Show("Desea cargar este grupo?", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        private void cboxEscuelas_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AgarrarID();
        }
    }
}

//TASKS: 
// REPORTE DE ALUMNOS por fechas
// LLENAR MAS DATOS
//REPORTE DE VENTAS
