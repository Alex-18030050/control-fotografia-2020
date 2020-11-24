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
    public partial class frm_paquetes : Form
    {
        public string username3;
        int el, estatuus;
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        // TASKS: Revisar precio venta.

        private static extern IntPtr CreateRoundRectRgn
          (
          int nLeftRect,
          int nTopRect,
          int RightRect,
          int nBottomRect,
          int nWidthEllipse,
          int nHeightEllipse

          );
        public frm_paquetes()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void frm_paquetes_Load(object sender, EventArgs e)
        {
            LlenarPaquetes();
            txtclave.Enabled = true;
            txtclave.Focus();
        }

        private void LlenarPaquetes()
        {
            cboxpaquetes.Enabled = true;
            DataTable dt = new DataTable();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM PAQUETES where PAQ_Estatus = 1 order by PAQ_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            SqlDataAdapter da = new SqlDataAdapter(comando);
            da.Fill(dt);
            this.cboxpaquetes.DataSource = dt;
            this.cboxpaquetes.ValueMember = "PAQ_Id";
            this.cboxpaquetes.DisplayMember = "PAQ_Nombre";
            conexion.Close();
            cboxpaquetes.Enabled = false;
            txtclave.Focus();
        }
        private void Clean()
        {
            txtclave.Enabled = true;
            txtclave.Clear();

            txtnombre.Enabled = true;
            txtnombre.Clear();
            txtnombre.Enabled = false;

            txtpventa.Enabled = true;
            txtpventa.Clear();
            txtpventa.Enabled = false;

            txtdescripcion.Enabled = true;
            txtdescripcion.Clear();
            txtdescripcion.Enabled = false;

            cboxEstatus.SelectedIndex = 0;
            cboxEstatus.Enabled = false;

            btnlupa.Visible = true;

            cboxpaquetes.Enabled = false;

            btneditar.Visible = false;
            btnguardar.Visible = false;

            txtclave.Focus();
        }
        private void Agregar()
        {
            try
            {
                if (cboxEstatus.SelectedIndex == 0)
                {
                    estatuus = 1;
                }
                if (cboxEstatus.SelectedIndex == 1)
                {
                    estatuus = 0;
                }
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "INSERT into PAQUETES VALUES(@PAQ_Id, @PAQ_Nombre, @PAQ_PrecioVenta, @PAQ_Contenido, @PAQ_Estatus)";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@PAQ_Id", txtclave.Text);
                comando.Parameters.AddWithValue("@PAQ_Nombre", txtnombre.Text);
                comando.Parameters.AddWithValue("@PAQ_PrecioVenta", Convert.ToDouble(txtpventa.Text));
                comando.Parameters.AddWithValue("@PAQ_Contenido", txtdescripcion.Text);
                comando.Parameters.AddWithValue("@PAQ_Estatus", estatuus.ToString());
                comando.ExecuteNonQuery();
                MessageBox.Show("Paquete agregado con exito.", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string accion = "Agrego un paquete nuevo";
                Clases.cl_globales hecho = new Clases.cl_globales();
                hecho.auditoria(username3, accion);
                LlenarPaquetes();
                Clean();
            }
            catch (Exception)
            {
                MessageBox.Show("Verifique que los datos introducidos sean correctos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnguardar.Focus();
            }
         
        }
        private void Actualizar()
        {
            try
            {
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                if (cboxEstatus.SelectedIndex == 0)
                {
                    estatuus = 1;
                }
                if (cboxEstatus.SelectedIndex == 1)
                {
                    estatuus = 0;
                }
                conexion.Open();
                string query = "UPDATE PAQUETES SET PAQ_Nombre=@PAQ_Nombre, PAQ_PrecioVenta=@PAQ_PrecioVenta, PAQ_Contenido=@PAQ_Contenido, PAQ_Estatus=@PAQ_Estatus where PAQ_Id=@PAQ_Id";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@PAQ_Id", txtclave.Text);
                comando.Parameters.AddWithValue("@PAQ_Nombre", txtnombre.Text);
                comando.Parameters.AddWithValue("@PAQ_PrecioVenta", Convert.ToDouble(txtpventa.Text));
                comando.Parameters.AddWithValue("@PAQ_Contenido", txtdescripcion.Text);
                comando.Parameters.AddWithValue("@PAQ_Estatus", estatuus.ToString());
                comando.ExecuteNonQuery();
                MessageBox.Show("Paquete actualizado con exito.", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string accion = "Actualizo un paquete";
                Clases.cl_globales hecho = new Clases.cl_globales();
                hecho.auditoria(username3, accion);
                LlenarPaquetes();
                Clean();
            }
            catch (Exception)
            {
                MessageBox.Show("Verifique que los datos introducidos sean correctos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnguardar.Focus();
            }
            
        }
        private void btnlupa_Click(object sender, EventArgs e)
        {
            cboxpaquetes.Enabled = true;
            cboxpaquetes.Visible = true;
            el = 2;
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario paquetes.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username3, accion);
            this.Close();
        }

        private void txtclave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtclave.Text.Trim()))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtclave.Clear();
                    txtclave.Focus();
                }
                else
                {
                    txtclave.Enabled = false;
                    txtnombre.Enabled = true;
                    txtnombre.Focus();
                    el = 1;
                }
            }
        }

        private void txtnombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtnombre.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtnombre.Clear();
                    txtnombre.Focus();
                }
                else
                {
                    txtnombre.Enabled = false;
                    txtpventa.Enabled = true;
                    txtpventa.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtpventa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtpventa.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtpventa.Clear();
                    txtpventa.Focus();
                }
                else
                {
                    txtpventa.Enabled = false;
                    txtdescripcion.Enabled = true;
                    txtdescripcion.Focus();
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void txtdescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (String.IsNullOrEmpty(txtdescripcion.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtdescripcion.Clear();
                    txtdescripcion.Focus();
                }
                else
                {
                    txtdescripcion.Enabled = false;
                    cboxEstatus.Enabled = true;
                    cboxEstatus.Focus();
                    btnguardar.Visible = true;
                }
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (txtpventa.TextLength <= 10)
            {
                if (el == 1)
                {
                    Agregar();
                }
                if (el == 2)
                {
                    Actualizar();
                }
            }
            else
            {
                MessageBox.Show("El precio es desorbitante.", "Se aceptan como maximo 99999999,99", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtpventa.Clear();
                txtpventa.Focus();
            }
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(cboxpaquetes.Text))
                {
                    MessageBox.Show("El recuadro no deberia estar vacio!", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboxpaquetes.Focus();
                }
                else
                {
                    cboxpaquetes.Visible = false;
                    btneditar.Visible = false;

                    txtclave.Enabled = true;
                    txtnombre.Enabled = true;
                    txtpventa.Enabled = true;
                    txtdescripcion.Enabled = true;
                    cboxEstatus.Enabled = true;

                    obj_conexion = new Clases.Conexion();
                    conexion = new SqlConnection(obj_conexion.Con());
                    conexion.Open();
                    string query = "select * from PAQUETES where PAQ_Nombre=@PAQ_Nombre and PAQ_Estatus=1";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@PAQ_Nombre", cboxpaquetes.Text);
                    SqlDataReader leer = comando.ExecuteReader();
                    if (leer.Read())
                    {
                        string accion = "Consulto un paquete.";
                        Clases.cl_globales hecho = new Clases.cl_globales();
                        hecho.auditoria(username3, accion);

                        txtclave.Text = (leer["PAQ_Id"].ToString());
                        txtnombre.Text = (leer["PAQ_Nombre"].ToString());
                        txtpventa.Text = (leer["PAQ_PrecioVenta"].ToString());
                        txtdescripcion.Text = (leer["PAQ_Contenido"].ToString());
                        cboxEstatus.SelectedIndex = 0;
                        txtclave.Enabled = false;
                        btnguardar.Visible = true;
                        el = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboxpaquetes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btneditar.Visible = true;
                btneditar.Focus();
            }
            if (e.KeyChar == 27)
            {
                Clean();
            }
        }
    }
}
