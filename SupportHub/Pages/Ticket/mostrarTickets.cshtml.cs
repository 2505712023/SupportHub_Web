using Microsoft.AspNetCore.Mvc;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace SupportHub.Pages.Ticket
{
    [Authorize]
    public class mostrarTicketsModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Tickets> listaTickets = new List<Tickets>();
        public Tickets newTickets = new Tickets();
        public String mensajeError = "";

        public mostrarTicketsModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }



        public void OnGet()
        {




        }
    }
}
