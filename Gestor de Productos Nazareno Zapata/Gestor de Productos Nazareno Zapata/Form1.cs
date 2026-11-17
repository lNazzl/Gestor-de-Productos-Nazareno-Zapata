using Gestor_de_Productos_Nazareno_Zapata.Controllers;
using Gestor_de_Productos_Nazareno_Zapata.Models;
using System.Security.Cryptography.X509Certificates;

namespace Gestor_de_Productos_Nazareno_Zapata
{
    public partial class Form1 : Form
    {
        private ProductoController _controller;
        public Form1(ProductoController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizarTabla();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre no puede estar  vacio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombre.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("Ingrese un precio valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrecio.Focus();
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Ingrese un stock valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStock.Focus();
                return;
            }

            _controller.Agregar(nombre, precio, stock);

            ActualizarTabla();
            LimpiarCampos();
        }

        private void ActualizarTabla()
        {
            var lista = _controller.ObtenerTodos();
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = lista;

        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtNombre.Focus();
        }


        private void dgvProductos_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Verificamos si la columna actual se llama "Precio" y tiene un valor
            if (dgvProductos.Columns[e.ColumnIndex].Name == "Precio" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal precio))
                {
                    // Cambia únicamente cómo se ve visualmente, sin alterar el objeto real
                    e.Value = precio.ToString("C");
                    e.FormattingApplied = true;
                }
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            var lista = _controller.ObtenerTodos();

            if (lista.Count == 0)
            {
                MessageBox.Show("No hay productos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
                saveFileDialog.Title = "Exportar productos a TXT";
                saveFileDialog.FileName = "productos.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var lineas = new List<string>();
                        lineas.Add("==== LISTADO DE PRODUCTOS ====");
                        lineas.Add($"Fecha de exportacion: {DateTime.Now}");
                        lineas.Add("-----------------------------------");

                        foreach (var p in lista)
                        {
                            string linea = $"ID: {p.Id} | Nombre: {p.Nombre} | Precio: {p.Precio:C} | Stock: {p.Stock}";
                            lineas.Add(linea);
                        }

                        System.IO.File.WriteAllLines(saveFileDialog.FileName, lineas);

                        MessageBox.Show("Productos exportados con exito.", "Exito.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrio un error al guardar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count == 0)
                {
                    MessageBox.Show($"Porfavor, selecciona un producto de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

                using var formulario = new FormDetalle(productoSeleccionado);
                if (formulario.ShowDialog() != DialogResult.OK)
                    return;

                _controller.Modificar(formulario.ProductoModificado);
                ActualizarTabla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Porfavor, seleccione un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var productoSeleccionado = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;

                var confirmacion = MessageBox.Show($"Esta seguro de que desea eliminar el producto {productoSeleccionado.Nombre}?", "Confirmar eliminacion.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    _controller.Eliminar(productoSeleccionado.Id);

                    ActualizarTabla();

                    MessageBox.Show("Producto eliminado con exito.", "Exito.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
        }
    }
}
