using Microsoft.AspNetCore.Mvc;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
namespace SupportHub.Pages.Empleado
{
    [Authorize]
    public class mostrarEmpleadosModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Empleados> listaEmpleado = new List<Empleados>();
        public Empleados newEmpleado = new Empleados();
        public String mensajeError = "";
        public List<SelectListItem> Area = new List<SelectListItem>();
        public List<SelectListItem> Cargo = new List<SelectListItem>();
        public mostrarEmpleadosModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        public void OnGet(string searchQuery = null, bool exito = false, bool intentoRealizado = false, bool esEliminacion = false, bool eliminado = false)
        {
            string cadena = GetAvailableConnectionString();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando;
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_empleado", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codEmpleado", searchQuery);
                        comando.Parameters.AddWithValue("@nombreEmpleado", searchQuery);
                        comando.Parameters.AddWithValue("@apellidoEmpleado", searchQuery);
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_empleados_general", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Empleados empleado = new Empleados();

                            empleado.codEmpleado = lector.GetString(0);
                            empleado.nombreEmpleado = lector.GetString(1);
                            empleado.apellidoEmpleado = lector.GetString(2);
                            empleado.telefonoEmpleado = lector.GetString(3);
                            empleado.emailEmpleado = lector.GetString(4);
                            empleado.codArea = lector.GetString(5);
                            empleado.nombreArea = lector.GetString(6);
                            empleado.codCargo = lector.GetString(7);
                            empleado.nombreCargo = lector.GetString(8);

                            listaEmpleado.Add(empleado);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
            // Lista de Area
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand("select idArea,  codArea  + ' - ' + nombreArea as [Areas] from Areas", conexion);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            SelectListItem area = new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            };
                            Area.Add(area);
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al obtener clientes: " + ex.Message);
            }
            //Lista de  Cargo 
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand("select idCargo,  codCargo  + ' - ' + nombreCargo as [Cargo] from Cargos", conexion);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            SelectListItem cargo = new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            };
                            Cargo.Add(cargo);
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al obtener clientes: " + ex.Message);
            }

        }

        [TempData]
        public bool exito { get; set; } = false;
        [TempData]
        public bool intentoRealizado { get; set; } = false;
        public bool esEliminacion { get; set; } = false;
        [TempData]
        public bool eliminado { get; set; } = false;
        public int coincidencia { get; set; } = 0;

        public IActionResult OnPost()
        {
            newEmpleado.nombreEmpleado = Request.Form["nombre"];
            newEmpleado.apellidoEmpleado = Request.Form["apellido"];
            newEmpleado.telefonoEmpleado = Request.Form["telefono"];
            newEmpleado.emailEmpleado = Request.Form["email"];
            newEmpleado.IdArea = int.Parse(Request.Form["IdArea"]);
            newEmpleado.IdCargo = int.Parse(Request.Form["IdCargo"]);
            // Verificamos si el modelo es válido
            if (!ModelState.IsValid)
            {
                mensajeError = "Todos los campos son requeridos.";
                return Page();
            }

           

            // Agregar un nuevo empleado
            try
            {
                // Obtenemos la cadena de conexión
                string cadena = GetAvailableConnectionString();
                int registrosAgregados = 0;
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    string query = "sp_crear_empleado @nombreEmpleado, @apellidoEmpleado, @telefonoEmpleado, @emailEmpleado, @IdCargo, @IdArea";
                    SqlCommand comando = new SqlCommand(query, conexion);

                    // Asignamos los parámetros
                    comando.Parameters.AddWithValue("@nombreEmpleado", newEmpleado.nombreEmpleado);
                    comando.Parameters.AddWithValue("@apellidoEmpleado", newEmpleado.apellidoEmpleado);
                    comando.Parameters.AddWithValue("@telefonoEmpleado", newEmpleado.telefonoEmpleado);
                    comando.Parameters.AddWithValue("@emailEmpleado", newEmpleado.emailEmpleado);
                    comando.Parameters.AddWithValue("@IdArea", newEmpleado.IdArea);
                    comando.Parameters.AddWithValue("@IdCargo", newEmpleado.IdCargo);

                    registrosAgregados = Convert.ToInt32(comando.ExecuteScalar().ToString());
                }
                exito = (registrosAgregados == 1) ? true : false; intentoRealizado = true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return Page();
            }
            return RedirectToPage("/Empleado/mostrarEmpleados");
        }




        private string GetAvailableConnectionString()
        {

            if (PingHelper.PingHost("100.101.36.39"))
            {
                return configuracion.GetConnectionString("CadenaConexion");
            }
            else if (PingHelper.PingHost("25.2.143.28"))
            {
                return configuracion.GetConnectionString("CadenaConexionHamachi");
            }
            else
            {
                throw new Exception("No se puede conectar a ninguna base de datos.");
            }
        }
    }




}

