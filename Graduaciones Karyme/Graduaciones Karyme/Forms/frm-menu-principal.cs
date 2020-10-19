using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graduaciones_Karyme.Forms
{
    public partial class frm_menu_principal : Form
    {
        public string username;
        public frm_menu_principal()
        {
            InitializeComponent();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {

            string accion = " Cerro sesion.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Application.Exit();
        }

        private void bunifuCustomLabel2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a pantalla acerca de.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_acercade a = new Forms.frm_acercade();
            a.username2 = username;
            a.Show();
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a punto de venta";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_venta a = new Forms.frm_venta();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void btnSobreNosotros_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a informacion empresarial.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_info_empresarial a = new Forms.frm_info_empresarial();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void btnAcademico_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a submenu academico.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_menuconsultas_academy a = new Forms.frm_menuconsultas_academy();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void btnLocal_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a submenu local.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_consultas_gral a = new Forms.frm_consultas_gral();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a pantalla de informes.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_informes a = new Forms.frm_informes();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void bunifuImageButton14_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a respaldos.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_utilerias a = new Forms.frm_utilerias();
            a.txtEmpleado2.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }

        private void imgSalir_Click(object sender, EventArgs e)
        {
             string accion = " Cerro sesion.";
             Clases.cl_globales hecho = new Clases.cl_globales();
             hecho.auditoria(username, accion);
            Application.Exit();
        }

        private void btnAuditorias_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a pantalla de auditorias del sistema.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username, accion);
            Forms.frm_auditorias a = new Forms.frm_auditorias();
            a.txtEmpleado.Text = txtEmpleadoP.Text;
            a.username2 = username;
            a.Show();
        }
    }
}
