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

    public partial class frm_seguridad : Form
    {
        public string username3;
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

        public frm_seguridad()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario de servicios fotograficos";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }
    }
}
