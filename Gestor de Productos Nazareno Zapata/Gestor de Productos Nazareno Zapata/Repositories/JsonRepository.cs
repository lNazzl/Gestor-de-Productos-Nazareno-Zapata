using Gestor_de_Productos_Nazareno_Zapata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gestor_de_Productos_Nazareno_Zapata.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : IEntidad
    {
        private readonly string _ruta;

        public JsonRepository(string ruta)
        {
            _ruta = ruta;
        }

        public List<T> LeerTodos()
        {
            if (!File.Exists(_ruta)) return new List<T>();

            try
            {
                var json = File.ReadAllText(_ruta);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Archivo dañado: " + ex.Message);
            }
        }

        public T? BuscarPorId(int id)
        {
            return LeerTodos().FirstOrDefault(e => e.Id == id);
        }

        private void Persistir(List<T> lista)
        {
            try
            {
                var opciones = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                File.WriteAllText(_ruta, JsonSerializer.Serialize(lista, opciones));
            }
            catch(IOException ex)
            {
                throw new InvalidOperationException("No se pudo guardar: " + ex.Message);
            }
        }

        public void Agregar(T item)
        {
            var lista = LeerTodos();
            item.Id = lista.Count > 0 ? lista.Max(e => e.Id) + 1 : 1;
            lista.Add(item);
            Persistir(lista);
        }

        public void Actualizar(T item)
        {
            var lista = LeerTodos();
            var index = lista.FindIndex(e => e.Id == item.Id);
            if(index >= 0)
            {
                lista[index] = item;
                Persistir(lista);
            }
        }

        public void Eliminar(int id)
        {
            var lista = LeerTodos();
            lista.RemoveAll(e => e.Id == id);
            Persistir(lista);
        }
    }
}
