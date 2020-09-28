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
    public partial class frm_splashInicio : Form
    {
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
        public frm_splashInicio()
        {
            InitializeComponent();
            //this.TransparencyKey = (BackColor);
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            progressbar.Value = 10;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressbar.Value++;
            progressbar.Text = progressbar.Value.ToString() + "%";;

            if (progressbar.Value == 100)
            {
                timer1.Enabled = false;
                Forms.login a = new Forms.login();
                a.Show();
                this.Hide();
            }
        }

        private void frm_splashInicio_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
    }
}
