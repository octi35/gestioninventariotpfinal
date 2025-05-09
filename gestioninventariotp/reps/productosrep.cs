using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using gestioninventariotp.datos;

namespace gestioninventariotp
{
    public class productosrep
    {
        private readonly Conexion conexion = new Conexion();

    
        public List<productosdb> getall()
        {
            var list = new List<productosdb>();

            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();
                var query = "SELECT * FROM Productos";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var producto = new productosdb()
                        {
                            Codigo = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Precio = reader.GetDecimal(3),
                            Stock = reader.GetInt32(4),
                            CategoriaID = reader.GetInt32(5)
                        };
                        list.Add(producto);
                    }
                }
            }

            return list;
        }

     
        public void agregar(productosdb producto)
        {
            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();

                var query = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, CategoriaID) VALUES (@nombre, @descripcion, @precio, @stock, @categoriaId)";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                    command.Parameters.AddWithValue("@nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.Parameters.AddWithValue("@stock", producto.Stock);
                    command.Parameters.AddWithValue("@categoriaId", producto.CategoriaID);

                    command.ExecuteNonQuery();
                    MessageBox.Show(" Producto agregado correctamente.");
                }
            }
        }

     
        public void modificar(productosdb producto)
        {
            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();

                var query = @"UPDATE Productos SET Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, Stock = @stock, CategoriaID = @categoriaId WHERE Codigo = @codigo";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                    command.Parameters.AddWithValue("@nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.Parameters.AddWithValue("@stock", producto.Stock);
                    command.Parameters.AddWithValue("@categoriaId", producto.CategoriaID);
                    command.Parameters.AddWithValue("@codigo", producto.Codigo);

                    int filas = command.ExecuteNonQuery();
                    MessageBox.Show(filas > 0 ? " Producto modificado correctamente." : " Producto no encontrado.");
                }
            }
        }

   
        public void eliminar(int codigo)
        {
            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();

                var query = "DELETE FROM Productos WHERE Codigo = @codigo";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                    command.Parameters.AddWithValue("@codigo", codigo);

                    int filas = command.ExecuteNonQuery();
                    MessageBox.Show(filas > 0 ? " Producto eliminado correctamente." : " Producto no encontrado.");
                }
            }
        }

        
        public productosdb obtenerPorCodigo(int codigo)
        {
            using (SqlConnection connectionm = conexion.ObtenerConexion())
            {
                connectionm.Open();
                var query = "SELECT * FROM Productos WHERE Codigo = @codigo";

                using (SqlCommand command = new SqlCommand(query, connectionm))
                {
                    command.Parameters.AddWithValue("@codigo", codigo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new productosdb()
                            {
                                Codigo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Descripcion = reader.GetString(2),
                                Precio = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4),
                                CategoriaID = reader.GetInt32(5)
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}

