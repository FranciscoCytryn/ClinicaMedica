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

        public bool ExisteEmail(string email, int? pacienteId = null)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ExisteEmailPaciente");
                datos.SetearParametro("@Email", email);
                if (pacienteId.HasValue)
                {
                    datos.SetearParametro("@PacienteId", pacienteId.Value);
                }

                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    return Convert.ToInt32(datos.Lector[0]) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el email", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return false;
        }

        public void AltaPaciente(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("AltaPaciente");
                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Email", paciente.Email);
                datos.SetearParametro("@Telefono", paciente.Telefono);
                datos.SetearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Direccion", paciente.Direccion);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de alta al paciente", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
