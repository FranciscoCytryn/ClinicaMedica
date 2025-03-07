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

                gestionPasos.AgregarPaso("Paciente");
                gestionPasos.AgregarPaso("Especialidad");
                gestionPasos.AgregarPaso("Medico");
                gestionPasos.AgregarPaso("FechaHora");
                gestionPasos.AgregarPaso("Observacion"); 

                gestionPasos.HabilitarPaso("Paciente");

                CargarPacientes();
                CargarEspecialidades();

                ActualizarControles();
            }
        }

        private void ActualizarControles()
        {
            ddlPaciente.Enabled = gestionPasos.ObtenerPaso("Paciente").Habilitado;
            ddlEspecialidad.Enabled = gestionPasos.ObtenerPaso("Especialidad").Habilitado;
            ddlMedico.Enabled = gestionPasos.ObtenerPaso("Medico").Habilitado;
            txtFecha.Enabled = gestionPasos.ObtenerPaso("FechaHora").Habilitado;
            ddlHora.Enabled = gestionPasos.ObtenerPaso("FechaHora").Habilitado;
            txtObservacion.Enabled = gestionPasos.ObtenerPaso("Observacion").Habilitado;

            btnVolverPasoAnterior.Visible = !gestionPasos.ObtenerPaso("Paciente").Habilitado;

            btnNuevoPaciente.Enabled = gestionPasos.ObtenerPaso("Paciente").Habilitado;

            btnConfirmarTurno.Enabled = gestionPasos.ObtenerPaso("Observacion").Habilitado;

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
                ddlPaciente.DataTextField = "Nombre";
                ddlPaciente.DataValueField = "PacienteId"; 
                ddlPaciente.DataBind();

                ddlPaciente.Items.Insert(0, new ListItem("Seleccione un paciente", "0"));
            }
            catch (Exception ex)
            {
                //MostrarError("Error al cargar los pacientes: " + ex.Message);
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                List<Especialidad> listaEspecialidades = especialidadNegocio.Listar();

                ddlEspecialidad.DataSource = listaEspecialidades;
                ddlEspecialidad.DataTextField = "Nombre"; 
                ddlEspecialidad.DataValueField = "EspecialidadId"; 
                ddlEspecialidad.DataBind();

                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione una especialidad", "0"));
            }
            catch (Exception ex)
            {
                //MostrarError("Error al cargar las especialidades: " + ex.Message);
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
                gestionPasos.DeshabilitarPaso("Paciente");

                gestionPasos.HabilitarPaso("Especialidad");

                ActualizarControles();
            }
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int especialidadId = Convert.ToInt32(ddlEspecialidad.SelectedValue);

            if (especialidadId > 0)
            {
                gestionPasos.DeshabilitarPaso("Especialidad");

                gestionPasos.HabilitarPaso("Medico");

                CargarMedicosPorEspecialidad(especialidadId);

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
                ddlMedico.DataTextField = "Nombre"; 
                ddlMedico.DataValueField = "MedicoId"; 
                ddlMedico.DataBind();

                ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico", "0"));
            }
            catch (Exception ex)
            {
                //MostrarError("Error al cargar los médicos: " + ex.Message); 
            }
        }

        protected void btnVolverPasoAnterior_Click(object sender, EventArgs e)
        {
            string pasoActual = ObtenerPasoActual();

            if (pasoActual == null)
            {
                MostrarMensaje("No se pudo determinar el paso actual.", false);
                return;
            }

            string pasoAnterior = gestionPasos.ObtenerPasoAnterior(pasoActual);
            if (pasoAnterior != null)
            {
                gestionPasos.HabilitarPaso(pasoAnterior);

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
                gestionPasos.DeshabilitarPaso("Medico");

                gestionPasos.HabilitarPaso("FechaHora");

                txtFecha.Enabled = true;
                ddlHora.Enabled = true;

                ActualizarControles();
            }
        }

        private string ObtenerPasoActual()
        {
            if (!gestionPasos.Pasos.Any(p => p.Habilitado))
            {
                return "FechaHora";
            }

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

            return null; 
        }

        protected void btnConfirmarTurno_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPaciente.SelectedValue == "0" || ddlEspecialidad.SelectedValue == "0" ||
                    ddlMedico.SelectedValue == "0" || string.IsNullOrEmpty(txtFecha.Text) ||
                    ddlHora.SelectedValue == "0" || string.IsNullOrEmpty(txtObservacion.Text))
                {
                    MostrarMensaje("Por favor, complete todos los campos antes de confirmar el turno.", false);
                    return;
                }

                DateTime fechaSeleccionada = DateTime.Parse(txtFecha.Text);
                if (fechaSeleccionada < DateTime.Today)
                {
                    MostrarMensaje("La fecha seleccionada no puede ser anterior a la fecha actual.", false);
                    return;
                }

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

        public List<TimeSpan> ObtenerHorariosDisponibles(int medicoId, DateTime fecha)
        {
            List<TimeSpan> horariosDisponibles = new List<TimeSpan>();
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            TurnoNegocio turnoNegocio = new TurnoNegocio();

            try
            {
                List<TurnoTrabajo> horarioTrabajo = medicoNegocio.ObtenerTurnosTrabajoPorMedico(medicoId);

                List<Turno> turnosAsignados = turnoNegocio.ListarTurnosPorMedicoYFecha(medicoId, fecha);

                foreach (var turnoTrabajo in horarioTrabajo)
                {
                    TimeSpan horaActual = turnoTrabajo.HoraEntrada;

                    while (horaActual < turnoTrabajo.HoraSalida)
                    {
                        bool ocupado = turnosAsignados.Any(t => t.HoraInicio == horaActual);

                        if (!ocupado)
                        {
                            horariosDisponibles.Add(horaActual);
                        }

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

                ddlHora.Items.Clear();

                foreach (var hora in horariosDisponibles)
                {
                    ddlHora.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                ddlHora.Enabled = true;

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
                gestionPasos.DeshabilitarPaso("FechaHora");

                gestionPasos.HabilitarPaso("Observacion");

                ActualizarControles();
            }
        }

        protected void btnConfirmarHidden_Click(object sender, EventArgs e)
        {
            try
            {
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

                TurnoNegocio turnoNegocio = new TurnoNegocio();
                turnoNegocio.GuardarTurno(turno);

                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                Paciente paciente = pacienteNegocio.ObtenerPacientePorId(turno.PacienteId);
                string nombreEspecialidad = ddlEspecialidad.SelectedItem.Text;
                string nombreMedico = ddlMedico.SelectedItem.Text;
                EmailService emailService = new EmailService();
                string asunto = emailService.CrearAsuntoConfirmacionTurno();
                string cuerpo = emailService.CrearTemplateConfirmacionTurno(
                    paciente.Nombre,
                    turno.Fecha,
                    turno.HoraInicio,
                    nombreEspecialidad,
                    nombreMedico,
                    turno.Observaciones
                );

                emailService.EnviarCorreo(paciente.Email, asunto, cuerpo);

                MostrarMensaje("Nuevo turno confirmado.", true);

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
                string script = $@"alert('{mensaje}'); window.location.href = 'GestionTurnos.aspx';";
                ScriptManager.RegisterStartupScript(this, GetType(), "MensajeExito", script, true);
            }
            else
            {
                lblMensaje.Text = mensaje;
                lblMensaje.Visible = true;
            }
        }

    }
}