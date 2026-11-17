using Gestor_de_Productos_Nazareno_Zapata.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestor_de_Productos_Nazareno_Zapata
{
    public partial class FormDetalle : Form
    {
        private int _id;
        public FormDetalle(Producto producto)
        {
            InitializeComponent();

            _id = producto.Id;
            txtNombre.Text = producto.Nombre;
            txtPrecio.Text = producto.Precio.ToString();
            nudStock.Value = producto.Stock;
        }

        public Producto ProductoModificado => new Producto
        {
            Id = _id,
            Nombre = txtNombre.Text,
            Precio = decimal.TryParse(txtPrecio.Text, out decimal p) ? p : 0,
            Stock = (int)nudStock.Value
        };


        private void FormDetalle_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre no puede estar vacio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if(!decimal.TryParse(txtPrecio.Text, out decimal _p))
            {
                MessageBox.Show("El precio debe ser un numero valido.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
