using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PacienteNegocio
    {
        public List<Paciente> Listar()
        {
            List<Paciente> pacientes = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ListarPacientes");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    pacientes.Add(new Paciente
                    {
                        PacienteId = (int)datos.Lector["PacienteId"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"],
                        Direccion = datos.Lector["Direccion"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pacientes", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return pacientes;
        }

        public void Actualizar(int pacienteId, string nombre, string email, string telefono, DateTime fechaNacimiento, string direccion)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ActualizarPaciente");
                datos.SetearParametro("@PacienteId", pacienteId);
                datos.SetearParametro("@Nombre", nombre);
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Telefono", telefono);
                datos.SetearParametro("@FechaNacimiento", fechaNacimiento);
                datos.SetearParametro("@Direccion", direccion);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar paciente", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Eliminar(int pacienteId)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_EliminarPaciente");
                datos.SetearParametro("@PacienteId", pacienteId);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar paciente", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
