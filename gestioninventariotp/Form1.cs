using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace gestioninventariotp
{
    public partial class Form1 : Form
    {
        productosrep repositorio = new productosrep();
        List<productosdb> productos = new List<productosdb>();
        bool esNuevo = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarProductos();
            CargarCategorias();

            btnNuevo.Click += btnNuevo_Click;
            btnModificar.Click += btnModificar_Click;
            btnEliminar.Click += btnEliminar_Click;
            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;
            btnFiltrar.Click += btnFiltrar_Click;
        }

        private void CargarProductos()
        {
            productos = repositorio.getall();

            categoriasrep catRep = new categoriasrep();
            var categorias = catRep.getall();

            var productosConCategoria = productos.Select(p => new
            {
                p.Codigo,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.Stock,
                Categoria = categorias.FirstOrDefault(c => c.CategoriaID == p.CategoriaID)?.Nombre ?? "Sin categoría"
            }).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productosConCategoria;
        }


        private void CargarCategorias()
        {
            categoriasrep catRep = new categoriasrep();
            var categorias = catRep.getall();

            
            var categoriasFilter = new List<categoriasdb>(categorias);
            categoriasFilter.Insert(0, new categoriasdb { CategoriaID = 0, Nombre = "Todas" });

            cmbCategoria.DataSource = categorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "CategoriaID";

            cmbFiltroCategorias.DataSource = categoriasFilter;
            cmbFiltroCategorias.DisplayMember = "Nombre";
            cmbFiltroCategorias.ValueMember = "CategoriaID";
        }



        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            if (dataGridView1.CurrentRow != null)
            {
                var filaSeleccionada = (dynamic)dataGridView1.CurrentRow.DataBoundItem;

                var producto = new productosdb
                {
                    Codigo = filaSeleccionada.Codigo,
                    Nombre = filaSeleccionada.Nombre,
                    Descripcion = filaSeleccionada.Descripcion,
                    Precio = filaSeleccionada.Precio,
                    Stock = filaSeleccionada.Stock,

                    CategoriaID = ObtenerCategoriaId(filaSeleccionada.Categoria)
                };


                CargarEnFormulario(producto);
                
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var filaSeleccionada = (dynamic)dataGridView1.CurrentRow.DataBoundItem;

                var producto = new productosdb
                {
                    Codigo = filaSeleccionada.Codigo,
                    Nombre = filaSeleccionada.Nombre,
                    Descripcion = filaSeleccionada.Descripcion,
                    Precio = filaSeleccionada.Precio,
                    Stock = filaSeleccionada.Stock,
                    CategoriaID = ObtenerCategoriaId(filaSeleccionada.Categoria) 
                };
                repositorio.eliminar(producto.Codigo);

                CargarProductos();
                LimpiarCampos();
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var producto = ObtenerDesdeFormulario();

            if (esNuevo)
            {
                repositorio.agregar(producto);
            }
            else
            {
                repositorio.modificar(producto);
            }

            CargarProductos();
            LimpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            var nombreFiltro = txtNombreFilter.Text.ToLower().Trim();
            int categoriaIdSeleccionada = (int)cmbFiltroCategorias.SelectedValue;
            categoriasrep catRep = new categoriasrep();
            var categorias = catRep.getall();

            var filtrados = productos.Where(p =>
                (string.IsNullOrEmpty(nombreFiltro) || p.Nombre.ToLower().StartsWith(nombreFiltro)) &&
                (categoriaIdSeleccionada == 0 || p.CategoriaID == categoriaIdSeleccionada)
            ).ToList();

            var productosConCategoria = filtrados.Select(p => new
            {
                p.Codigo,
                p.Nombre,
                p.Descripcion,
                p.Precio,
                p.Stock,
                Categoria = categorias.FirstOrDefault(c => c.CategoriaID == p.CategoriaID)?.Nombre ?? "Sin categoría"
            }).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = productosConCategoria;
        }



        private void LimpiarCampos()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            numPrecio.Value = 0;
            numStock.Value = 0;
            cmbCategoria.SelectedIndex = -1;
        }

        private productosdb ObtenerDesdeFormulario()
        {
            int codigo = 0;
            int.TryParse(txtCodigo.Text, out codigo);

            return new productosdb
            {
                Codigo = codigo,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Precio = numPrecio.Value,
                Stock = (int)numStock.Value,
                CategoriaID = ((dynamic)cmbCategoria.SelectedItem).CategoriaID
            };
        }


        private void CargarEnFormulario(productosdb producto)
        {
            txtCodigo.Text = producto.Codigo.ToString();
            txtNombre.Text = producto.Nombre;
            txtDescripcion.Text = producto.Descripcion;
            numPrecio.Value = producto.Precio;
            numStock.Value = producto.Stock;
            for (int i = 0; i < cmbCategoria.Items.Count; i++)
            {
                var cat = (categoriasdb)cmbCategoria.Items[i]; 
                if (cat.CategoriaID == producto.CategoriaID)   
                {
                    cmbCategoria.SelectedIndex = i;
                    break;
                }
            }

        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {

        }

        private int ObtenerCategoriaId(string nombreCategoria)
        {

            categoriasrep catRep = new categoriasrep();
            var categorias = catRep.getall();
            var categoria = categorias.FirstOrDefault(c => c.Nombre == nombreCategoria);

            return categoria?.CategoriaID ?? 0; 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de texto (*.txt)|*.txt",
                Title = "Guardar como TXT",
                FileName = "Productos"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Línea con fecha y hora
                        writer.WriteLine("Exportado el: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WriteLine();

                        // Escribir los encabezados
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            writer.Write(dataGridView1.Columns[i].HeaderText);
                            if (i < dataGridView1.Columns.Count - 1)
                                writer.Write(" | ");
                        }
                        writer.WriteLine();

                        // Escribir los datos
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].IsNewRow) continue;

                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                writer.Write(dataGridView1.Rows[i].Cells[j].Value?.ToString());
                                if (j < dataGridView1.Columns.Count - 1)
                                    writer.Write(" | ");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show("Datos exportados", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
