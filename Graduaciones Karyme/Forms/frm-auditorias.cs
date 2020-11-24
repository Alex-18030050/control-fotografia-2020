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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Graduaciones_Karyme.Forms
{
    public partial class frm_auditorias : Form
    {
        int inf, imp, option;
        public string username2;
        string fecha1, fecha2, quer;
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
        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        //  METODOS COMPARTIDOS
        private void frm_auditorias_Load(object sender, EventArgs e)
        {
            LlenarUsuarioDep();
            LlenarUsuarioInf();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        // PANEL REGISTRO DE USUARIOS

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

        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        //    PANEL DEPURACION


        private void CleanDep() // Metodo limpieza del panel depuracion
        {
            rbTodosdep.AutoCheck = false;
            rbTodosdep.Checked = false;
            rbTodosdep.AutoCheck = true;

            rbUsuariodep.AutoCheck = false;
            rbUsuariodep.Checked = false;
            rbUsuariodep.AutoCheck = true;

            cboxUsuariosdep.Enabled = false;

            btnLimpiarDep.Visible = false;
            btnEliminarDep.Visible = false;

            dpicInicialDep.Visible = false;
            dpicTerminalDep.Visible = false;

            dgridMovimientosdep.Visible = false;
            btnOkayDep.Visible = false;
            rbTodosdep.Enabled = true;
            rbUsuariodep.Enabled = true;
        }
        private void rbTodosdep_CheckedChanged(object sender, EventArgs e)
        {
            rbUsuariodep.Enabled = false;
            rbTodosdep.Enabled = false;
            dpicInicialDep.Visible = true;
            dpicTerminalDep.Visible = true;
            btnOkayDep.Visible = true;
            option = 3;
        }
        private void rbUsuariodep_CheckedChanged(object sender, EventArgs e)
        {
            rbUsuariodep.Enabled = false;
            rbTodosdep.Enabled = false;
            cboxUsuariosdep.Enabled = true;
            cboxUsuariosdep.Focus();
            option = 4;
        }
        private void cboxUsuariosdep_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboxUsuariosdep.Text == null || cboxUsuariosdep.Text == " ")
                {
                    MessageBox.Show("No debe dejar el recuadro vacio.", "Mensaje Importante", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboxUsuariosdep.Focus();
                }
                else
                {
                    dpicInicialDep.Visible = true;
                    dpicTerminalDep.Visible = true;
                    btnOkayDep.Visible = true;
                }
            }
            if (e.KeyChar == 27)
            {
                CleanDep();
            }
        }
        private void MostrarGridDep()
        {
            //dgridMovimientosdep
            try
            {
                dgridMovimientosdep.Visible = true;
                if (option == 3)
                {
                    string quer = "select AU_Login as 'USUARIO' , AU_Actividad as 'ACTIVIDAD', AU_Fecha as 'FECHA' from AUDITORIAS WHERE AU_Fecha between " + "'" + dpicInicialDep.Value.ToString() + "'" + "and" + "'" + dpicTerminalDep.Value.ToString() + "'" + "order by AU_Fecha DESC";
                    try
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand(quer, conexion);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dgridMovimientosdep.DataSource = ds.Tables[0];
                        conexion.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                if (option == 4)
                {
                    string quer = "select AU_Login as 'USUARIO' , AU_Actividad as 'ACTIVIDAD', AU_Fecha as 'FECHA' from AUDITORIAS WHERE AU_Login=" + " '" + cboxUsuariosdep.Text + "' " + " and " + "(AU_Fecha between " + " ' " + dpicInicialDep.Value.ToString() + " ' " + " and " + " ' " + dpicTerminalDep.Value.ToString() + " ' " + ")order by AU_Fecha DESC";
                    try
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        SqlCommand cmd = new SqlCommand(quer, conexion);
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dgridMovimientosdep.DataSource = ds.Tables[0];
                        conexion.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("EL error " + ex + " ha ocurrido.");
            }

        }
        private void LlenarUsuarioDep()  // Funcionando OK
        {
            cboxUsuariosdep.Enabled = true;
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM USUARIOS order by US_Login";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cboxUsuariosdep.Items.Add(dt.Rows[i]["US_Login"].ToString());
            }
            cboxUsuariosdep.SelectedIndex = 0;
            conexion.Close();
            cboxUsuariosdep.Enabled = false;
        }
        private void btnOkayDep_Click(object sender, EventArgs e)
        {
            MostrarGridDep();
            btnEliminarDep.Visible = true;
            btnLimpiarDep.Visible = true;
        }
        private void btnEliminarDep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta a punto de eliminar por completo los datos.  Continuar?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                fecha1 = dpicInicialDep.Value.ToString();
                fecha2 = dpicTerminalDep.Value.ToString();
                if (option == 3)
                {
                    try
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        string query = "DELETE FROM AUDITORIAS";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.Clear();
                        comando.ExecuteNonQuery();
                        conexion.Close();
                        string accion = "Depuro todos los movimientos auditoriales.";
                        Clases.cl_globales hecho = new Clases.cl_globales();
                        hecho.auditoria(username2, accion);
                        CleanDep();
                        rbTodosdep.Enabled = true;
                        rbUsuariodep.Enabled = false;
                        MessageBox.Show("Datos depurados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("No hay datos para eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CleanDep();
                    }

                }
                if (option == 4)
                {
                    try
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        string query = "DELETE FROM AUDITORIAS WHERE AU_Login=@AU_Login";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.Clear();
                        comando.Parameters.AddWithValue("@AU_Login", cboxUsuariosdep.Text);
                        comando.ExecuteNonQuery();
                        conexion.Close();
                        cboxUsuariosdep.Enabled = true;
                        string accion = "Depuro movimientos auditoriales del usuario " + cboxUsuariosdep.Text;
                        Clases.cl_globales hecho = new Clases.cl_globales();
                        hecho.auditoria(username2, accion);
                        CleanDep();
                        rbTodosdep.Enabled = true;
                        rbUsuariodep.Enabled = false;
                        MessageBox.Show("Datos depurados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Este usuario no tiene datos que eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CleanDep();
                    }
                }
            }
        }
        private void btnLimpiarDep_Click(object sender, EventArgs e)
        {
            CleanDep();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------//
        //    PANEL INFORMES
        private void LlenarUsuarioInf()  // Funcionando OK
        {
            cboxUsuarioInf.Enabled = true;
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM USUARIOS order by US_Login";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++) // Me saca todos los datos de los rows
            {
                cboxUsuarioInf.Items.Add(dt.Rows[i]["US_Login"].ToString());
            }
            cboxUsuarioInf.SelectedIndex = 0;
            conexion.Close();
            cboxUsuarioInf.Enabled = false;
        }
        private void CleanInf()
        {
            rbTodosInf.Enabled = true;
            rbTodosInf.AutoCheck = false;
            rbTodosInf.Checked = false;
            rbTodosInf.AutoCheck = true;

            rbUsuariosInf.Enabled = true;
            rbUsuariosInf.AutoCheck = false;
            rbUsuariosInf.Checked = false;
            rbUsuariosInf.AutoCheck = true;

            cboxUsuarioInf.Enabled = false;

            dpicInicioInf.Visible = false;
            dpicTerminalInf.Visible = false;

            rbPantallaInf.Enabled = true;
            rbPantallaInf.AutoCheck = false;
            rbPantallaInf.Checked = false;
            rbPantallaInf.AutoCheck = true;
            rbPantallaInf.Enabled = false;

            rbImpresoraInf.Enabled = true;
            rbImpresoraInf.AutoCheck = false;
            rbImpresoraInf.Checked = false;
            rbImpresoraInf.AutoCheck = true;
            rbImpresoraInf.Enabled = false;

            btnLimpiarInf.Visible = false;
            btnGuardarInf.Visible = false;
        }

        private void rbTodosInf_CheckedChanged(object sender, EventArgs e)
        {
            cboxUsuarioInf.Enabled = false;
            inf = 1;
            dpicInicioInf.Visible = true;
            dpicTerminalInf.Visible = true;
            rbPantallaInf.Enabled = true;
            rbImpresoraInf.Enabled = true;
            dpicInicioInf.Focus();
        }

        private void rbUsuariosInf_CheckedChanged(object sender, EventArgs e)
        {
            dpicInicioInf.Visible = false;
            dpicTerminalInf.Visible = false;
            rbPantallaInf.Enabled = false;
            rbImpresoraInf.Enabled = false;
            inf = 2;
            cboxUsuarioInf.Enabled = true;
            //cboxUsuarioInf.Focus();
        }

        private void cboxUsuarioInf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dpicInicioInf.Visible = true;
                dpicTerminalInf.Visible = true;
                rbPantallaInf.Enabled = true;
                rbImpresoraInf.Enabled = true;
                dpicInicioInf.Focus();
            }
            if (e.KeyChar == 27)
            {
                CleanInf();
            }
        }

        private void rbPantallaInf_CheckedChanged(object sender, EventArgs e)
        {
            imp = 1;
            btnLimpiarInf.Visible = true;
            btnGuardarInf.Visible = true;
            btnGuardarInf.Focus();
        }

        private void rbImpresoraInf_CheckedChanged(object sender, EventArgs e)
        {
            imp = 2;
            btnLimpiarInf.Visible = true;
            btnGuardarInf.Visible = true;
            btnGuardarInf.Focus();
        }

        private void btnGuardarInf_Click(object sender, EventArgs e)
        {
            try
            {
                if (inf == 1 && imp == 1)
                {
                    //MessageBox.Show("Todos y pantalla");
                    option = 1;
                    fecha1 = dpicInicioInf.Value.ToString();
                    fecha2 = dpicTerminalInf.Value.ToString();

                    //MOSTRAR EN REPORTE CREANDO INSTANCIA DE FORM REPORTES
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\AuditAll.rpt");
                    oRep.SetParameterValue("@Fecha1", fecha1);
                    oRep.SetParameterValue("@Fecha2", fecha2);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    string accion = "Genero reporte auditorial de todos los usuarios";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                if (inf == 1 && imp == 2)
                {
                    //MessageBox.Show("Todos e impresora");
                    option = 1;
                    fecha1 = dpicInicioInf.Value.ToString();
                    fecha2 = dpicTerminalInf.Value.ToString();
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\AuditAll.rpt");
                    oRep.SetParameterValue("@Fecha1", fecha1);
                    oRep.SetParameterValue("@Fecha2", fecha2);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    string accion = "Genero reporte auditorial de todos los usuarios";
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                if (inf == 2 && imp == 1)
                {
                    //MessageBox.Show("Usuario y pantalla");
                    option = 2;
                    fecha1 = dpicInicioInf.Value.ToString();
                    fecha2 = dpicTerminalInf.Value.ToString();
                    //RunAud(opcion, cboxUsuarioInf.Text, fecha1,fecha2);
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\AuditUser.rpt");
                    string pruebaname = cboxUsuarioInf.Text;
                    //MessageBox.Show(pruebaname);
                    oRep.SetParameterValue("@login", pruebaname);
                    oRep.SetParameterValue("@Fecha1", fecha1);
                    oRep.SetParameterValue("@Fecha2", fecha2);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    string accion = "Genero reporte auditorial del usuario " + cboxUsuarioInf.Text;
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
                if (inf == 2 && imp == 2)
                {
                   // MessageBox.Show("Usuario e impresora");
                    option = 2;
                    fecha1 = dpicInicioInf.Value.ToString();
                    fecha2 = dpicTerminalInf.Value.ToString();
                    Forms.frm_reportes R = new Forms.frm_reportes();
                    ReportDocument oRep = new ReportDocument();
                    oRep.Load(@"C:\graduaciones-karyme\Graduaciones Karyme\Informes\AuditUser.rpt");
                    string pruebaname = cboxUsuarioInf.Text;
                    //MessageBox.Show(pruebaname);
                    oRep.SetParameterValue("@login", pruebaname);
                    oRep.SetParameterValue("@Fecha1", fecha1);
                    oRep.SetParameterValue("@Fecha2", fecha2);
                    R.crystalReportViewer1.ReportSource = oRep;
                    R.Show();
                    string accion = "Genero reporte auditorial del usuario " + cboxUsuarioInf.Text;
                    Clases.cl_globales hecho = new Clases.cl_globales();
                    hecho.auditoria(username2, accion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnLimpiarInf_Click(object sender, EventArgs e)
        {
            CleanInf();
        }
    }
}