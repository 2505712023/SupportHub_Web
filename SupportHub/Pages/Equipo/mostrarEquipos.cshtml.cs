using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using SupportHub.Helpers;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
namespace SupportHub.Pages.Equipo
{
    [Authorize]
    public class mostrarEquiposModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Equipos> listaEquipos = new List<Equipos>();
        public Equipos newEquipo = new Equipos();
        public String mensajeError = "";
        public String mensajeExito = "";

        public mostrarEquiposModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        public void OnGet(string searchQuery = null, bool exito = false, bool intentoRealizado = false, bool esEliminacion = false, bool eliminado = false)
        {
            this.exito = exito;
            this.intentoRealizado = intentoRealizado;
            this.esEliminacion = esEliminacion;
            this.eliminado = eliminado;
            string cadena = GetAvailableConnectionString();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando;

                    if (string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_equipos", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_equipo", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codEquipo", searchQuery);
                        comando.Parameters.AddWithValue("@tipoEquipo", searchQuery);
                        comando.Parameters.AddWithValue("@marcaEquipo", searchQuery);
                        comando.Parameters.AddWithValue("@modeloEquipo", searchQuery);

                    }

                    using (SqlDataReader lector = comando.ExecuteReader()) 
                    {
                        while (lector.Read()) 
                        {
                            Equipos equipo = new Equipos();
                            equipo.codEquipo = lector.GetString(0);
                            equipo.tipoEquipo = lector.GetString(1);
                            equipo.marcaEquipo = lector.GetString(2);
                            equipo.modeloEquipo = lector.GetString(3);
                            equipo.cantidadAdquirida = lector.GetInt32(4);
                            equipo.cantidadDisponible = lector.GetInt32(5);
                            equipo.precioEquipo = lector.GetDecimal(6);
                            equipo.nombreproveedor= lector.GetString(7);
                            equipo.descripcionEquipo = lector.GetString(9);
                            listaEquipos.Add(equipo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
              


        }
        /*la propiedad [TempData] almacena temporalmente el valor de la variable durante la redirección. Cuando la vista
       lee el valor de la variable, TempData se encarga de eliminarlo de sí misma, lo que hace que al recargar la página
       la variable vuelva a tomar el valor que le hemos asignado. */
        [TempData]
        public bool exito { get; set; } = false;
        [TempData]
        public bool intentoRealizado { get; set; } = false;
        public bool esEliminacion { get; set; } = false;
        [TempData]
        public bool eliminado { get; set; } = false;
        public int coincidencia { get; set; } = 0;


        private string GetAvailableConnectionString()
        {
            // Intenta primero con la cadena de conexión principal
            if (PingHelper.PingHost("100.101.36.39")) // Reemplaza con tu dirección del servidor
            {
                return configuracion.GetConnectionString("CadenaConexion");
            }
            else if (PingHelper.PingHost("25.2.143.28")) // Reemplaza con tu dirección del servidor Hamachi
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
