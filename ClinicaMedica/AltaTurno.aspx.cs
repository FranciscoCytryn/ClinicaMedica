using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaMedica
{
    public partial class AltaTurno : System.Web.UI.Page
    {
        private GestionPasos gestionPasos
        {
            get
            {
                if (ViewState["GestionPasos"] == null)
                {
                    ViewState["GestionPasos"] = new GestionPasos();
                }
                return (GestionPasos)ViewState["GestionPasos"];
            }
            set
            {
                ViewState["GestionPasos"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                gestionPasos = new GestionPasos();

                // Agregar los pasos
                gestionPasos.AgregarPaso("Paciente");
                gestionPasos.AgregarPaso("Especialidad");
                gestionPasos.AgregarPaso("Medico");
                gestionPasos.AgregarPaso("FechaHora");
                gestionPasos.AgregarPaso("Observacion"); // Nuevo paso

                // Habilitar el primer paso (Paciente)
                gestionPasos.HabilitarPaso("Paciente");

                CargarPacientes();
                CargarEspecialidades();

                ActualizarControles();
            }
        }

        private void ActualizarControles()
        {
            // Habilitar/deshabilitar controles según el estado de los pasos
            ddlPaciente.Enabled = gestionPasos.ObtenerPaso("Paciente").Habilitado;
            ddlEspecialidad.Enabled = gestionPasos.ObtenerPaso("Especialidad").Habilitado;
            ddlMedico.Enabled = gestionPasos.ObtenerPaso("Medico").Habilitado;
            txtFecha.Enabled = gestionPasos.ObtenerPaso("FechaHora").Habilitado;
            ddlHora.Enabled = gestionPasos.ObtenerPaso("FechaHora").Habilitado;
            txtObservacion.Enabled = gestionPasos.ObtenerPaso("Observacion").Habilitado;

            // Mostrar el botón "Volver al paso anterior" siempre que no estemos en el primer paso
            btnVolverPasoAnterior.Visible = !gestionPasos.ObtenerPaso("Paciente").Habilitado;

            // Desactivar el botón "Nuevo Paciente" si el paso de paciente está deshabilitado
            btnNuevoPaciente.Enabled = gestionPasos.ObtenerPaso("Paciente").Habilitado;

            // Habilitar el botón "Confirmar Turno" cuando el paso "Observacion" esté habilitado
            btnConfirmarTurno.Enabled = gestionPasos.ObtenerPaso("Observacion").Habilitado;

            // Limpiar el mensaje de error
            lblMensaje.Visible = false;
            lblMensaje.Text = string.Empty;
        }


        private void CargarPacientes()
        {
            try
            {
                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                List<Paciente> listaPacientes = pacienteNegocio.Listar();

                ddlPaciente.DataSource = listaPacientes;
                ddlPaciente.DataTextField = "Nombre"; // Mostrar el nombre del paciente
                ddlPaciente.DataValueField = "PacienteId"; // Valor asociado al ID del paciente
                ddlPaciente.DataBind();

                // Agregar un ítem predeterminado
                ddlPaciente.Items.Insert(0, new ListItem("Seleccione un paciente", "0"));
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar los pacientes: " + ex.Message);
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                List<Especialidad> listaEspecialidades = especialidadNegocio.Listar();

                ddlEspecialidad.DataSource = listaEspecialidades;
                ddlEspecialidad.DataTextField = "Nombre"; // Mostrar el nombre de la especialidad
                ddlEspecialidad.DataValueField = "EspecialidadId"; // Valor asociado al ID de la especialidad
                ddlEspecialidad.DataBind();

                // Agregar un ítem predeterminado
                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione una especialidad", "0"));
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar las especialidades: " + ex.Message);
            }
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionPacientes.aspx");
        }

        protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pacienteId = Convert.ToInt32(ddlPaciente.SelectedValue);

            if (pacienteId > 0)
            {
                // Deshabilitar el paso actual (Paciente)
                gestionPasos.DeshabilitarPaso("Paciente");

                // Habilitar el siguiente paso (Especialidad)
                gestionPasos.HabilitarPaso("Especialidad");

                // Actualizar la interfaz de usuario
                ActualizarControles();
            }
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int especialidadId = Convert.ToInt32(ddlEspecialidad.SelectedValue);

            if (especialidadId > 0)
            {
                // Deshabilitar el paso actual (Especialidad)
                gestionPasos.DeshabilitarPaso("Especialidad");

                // Habilitar el siguiente paso (Medico)
                gestionPasos.HabilitarPaso("Medico");

                // Cargar los médicos asociados a la especialidad seleccionada
                CargarMedicosPorEspecialidad(especialidadId);

                // Actualizar la interfaz de usuario
                ActualizarControles();
            }
        }

        private void CargarMedicosPorEspecialidad(int especialidadId)
        {
            try
            {
                MedicoNegocio medicoNegocio = new MedicoNegocio();
                List<Medico> listaMedicos = medicoNegocio.ListarMedicosPorEspecialidad(especialidadId);

                ddlMedico.DataSource = listaMedicos;
                ddlMedico.DataTextField = "Nombre"; // Mostrar el nombre del médico
                ddlMedico.DataValueField = "MedicoId"; // Valor asociado al ID del médico
                ddlMedico.DataBind();

                // Agregar un ítem predeterminado
                ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico", "0"));
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar los médicos: " + ex.Message);
            }
        }

        protected void btnVolverPasoAnterior_Click(object sender, EventArgs e)
        {
            // Obtener el paso actual
            string pasoActual = ObtenerPasoActual();

            if (pasoActual == null)
            {
                MostrarMensaje("No se pudo determinar el paso actual.", false);
                return;
            }

            // Habilitar el paso anterior
            string pasoAnterior = gestionPasos.ObtenerPasoAnterior(pasoActual);
            if (pasoAnterior != null)
            {
                gestionPasos.HabilitarPaso(pasoAnterior);

                // Si volvemos al paso de selección de paciente, reactivar el botón "Nuevo Paciente"
                if (pasoAnterior == "Paciente")
                {
                    btnNuevoPaciente.Enabled = true;
                }
            }

            // Deshabilitar el paso actual
            gestionPasos.DeshabilitarPaso(pasoActual);

            // Deshabilitar el campo "Observacion" si se retrocede desde el paso "Observacion"
            if (pasoActual == "Observacion")
            {
                txtObservacion.Enabled = false;
            }

            // Actualizar la interfaz de usuario
            ActualizarControles();
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            int medicoId = Convert.ToInt32(ddlMedico.SelectedValue);

            if (medicoId > 0)
            {
                // Deshabilitar el paso actual (Medico)
                gestionPasos.DeshabilitarPaso("Medico");

                // Habilitar el siguiente paso (FechaHora)
                gestionPasos.HabilitarPaso("FechaHora");

                // Habilitar los controles de fecha y hora
                txtFecha.Enabled = true;
                ddlHora.Enabled = true;

                // Actualizar la interfaz de usuario
                ActualizarControles();
            }
        }

        private string ObtenerPasoActual()
        {
            // Si todos los pasos están deshabilitados, el paso actual es el último paso completado ("FechaHora")
            if (!gestionPasos.Pasos.Any(p => p.Habilitado))
            {
                return "FechaHora";
            }

            // Determinar el paso actual basado en los controles habilitados
            if (txtFecha.Enabled)
            {
                return "FechaHora";
            }
            else if (ddlMedico.Enabled)
            {
                return "Medico";
            }
            else if (ddlEspecialidad.Enabled)
            {
                return "Especialidad";
            }
            else if (ddlPaciente.Enabled)
            {
                return "Paciente";
            }

            return null; // En caso de que no se encuentre un paso actual
        }

        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar que todos los campos estén completos
                if (ddlPaciente.SelectedValue == "0" || ddlEspecialidad.SelectedValue == "0" ||
                    ddlMedico.SelectedValue == "0" || string.IsNullOrEmpty(txtFecha.Text) ||
                    ddlHora.SelectedValue == "0" || string.IsNullOrEmpty(txtObservacion.Text))
                {
                    MostrarMensaje("Por favor, complete todos los campos antes de confirmar el turno.", false);
                    return;
                }

                // Validar que la fecha no sea anterior a la fecha actual
                DateTime fechaSeleccionada = DateTime.Parse(txtFecha.Text);
                if (fechaSeleccionada < DateTime.Today)
                {
                    MostrarMensaje("La fecha seleccionada no puede ser anterior a la fecha actual.", false);
                    return;
                }

                // Mostrar cuadro de confirmación
                string confirmacionScript = $@"
                if (confirm('¿Está seguro de que desea confirmar el turno?')) {{
                    document.getElementById('{btnConfirmarHidden.ClientID}').click();
                }}";
                ScriptManager.RegisterStartupScript(this, GetType(), "Confirmacion", confirmacionScript, true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al validar el turno: " + ex.Message, false);
            }
        }

        private void LimpiarFormulario()
        {
            // Limpiar los controles del formulario
            ddlPaciente.SelectedIndex = 0;
            ddlEspecialidad.SelectedIndex = 0;
            ddlMedico.SelectedIndex = 0;
            txtFecha.Text = string.Empty;
            ddlHora.SelectedIndex = 0;

            // Deshabilitar los pasos posteriores
            gestionPasos.DeshabilitarPaso("Especialidad");
            gestionPasos.DeshabilitarPaso("Medico");
            gestionPasos.DeshabilitarPaso("FechaHora");

            // Actualizar la interfaz de usuario
            ActualizarControles();
        }

        public List<TimeSpan> ObtenerHorariosDisponibles(int medicoId, DateTime fecha)
        {
            List<TimeSpan> horariosDisponibles = new List<TimeSpan>();
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            TurnoNegocio turnoNegocio = new TurnoNegocio();

            try
            {
                // Obtener el horario de trabajo del médico
                List<TurnoTrabajo> horarioTrabajo = medicoNegocio.ObtenerTurnosTrabajoPorMedico(medicoId);

                // Obtener los turnos ya asignados al médico en la fecha seleccionada
                List<Turno> turnosAsignados = turnoNegocio.ListarTurnosPorMedicoYFecha(medicoId, fecha);

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

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            int medicoId = Convert.ToInt32(ddlMedico.SelectedValue);
            DateTime fecha = DateTime.Parse(txtFecha.Text);

            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                List<TimeSpan> horariosDisponibles = turnoNegocio.ObtenerHorariosDisponibles(medicoId, fecha);

                // Limpiar el DropDownList antes de cargar nuevos datos
                ddlHora.Items.Clear();

                // Cargar los horarios disponibles en el DropDownList
                foreach (var hora in horariosDisponibles)
                {
                    ddlHora.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                // Habilitar el DropDownList de horas
                ddlHora.Enabled = true;

                // No deshabilitar el paso "FechaHora" aquí
                // gestionPasos.DeshabilitarPaso("FechaHora"); // Eliminar esta línea

                // Actualizar la interfaz de usuario
                ActualizarControles();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar horarios disponibles: " + ex.Message, false);
            }
        }

        protected void ddlHora_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHora.SelectedValue != "0")
            {
                // Deshabilitar el paso "FechaHora"
                gestionPasos.DeshabilitarPaso("FechaHora");

                // Habilitar el paso "Observacion"
                gestionPasos.HabilitarPaso("Observacion");

                // Actualizar la interfaz de usuario
                ActualizarControles();
            }
        }

        protected void btnConfirmarHidden_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un objeto Turno con los datos seleccionados
                Turno turno = new Turno
                {
                    PacienteId = Convert.ToInt32(ddlPaciente.SelectedValue),
                    Medico = new Medico
                    {
                        MedicoId = Convert.ToInt32(ddlMedico.SelectedValue)
                    },
                    Especialidad = new Especialidad
                    {
                        EspecialidadId = Convert.ToInt32(ddlEspecialidad.SelectedValue)
                    },
                    Fecha = DateTime.Parse(txtFecha.Text),
                    HoraInicio = TimeSpan.Parse(ddlHora.SelectedValue),
                    Observaciones = txtObservacion.Text,
                    Estado = EstadoTurno.Nuevo
                };

                // Guardar el turno en la base de datos
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                turnoNegocio.GuardarTurno(turno);

                // Mostrar mensaje de éxito
                MostrarMensaje("Nuevo turno confirmado.", true);

                // Redirigir a la página GestionTurnos.aspx
                Response.Redirect("GestionTurnos.aspx");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar el turno: " + ex.Message, false);
            }
        }
        private void MostrarMensaje(string mensaje, bool esExito)
        {
            if (esExito)
            {
                // Mostrar mensaje de éxito como alert y redirigir
                string script = $@"alert('{mensaje}'); window.location.href = 'GestionTurnos.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "MensajeExito", script, true);
            }
            else
            {
                // Mostrar mensaje de error en el Label
                lblMensaje.Text = mensaje;
                lblMensaje.Visible = true;
            }
        }

        private void MostrarError(string mensaje)
        {
            // Implementa la lógica para mostrar un mensaje de error en la interfaz
        }
    }
}