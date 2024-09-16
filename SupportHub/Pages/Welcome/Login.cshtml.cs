using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SupportHub.Pages.Welcome
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Aqu� deber�as implementar la l�gica de autenticaci�n
            // Por ejemplo, validar las credenciales

            // Si la autenticaci�n es exitosa
            if (Username == "a" && Password == "a") // Ejemplo simple, reemplaza con l�gica real
            {
                Response.Redirect("/Index");
            }

            // Si la autenticaci�n falla
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
