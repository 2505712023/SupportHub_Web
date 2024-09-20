using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using SupportHub.Modelos;

namespace SupportHub.Pages.Proveedor
{
    public class mostrarProveedorModel : PageModel
    {
        // Definir variable para acceder al archivo appsettings.json
        private readonly IConfiguration configuracion;

        // Lista de objetos de la clase Proveedores
        public List<Proveedores> listaProveedor = new List<Proveedores>();

        // Constructor para inyectar la configuración
        public mostrarProveedorModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        // Método que se ejecuta en la carga de la página
        public void OnGet()
        {
            try
            {
                // Definir la cadena de conexión
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                // Crear objeto de tipo "SqlConnection"
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    // Abrir la conexión
                    conexion.Open();

                    // Crear objeto "SqlCommand"
                    using (SqlCommand comando = new SqlCommand("sp_obtener_proveedores_general", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        // Crear objeto "SqlDataReader"
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                // Crear objeto de tipo "Proveedores"
                                Proveedores proveedor = new Proveedores();
                                proveedor.idProveedor = lector.GetInt32(0);
                                proveedor.codProveedor = lector.GetString(1);
                                proveedor.nombreProveedor = lector.GetString(2);
                                proveedor.direccionProveedor = lector.GetString(3);
                                proveedor.telefonoProveedor = lector.GetString(4);

                                // Agregar objeto a la lista
                                listaProveedor.Add(proveedor);
                            }
                            conexion.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Método a ejecutar para manejar eliminación de un proveedor
        public async Task<IActionResult> OnPostDeleteAsync(int idProveedor)
        {
            try
            {
                // Definir la cadena de conexión
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                // Crear objeto de tipo "SqlConnection"
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    // Abrir la conexión
                    await conexion.OpenAsync();

                    // Crear objeto "SqlCommand" para ejecutar el procedimiento almacenado de eliminación
                    using (SqlCommand comando = new SqlCommand("sp_eliminar_proveedor", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idProveedor", idProveedor);

                        // Ejecutar el comando
                        await comando.ExecuteNonQueryAsync();
                    }
                }

                // Redirigir la página de proveedores después de eliminar
                return RedirectToPage("/Proveedor/mostrarProveedor");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                // Permanecer en la misma página en caso de error
                return Page();
            }
        }
    }
}
