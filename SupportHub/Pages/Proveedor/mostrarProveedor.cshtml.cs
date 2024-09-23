using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using SupportHub.Modelos;
using System.Text.Json;
namespace SupportHub.Pages.Proveedor
{
    public class mostrarProveedorModel : PageModel
    {   //definir variable para acceder al archivo appsettings.json
        private readonly IConfiguration configuracion;

        //Lista de objetos de la clase clientes 
        public List<Proveedores> listaProveedor = new List<Proveedores>();

        public mostrarProveedorModel(IConfiguration configuration)
        {

            this.configuracion = configuration;
        }
        public void OnGet(string searchQuery = null)
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

                    // Crear objeto "SqlCommand" dependiendo de si se proporciona searchQuery
                    SqlCommand comando;

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        // Si hay un valor de búsqueda, usar el procedimiento de búsqueda
                        comando = new SqlCommand("sp_obtener_proveedor", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        // Asignar parámetros con el valor de búsqueda o '-1'
                        comando.Parameters.AddWithValue("@codProveedor", searchQuery);
                        comando.Parameters.AddWithValue("@nombreProveedor", searchQuery);
                    }
                    else
                    {
                        // Si no hay búsqueda, usar el procedimiento general
                        comando = new SqlCommand("sp_obtener_proveedores_general", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    // Ejecutar el comando y leer los resultados
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            // Crear objeto de tipo "Proveedor"
                            Proveedores proveedor = new Proveedores();
                            proveedor.idProveedor = lector.GetInt32(0);
                            proveedor.codProveedor = lector.GetString(1);
                            proveedor.nombreProveedor = lector.GetString(2);
                            proveedor.direccionProveedor = lector.GetString(3);
                            proveedor.telefonoProveedor = lector.GetString(4);

                            // Agregar objeto a la lista
                            listaProveedor.Add(proveedor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public IActionResult OnDelete(char codProveedor)
        {
            try
            {
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand("sp_eliminar_proveedor", conexion);
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@codProveedor", codProveedor);
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return new JsonResult(new { success = true });
                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }
    }
}