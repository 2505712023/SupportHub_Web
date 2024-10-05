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
         
            if (Username == "a" || Username =="aalvarez" && Password == "a") 
            {

                HttpContext.Session.SetString("usuario",Username);

                Response.Redirect("/Index");
            }

            // Si la autenticación falla
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
