using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupportHub.Helpers;
using SupportHub.Modelos;
using System.Data.SqlClient;
using System.Text;

namespace SupportHub.Pages.Entregas
{
    public class mostrarEntregasModel : PageModel
    {
        private readonly IConfiguration configuracion;
        private readonly ILogger logger;
        public List<Entrega> listaEntregas = new();

        public mostrarEntregasModel(IConfiguration configuracion, ILogger<mostrarEntregasModel> logger)
        {
            this.configuracion = configuracion;
            this.logger = logger;
        }

        public void OnGet()
        {
            try
            {
                using (SqlConnection conexion = new(GetAvailableConnectionString()))
                {
                    conexion.Open();
                    using (SqlCommand comando = new("[dbo].[sp_obtener_entregas]", conexion))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                Entrega newEntrega = new();
                                newEntrega.idEntrega = lector.GetInt32(0);
                                newEntrega.codEntrega = lector.GetString(1);
                                newEntrega.nombreTipoEntrega = lector.GetString(2);
                                newEntrega.tipoEquipo = lector.GetString(3);
                                newEntrega.marcaEquipo = lector.GetString(4);
                                newEntrega.modeloEquipo = lector.GetString(5);
                                newEntrega.cantidadEntrega = lector.GetInt32(6);
                                newEntrega.nombreEmpleadoEntrego = lector.GetString(7);
                                newEntrega.nombreEmpleadoRecibio = lector.GetString(8);
                                newEntrega.fechaEntrega = lector.IsDBNull(9) ? string.Empty : lector.GetDateTime(9).ToString("dd-MM-yyyy");
                                newEntrega.fechaDevolucion = lector.IsDBNull(10) ? string.Empty : lector.GetDateTime(10).ToString("dd-MM-yyyy");
                                newEntrega.observacionEntrega = lector.GetString(11);
                                newEntrega.idTipoEntrega = lector.GetInt32(12);
                                newEntrega.idEquipo = lector.GetInt32(13);
                                newEntrega.idEmpleadoEntrega = lector.GetInt32(14);
                                newEntrega.idEmpleadoRecibe = lector.GetInt32(15);
                                listaEntregas.Add(newEntrega);
                            }
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

        public string GetTiposDeEntregas()
        {
            List<SelectListItem> tiposDeEntregas = [];
            using (SqlConnection conection = new(GetAvailableConnectionString()))
            {
                conection.Open();
                using (SqlCommand comando = new("select * from dbo.TiposEntregas", conection))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            tiposDeEntregas.Add(new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            });
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            if (tiposDeEntregas != null)
            {
                foreach (var tipoEntrega in tiposDeEntregas)
                {
                    sb.Append($"<option value='{tipoEntrega.Value}'>{tipoEntrega.Text}</option>");
                }
            }

            return sb.ToString();
        }

        public string GetEmpleados()
        {
            List<SelectListItem> empleados = [];
            using (SqlConnection conection = new(GetAvailableConnectionString()))
            {
                conection.Open();
                using (SqlCommand comando = new("select idEmpleado, codEmpleado + ' - ' + nombreEmpleado + ' ' + apellidoEmpleado from dbo.Empleados", conection))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            empleados.Add(new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            });
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            if (empleados != null)
            {
                foreach (var empleado in empleados)
                {
                    sb.Append($"<option value='{empleado.Value}'>{empleado.Text}</option>");
                }
            }

            return sb.ToString();
        }

        public string GetEquipos()
        {
            List<SelectListItem> equipos = [];
            using (SqlConnection conection = new(GetAvailableConnectionString()))
            {
                conection.Open();
                using (SqlCommand comando = new("select idEquipo, codEquipo + ' - ' + tipoEquipo + ' ' + marcaEquipo + ' ' + modeloEquipo from dbo.Equipos", conection))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            equipos.Add(new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            });
                        }
                    }
                }
            }

            var sb = new StringBuilder();
            if (equipos != null)
            {
                foreach (var equipo in equipos)
                {
                    sb.Append($"<option value='{equipo.Value}'>{equipo.Text}</option>");
                }
            }

            return sb.ToString();
        }

        public void OnPost()
        {
            foreach (var key in Request.Form.Keys)
            {
                logger.LogInformation($"Form key: {key}, value: {Request.Form[$"{key}"]}");
            }
        }
    }
}
