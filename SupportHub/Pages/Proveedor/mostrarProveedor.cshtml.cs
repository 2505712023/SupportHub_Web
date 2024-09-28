using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;

namespace SupportHub.Pages.Proveedor
{
    public class mostrarProveedorModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Proveedores> listaProveedores = new List<Proveedores>();
        public Proveedores newProveedor = new Proveedores();
        public String mensajeError = "";
        public String mensajeExito = "";

        public mostrarProveedorModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        public void OnGet(string searchQuery = null, bool exito = false, bool intentoRealizado = false)
        {
            this.exito = exito;
            this.intentoRealizado = intentoRealizado;

            try
            {
                string cadena = configuracion.GetConnectionString("CadenaConexion");
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando;

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_proveedor", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codProveedor", searchQuery);
                        comando.Parameters.AddWithValue("@nombreProveedor", searchQuery);
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_proveedores_general", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Proveedores proveedor = new Proveedores();
                            proveedor.idProveedor = lector.GetInt32(0);
                            proveedor.codProveedor = lector.GetString(1);
                            proveedor.nombreProveedor = lector.GetString(2);
                            proveedor.direccionProveedor = lector.GetString(3);
                            proveedor.telefonoProveedor = lector.GetString(4);
                            listaProveedores.Add(proveedor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public bool exito { get; set; } = false;
        public bool intentoRealizado { get; set; } = false;

        public IActionResult OnPost()
        {
            newProveedor.codProveedor = Request.Form["Codigo"];
            newProveedor.nombreProveedor = Request.Form["nombre"];
            newProveedor.direccionProveedor = Request.Form["direccion"];
            newProveedor.telefonoProveedor = Request.Form["telefono"];

            if (string.IsNullOrEmpty(newProveedor.codProveedor) ||
                string.IsNullOrEmpty(newProveedor.nombreProveedor) ||
                string.IsNullOrEmpty(newProveedor.direccionProveedor) ||
                string.IsNullOrEmpty(newProveedor.telefonoProveedor))
            {
                mensajeError = "Todos los campos son requeridos";
                return Page();
            }

            if (Request.Form["esModificacion"] == "false")
            {
                try
                {
                    string cadena = configuracion.GetConnectionString("CadenaConexion");
                    int registrosAgregados = 0;
                    using (SqlConnection conexion = new SqlConnection(cadena))
                    {
                        conexion.Open();
                        string query = "dbo.sp_crear_proveedor @codProveedor,@nombreProveedor,@direccionProveedor,@telefonoProveedor";
                        SqlCommand comando = new SqlCommand(query, conexion);

                        comando.Parameters.AddWithValue("@codProveedor", newProveedor.codProveedor);
                        comando.Parameters.AddWithValue("@nombreProveedor", newProveedor.nombreProveedor);
                        comando.Parameters.AddWithValue("@direccionProveedor", newProveedor.direccionProveedor);
                        comando.Parameters.AddWithValue("@telefonoProveedor", newProveedor.telefonoProveedor);

                        registrosAgregados = Convert.ToInt32(comando.ExecuteScalar().ToString());
                    }
                    exito = registrosAgregados == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                    return Page();
                }
            }
            else
            {
                newProveedor.idProveedor = Convert.ToInt32(Request.Form["id"]);
                try
                {
                    string cadena = configuracion.GetConnectionString("CadenaConexion");
                    using (SqlConnection conexion = new SqlConnection(cadena))
                    {
                        conexion.Open();
                        string query = "dbo.sp_modificar_proveedor @codProveedor,@idProveedor,@nombreProveedor,@direccionProveedor,@telefonoProveedor";
                        SqlCommand comando = new SqlCommand(query, conexion);

                        comando.Parameters.AddWithValue("@idProveedor", newProveedor.idProveedor);
                        comando.Parameters.AddWithValue("@codProveedor", newProveedor.codProveedor);
                        comando.Parameters.AddWithValue("@nombreProveedor", newProveedor.nombreProveedor);
                        comando.Parameters.AddWithValue("@direccionProveedor", newProveedor.direccionProveedor);
                        comando.Parameters.AddWithValue("@telefonoProveedor", newProveedor.telefonoProveedor);

                        comando.ExecuteNonQuery();
                    }
                    exito = true;
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                    return Page();
                }
            }

            if (exito)
            {
                return RedirectToPage("/Proveedor/mostrarProveedor", new { exito = true });
            }
            else
            {
                return RedirectToPage("/Proveedor/mostrarProveedor", new { intentoRealizado = true });
            }
        }

        public int idProveedor { get; set; }
        public IActionResult OnPostEliminar([FromBody] int idProveedor)
        {
            if (idProveedor <= 0)
            {
                return new JsonResult(new { success = false, message = "ID de proveedor no válido." });
            }

            try
            {
                using (var conexion = new SqlConnection(configuracion.GetConnectionString("CadenaConexion")))
                {
                    conexion.Open();
                    using (var comando = new SqlCommand("sp_eliminar_proveedor", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idProveedor", idProveedor);
                        comando.ExecuteNonQuery();
                    }
                }

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
               
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
