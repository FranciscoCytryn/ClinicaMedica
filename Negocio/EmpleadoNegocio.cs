using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class EmpleadoNegocio
    {
        public List<Usuario> ListarEmpleados()
        {
            List<Usuario> empleados = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ListarEmpleados");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Usuario empleado = new Usuario
                    {
                        UsuarioId = (int)datos.Lector["UsuarioId"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    empleados.Add(empleado);
                }

                return empleados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void AgregarEmpleado(Usuario empleado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_AgregarEmpleado");
                datos.SetearParametro("@Nombre", empleado.Nombre);
                datos.SetearParametro("@Email", empleado.Email);
                datos.SetearParametro("@Telefono", empleado.Telefono);
                datos.SetearParametro("@Contraseña", empleado.Password);
                datos.SetearParametro("@Activo", true); 
                datos.SetearParametro("@Rol", "Recepcionista");
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar empleado", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void ModificarEmpleado(Usuario empleado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ModificarEmpleado");
                datos.SetearParametro("@Id", empleado.UsuarioId);
                datos.SetearParametro("@Nombre", empleado.Nombre);
                datos.SetearParametro("@Email", empleado.Email);
                datos.SetearParametro("@Telefono", empleado.Telefono);
                datos.SetearParametro("@Activo", empleado.Activo);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar empleado", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarEmpleado(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_EliminarEmpleado");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar empleado", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}


