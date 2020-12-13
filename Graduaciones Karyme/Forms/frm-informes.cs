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
        char VentaEleccion;
        string fecha1Alum, fecha2Alum, fecha1F1, fecha2F2;
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

        private void Cleanalumn()
        {
            dpfecha1.Value = DateTime.Now;
            dpfecha2.Value = DateTime.Now;
            dpfecha1.Visible = false;
            dpfecha2.Visible = false;
            btnOkAlum.Visible = false;
            cboxgrupo.Text = "";
            cboxgrupo.Items.Clear();
            dgAlumnos.DataSource = null;
            dgAlumnos.Rows.Clear();
            dgAlumnos.Refresh();
            btnblimpiaralumn.Visible = false;
            btnImprimirAlum.Visible = false;
        }
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
        public void LlenarGrid() //Okay
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
            fecha1Alum = dpfecha1.Value.ToString();
            fecha2Alum = dpfecha2.Value.ToString();
            LlenarGrid();
            btnblimpiaralumn.Visible = true;
            btnImprimirAlum.Visible = true;
            string accion = "Consulto registro de venta de los alumnos";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
        }

        private void btnImprimirAlum_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea cargar este grupo?", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Forms.frm_reportes R = new Forms.frm_reportes();
                ReportDocument oRep = new ReportDocument();
                oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\GrupoALumnoNotaFechas.rpt");
                oRep.SetParameterValue("@Fecha1", fecha1Alum);
                oRep.SetParameterValue("@Fecha2", fecha2Alum);
                oRep.SetParameterValue("@Institucion", cboxgrupo.SelectedItem.ToString());
                oRep.SetParameterValue("@IDIA", id);
                R.crystalReportViewer1.ReportSource = oRep;
                R.Show();
                Cleanalumn();
                string accion = "Genero informe de ventas del alumnado";
                Clases.cl_globales hecho = new Clases.cl_globales();
                hecho.auditoria(username2, accion);
            } 
            else
            {
                btnblimpiaralumn.Focus();
            }
        }

        private void cboxEscuelas_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AgarrarID();
        }

        private void cboxgrupo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dpfecha1.Visible = true;
        }

        private void dpfecha1_MouseClick(object sender, MouseEventArgs e)
        {
            dpfecha2.Visible = true;
        }

        private void dpfecha2_MouseClick(object sender, MouseEventArgs e)
        {
            btnOkAlum.Visible = true;
        }

        private void dpfecha1_onValueChanged(object sender, EventArgs e)
        {
            dpfecha2.Visible = true;
        }

        private void dpfecha2_onValueChanged(object sender, EventArgs e)
        {
            btnOkAlum.Visible = true;
        }

        private void btnblimpiaralumn_Click(object sender, EventArgs e)
        {
            Cleanalumn();
        }
        //------------------------------------------------------------------VENTAS-----------------------------------------------------------------
        private void CleanVenta()
        {
            rbTodas.AutoCheck = false;
            rbTodas.Checked = false;
            rbTodas.AutoCheck = true;
            rbLiquidadas.AutoCheck = false;
            rbLiquidadas.Checked = false;
            rbLiquidadas.AutoCheck = true;
            rbPorUsuario.AutoCheck = false;
            rbPorUsuario.Checked = false;
            rbPorUsuario.AutoCheck = true;
            dpVentaF1.Value = DateTime.Now;
            dpVentaF2.Value = DateTime.Now;
            dpVentaF1.Visible = false;
            dpVentaF2.Visible = false;
            btnVentaOk.Visible = false;
            dgVentas.DataSource = null;
            dgVentas.Rows.Clear();
            dgVentas.Refresh();
            btnLimpiarVenta.Visible = false;
            bntImprimirVenta.Visible = false;
            cboxUsersVent.Enabled = false;
            rbTodas.Enabled = true;
            rbLiquidadas.Enabled = true;
            rbPorUsuario.Enabled = true;
        }
        private void LlenarUsuarios() 
        {
            try
            {
                cboxUsersVent.Text = "";
                cboxUsersVent.Items.Clear();
                 DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM USUARIOS ORDER BY US_Login";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxUsersVent.Items.Add(dt.Rows[i]["US_Login"].ToString());
                }
                cboxUsersVent.SelectedIndex = 0;
                conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR UsersVenta ERROR");
            }
        }
        public void LlenarGridTodas()
        {
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "select ND.ND_Login as 'USUARIO',ND.ND_Fecha as 'FECHA',AL.AL_Nombre as 'NOMBRE',AL.AL_ApellidoPat as 'APELLIDO PATERNO', AL.AL_ApellidoMat as 'APELLIDO MATERNO',p.PAQ_Nombre as 'NOMBRE DEL PAQUETE',p.PAQ_Contenido as 'CONTENIDO',p.PAQ_PrecioVenta as 'PRECIO PAQUETE',s.Nombre as 'SERVICIO',s.Precio as 'PRECIO DEL SERVICIO',e.EX_Nombre as 'EXTRA',e.EX_Precio as 'PRECIO DEL EXTRA',ND.ND_Descripcion as 'DESCRIPCION',ND.ND_PrecioTotal as 'TOTAL', ND.ND_Anticipo as 'ANTICIPO',ND.ND_Debe as 'DEBE' FROM NOTA_DETALLE ND inner join ALUMNOS AL on ND.ND_IdAlumno = Al.AL_Id inner join PAQUETES P on ND.ND_IdPaquete = P.PAQ_Id inner join SERVICIOS S on ND.ND_IdServicio = s.Ser_id inner join EXTRAS E on ND.ND_IdExtra = E.EX_Id ";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                dgVentas.DataSource = dt;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRID ERROR");
            }
        }//OKAY
        public void LlenarGridLiq()
        {
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "select ND.ND_Fecha as 'FECHA', CONCAT (AL.AL_Nombre,' ', AL.AL_ApellidoPat,' ', AL.AL_ApellidoMat) as 'ALUMNO', ND.ND_Descripcion as 'DESCRIPCION', ND.ND_PrecioTotal as 'TOTAL', ND.ND_Anticipo as 'ANTICIPO', ND.ND_Debe as 'DEBE' from NOTA_DETALLE ND inner join ALUMNOS AL on ND.ND_IdAlumno = Al.AL_Id where ND_Debe = 0 and ND.ND_Fecha between '" + fecha1Alum + "' and  '" + fecha2Alum + "' order by ND.ND_Fecha";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                dgVentas.DataSource = dt;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRID ERROR");
            }
        }//Okay
        public void LlenarGridUser()
        {
            try
            {
                string user = cboxUsersVent.SelectedItem.ToString();
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "select ND.ND_Login as 'USUARIO',ND.ND_Fecha as 'FECHA',AL.AL_Nombre as 'NOMBRE',AL.AL_ApellidoPat as 'APELLIDO PATERNO', AL.AL_ApellidoMat as 'APELLIDO MATERNO',p.PAQ_Nombre as 'NOMBRE DEL PAQUETE',p.PAQ_Contenido as 'CONTENIDO',p.PAQ_PrecioVenta as 'PRECIO PAQUETE',s.Nombre as 'SERVICIO',s.Precio as 'PRECIO DEL SERVICIO',e.EX_Nombre as 'EXTRA',e.EX_Precio as 'PRECIO DEL EXTRA',ND.ND_Descripcion as 'DESCRIPCION',ND.ND_PrecioTotal as 'TOTAL', ND.ND_Anticipo as 'ANTICIPO',(select ND.ND_PrecioTotal - ND.ND_Anticipo) as 'DEBE' FROM NOTA_DETALLE ND inner join ALUMNOS AL on ND.ND_IdAlumno = Al.AL_Id inner join PAQUETES P on ND.ND_IdPaquete = P.PAQ_Id inner join SERVICIOS S on ND.ND_IdServicio = s.Ser_id inner join EXTRAS E on ND.ND_IdExtra = E.EX_Id where ND.ND_Login = '"+ user +"' and (ND.ND_Fecha between '"+fecha1Alum+"' and  '"+ fecha2Alum + "') order by ND.ND_Fecha ";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                dgVentas.DataSource = dt;
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRID ERROR");
            }
        }//OKAY

        private void rbTodas_CheckedChanged(object sender, EventArgs e)
        {
            rbLiquidadas.Enabled = false;
            rbPorUsuario.Enabled = false;
            dpVentaF1.Visible = true;
            VentaEleccion = 'T';
            btnLimpiarVenta.Visible = true;
        }

        private void rbLiquidadas_CheckedChanged(object sender, EventArgs e)
        {
            rbTodas.Enabled = false;
            rbPorUsuario.Enabled = false;
            dpVentaF1.Visible = true;
            VentaEleccion = 'L';
            btnLimpiarVenta.Visible = true;
        }

        private void rbPorUsuario_CheckedChanged(object sender, EventArgs e)
        {
            rbTodas.Enabled = false;
            rbLiquidadas.Enabled = false;
            VentaEleccion = 'P';
            cboxUsersVent.Enabled = true;
            LlenarUsuarios();
            btnLimpiarVenta.Visible = true;
        }

        private void cboxUsersVent_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dpVentaF1.Visible = true;

        }

        private void dpVentaF1_onValueChanged(object sender, EventArgs e)
        {
            dpVentaF2.Visible = true;
            dgVentas.DataSource = null;
            dgVentas.Rows.Clear();
            dgVentas.Refresh();
        }

        private void dpVentaF2_onValueChanged(object sender, EventArgs e)
        {
            btnVentaOk.Visible = true;
            dgVentas.DataSource = null;
            dgVentas.Rows.Clear();
            dgVentas.Refresh();
        }

        private void btnVentaOk_Click(object sender, EventArgs e) //Still NO
        {
            //MessageBox.Show(VentaEleccion.ToString());
            fecha1Alum = dpVentaF1.Value.ToString();
            fecha2Alum = dpVentaF2.Value.ToString();
            bntImprimirVenta.Visible = true;
            if (VentaEleccion == 'T')
            {
                LlenarGridTodas();
            }
            if (VentaEleccion == 'L')
            {
                LlenarGridLiq();
            }
            if (VentaEleccion == 'P')
            {
                LlenarGridUser();
                //string accion = "Genero consultol registro de ventas  usuario";
                //Clases.cl_globales hecho = new Clases.cl_globales();
                //hecho.auditoria(username2, accion);
            }
        }
        private void btnLimpiarVenta_Click(object sender, EventArgs e)
        {
            CleanVenta();
        }
        private void bntImprimirVenta_Click(object sender, EventArgs e) //Still NO
        {
            if (VentaEleccion == 'T')
            {
                if (MessageBox.Show("Desea cargar estos datos?", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\VentasTodas.rpt");
                    oRep.SetParameterValue("@Fecha1", fecha1Alum);
                    oRep.SetParameterValue("@Fecha2", fecha2Alum);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    CleanVenta();
                    string accion = "Genero informe de todas las ventas";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                else
                {
                    btnLimpiarVenta.Focus();
                }
            }//NO
            if (VentaEleccion == 'L')
            {
                if (MessageBox.Show("Desea cargar estos datos?", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\VentasLiquidadas.rpt");
                    oRep.SetParameterValue("@Fecha1", fecha1Alum);
                    oRep.SetParameterValue("@Fecha2", fecha2Alum);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    CleanVenta();
                    string accion = "Genero informe de ventas liquidadas";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                else
                {
                    btnLimpiarVenta.Focus();
                }
            }//NO
            if (VentaEleccion == 'P')
            {
                //reporte users
                if (MessageBox.Show("Desea cargar estos datos?", "AVISO", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\VentasPorUsuarios.rpt");
                    oRep.SetParameterValue("@Fecha1", fecha1Alum);
                    oRep.SetParameterValue("@Fecha2", fecha2Alum);
                    oRep.SetParameterValue("@login", cboxUsersVent.SelectedItem.ToString());
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    CleanVenta();
                    string accion = "Genero informe de venta de usuario";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                else
                {
                    btnLimpiarVenta.Focus();
                }
            }//SI
        }
    }
}

//TASKS: 
// REPORTE DE ALUMNOS por fechas DONE
//REPORTE  de usuario y DE VENTAS por fechas Done
//REPORTE DE LIQUIDADAS
//REPORTE DE TODAS LAS VENTAS
// LLENAR MAS DATOS
//Finish grid methods DONE
