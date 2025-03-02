using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class MedicoNegocio
    {
        private AccesoDatos datos = new AccesoDatos();

        public List<Medico> Listar()
        {
            List<Medico> medicos = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ListarMedicos");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Medico medico = new Medico
                    {
                        MedicoId = (int)datos.Lector["MedicoId"],
                        Usuario = new Usuario
                        {
                            UsuarioId = (int)datos.Lector["UsuarioId"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Email = datos.Lector["Email"].ToString(),
                            Telefono = datos.Lector["Telefono"].ToString(),
                            Activo = (bool)datos.Lector["Activo"]
                        },
                        Especialidades = ObtenerEspecialidadesPorMedico((int)datos.Lector["MedicoId"]) 
                    };

                    medicos.Add(medico);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar médicos", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return medicos;
        }

        public void AltaMedico(Medico medico)
        {
            try
            {
                datos.SetearProcedimiento("sp_AltaMedico");
                datos.SetearParametro("@Nombre", medico.Usuario.Nombre);
                datos.SetearParametro("@Email", medico.Usuario.Email);
                datos.SetearParametro("@Telefono", medico.Usuario.Telefono);
                datos.SetearParametro("@Password", medico.Usuario.Password);
                datos.SetearParametro("@Rol", "Médico");
                datos.SetearParametro("@Especialidades", ObtenerIdsEspecialidades(medico.Especialidades));

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de alta el médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EditarMedico(Medico medico)
        {
            try
            {
                datos.SetearProcedimiento("sp_EditarMedico");
                datos.SetearParametro("@MedicoId", medico.MedicoId);
                datos.SetearParametro("@Nombre", medico.Usuario.Nombre);
                datos.SetearParametro("@Email", medico.Usuario.Email);
                datos.SetearParametro("@Telefono", medico.Usuario.Telefono);
                datos.SetearParametro("@Especialidades", ObtenerIdsEspecialidades(medico.Especialidades));

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EditarMedico(int medicoId, string nombre, string email, string telefono)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_EditarMedico");

                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@Nombre", nombre);
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Telefono", telefono);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ActualizarEspecialidadesMedico(int medicoId, List<Dominio.Especialidad> especialidades)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_EliminarEspecialidadesMedico");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.EjecutarAccion();

                foreach (var especialidad in especialidades)
                {
                    datos.SetearProcedimiento("sp_InsertarEspecialidadMedico");
                    datos.comando.Parameters.Clear();
                    datos.SetearParametro("@MedicoId", medicoId);
                    datos.SetearParametro("@EspecialidadId", especialidad.EspecialidadId);
                    datos.EjecutarAccion();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar las especialidades del médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void BajaMedico(int medicoId)
        {
            try
            {
                datos.SetearProcedimiento("sp_BajaMedico");
                datos.SetearParametro("@MedicoId", medicoId);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de baja el médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Especialidad> ObtenerEspecialidadesPorMedico(int medicoId)
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ObtenerEspecialidadesPorMedico");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Especialidad especialidad = new Especialidad
                    {
                        EspecialidadId = (int)datos.Lector["EspecialidadId"],
                        Nombre = datos.Lector["Nombre"].ToString()
                    };

                    especialidades.Add(especialidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener especialidades", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return especialidades;
        }

        private string ObtenerIdsEspecialidades(List<Especialidad> especialidades)
        {
            return string.Join(",", especialidades.Select(e => e.EspecialidadId));
        }

        public Medico ObtenerPorId(int medicoId)
        {
            Medico medico = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ObtenerMedicoPorId");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    medico = new Medico
                    {
                        MedicoId = (int)datos.Lector["MedicoId"],
                        Usuario = new Usuario
                        {
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Email = datos.Lector["Email"].ToString(),
                            Telefono = datos.Lector["Telefono"].ToString(),
                            Activo = (bool)datos.Lector["Activo"]
                        },
                        Especialidades = ObtenerEspecialidadesPorMedico(medicoId)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el médico por ID.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return medico;
        }
    }
}

