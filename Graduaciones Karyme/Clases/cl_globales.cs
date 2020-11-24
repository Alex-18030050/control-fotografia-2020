using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Graduaciones_Karyme.Clases
{
    class cl_globales
    {
        SqlConnection conexion;
        char[] May = { 'Z', 'Y', 'X', 'W', 'V', 'E', 'D', 'C', 'A', 'H' };
        public string auditoria(string usuario, string accion)
        {
            Clases.Conexion obj_conexion = new Clases.Conexion();
            conexion = new SqlConnection(obj_conexion.Con());
            conexion.Open();
            string query = "INSERT into AUDITORIAS (AU_Login, AU_Fecha, AU_Actividad) VALUES (@AU_Login, @AU_Fecha,@AU_Actividad)";
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@AU_Login", usuario);
            comando.Parameters.AddWithValue("@AU_Fecha", DateTime.Now.ToString());
            comando.Parameters.AddWithValue("@AU_Actividad", accion);
            comando.ExecuteNonQuery();
            conexion.Close();
            return null;
        }
        public string GenerateRandom(string userpass) // toDo: Finish the random password for users
        {
            return userpass;
        }
        public string Encrypt(string passwd)
        {
            try
            {
                passwd.ToLower();
                char[] cadena = passwd.ToArray();
                if (passwd.Contains('0'))
                {
                    char value = '0';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[0];
                }
                if (passwd.Contains("1"))
                {
                    char value = '1';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[1];
                }
                if (passwd.Contains("2"))
                {
                    char value = '2';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[2];
                }
                if (passwd.Contains("3"))
                {
                    char value = '3';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[3];
                }
                if (passwd.Contains("4"))
                {
                    char value = '4';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[4];
                }
                if (passwd.Contains("5"))
                {
                    char value = '5';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[5];
                }
                if (passwd.Contains("6"))
                {
                    char value = '6';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[6];
                }
                if (passwd.Contains("7"))
                {
                    char value = '7';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[7];
                }
                if (passwd.Contains("8"))
                {
                    char value = '8';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[8];
                }
                if (passwd.Contains("9"))
                {
                    char value = '9';
                    int position = Array.IndexOf(cadena, value);
                    cadena[position] = May[9];
                }

                char temporal0 = cadena[0];
                char temporal1 = cadena[1];
                char temporal2 = cadena[2];
                char temporal3 = cadena[3];
                char temporal4 = cadena[4];
                cadena[0] = cadena[8];
                cadena[8] = temporal0;
                cadena[1] = cadena[7];
                cadena[7] = temporal1;
                cadena[2] = cadena[6];
                cadena[6] = temporal2;
                cadena[3] = cadena[5];
                cadena[5] = temporal3;
                cadena[4] = cadena[4];
                // string toDisplay = string.Join(Environment.NewLine, cadena);  // Para prueba de verificacion de array
                //MessageBox.Show(toDisplay); // Para prueba de verificacion de array
                passwd = new string(cadena);
            }
            catch (Exception)
            {
                MessageBox.Show("Las contraseñas de usuario no pueden tener menos de 9 caracteres", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return passwd;
        }
    }
}
