using Microsoft.AspNetCore.Mvc;
using SupportHub.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SupportHub.Modelos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SupportHub.Pages.Empleado
{
    public class mostrarEmpleadosModel : PageModel
    {
        private readonly IConfiguration configuracion;
        public List<Empleados> listaEmpleado = new List<Empleados>();
        public Empleados newEmpleado = new Empleados();
        public List<SelectListItem> Area = new List<SelectListItem>();
        public List<SelectListItem> Cargo = new List<SelectListItem>();
        public mostrarEmpleadosModel(IConfiguration configuration) {
            this.configuracion = configuration;
        }

        public void OnGet(string searchQuery = null, bool exito = false, bool intentoRealizado = false, bool esEliminacion = false, bool eliminado = false)
        {
            string cadena = GetAvailableConnectionString();

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena)) {
                    conexion.Open();
                    SqlCommand comando;
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        comando = new SqlCommand("sp_obtener_empleado", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@codEmpleado", searchQuery);
                        comando.Parameters.AddWithValue("@nombreEmpleado", searchQuery);
                        comando.Parameters.AddWithValue("@apellidoEmpleado", searchQuery);
                    }
                    else
                    {
                        comando = new SqlCommand("sp_obtener_empleados_general", conexion);
                        comando.CommandType = CommandType.StoredProcedure;
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Empleados empleado = new Empleados();

                            empleado.codEmpleado = lector.GetString(0);
                            empleado.nombreEmpleado = lector.GetString(1);
                            empleado.apellidoEmpleado = lector.GetString(2);
                            empleado.telefonoEmpleado = lector.GetString(3);
                            empleado.emailEmpleado = lector.GetString(4);
                            empleado.codArea = lector.GetString(5);
                            empleado.nombreArea = lector.GetString(6);
                            empleado.codCargo = lector.GetString(7);
                            empleado.nombreCargo = lector.GetString(8);

                            listaEmpleado.Add(empleado);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
            }
            // Lista de Area
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand("select idArea,  codArea  + ' - ' + nombreArea as [Areas] from Areas", conexion);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            SelectListItem area = new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            };
                            Area.Add(area);
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al obtener clientes: " + ex.Message);
            }
            //Lista de  Cargo 
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
                {
                    conexion.Open();
                    SqlCommand comando = new SqlCommand("select idCargo,  codCargo  + ' - ' + nombreCargo as [Cargo] from Cargos", conexion);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            SelectListItem cargo = new SelectListItem
                            {
                                Value = lector.GetInt32(0).ToString(),
                                Text = lector.GetString(1)
                            };
                            Cargo.Add(cargo);
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al obtener clientes: " + ex.Message);
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

      //  public IActionResult OnPost(bool esEliminacion = false)
     //   {
       

           

            //if (Request.Form["esModificacion"] == "false")
            //{
            //    try
            //    {
            //        string cadena = configuracion.GetConnectionString("CadenaConexion");
            //        int registrosAgregados = 0;
            //        using (SqlConnection conexion = new SqlConnection(cadena))
            //        {
            //            conexion.Open();
            //            string query = "sp_crear_empleado @codProveedor,@nombreProveedor,@direccionProveedor,@telefonoProveedor";
            //            SqlCommand comando = new SqlCommand(query, conexion);

            //            comando.Parameters.AddWithValue("@codProveedor", newEmpleado.codProveedor);
            //            comando.Parameters.AddWithValue("@nombreProveedor", newEmpleado.nombreProveedor);
            //            comando.Parameters.AddWithValue("@direccionProveedor", newEmpleado.direccionProveedor);
            //            comando.Parameters.AddWithValue("@telefonoProveedor", newEmpleado.telefonoProveedor);

            //            registrosAgregados = Convert.ToInt32(comando.ExecuteScalar().ToString());
            //        }
            //        exito = (registrosAgregados == 1) ? true : false; intentoRealizado = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        mensajeError = ex.Message;
            //        return Page();
            //    }
            //}
            //else
            //{
            //    newProveedor.idProveedor = Convert.ToInt32(Request.Form["id"]);
            //    try
            //    {
            //        #region validar coincidencias
            //        //voy a obtener algunos campos de los proveedores para hacer algunas comparaciones

            //        List<Proveedores> nombreYCodigoProveedores = new List<Proveedores>();

            //        string cadena = configuracion.GetConnectionString("CadenaConexion");
            //        using (SqlConnection conexion = new SqlConnection(cadena))
            //        {
            //            conexion.Open();
            //            SqlCommand comando = new SqlCommand("sp_obtener_proveedores_general", conexion);
            //            comando.CommandType = System.Data.CommandType.StoredProcedure;

            //            using (SqlDataReader lector = comando.ExecuteReader())
            //            {
            //                while (lector.Read())
            //                {
            //                    Proveedores proveedor = new Proveedores();
            //                    proveedor.codProveedor = lector.GetString(1);
            //                    proveedor.nombreProveedor = lector.GetString(2);

            //                    nombreYCodigoProveedores.Add(proveedor);
            //                }
            //            }
            //        }
            //        //recorriendo la lista para ver si el nuevo nombre que estamos asignando al proveedor ya está 
            //        //asignado a alguien más 
            //        foreach (var i in nombreYCodigoProveedores)
            //        {
            //            //si tienen el mismo código y nombre, significa que quiere cambiar un campo distinto al nombre
            //            if (i.codProveedor == newProveedor.codProveedor && i.nombreProveedor == newProveedor.nombreProveedor)
            //            {
            //                break;
            //            }//si tienen distinto código y mismo nombre significa que quiere asignar un nombre que ya está ocupado
            //            else if (i.codProveedor != newProveedor.codProveedor && i.nombreProveedor == newProveedor.nombreProveedor)
            //            {
            //                coincidencia += 1;
            //                break;
            //            }
            //        }

            //        #endregion

            //        if (coincidencia == 0)
            //        {
            //            //string cadena = configuracion.GetConnectionString("CadenaConexion");
            //            using (SqlConnection conexion = new SqlConnection(cadena))
            //            {
            //                conexion.Open();
            //                string query = "dbo.sp_modificar_proveedor @codProveedor,@idProveedor,@nombreProveedor,@direccionProveedor,@telefonoProveedor";
            //                SqlCommand comando = new SqlCommand(query, conexion);

            //                comando.Parameters.AddWithValue("@idProveedor", newProveedor.idProveedor);
            //                comando.Parameters.AddWithValue("@codProveedor", newProveedor.codProveedor);
            //                comando.Parameters.AddWithValue("@nombreProveedor", newProveedor.nombreProveedor);
            //                comando.Parameters.AddWithValue("@direccionProveedor", newProveedor.direccionProveedor);
            //                comando.Parameters.AddWithValue("@telefonoProveedor", newProveedor.telefonoProveedor);

            //                comando.ExecuteNonQuery();
            //            }
            //            exito = true;
            //        }
            //        else
            //        {
            //            exito = false;
            //            intentoRealizado = true;
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        mensajeError = ex.Message;
            //        return Page();
            //    }
            //}

            ////ya no es necesario validar si exito es true o false porque de igual manera vamos a redirigirnos a la misma
            //// página sin enviar objetos adicionales como new exito = true; porque tempData se encarda de enviar esos datos 
            //return RedirectToPage("/Proveedor/mostrarProveedor");

     //   }
    

private string GetAvailableConnectionString()
        {

            if (PingHelper.PingHost("100.101.36.39"))
            {
                return configuracion.GetConnectionString("CadenaConexion");
            }
            else if (PingHelper.PingHost("25.2.143.28"))
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
