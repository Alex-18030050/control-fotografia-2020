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
    public partial class frm_consultas_gral : Form
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
        public frm_consultas_gral()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string accion = "Cerro submenu local.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
        }

        private void bunifuGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Forms.frm_clientes a = new Forms.frm_clientes();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            Forms.frm_empleados a = new Forms.frm_empleados();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnPaquetes_Click(object sender, EventArgs e)
        {
            Forms.frm_paquetes a = new Forms.frm_paquetes();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnExtras_Click(object sender, EventArgs e)
        {
            Forms.frm_extras a = new Forms.frm_extras();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }

        private void btnServicios_Click(object sender, EventArgs e)
        {
            Forms.frm_seguridad a = new Forms.frm_seguridad();
            a.txtEmpleado3.Text = txtEmpleado2.Text;
            a.username3 = username2;
            this.Hide();
            a.Show();
        }
    }
}
