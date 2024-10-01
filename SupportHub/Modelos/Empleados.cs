namespace SupportHub.Modelos
{
    public class Empleados
    {
        public int idEmpleado { get; set; }
        public string codEmpleado { get; set; }
        public string nombreEmpleado { get; set; }
        public string apellidoEmpleado { get; set; }
        public string telefonoEmpleado { get; set; }
        public string emailEmpleado { get; set; }

        public string codArea { get; set; }
        public string nombreArea { get; set; }  

        public string codCargo { get; set; }
        public string nombreCargo { get; set; } 
    }
}

