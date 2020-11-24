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
    public partial class frm_localidades : Form
    {
        public string username3;
        DataSet views = new DataSet();
        DataView filtro; 
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
        public frm_localidades()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }


        private void frm_localidades_Load(object sender, EventArgs e)
        {
            this.Read("Select  L.LOC_Nombre AS 'LOCALIDAD',IA_Nombre 'ESCUELA', IA_ReNom AS 'REPRESENTANTE', IA_Telefono AS 'NUMERO TEL.', IA_Email AS 'CORREO' from INSTITUCIONES_ACADEMICAS INNER JOIN LOCALIDADES L ON IA_IdLocalidad = L.LOC_Id", ref views, "INSTITUCIONES_ACADEMICAS");
            this.filtro = ((DataTable)views.Tables["INSTITUCIONES_ACADEMICAS"]).DefaultView;
            this.dataGridView1.DataSource = filtro;
        }


        public void Read (string query, ref DataSet principal, string tabla)
        {
            try
            {
                string cadena = "Data Source=LENOVO\\SQLEXPRESS;Initial Catalog=GraduacionesKaryme;Integrated Security=True";
                SqlConnection con = new SqlConnection(cadena);
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(principal, tabla);
                da.Dispose();
                con.Close();
               /// string accion = "Realizo una consulta de localidades en apartado academico.";
                ///Clases.cl_globales hecho = new Clases.cl_globales();
                //hecho.auditoria(username3, accion);
                // Checar el punto de datos se truncarian
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtbusqueda_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string datos = "";
                string[] words = this.txtbusqueda.Text.Split(' ');
                foreach (string palabra in words)
                {
                    if (datos.Length == 0)
                    {
                        datos = "(Localidad LIKE '%" + palabra + "%' )";
                    }
                    else
                    {
                        datos += " AND (Localidad LIKE '%" + palabra + "%' )";
                    }
                }
                this.filtro.RowFilter = datos;
            }
            catch (Exception)
            {
                MessageBox.Show("Asegurese de escribir correctamente el nombre de la localidad.");
                txtbusqueda.Clear();
                txtbusqueda.Focus();
            }
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Salio de ventana de localidades academicas.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }

        private void imgCerrar_Click(object sender, EventArgs e)
        {
            string accion = "Salio de ventana de localidades academicas.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }
    }
}
