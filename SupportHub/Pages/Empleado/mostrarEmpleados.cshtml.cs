using Microsoft.AspNetCore.Mvc;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
namespace SupportHub.Pages.Empleado
{
    public class mostrarEmpleadosModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Empleados> listaEmpleado= new List<Empleados>();
        public Empleados newEmpleado = new Empleados();

        public mostrarEmpleadosModel(IConfiguration configuration) { 
        this.configuracion = configuration;
        }

        public void OnGet(string searchQuery = null)
        {
            string cadena = GetAvailableConnectionString();

            try
            {
                using (SqlConnection conexion= new SqlConnection(cadena)) {
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
