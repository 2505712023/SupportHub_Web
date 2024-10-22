using Microsoft.AspNetCore.Mvc;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace SupportHub.Pages.Ticket
{
    [Authorize]
    public class mostrarTicketsModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Tickets> listaTickets = new List<Tickets>();
        public Tickets newTickets = new Tickets();
        public String mensajeError = "";

        public mostrarTicketsModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }



        public void OnGet(string searchQuery = null, bool exito = false, bool intentoRealizado = false, bool esEliminacion = false, bool eliminado = false)
        {
            string cadena = GetAvailableConnectionString();
            if (cadena == null && !string.IsNullOrEmpty(mensajeError))
            {
                // Pasar ErrorMessage a la vista si hay un error de conexión
                ViewData["ErrorMessage"] = mensajeError;

            }


            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando;
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_ticket_filtrada", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codTicket", searchQuery);
                        comando.Parameters.AddWithValue("@titulo", searchQuery);
                        comando.Parameters.AddWithValue("@nombreEmpleado", searchQuery);
                        comando.Parameters.AddWithValue("@nombreEmpleadoIT", searchQuery);
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_ticket_general", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Tickets ticket = new Tickets();

                            ticket.codTicket = !lector.IsDBNull(0) ? lector.GetString(0) : string.Empty;
                            ticket.titulo = !lector.IsDBNull(1) ? lector.GetString(1) : string.Empty;
                            ticket.descripcion = !lector.IsDBNull(2) ? lector.GetString(2) : string.Empty;
                            ticket.imagen = !lector.IsDBNull(3) ? lector.GetString(3) : string.Empty;
                            ticket.fechaCreacion = !lector.IsDBNull(4) ? lector.GetDateTime(4).ToString() : string.Empty;
                            ticket.fechaFinalizado = !lector.IsDBNull(5) ? lector.GetDateTime(5).ToString() : string.Empty;
                            ticket.nombreEmpleado = !lector.IsDBNull(6) ? lector.GetString(6) : string.Empty;
                            ticket.nombreEstado = !lector.IsDBNull(7) ? lector.GetString(7) : string.Empty;
                            ticket.nombrePrioridad = !lector.IsDBNull(8) ? lector.GetString(8) : string.Empty;
                            ticket.nombreEmpleadoIT = !lector.IsDBNull(9) ? lector.GetString(9) : string.Empty;
                            listaTickets.Add(ticket);
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

            try
            {
                // Intenta primero con la cadena de conexión principal
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
            catch (Exception ex)
            {

                mensajeError = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
                return null;
            }
        }
    }
}









