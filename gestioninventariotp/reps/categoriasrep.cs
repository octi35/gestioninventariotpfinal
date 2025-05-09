using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using gestioninventariotp.datos;

namespace gestioninventariotp
{
    public class categoriasrep
    {
        private readonly Conexion conexion = new Conexion();

        public List<categoriasdb> getall()
        {
            var list = new List<categoriasdb>();

            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();
                var query = "select * from Categorias";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                   using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var categoria = new categoriasdb()
                            {
                                CategoriaID = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                            };
                            list.Add(categoria);
                        }
                    }
                }  
                
                return list;
             
            }

        }

        public void agregar(int id, string nombre)
        {
            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();

                var query = $@"Insert into Categorias values  ({nombre})";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                    MessageBox.Show("agregaou");
                }


            }
        }
    }
}
