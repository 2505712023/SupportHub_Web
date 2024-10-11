using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace SupportHub.Pages.Welcome
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuracion;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            configuracion = configuration;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            using (SqlConnection conn = new SqlConnection(GetAvailableConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_autenticar_usuario", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LoginName", Username);
                cmd.Parameters.AddWithValue("@Password", Password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())  // Si el procedimiento devuelve una fila
                    {
                        // Obtener el nombre y apellido del usuario
                        string nombreCompleto = reader["nombreUsuario"].ToString() + " " + reader["apellidoUsuario"].ToString();

                        // Crear los claims del usuario autenticado
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, nombreCompleto)  // Usar el nombre completo
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Autenticar al usuario creando la cookie
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        // Redirigir al usuario autenticado
                        return Redirect("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ErrorMessage = "Usuario o contraseña incorrecta"; // Mensaje de error
                        return Page();
                    }
                }
            }
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
