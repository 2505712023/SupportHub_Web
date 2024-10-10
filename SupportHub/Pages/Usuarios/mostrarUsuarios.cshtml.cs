using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data.SqlClient;
using System.Data;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace SupportHub.Pages.Usuarios
{
    public class mostrarUsuariosModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Usuario> listaUsuarios = new List<Usuario>();
        public Usuario newUsuario= new Usuario();
        public String mensajeError = "";
        public String mensajeExito = "";

        public mostrarUsuariosModel(IConfiguration configuration)
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

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_usuario", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@idUsuario ", searchQuery);
                        
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_usuarios", conexion);
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Usuario Usuario = new Usuario();
                            Usuario.LoginUsuario = lector.GetString(0);
                            Usuario.NombreUsuario = lector.GetString(1);
                            Usuario.ApellidoUsuario = lector.GetString(2);
                            Usuario.CodEmpleado = lector.GetString(3);
                            Usuario.IDEmpleado= lector.GetInt32(4);
                            listaUsuarios.Add(Usuario);
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
