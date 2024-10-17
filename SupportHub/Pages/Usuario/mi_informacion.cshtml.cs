using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Helpers;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace SupportHub.Pages.Usuario
{
    [Authorize]
    public class mi_informacionModel : PageModel
    {
        [TempData]
        public bool exito { get; set; } = false;
        public readonly IConfiguration configuracion;
        public List<Usuarios> Usuario = new List<Usuarios>();
        public String mensajeError = "";
        public mi_informacionModel(IConfiguration configuracion)
        {
            this.configuracion = configuracion;
        }
        public void OnGet()
        {
            try
            {
                string usuario = HttpContext.Session.GetString("usuario");
                string cadena = GetAvailableConnectionString();
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
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public IActionResult OnPost()
        {
            Usuarios setUsuario = new Usuarios();
            string contraseñaActual = Request.Form["contraA"];
            string nuevaContraseña = Request.Form["nContra"];
            string ConfirmarNuevaContra = Request.Form["CnContra"];
            setUsuario.idUsuario = int.Parse(Request.Form["id"]);
            setUsuario.nombreUsuario = Request.Form["nombre"];
            setUsuario.apellidoUsuario = Request.Form["apellido"];
            setUsuario.loginUsuario = Request.Form["usuario"];
            
                try
                {
                    if (nuevaContraseña == ConfirmarNuevaContra)
                    {
                        setUsuario.ClaveUsuario = ConfirmarNuevaContra;
                    }

                    string cadena = configuracion.GetConnectionString("CadenaConexion");
                    string consulta1 = "UPDATE Usuarios SET nombreUsuario = @nombre, apellidoUsuario = @apellido" +
                        " where idUsuario = @idUsuario; ";

                    using (SqlConnection conexion = new SqlConnection(cadena))
                    {
                        conexion.Open();
                        
                        if (setUsuario.ClaveUsuario == "")
                        {
                            //si la clave de usuario está vacía significa que solo ha modificado nombre o apellido
                            using (SqlCommand comando = new SqlCommand(consulta1, conexion))
                            {
                                comando.Parameters.AddWithValue("@nombre", setUsuario.nombreUsuario);
                                comando.Parameters.AddWithValue("@apellido", setUsuario.apellidoUsuario);
                                comando.Parameters.AddWithValue("@idUsuario", setUsuario.idUsuario);

                                comando.ExecuteNonQuery();
                            }
                            exito =  true;                           
                        }
                        else
                        {
                            using (SqlCommand comando = new SqlCommand("sp_modificar_contraseña", conexion))
                            {
                                comando.CommandType = CommandType.StoredProcedure;
                                comando.Parameters.AddWithValue("@idUsuario", setUsuario.idUsuario);
                                comando.Parameters.AddWithValue("@claveUsuario", setUsuario.ClaveUsuario);

                                comando.ExecuteNonQuery();
                            }
                            exito =  true;                                                      
                        }
                        if (exito)
                        {
                            return RedirectToPage("/Usuario/mi_informacion");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error " + ex);
                    return Page();
                }
            
            return RedirectToPage("/Usuario/mi_informacion");
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
