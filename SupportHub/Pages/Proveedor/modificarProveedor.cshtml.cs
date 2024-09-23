using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using SupportHub.Pages.Proveedor;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
namespace SupportHub.Pages.Proveedor
{
    public class modificarProveedorModel : PageModel
    {
        //Definir variables para acceder a parámetros de configuración
        private readonly IConfiguration configuracion;

        //Creart un objeto de tipo "Proveedor"
        public Proveedores newProveedor = new Proveedores();

        //Crear variables para manejo de errores
        public String mensajeError = "";
        public String mensajeExito = "";

        //Constructor
        public modificarProveedorModel(IConfiguration configuration)
        {
            this.configuracion = configuration;
        }

        public void OnGet(string codProveedor)
        {
            try
            {
                //Definamos una variable y le asignamos la cadena de conexion definida en el archvio appsettings.json
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                /*creamos un objeto de la case SQLConnetion indicando como partametros la cadena de comnexion creada anteriormente*/
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrir conexion 
                conexion.Open();

                string query = "SupportHub.dbo.sp_obtener_proveedor";

                //Creamos un objeto de la clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@codProveedor", codProveedor);

                //Creamos un SqlDataReader
                SqlDataReader lector = comando.ExecuteReader();

                //Leemos el resultado y asignamos los valores a los controles del form
                while (lector.Read())
                {
                    newProveedor.codProveedor = lector.GetString(1);
                    newProveedor.nombreProveedor = lector.GetString(2);
                    newProveedor.direccionProveedor = lector.GetString(3);
                    newProveedor.telefonoProveedor = lector.GetString(4);
                }

                //cerramos conexion
                conexion.Close();
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return;
            }
        }

        //Agregar método "OnPost"
        public void OnPost()
        {
            newProveedor.codProveedor = Request.Form["codigo"];
            newProveedor.nombreProveedor = Request.Form["nombre"];
            newProveedor.direccionProveedor = Request.Form["direccion"];
            newProveedor.telefonoProveedor = Request.Form["telefono"];
            if (newProveedor.codProveedor == "" || newProveedor.nombreProveedor == "" || newProveedor.direccionProveedor == "" || newProveedor.telefonoProveedor == "")
            {
                mensajeError = "Todos los campos son requeridos.";
                return;
            }
            try
            {
                //Definamos una variable y le asignamos la cadena de conexion definida en el archvio appsettings.json
                string cadena = configuracion.GetConnectionString("CadenaConexion");

                /*creamos un objeto de la case SQLConnetion indicando como partametros la cadena de comnexion creada anteriormente*/
                SqlConnection conexion = new SqlConnection(cadena);

                //Abrir conexion 
                conexion.Open();

                string query = "SupportHub.dbo.sp_modificar_proveedor";

                //Creamos un objeto de la clase SqlCommand
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;

                //Pasar datos ingresados a los parametros
                comando.Parameters.AddWithValue("@codProveedor", newProveedor.codProveedor);
                comando.Parameters.AddWithValue("@nombreProveedor", newProveedor.nombreProveedor);
                comando.Parameters.AddWithValue("@direccionProveedor", newProveedor.direccionProveedor);
                comando.Parameters.AddWithValue("@telefonoProveedor", newProveedor.telefonoProveedor);

                // Le indicamos a SQL server que ejecute el comando espesificado anteriormente
                comando.ExecuteNonQuery();

                //cerramos conexion
                conexion.Close();
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
                return;
            }

            //Limpiar controles
            newProveedor.codProveedor = "";
            newProveedor.nombreProveedor = "";
            newProveedor.direccionProveedor = "";
            newProveedor.telefonoProveedor = "";
            mensajeExito = "Proveedor modificado correctamente.";

            //Redirigir a la pagina INDEX
            Response.Redirect("/Proveedor/mostrarProveedor");
        }
    }
}
