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

namespace Graduaciones_Karyme.Forms
{
    public partial class frm_menuconsultas_academy : Form
    {
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
        public frm_menuconsultas_academy()
        {
            InitializeComponent();
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string accion = "Cerro submenu academico";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void btnEscuelas_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a instituciones academicas.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_escuelas a = new Forms.frm_escuelas();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
            a.txtNombre.Focus();
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a control academico";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_control a = new Forms.frm_control();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnAlumnos_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a control de alumnos";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_alumnos a = new Forms.frm_alumnos();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnGrupos_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a control de grupos.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_grupos a = new Forms.frm_grupos();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            a.cboxEscuelas.Focus();
            this.Hide();
            a.Show();
        }

        private void btnPaquetes_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a control de paquetes academicos.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_paquetes a = new Forms.frm_paquetes();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnlocalidades_Click(object sender, EventArgs e)
        {
            string accion = "Ingreso a consulta de localidades academicas.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            Forms.frm_localidades a = new Forms.frm_localidades();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }
    }
}
