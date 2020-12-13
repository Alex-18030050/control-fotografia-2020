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

namespace Graduaciones_Karyme.Forms
{
    public partial class frm_venta : Form // TODO: abonos, guardar venta
        //form para abonos, y cancelar venta en abonos
        //aqui, metodo para saber si usuario tiene abono
        //usar en metodo al seleccionar alumno en cbox
        //preguntar si desea agregarle otra venta
    {
        Clases.Conexion obj_conexion;
        SqlConnection conexion;
        public string username2;
        string x, PrecioPaquete, PrecioServicio, PrecioExtra,idalumno, desc;
        int valorid, idgrupos, desicion;
        bool existe;
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
        public frm_venta()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }

        private void frm_venta_Load(object sender, EventArgs e)
        {
            Max();
            LlenarEscuelas();
            cboxEscuela.SelectedIndex = 0;
            cboxGrupo.SelectedIndex = 0;
            cboxAlumno.SelectedIndex = 0;
            cboxPaquetes.SelectedIndex = 0;
            cboxServicios.SelectedIndex = 0;
            cboxExtras.SelectedIndex = 0;
            existe = true;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            string accion = "Salio de formulario punto de venta.";
            Clases.cl_globales hecho = new Clases.cl_globales();
            hecho.auditoria(username2, accion);
            this.Close();
        }

