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
                            },                           
                        },
                        Especialidad = new Especialidad  // Nueva propiedad para la especialidad
                        {
                            Nombre = datos.Lector["NombreEspecialidad"].ToString()
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

        public bool VerificarDisponibilidadMedico(int medicoId, DateTime fecha, TimeSpan hora)
        {
            bool disponible = false;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_VerificarDisponibilidadMedico");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@Fecha", fecha.Date);
                datos.SetearParametro("@Hora", hora);

                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    disponible = (bool)datos.Lector["Disponible"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar disponibilidad del médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return disponible;
        }

        public List<Turno> ListarTurnosPorMedicoYFecha(int medicoId, DateTime fecha)
        {
            List<Turno> turnosAsignados = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Configurar el procedimiento almacenado y los parámetros
                datos.SetearProcedimiento("sp_ListarTurnosPorMedicoYFecha");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@Fecha", fecha.Date); // Usar solo la parte de la fecha (sin hora)

                // Ejecutar la consulta
                datos.EjecutarConsulta();

                // Recorrer los resultados y crear objetos Turno
                while (datos.Lector.Read())
                {
                    Turno turno = new Turno
                    {
                        TurnoId = (int)datos.Lector["TurnoId"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        HoraInicio = (TimeSpan)datos.Lector["HoraInicio"],
                        Estado = MapEstadoFromDB(datos.Lector["Estado"].ToString()),
                        Observaciones = datos.Lector["Observaciones"].ToString(),
                        Paciente = new Paciente
                        {
                            PacienteId = (int)datos.Lector["PacienteId"],
                            Nombre = datos.Lector["NombrePaciente"].ToString()
                        },
                        Medico = new Medico
                        {
                            MedicoId = (int)datos.Lector["MedicoId"],
                            Usuario = new Usuario
                            {
                                Nombre = datos.Lector["NombreMedico"].ToString()
                            }
                        }
                    };

                    turnosAsignados.Add(turno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar turnos por médico y fecha", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return turnosAsignados;
        }
        public List<TimeSpan> ObtenerHorariosDisponibles(int medicoId, DateTime fecha)
        {
            List<TimeSpan> horariosDisponibles = new List<TimeSpan>();
            MedicoNegocio medicoNegocio = new MedicoNegocio();

            try
            {
                // Obtener el horario de trabajo del médico
                List<TurnoTrabajo> horarioTrabajo = medicoNegocio.ObtenerTurnosTrabajoPorMedico(medicoId);

                // Obtener los turnos ya asignados al médico en la fecha seleccionada
                List<Turno> turnosAsignados = ListarTurnosPorMedicoYFecha(medicoId, fecha);

                // Generar horarios disponibles
                foreach (var turnoTrabajo in horarioTrabajo)
                {
                    TimeSpan horaActual = turnoTrabajo.HoraEntrada;

                    while (horaActual < turnoTrabajo.HoraSalida)
                    {
                        // Verificar si el horario está ocupado
                        bool ocupado = turnosAsignados.Any(t => t.HoraInicio == horaActual);

                        if (!ocupado)
                        {
                            horariosDisponibles.Add(horaActual);
                        }

                        // Incrementar la hora en intervalos de 30 minutos
                        horaActual = horaActual.Add(TimeSpan.FromMinutes(30));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener horarios disponibles", ex);
            }

            return horariosDisponibles;
        }

        public void GuardarTurno(Turno turno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Configurar el procedimiento almacenado y los parámetros
                datos.SetearProcedimiento("sp_GuardarTurno");
                datos.SetearParametro("@PacienteId", turno.PacienteId);
                datos.SetearParametro("@MedicoId", turno.Medico.MedicoId);
                datos.SetearParametro("@EspecialidadId", turno.Especialidad.EspecialidadId);
                datos.SetearParametro("@Fecha", turno.Fecha);
                datos.SetearParametro("@Hora", turno.HoraInicio);
                datos.SetearParametro("@Estado", turno.Estado.ToString());
                datos.SetearParametro("@Observaciones", turno.Observaciones);

                // Ejecutar la consulta
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el turno", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void ActualizarTurno(Turno turno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ActualizarTurno");
                datos.SetearParametro("@TurnoId", turno.TurnoId);
                datos.SetearParametro("@Fecha", turno.Fecha);
                datos.SetearParametro("@HoraInicio", turno.HoraInicio);
                datos.SetearParametro("@Observaciones", turno.Observaciones);
                datos.SetearParametro("@Estado", turno.Estado.ToString());
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el turno", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }
}

