using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using SupportHub.Pages.Proveedor;
using System.Data.SqlClient;
namespace SupportHub.Pages.Proveedor
{
    public class AgregarProveedorModel : PageModel
    {
        //Definir variables para acceder a parámetros de configuración
        private readonly IConfiguration configuracion;
        
        //Crear un objeto de tipo "Proveedor"
        public Proveedores newProveedor = new Proveedores();

        //Crear variable para manejo de errores 
        public String mensajeError = "";
        public String mensajeExito = "";

        //Constructor
        public AgregarProveedorModel(IConfiguration configuration) { 
            this.configuracion = configuration; 
        }

        public void OnGet()
        {
        }

        //Agregar método "OnPost"
        public void OnPost() {
            newProveedor.codProveedor = Request.Form["Codigo"];
            newProveedor.nombreProveedor = Request.Form["nombre"];
            newProveedor.direccionProveedor = Request.Form["direccion"];
            newProveedor.telefonoProveedor = Request.Form["telefono"];
            if (newProveedor.codProveedor == "" || newProveedor.nombreProveedor == ""|| newProveedor.direccionProveedor == "" || newProveedor.telefonoProveedor == "") {
                mensajeError = "Todos los campos son requeridos";
                return;
            }
            try
            {
                //Definamos una variable y le asignamos la cadena de conexion definida en el archvio appsetting.json
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                /*creamos un objeto de la case SQLConnetion indicando como partametros la cadena de comnexion creada anteriormente*/
                SqlConnection conexion = new SqlConnection(cadena); 

                //Abrir conexion 
                conexion.Open();

                string query = "dbo.sp_crear_proveedor @codProveedor,@nombreProveedor,@direccionProveedor,@telefonoProveedor";

                //Creamos un objeto de la clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);

                //Pasar datos ingresados a los parametros
                comando.Parameters.AddWithValue("@codProveedor", newProveedor.codProveedor);
                comando.Parameters.AddWithValue("@nombreProveedor", newProveedor.nombreProveedor);
                comando.Parameters.AddWithValue("@direccionProveedor", newProveedor.direccionProveedor);
                comando.Parameters.AddWithValue("@telefonoProveedor", newProveedor.telefonoProveedor);

                //Le indicamos a SQL server que ejecute el comando espesificado anteriormente
                comando.ExecuteNonQuery();

                //Cerramos conexión
                conexion.Close();
            }
            catch (Exception ex )
            {
                mensajeError = ex.Message;
                return;
            }

            //Limpiar controles
            newProveedor.codProveedor = "";
            newProveedor.nombreProveedor = "";
            newProveedor.direccionProveedor = "";
            newProveedor.telefonoProveedor = "";
            mensajeExito = "Proveedor agregado correctamente.";

            //Redirigir a la pagina INDEX
            Response.Redirect("/Proveedor/mostrarProveedor");
        }
    }
}
