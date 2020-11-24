using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduaciones_Karyme.Clases
{
    class Conexion
    {
        public string Con() 

        {
            string mi_conexion = ("Data Source=LENOVO\\SQLEXPRESS;Initial Catalog=GraduacionesKaryme;Integrated Security=True");
            return mi_conexion;
        }
    }
}
