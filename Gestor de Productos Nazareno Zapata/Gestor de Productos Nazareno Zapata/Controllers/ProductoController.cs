using Gestor_de_Productos_Nazareno_Zapata.Models;
using Gestor_de_Productos_Nazareno_Zapata.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestor_de_Productos_Nazareno_Zapata.Controllers
{
    public class ProductoController
    {
        private readonly IRepository<Producto> _repo;

        public ProductoController(IRepository<Producto> repo)
        {
            _repo = repo;
        }

        public void Agregar(string nombre, decimal precio, int stock)
        {
            _repo.Agregar(new Producto {
                Nombre = nombre,
                Precio = precio,
                Stock = stock
            });
        }

        public List<Producto> ObtenerTodos()
        {
            return _repo.LeerTodos();
        }

        public void Eliminar(int id)
        {
            _repo.Eliminar(id);
        }

        public void Modificar(Producto p)
        {
            _repo.Actualizar(p);
        }
    }
}
