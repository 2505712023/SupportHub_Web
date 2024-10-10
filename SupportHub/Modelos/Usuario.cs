using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SupportHub.Modelos
{
    public class Usuario
    {
        public string LoginUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string CodEmpleado {  get; set; }
        public int IDEmpleado { get; set; }
        public bool ActivoUsuario { get; set; }
    }
}