        //-------------------------------------------------------------------------------- ELEGIR ALUMNO
        private void Max()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select max(ND_Id)+1 as ultimo from NOTA_DETALLE";
            SqlCommand command = new SqlCommand(query, conexion);
            SqlDataReader leer = command.ExecuteReader();
            if (leer.Read())
            {
                txtFolio.Text = leer["ultimo"].ToString();
            }
        }

        private void LlenarEscuelas()
        {
            cboxEscuela.Items.Clear();
            cboxEscuela.Items.Add("Seleccione una escuela");
            try
            {
                cboxEscuela.Enabled = true;
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
                    cboxEscuela.Items.Add(dt.Rows[i]["IA_Nombre"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "No lleno la school");
            }
        } // OK

        private void ObtenerIDEscuelas()
        {
            x = cboxEscuela.SelectedItem.ToString();
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from INSTITUCIONES_ACADEMICAS where IA_Nombre=@IA_Nombre";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Nombre", x);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                valorid = (Convert.ToInt32(leer["IA_Id"]));
                ObtenerIDGrupos();
            }
            else
            {
                MessageBox.Show("No obtuvo id escuelas");
            }
        } // OK

        private void ObtenerIDGrupos()
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "select * from GRUPOS where GR_IdIA=@IA_Id";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@IA_Id", valorid);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                idgrupos = (Convert.ToInt32(leer["GR_Id"]));
            }
            else
            {
                MessageBox.Show( "No leyo");
            }
        } // OK

        private void LlenarGrupos()  // OK
        {
            cboxGrupo.Items.Clear();
            cboxGrupo.Items.Add("Seleccione un grupo");
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM GRUPOS where GR_Estatus = 1 and GR_IdIA = " + idgrupos.ToString() + "  order by GR_Especialidad";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxGrupo.Items.Add(dt.Rows[i]["GR_Especialidad"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR GRUPOS ERROR");
            }
        }

        public void LlenarAlumnos()
        {
            cboxAlumno.Items.Clear();
            cboxAlumno.Items.Add("Seleccione un(a) alumno(a)");
            try
            {
                string grupo = cboxGrupo.SelectedItem.ToString();
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM ALUMNOS  inner join GRUPOS  ON AL_Id = GR_Id  where GR_Especialidad = '" + grupo + "' and GR_IdIA = " + valorid.ToString() + " order by GR_Especialidad";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxAlumno.Items.Add(dt.Rows[i]["AL_Nombre"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR alumn GRID ERROR");
            }
        }//OK
        public void ObtenerIDAlumno()
        {
            try
            {
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM ALUMNOS WHERE AL_Nombre = '" + cboxAlumno.SelectedItem.ToString() + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    idalumno = (leer["AL_Id"].ToString());
                    MessageBox.Show(idalumno);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void VerificarAbono() // Para ver si el alumno elegido tiene algun abono regsitrado
        {
            obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "SELECT * FROM ALUMNOS WHERE AL_Nombre = '" + cboxAlumno.SelectedItem.ToString() + "'";
            SqlCommand comando = new SqlCommand(query, conexion);
        }

        // DISPONIBILIDAD

        public void LlenarPaquetes()
        {
            cboxPaquetes.Items.Clear();
            cboxPaquetes.Items.Add("Seleccione un paquete");
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM PAQUETES where PAQ_Estatus=1";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxPaquetes.Items.Add(dt.Rows[i]["PAQ_Nombre"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR paquetes ERROR");
            }
        } //OK
        public void LlenarServicios()
        {
            cboxServicios.Items.Clear();
            cboxServicios.Items.Add("Seleccione un servicio (opcional)");
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM SERVICIOS where Estatus = 1";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxServicios.Items.Add(dt.Rows[i]["Nombre"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR servicios ERROR");
            }
        }//OK
        public void LlenarExtras()
        {
            cboxExtras.Items.Clear();
            cboxExtras.Items.Add("Seleccione un extra (opcional)");
            try
            {
                DataTable dt = new DataTable();
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM EXTRAS where EX_Estatus = 1";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataAdapter da = new SqlDataAdapter(comando);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cboxExtras.Items.Add(dt.Rows[i]["EX_Nombre"].ToString());
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "LLENAR servicios ERROR");
            }
        }//OK

        // PRECIOS

        public void ObtenerPrecioPaquete(string paquete) 
        {
            try
            {
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM PAQUETES where PAQ_Nombre = @paquete";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@paquete", paquete);
                comando.ExecuteNonQuery();
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    PrecioPaquete = (leer["PAQ_PrecioVenta"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Obtener Precio Paquete ERROR");
            }
        }//OK
        public void ObtenerPrecioServicio(string servicio) 
        {
            try
            {
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM SERVICIOS where Nombre = @servicio";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@servicio", servicio);
                comando.ExecuteNonQuery();
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    PrecioServicio = (leer["Precio"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Obtener Precio Paquete ERROR");
            }
        }//OK
        public void ObtenerPrecioExtra(string extra) 
        {
            try
            {
                obj_conexion = new Clases.Conexion();
                conexion = new SqlConnection(obj_conexion.Con());
                conexion.Open();
                string query = "SELECT * FROM EXTRAS where EX_Nombre = @extra";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@extra", extra);
                comando.ExecuteNonQuery();
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    PrecioExtra = (leer["EX_Precio"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Obtener Precio Paquete ERROR");
            }
        }//OK

        private void cboxEscuela_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ObtenerIDEscuelas();
            ObtenerIDGrupos();
            LlenarGrupos();
        }

        private void cboxEscuela_DropDown(object sender, EventArgs e)
        {
            LlenarEscuelas();
        }

        private void cboxPaquetes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ObtenerPrecioPaquete(cboxPaquetes.SelectedItem.ToString());
            //txtDescripcion.Text = "Paquete: "
            //                     + cboxPaquetes.SelectedItem.ToString()
            //                     + "           "
            //                     + "Precio Unitario: "
            //                     + PrecioPaquete.ToString();
            //btnAgregar.Enabled = true;
        }

        private void cboxServicios_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ObtenerPrecioServicio(cboxServicios.SelectedItem.ToString());
            //txtDescripcion.Text = "Paquete: "
            //                     + cboxPaquetes.SelectedItem.ToString()
            //                     + "           "
            //                     + "Precio Unitario: "
            //                     + PrecioPaquete.ToString()
            //                     + String.Format(Environment.NewLine)
            //                     + "Servicio: "
            //                     + cboxServicios.SelectedItem.ToString()
            //                     + "           "
            //                     + "Precio Unitario: "
            //                    + PrecioServicio.ToString();
        }

        private void cboxExtras_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ObtenerPrecioExtra(cboxExtras.SelectedItem.ToString());
            //MessageBox.Show(PrecioExtra);
            //txtDescripcion.Text = "Paquete: "
            //                      + cboxPaquetes.SelectedItem.ToString()
            //                      + "           "
            //                      + "Precio Unitario: "
            //                      + PrecioPaquete.ToString()
            //                      + String.Format(Environment.NewLine)
            //                      + "Servicio: "
            //                      + cboxServicios.SelectedItem.ToString()
            //                      + "           "
            //                      + "Precio Unitario: "
            //                     + PrecioServicio.ToString()
            //                     + String.Format(Environment.NewLine)
            //                     + "Extra: "
            //                     + cboxExtras.SelectedItem.ToString()
            //                     + "           "
            //                     + "Precio Unitario: "
            //                    + PrecioExtra.ToString();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            dgVentas.ColumnHeadersVisible = true;
            try
            {
                if (cboxPaquetes.SelectedItem == null || cboxPaquetes.SelectedIndex == 0 || cboxPaquetes.Text == "Seleccione un paquete")
                {
                    cboxPaquetes.SelectedItem = "No Seleccionado";
                    PrecioPaquete = "0";
                }
                else
                {
                    ObtenerPrecioPaquete(cboxPaquetes.SelectedItem.ToString());
                    dgVentas.Rows.Add(
                        cboxPaquetes.SelectedItem.ToString(),
                        PrecioPaquete,
                        PrecioPaquete);
                }
                if (cboxServicios.SelectedItem == null || cboxServicios.SelectedIndex == 0 || cboxServicios.Text == "Seleccione un servicio (opcional)")
                {
                    cboxServicios.SelectedItem = "No Seleccionado";
                    PrecioServicio = "0";
                }
                else
                {
                    ObtenerPrecioServicio(cboxServicios.SelectedItem.ToString());
                    dgVentas.Rows.Add(
                        cboxServicios.SelectedItem.ToString(),
                        PrecioServicio,
                        PrecioServicio);

                }
                if (cboxExtras.SelectedItem == null || cboxExtras.SelectedIndex == 0 || cboxExtras.Text == "Seleccione un extra (opcional)")
                {
                    cboxExtras.SelectedItem = "No Seleccionado";
                    PrecioExtra = "0";
                }
                else
                {
                    ObtenerPrecioExtra(cboxExtras.SelectedItem.ToString());
                    dgVentas.Rows.Add(
                        cboxExtras.SelectedItem.ToString(),
                        PrecioExtra,
                        PrecioExtra);
                }
                double suma = 0;
                foreach (DataGridViewRow row in dgVentas.Rows)
                {
                    if (row.Cells["TOTAL"].Value != null)
                        suma += (Convert.ToDouble(row.Cells["TOTAL"].Value));
                }
                txtTotal.Text = suma.ToString();
                cboxPaquetes.SelectedIndex = 0;
                cboxServicios.SelectedIndex = 0;
                cboxExtras.SelectedIndex = 0;
                groupBox2.Enabled = true;
                rbNo.Enabled = true;
                rbSi.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }// OK

        private void dgVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Seguro que desea eliminar este elemento?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    if (dgVentas.RowCount > 1)
                    {
                        dgVentas.Rows.RemoveAt(dgVentas.CurrentRow.Index);
                        double suma = 0;
                        foreach (DataGridViewRow row in dgVentas.Rows)
                        {
                            if (row.Cells["TOTAL"].Value != null)
                                suma += (Convert.ToDouble(row.Cells["TOTAL"].Value));
                        }
                        txtTotal.Text = suma.ToString();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Este elemento no puede eliminarse.");
                }
            }
        }

        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            desicion = 2;
            txtPagaCon.Enabled = true;
            txtCambio.Enabled = true;
            txtTotal.Enabled = false;
        }

        private void rbSi_CheckedChanged(object sender, EventArgs e)
        {
            desicion = 1;
            txtAbono.Enabled = true;
            txtAbono.Focus();
        }

        private void txtAbono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPagaCon.Enabled = true;
                txtdebe.Text = (Convert.ToDouble(txtTotal.Text) - Convert.ToDouble(txtAbono.Text)).ToString();
                txtPagaCon.Focus();
            }
            if (e.KeyChar == 27)
            {
                CleanVenta();
            }
        }//OK

        private void txtPagaCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (desicion == 1) //abono
                {
                    //registra abono
                    //total - abono = pagara
                    //cambio = pago con - pagara
                    txtCambio.Enabled = true;
                    txtCambio.Text = (Convert.ToDouble(txtPagaCon.Text) - Convert.ToDouble(txtAbono.Text)).ToString();
                }
                if (desicion == 2)//no
                {
                    //txtCambio = txtPaga con - total
                    txtTotal.Enabled = true;
                    txtCambio.Enabled = true;
                    txtCambio.Text = (Convert.ToDouble(txtPagaCon.Text) - Convert.ToDouble(txtTotal.Text)).ToString();
                    txtTotal.Enabled = false ;
                    txtCambio.Enabled = false;
                }
            }
        }//OK

        private void cboxGrupo_DropDown(object sender, EventArgs e)
        {

        }

        private void cboxAlumno_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LlenarPaquetes();
            LlenarServicios();
            LlenarExtras();
        }

        private void cboxGrupo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LlenarAlumnos();
        }

        private void btnOkay_Click(object sender, EventArgs e) //Still if not exists
        {
            ObtenerIDAlumno();
            MessageBox.Show(idalumno.ToString());
            try
            {
                if (existe)
                {
                    desc = "";
                    foreach (DataGridViewRow row in dgVentas.Rows)
                    {
                        if (row.Cells["SOLICITO"].Value != null)
                        {
                            desc = (row.Cells["SOLICITO"].Value).ToString();
                        }
                        MessageBox.Show(desc);
                    }
                    if (MessageBox.Show("Desea generar un recibo?", "Pregunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        string query = "UPDATE NOTA_DETALLE SET ND_Id=@ND_Id, ND_Fecha=@ND_Fecha, ND_Anticipo=@ND_Anticipo, ND_Descripcion=@ND_Descripcion, ND_IdAlumno=@ND_IdAlumno, ND_PrecioTotal=@ND_PrecioTotal,ND_Debe=@ND_Debe";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.Clear();
                        comando.Parameters.AddWithValue("@ND_Id", txtFolio.Text);
                        comando.Parameters.AddWithValue("@ND_Fecha", fecha.Value.ToString());
                        comando.Parameters.AddWithValue("@ND_Anticipo", txtAbono.Text);
                        comando.Parameters.AddWithValue("@ND_Descripcion", desc);
                        comando.Parameters.AddWithValue("@ND_IdAlumno", idalumno);
                        comando.Parameters.AddWithValue("@ND_PrecioTotal", txtTotal.Text);
                        comando.Parameters.AddWithValue("@ND_Debe", txtdebe.Text);
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Datos agregados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conexion.Close();
                    }
                    else
                    {
                        MessageBox.Show("Abono registrado con exito!", "Bien hecho!", MessageBoxButtons.OK);
                    }
                }
                if (!existe)
                {
                    foreach (DataGridViewRow row in dgVentas.Rows)
                    {
                        if (row.Cells["SOLICITO"].Value != null)
                        {
                            desc = (row.Cells["SOLICITO"].Value).ToString();
                        }
                        MessageBox.Show(desc);
                    }
                    if (MessageBox.Show("Desea generar un recibo?", "Pregunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        obj_conexion = new Clases.Conexion();
                        conexion = new SqlConnection(obj_conexion.Con());
                        conexion.Open();
                        string query = "INSERT INTO NOTA_DETALLE VALUES(@ND_Id, @ND_Fecha, @ND_Anticipo, @ND_Descripcion,@ND_IdAlumno, @ND_PrecioTotal,@ND_Debe)";
                        SqlCommand comando = new SqlCommand(query, conexion);
                        comando.Parameters.Clear();
                        comando.Parameters.AddWithValue("@ND_Id", txtFolio.Text);
                        comando.Parameters.AddWithValue("@ND_Fecha", fecha.Value.ToString());
                        comando.Parameters.AddWithValue("@ND_Anticipo", txtAbono.Text);
                        comando.Parameters.AddWithValue("@ND_Descripcion", desc);
                        comando.Parameters.AddWithValue("@ND_IdAlumno", idalumno);
                        comando.Parameters.AddWithValue("@ND_PrecioTotal", txtTotal.Text);
                        comando.Parameters.AddWithValue("@ND_Debe", txtdebe.Text);
                        comando.ExecuteNonQuery();
                        MessageBox.Show("Datos agregados con exito", "Operacion Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conexion.Close();
                    }
                    else
                    {
                        MessageBox.Show("Venta realizada con exito!", "Bien hecho!", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void CleanVenta()
        {
            //ALUMNO (A)
            cboxEscuela.Enabled = false;
            cboxGrupo.Enabled = false;
            //btnbuscar.Enabled = true;
            //DISPONIBILIDAD
            cboxPaquetes.Enabled = false;
            cboxServicios.Enabled = false;
            cboxExtras.Enabled = false;
            // Abono
            rbSi.AutoCheck = false;
            rbSi.Checked = false;
            rbSi.AutoCheck = true;
            rbSi.Enabled = false;
            rbNo.AutoCheck = false;
            rbNo.Checked = false;
            rbNo.AutoCheck = true;
            rbNo.Enabled = false;
            txtAbono.Clear();
            txtAbono.Enabled = false;
            txtTotal.Clear();
            txtTotal.Enabled = false;
            txtPagaCon.Clear();
            txtPagaCon.Enabled = false;
            txtCambio.Clear();
            txtCambio.Enabled = false;
            //Grid
            dgVentas.DataSource = null;
            dgVentas.Rows.Clear();
            dgVentas.Refresh();
            //Botones Grid
            // Botones
            btnCancelar.Visible = false;
            btnOkay.Visible = false;
        } //still NO

    }
}
