using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

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
                Console.WriteLine("Error: " + ex.Message);
            }

        }
        
        public void OnPost()
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
                if(nuevaContraseña == ConfirmarNuevaContra)
                {
                    setUsuario.ClaveUsuario = ConfirmarNuevaContra;
                }

                string cadena = configuracion.GetConnectionString("CadenaConexion");
                string consulta1 = "UPDATE Usuarios SET nombreUsuario = @nombre, apellidoUsuario = @apellido, where idUsuario = @idUsuario; ";
                string consulta2 = "update Usuarios " +
                    "set claveUsuario = ENCRYPTBYPASSPHRASE('rhpn1aHA1q8CkyEMELw6eynB4OOVOGVg', @clave)" +
                    "nombreUsuario = @nombre, apellidoUsuario = @apellido, where idUsuario = @idUsuario; ";
                       

                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();

                    if (setUsuario.ClaveUsuario == null)
                    {
                        using (SqlCommand comando = new SqlCommand(consulta1,conexion))
                        {
                            comando.Parameters.AddWithValue("@nombre",setUsuario.nombreUsuario);
                            comando.Parameters.AddWithValue("@apellido",setUsuario.apellidoUsuario);
                            comando.Parameters.AddWithValue("@idUsuario",setUsuario.idUsuario);
                           
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand comando = new SqlCommand(consulta2, conexion))
                        {
                            comando.Parameters.AddWithValue("@nombre", setUsuario.nombreUsuario);
                            comando.Parameters.AddWithValue("@apellido", setUsuario.apellidoUsuario);
                            comando.Parameters.AddWithValue("@idUsuario", setUsuario.idUsuario);
                            comando.Parameters.AddWithValue("@clave",setUsuario.ClaveUsuario);

                            comando.ExecuteNonQuery();
                        }
                    }




                }


            }
            catch(Exception ex)
            {
                Console.WriteLine("error "+ ex);
            }
           
        }
    }
}
