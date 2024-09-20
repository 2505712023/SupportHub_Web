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

        // Constructor para inyectar la configuraci�n
        public mostrarProveedorModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        // M�todo que se ejecuta en la carga de la p�gina
        public void OnGet()
        {
            try
            {
                // Definir la cadena de conexi�n
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                // Crear objeto de tipo "SqlConnection"
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    // Abrir la conexi�n
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

        // M�todo a ejecutar para manejar eliminaci�n de un proveedor
        public async Task<IActionResult> OnPostDeleteAsync(int idProveedor)
        {
            try
            {
                // Definir la cadena de conexi�n
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                // Crear objeto de tipo "SqlConnection"
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    // Abrir la conexi�n
                    await conexion.OpenAsync();

                    // Crear objeto "SqlCommand" para ejecutar el procedimiento almacenado de eliminaci�n
                    using (SqlCommand comando = new SqlCommand("sp_eliminar_proveedor", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idProveedor", idProveedor);

                        // Ejecutar el comando
                        await comando.ExecuteNonQueryAsync();
                    }
                }

                // Redirigir la p�gina de proveedores despu�s de eliminar
                return RedirectToPage("/Proveedor/mostrarProveedor");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

                // Permanecer en la misma p�gina en caso de error
                return Page();
            }
        }
    }
}
