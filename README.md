# gestioninventariotp
![image](https://github.com/user-attachments/assets/59f293a0-2388-493d-b251-e7cc62445a4f)
![image](https://github.com/user-attachments/assets/95985961-10c3-4b4a-ad56-d4db89a47c18)

![image](https://github.com/user-attachments/assets/43bc1e55-20b5-4f65-a2bf-f8e1ecbed2ce)
bool esnuevo es utilizado como bandera para ver si el producto existe

codigo de buscar por codigodeproducto : private void btnBuscarPorCodigo_Click(object sender, EventArgs e)
{
    if (int.TryParse(txtCodigoBuscar.Text.Trim(), out int codigo))
    {
        var producto = repositorio.obtenerPorCodigo(codigo);
        if (producto != null)
        {
            var categorias = new categoriasrep().getall();
            var productoConCategoria = new[]
            {
                new
                {
                    producto.Codigo,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    Categoria = categorias.FirstOrDefault(c => c.CategoriaID == producto.CategoriaID)?.Nombre ?? "Sin categoría"
                }
            };

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productoConCategoria;
        }
        else
        {
            MessageBox.Show("Producto no encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
    else
    {
        MessageBox.Show("Ingrese un código válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}

