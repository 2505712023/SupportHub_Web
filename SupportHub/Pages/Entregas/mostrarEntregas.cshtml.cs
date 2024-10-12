using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SupportHub.Helpers;
using SupportHub.Modelos;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata.Ecma335;
using System.Globalization;

namespace SupportHub.Pages.Entregas
{
    public class mostrarEntregasModel : PageModel
    {
        private readonly IConfiguration configuracion;
        private readonly Conexiones conexion;
        public List<Entrega> listaEntregas = new();
        public Entrega newEntrega = new();
        public string mensajeError { get; set; } = "";
        [TempData]
        public bool exito { get; set; } = false;
        [TempData]
        public bool eliminada { get; set; } = false;
        [TempData]
        public bool devolucion { get; set; } = false;
        [TempData]
        public bool devolucionEliminada { get; set; } = false;

        public mostrarEntregasModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
            conexion = new(this.configuracion);
        }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(conexion.ObtenerCadenaDisponible()))
            {
                ViewData["errorConexion"] = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
            }

            try
            {
                using (SqlConnection conexion = new(cadena))
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
                                newEntrega.equipo = lector.GetString(3);
                                newEntrega.cantidadEntrega = lector.GetInt32(4);
                                newEntrega.nombreEmpleadoEntrego = lector.GetString(5);
                                newEntrega.nombreEmpleadoRecibio = lector.GetString(6);
                                newEntrega.fechaEntrega = lector.IsDBNull(7) ? string.Empty : lector.GetDateTime(7).ToString("dd-MM-yyyy");
                                newEntrega.fechaDevolucion = lector.IsDBNull(8) ? string.Empty : lector.GetDateTime(8).ToString("dd-MM-yyyy");
                                newEntrega.observacionEntrega = lector.IsDBNull(9) ? string.Empty : lector.GetString(9);
                                newEntrega.idTipoEntrega = lector.GetInt32(10);
                                newEntrega.idEquipo = lector.GetInt32(11);
                                newEntrega.idEmpleadoEntrega = lector.GetInt32(12);
                                newEntrega.idEmpleadoRecibe = lector.GetInt32(13);
                                listaEntregas.Add(newEntrega);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               ViewData["errorSQL"] = "Error: " + ex.Message;
            }
        }

        public string GetTiposDeEntregas()
        {
            string cadena = conexion.ObtenerCadenaDisponible();
            if (string.IsNullOrEmpty(cadena))
            {
                ViewData["errorConexion"] = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
                return "";
            }

            List<SelectListItem> tiposDeEntregas = [];
            using (SqlConnection conection = new(cadena))
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
            string cadena = conexion.ObtenerCadenaDisponible();
            if (string.IsNullOrEmpty(cadena))
            {
                ViewData["errorConexion"] = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
                return "";
            }

            List<SelectListItem> empleados = [];
            using (SqlConnection conection = new(cadena))
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
            string cadena = conexion.ObtenerCadenaDisponible();
            if (string.IsNullOrEmpty(cadena))
            {
                ViewData["errorConexion"] = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
                return "";
            }

            var sb = new StringBuilder();
            using (SqlConnection conection = new(cadena))
            {
                conection.Open();
                using (SqlCommand comando = new("select E.idEquipo, E.codEquipo + ' - ' + E.tipoEquipo + ' ' + E.marcaEquipo + ' ' + E.modeloEquipo, ED.cantidadDisponible from dbo.Equipos E inner join dbo.vwCantidadEquiposDisponibles ED on E.idEquipo = ED.idEquipo", conection))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            sb.Append($"<option data-disponible='{lector.GetInt32(2)}' value='{lector.GetInt32(0)}'>{lector.GetString(1)} ({lector.GetInt32(2)} disponibles)</option>");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public IActionResult OnPost()
        {
            string cadena = conexion.ObtenerCadenaDisponible();
            if (string.IsNullOrEmpty(cadena))
            {
                ViewData["errorConexion"] = "El sistema no tiene conexión con el servidor. Favor notifique el impase al administrador.";
                return Page();
            }

            newEntrega.codEntrega = Request.Form["codEntrega"].ToString();

            try
            {
                int registrosAlterados = 0;
                int registrosEliminados = 0;
                int devolucionesAgregadas = 0;
                int devolucionesEliminadas = 0;
                if (Request.Form["esEliminacion"] == "true")
                {
                    using (SqlConnection conexion = new(cadena))
                    {
                        conexion.Open();
                        SqlCommand comando = new("dbo.sp_eliminar_entrega @codEntrega", conexion);

                        comando.Parameters.AddWithValue("@codEntrega", newEntrega.codEntrega);

                        registrosEliminados = Convert.ToInt32(comando.ExecuteScalar().ToString());
                    }
                }
                else if (Request.Form["esDevolucion"] == "true")
                {
                    DateTime fechaDevolucion = DateTime.ParseExact(Request.Form["fechaDevolucion"], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    using (SqlConnection conexion = new(cadena))
                    {
                        conexion.Open();
                        SqlCommand comando = new("update dbo.Entregas set fechaDevolucion = @fechaDevolucion where codEntrega = @codEntrega", conexion);

                        comando.Parameters.AddWithValue("@codEntrega", newEntrega.codEntrega);
                        comando.Parameters.AddWithValue("@fechaDevolucion", fechaDevolucion);

                        devolucionesAgregadas = comando.ExecuteNonQuery();
                    }
                }
                else if (Request.Form["esEliminacionDevolucion"] == "true")
                {
                    cadena = string.Empty;
                    using (SqlConnection conexion = new(cadena))
                    {
                        conexion.Open();
                        SqlCommand comando = new("update dbo.Entregas set fechaDevolucion = null where codEntrega = @codEntrega", conexion);

                        comando.Parameters.AddWithValue("@codEntrega", newEntrega.codEntrega);

                        devolucionesEliminadas = comando.ExecuteNonQuery();
                    }
                }
                else
                {
                    newEntrega.idTipoEntrega = Convert.ToInt32(Request.Form["idTipoEntrega"]);
                    //newEntrega.fechaEntrega = Convert.ToDateTime(Request.Form["fechaEntrega"]).ToString("yyyy-MM-dd");
                    DateTime fechaEntrega = DateTime.ParseExact(Request.Form["fechaEntrega"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    newEntrega.idEmpleadoEntrega = Convert.ToInt32(Request.Form["idEmpleadoEntrega"]);
                    newEntrega.idEmpleadoRecibe = Convert.ToInt32(Request.Form["idEmpleadoRecibe"]);
                    newEntrega.idEquipo = Convert.ToInt32(Request.Form["idEquipo"]);
                    newEntrega.cantidadEntrega = Convert.ToInt32(Request.Form["cantidadEntrega"]);
                    newEntrega.observacionEntrega = Request.Form["observacionEntrega"].ToString();
                    if (Request.Form["esModificacion"] == "true")
                    {
                        using (SqlConnection conexion = new(cadena))
                        {
                            conexion.Open();
                            SqlCommand comando = new("dbo.sp_modificar_entrega @codEntrega, @idTipoEntrega, @cantidadEntrega, @fechaEntrega, @observacionEntrega, @idEmpleadoEntrega, @idEmpleadoRecibe, @idEquipo", conexion);

                            comando.Parameters.AddWithValue("@codEntrega", newEntrega.codEntrega);
                            comando.Parameters.AddWithValue("@idTipoEntrega", newEntrega.idTipoEntrega);
                            comando.Parameters.AddWithValue("@fechaEntrega", fechaEntrega);
                            comando.Parameters.AddWithValue("@idEmpleadoEntrega", newEntrega.idEmpleadoEntrega);
                            comando.Parameters.AddWithValue("@idEmpleadoRecibe", newEntrega.idEmpleadoRecibe);
                            comando.Parameters.AddWithValue("@idEquipo", newEntrega.idEquipo);
                            comando.Parameters.AddWithValue("@cantidadEntrega", newEntrega.cantidadEntrega);
                            comando.Parameters.AddWithValue("@observacionEntrega", newEntrega.observacionEntrega);

                            registrosAlterados = Convert.ToInt32(comando.ExecuteScalar().ToString());
                        }
                    }
                    else
                    {
                        using (SqlConnection conexion = new(cadena))
                        {
                            conexion.Open();
                            SqlCommand comando = new("dbo.sp_crear_entrega @idTipoEntrega, @fechaEntrega, @idEmpleadoEntrega, @idEmpleadoRecibe, @idEquipo, @cantidadEntrega, @observacionEntrega", conexion);

                            comando.Parameters.AddWithValue("@idTipoEntrega", newEntrega.idTipoEntrega);
                            comando.Parameters.AddWithValue("@cantidadEntrega", newEntrega.cantidadEntrega);
                            comando.Parameters.AddWithValue("@fechaEntrega", fechaEntrega);
                            comando.Parameters.AddWithValue("@observacionEntrega", newEntrega.observacionEntrega);
                            comando.Parameters.AddWithValue("@idEmpleadoEntrega", newEntrega.idEmpleadoEntrega);
                            comando.Parameters.AddWithValue("@idEmpleadoRecibe", newEntrega.idEmpleadoRecibe);
                            comando.Parameters.AddWithValue("@idEquipo", newEntrega.idEquipo);

                            registrosAlterados = Convert.ToInt32(comando.ExecuteScalar().ToString());
                        }
                    }
                }
                exito = registrosAlterados == 1;
                eliminada = registrosEliminados == 1;
                devolucion = devolucionesAgregadas == 1;
                devolucionEliminada = devolucionesEliminadas == 1;
            }
            catch (Exception ex)
            {
                ViewData["errorSQL"] = $"Ocurrió el siguiente error al momento de agregar una nueva entrega: {ex.Message}.";
                return Page();
            }

            return RedirectToPage("/Entregas/mostrarEntregas");
        }
    }
}
