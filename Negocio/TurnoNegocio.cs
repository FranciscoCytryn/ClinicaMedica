using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TurnoNegocio
    {
        public List<Turno> Listar()
        {
            List<Turno> listaTurnos = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ListarTurnos");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Turno turno = new Turno
                    {
                        TurnoId = (int)datos.Lector["TurnoId"],
                        Paciente = new Paciente
                        {
                            Nombre = datos.Lector["NombrePaciente"].ToString()
                        },
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Medico = new Medico
                        {
                            Usuario = new Usuario
                            {
                                Nombre = datos.Lector["NombreMedico"].ToString()
                            }
                        },
                        Estado = MapEstadoFromDB(datos.Lector["Estado"].ToString()),
                        Observaciones = datos.Lector["Observaciones"].ToString()
                    };

                    if (!datos.Lector.IsDBNull(datos.Lector.GetOrdinal("HoraInicio")))
                    {
                        turno.HoraInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    }
                    else
                    {
                        turno.HoraInicio = TimeSpan.Zero;
                    }

                    listaTurnos.Add(turno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return listaTurnos;
        }

        private EstadoTurno MapEstadoFromDB(string estadoDB)
        {
            switch (estadoDB)
            {
                case "Nuevo":
                    return EstadoTurno.Nuevo;
                case "Reprogramado":
                    return EstadoTurno.Reprogramado;
                case "Cancelado":
                    return EstadoTurno.Cancelado;
                case "No Asistió":
                    return EstadoTurno.NoAsistio;
                case "Cerrado":
                    return EstadoTurno.Cerrado;
                default:
                    throw new Exception("Estado no válido: " + estadoDB);
            }
        }

        public bool ReprogramarTurno(int turnoId, DateTime nuevaFecha, TimeSpan nuevaHora)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearProcedimiento("sp_ReprogramarTurno");
                datos.SetearParametro("@TurnoId", turnoId);
                datos.SetearParametro("@NuevaFecha", nuevaFecha);
                datos.SetearParametro("@NuevaHora", nuevaHora);

                datos.EjecutarAccion();
                return true;  
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Turno ObtenerTurnoPorId(int turnoId)
        {
            Turno turno = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ObtenerTurnoPorId");
                datos.comando.Parameters.AddWithValue("@TurnoId", turnoId);
                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    turno = new Turno
                    {
                        TurnoId = (int)datos.Lector["TurnoId"],
                        Fecha = Convert.ToDateTime(datos.Lector["Fecha"]),
                        HoraInicio = TimeSpan.Parse(datos.Lector["HoraInicio"].ToString()),  // Asegurar que el alias sea correcto
                        Estado = (EstadoTurno)Enum.Parse(typeof(EstadoTurno), datos.Lector["Estado"].ToString()),
                        PacienteId = (int)datos.Lector["PacienteId"],
                        Medico = new Medico
                        {
                            MedicoId = (int)datos.Lector["MedicoId"]
                        },
                        Especialidad = new Especialidad
                        {
                            EspecialidadId = (int)datos.Lector["EspecialidadId"],
                            Nombre = datos.Lector["NombreEspecialidad"].ToString()
                        }
                    };

                    // Crear un objeto para el usuario asociado al médico y asignarle el nombre
                    Usuario usuarioMedico = new Usuario
                    {
                        UsuarioId = turno.Medico.MedicoId,
                        Nombre = datos.Lector["NombreMedico"].ToString()
                    };

                    // Si en alguna parte del código necesitas el nombre del médico, deberías buscarlo en usuarioMedico.Nombre.
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el turno por ID", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return turno;
        }

        public void ActualizarObservacion(int turnoId, string observaciones)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ActualizarObservacionTurno");
                datos.SetearParametro("@TurnoId", turnoId);
                datos.SetearParametro("@Observaciones", observaciones);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar las observaciones del turno", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void ActualizarEstado(int turnoId, string estado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearProcedimiento("sp_ActualizarEstadoTurno");
                datos.SetearParametro("@TurnoId", turnoId);
                datos.SetearParametro("@Estado", estado);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}

