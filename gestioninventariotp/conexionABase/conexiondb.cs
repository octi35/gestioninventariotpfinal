using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
    
namespace gestioninventariotp.datos
{
    public class Conexion
    {
        private readonly string cadena = "Server=OCTI\\SQLEXPRESS;Database=inventario;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadena);
        }

        public void ProbarConexion()
        {
            using (SqlConnection conn = ObtenerConexion())
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("✅ Conexión exitosa a la base de datos.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Error de conexión:\n" + ex.Message);
                }
            }
        }
    }
}
