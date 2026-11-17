using Gestor_de_Productos_Nazareno_Zapata.Controllers;
using Gestor_de_Productos_Nazareno_Zapata.Models;
using Gestor_de_Productos_Nazareno_Zapata.Repositories;

namespace Gestor_de_Productos_Nazareno_Zapata
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();


            var repo = new JsonRepository<Producto>("productos.json");
            var controller = new ProductoController(repo);
            var form = new Form1(controller);
            Application.Run(form);
        }
    }
}