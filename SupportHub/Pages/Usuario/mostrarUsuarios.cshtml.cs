using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data.SqlClient;
using System.Data;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace SupportHub.Pages.Usuario
{
    [Authorize(Roles = "Administrador,Común")]
    public class mostrarUsuariosModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Modelos.Usuario> listaUsuarios { get; set; } = new List<Modelos.Usuario>();
        public List<Modelos.Usuario> ListaCodigo = new List<Modelos.Usuario>();
        public List<Modelos.Usuario> Roles { get; set; } = new List<Modelos.Usuario>();
        public Modelos.Usuario newUsuario = new Modelos.Usuario();
        public string mensajeError = "";
        public string mensajeExito = "";

        public mostrarUsuariosModel(IConfiguration configuration)
        {
            configuracion = configuration;
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

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_usuario", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@LoginUsuario ", searchQuery);
                        comando.Parameters.AddWithValue("@codEmpleado ", searchQuery);

                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_usuarios", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Modelos.Usuario Usuario = new Modelos.Usuario();
                            Usuario.LoginUsuario = lector.GetString(0);
                            Usuario.NombreUsuario = lector.GetString(1);
                            Usuario.ApellidoUsuario = lector.GetString(2);
                            Usuario.CodEmpleado = lector.GetString(3);
                            Usuario.IDEmpleado = lector.GetInt32(4);
                            Usuario.ActivoUsuario = lector.GetBoolean(5);
                            listaUsuarios.Add(Usuario);
                        }
                    }
                    ;

                    string queryrol = "SELECT nombreRol FROM Roles";

                    using (SqlCommand command = new SqlCommand(queryrol, conexion))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rol = new Modelos.Usuario
                                {
                                    RolUsuario = reader.GetString(0)
                                };

                                Roles.Add(rol);
                            }
                        }
                    }

                    JsonResult OnGetPersonData(string codEmpleado)
                    {
                        string querycodigo = "SELECT nombreEmpleado, apellidoEmpleado FROM Empleado WHERE codEmpleado = @codEmpleado";
                        using (SqlCommand command = new SqlCommand(querycodigo, conexion))
                        {
                            command.Parameters.AddWithValue("@codEmpleado", codEmpleado);

                            conexion.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            string nombre = "";
                            string apellido = "";

                            if (reader.Read())
                            {
                                nombre = reader["nombreEmpleado"].ToString();
                                apellido = reader["apellidoEmpleado"].ToString();
                            }

                            conexion.Close();
                            return new JsonResult(new { nombre, apellido });
                        }




                    }



                    string querycodigo = "SELECT codempleado FROM Empleados";

                    using (SqlCommand command = new SqlCommand(querycodigo, conexion))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var codigoempleados = new Modelos.Usuario
                                {
                                    CodEmpleado = reader.GetString(0)
                                };

                                ListaCodigo.Add(codigoempleados);
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
            newUsuario.LoginUsuario = Request.Form["usuario"];
            newUsuario.NombreUsuario = Request.Form["nombre"];
            newUsuario.ApellidoUsuario = Request.Form["apellido"];
            newUsuario.CodEmpleado = Request.Form["codEmpleado"];
            newUsuario.ActivoUsuario = Convert.ToBoolean(Request.Form["activo"]);
            //si se quiere agregar un nuevo usuario
            if (Request.Form["esModificacion"] == "false")
            {

                //aquí va el código para agregar usuarios 


            }//si se quiere modificar un usuario
            else
            {
                newUsuario.IDEmpleado = Convert.ToInt32(Request.Form["id"]);

                try
                {
                    #region validar coincidencias
                    //voy a obtener algunos campos de los usuarios para hacer algunas comparaciones

                    List<Modelos.Usuario> nombreUsuarios = new List<Modelos.Usuario>();

                    string cadena = GetAvailableConnectionString();
                    using (SqlConnection conexion = new SqlConnection(cadena))
                    {
                        conexion.Open();
                        SqlCommand comando = new SqlCommand("sp_obtener_usuarios", conexion);
                        comando.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                Modelos.Usuario usuario = new Modelos.Usuario();
                                usuario.LoginUsuario = lector.GetString(2);
                                usuario.IDEmpleado = lector.GetInt32(4);
                                nombreUsuarios.Add(usuario);
                            }
                        }
                    }
                    //recorriendo la lista para ver si el nuevo nombre que estamos asignando al proveedor ya está 
                    //asignado a alguien más 
                    foreach (var i in nombreUsuarios)
                    {
                        //si tienen el mismo código y nombre de usuario, significa que quiere cambiar un campo distinto al nombre de usuario
                        if (i.IDEmpleado == newUsuario.IDEmpleado && i.LoginUsuario == newUsuario.LoginUsuario)
                        {
                            break;
                        }//si tienen distinto código y mismo nombre de usuario significa que quiere asignar un nombre de usuario que ya está ocupado
                        else if (i.IDEmpleado != newUsuario.IDEmpleado && i.LoginUsuario == newUsuario.LoginUsuario)
                        {
                            coincidencia += 1;
                            break;
                        }
                    }

                    #endregion

                    if (coincidencia == 0)
                    {
                        using (SqlConnection conexion = new SqlConnection(cadena))
                        {
                            conexion.Open();
                            SqlCommand comando = new SqlCommand("sp_modificar_usuario", conexion);

                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@loginUsuario", newUsuario.LoginUsuario);
                            comando.Parameters.AddWithValue("@nombresUsuario", newUsuario.NombreUsuario);
                            comando.Parameters.AddWithValue("@apellidosUsuario", newUsuario.ApellidoUsuario);
                            comando.Parameters.AddWithValue("@activoUsuario", newUsuario.ActivoUsuario);
                            comando.Parameters.AddWithValue("@idEmpleado", newUsuario.IDEmpleado);

                            comando.ExecuteNonQuery();
                        }
                        exito = true;
                    }
                    else
                    {
                        exito = false;
                    }
                }
                catch (Exception ex)
                {
                    mensajeError = ex.Message;
                    return Page();
                }
            }


            return RedirectToPage("/Usuarios/mostrarUsuarios");
        }








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
