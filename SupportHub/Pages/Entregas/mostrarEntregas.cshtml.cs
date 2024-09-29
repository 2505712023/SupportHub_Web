using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data.SqlClient;

namespace SupportHub.Pages.Entregas
{
    public class mostrarEntregasModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Entrega> listaEntregas = new();

        public mostrarEntregasModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }

        public void OnGet()
        {
            try
            {
                string cadena = configuracion.GetConnectionString("CadenaConexion");
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
    }
}
