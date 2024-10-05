using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data.SqlClient;

namespace SupportHub.Pages.Usuario
{
    public class mi_informacionModel : PageModel
    {
        public readonly IConfiguration configuracion;
        public List<Usuarios> Usuario = new List<Usuarios>();
        public mi_informacionModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public void OnGet()
        {
            try
            {
                string usuario = HttpContext.Session.GetString("usuario");
                string cadena = configuracion.GetConnectionString("CadenaConexion");
                string consulta = "Select * from Usuarios where loginUsuario = @usuario";

                if (!string.IsNullOrEmpty(usuario))
                {
                    using (SqlConnection conexion = new SqlConnection(cadena))
                    {
                        conexion.Open();

                        SqlCommand comando = new SqlCommand(consulta, conexion);
                        comando.Parameters.AddWithValue("@usuario", usuario);

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                Usuarios newUsuario = new Usuarios();

                                newUsuario.idUsuario = lector.GetInt32(0);
                                newUsuario.loginUsuario = lector.GetString(2);
                                newUsuario.nombreUsuario = lector.GetString(3);
                                newUsuario.apellidoUsuario = lector.GetString(4);

                                Usuario.Add(newUsuario);
                            }
                        }
                    }
                }









            }
            catch (Exception ex)
            {

            }









        }
    }
}
