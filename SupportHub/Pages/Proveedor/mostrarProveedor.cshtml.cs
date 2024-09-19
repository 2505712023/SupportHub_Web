using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using SupportHub.Modelos;
namespace SupportHub.Pages.Proveedor
{
    public class mostrarProveedorModel : PageModel
    {   //definir variable para acceder al archivo appsettings.json
        private readonly IConfiguration configuracion;

        //Lista de objetos de la clase clientes 
        public List<Proveedores> listaProveedor = new List<Proveedores>();

        public mostrarProveedorModel(IConfiguration configuration) { 
        
            this.configuracion = configuration;
        }
        public void OnGet()
        {
            try
            {   // Definir la cadena de conexión
                string cadena = configuracion.GetConnectionString("CadenaConexion");
                // crear objeto de tipo "SqlConnection"
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    // abrir la conexión
                    conexion.Open();

                    // crear objeto "SqlCommand"
                    using (SqlCommand comando = new SqlCommand("sp_obtener_proveedores_general", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        // crer objeto "SqlDataReader
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                // crear objeto de tipo "Cliente"
                                Proveedores Proveedor = new Proveedores();
                                Proveedor.idProveedor = lector.GetInt32(0);
                                Proveedor.codProveedor = lector.GetString(1);
                                Proveedor.nombreProveedor = lector.GetString(2);
                                Proveedor.direccionProveedor = lector.GetString(3);
                                Proveedor.telefonoProveedor = lector.GetString(4);

                                //agregar de objeto a la lista
                                listaProveedor.Add(Proveedor);
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
    }
}
