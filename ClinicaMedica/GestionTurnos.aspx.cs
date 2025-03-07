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
    public partial class GestionTurnos : System.Web.UI.Page
    {
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
                CargarTurnos();
            }
        }
        private void CargarTurnos()
        {
            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                List<Turno> listaTurnos = turnoNegocio.Listar();
                gvTurnos.DataSource = listaTurnos;
                gvTurnos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar los turnos: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-danger";
            lblMensaje.Visible = true;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#modalEditarTurno').modal('hide');", true);
        }

        public bool EstaDentroDelHorario(TimeSpan horaSeleccionada, Especialidad especialidad)
        {
            if (especialidad != null)
            {
                return horaSeleccionada >= especialidad.HoraInicio && horaSeleccionada <= especialidad.HoraFin;
            }
            return false;
        }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Turno turno = (Turno)e.Row.DataItem;

                Panel pnlAcciones = (Panel)e.Row.FindControl("pnlAcciones");

                if (pnlAcciones != null)
                {
                    Button btnReprogramar = (Button)pnlAcciones.FindControl("btnReprogramar");
                    Button btnCancelar = (Button)pnlAcciones.FindControl("btnCancelar");
                    Button btnCerrar = (Button)pnlAcciones.FindControl("btnCerrar");
                    Button btnNoAsistio = (Button)pnlAcciones.FindControl("btnNoAsistio");

                    if (btnReprogramar != null && btnCancelar != null && btnCerrar != null && btnNoAsistio != null)
                    {
                        DateTime fechaTurno = turno.Fecha;
                        DateTime fechaActual = DateTime.Now.Date;

                        bool esCancelado = turno.Estado == EstadoTurno.Cancelado;

                        if (esCancelado)
                        {
                            btnReprogramar.Visible = false;
                            btnCancelar.Visible = false;
                            btnCerrar.Visible = false;
                            btnNoAsistio.Visible = false;
                        }
                        else
                        {
                            bool esTurnoPasado = fechaTurno < fechaActual;
                            bool esEstadoValido = turno.Estado == EstadoTurno.Nuevo || turno.Estado == EstadoTurno.Reprogramado;

                            btnReprogramar.Visible = !esTurnoPasado; 
                            btnCancelar.Visible = !esTurnoPasado;    
                            btnCerrar.Visible = esTurnoPasado && esEstadoValido; 
                            btnNoAsistio.Visible = esTurnoPasado && esEstadoValido; 
                        }
                    }
                    else
                    {
                        MostrarError("No se encontraron los botones en la fila actual.");
                    }
                }
                else
                {
                    MostrarError("No se encontró el contenedor de botones en la fila actual.");
                }
            }
        }

        protected void btnCancelarTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Session["AccionTurno"] = "Cancelado";

            gvTurnos.EditIndex = row.RowIndex;

            CargarTurnos();
        }
        protected void btnCerrarTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Session["AccionTurno"] = "Cerrado";

            gvTurnos.EditIndex = row.RowIndex;

            CargarTurnos();
        }

        protected void btnNoAsistioTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Session["AccionTurno"] = "NoAsistio";

            gvTurnos.EditIndex = row.RowIndex;

            CargarTurnos();
        }

        protected string GetCommandName(System.Web.UI.IDataItemContainer container)
        {
            GridViewRow row = (GridViewRow)container;

            Button btnCancelar = (Button)row.FindControl("btnCancelar");
            Button btnCerrar = (Button)row.FindControl("btnCerrar");
            Button btnNoAsistio = (Button)row.FindControl("btnNoAsistio");

            if (btnCancelar.Visible)
            {
                return "Cancelar";
            }
            else if (btnCerrar.Visible)
            {
                return "Cerrar";
            }
            else if (btnNoAsistio.Visible)
            {
                return "NoAsistio";
            }

            return string.Empty;
        }

        protected void gvTurnos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int turnoId = Convert.ToInt32(gvTurnos.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvTurnos.Rows[e.RowIndex];

            TextBox txtObservaciones = (TextBox)row.FindControl("txtObservaciones");

            if (string.IsNullOrEmpty(txtObservaciones.Text))
            {
                MostrarError("La observación es obligatoria.");
                return;
            }

            string accion = Session["AccionTurno"] as string;
            if (string.IsNullOrEmpty(accion))
            {
                MostrarError("No se pudo determinar la acción a realizar.");
                return;
            }

            TurnoNegocio turnoNegocio = new TurnoNegocio();
            Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            Paciente paciente = pacienteNegocio.ObtenerPacientePorId(turno.PacienteId);
            try
            {
                turnoNegocio.ActualizarObservacion(turnoId, txtObservaciones.Text);

                switch (accion)
                {
                    case "Cancelado":
                        turnoNegocio.ActualizarEstado(turnoId, "Cancelado");
             
                        EmailService emailService = new EmailService();
                        string asunto = emailService.CrearAsuntoCancelacionTurno();
                        string cuerpo = emailService.CrearTemplateCancelacionTurno(
                            paciente.Nombre,
                            turno.Fecha,
                            turno.HoraInicio,
                            txtObservaciones.Text
                        );
                        emailService.EnviarCorreo(paciente.Email, asunto, cuerpo);
                        break;
                    case "Cerrado":
                        turnoNegocio.ActualizarEstado(turnoId, "Cerrado");
                        break;
                    case "NoAsistio":
                        turnoNegocio.ActualizarEstado(turnoId, "No Asistió");
                        break;
                    default:
                        throw new Exception("Acción no válida.");
                }

                Session["AccionTurno"] = null;

                gvTurnos.EditIndex = -1;

                CargarTurnos();
            }
            catch (Exception ex)
            {
                MostrarError("Error al actualizar el turno: " + ex.Message);
            }
        }

        protected void gvTurnos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            Session["AccionTurno"] = null;

            gvTurnos.EditIndex = -1;

            CargarTurnos();
        }

        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int turnoId = Convert.ToInt32(btn.CommandArgument);

            TurnoNegocio turnoNegocio = new TurnoNegocio();
            Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

            if (turno != null)
            {
                hiddenTurnoId.Value = turnoId.ToString();
                txtFechaReprogramar.Text = turno.Fecha.ToString("yyyy-MM-dd");
                txtObservacionReprogramar.Text = turno.Observaciones;

                List<TimeSpan> horariosDisponibles = turnoNegocio.ObtenerHorariosDisponibles(turno.Medico.MedicoId, turno.Fecha);

                if (!horariosDisponibles.Contains(turno.HoraInicio))
                {
                    horariosDisponibles.Add(turno.HoraInicio);
                }

                ddlHoraReprogramar.Items.Clear();

                horariosDisponibles.Sort();

                foreach (var hora in horariosDisponibles)
                {
                    ddlHoraReprogramar.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                ddlHoraReprogramar.SelectedValue = turno.HoraInicio.ToString();

                string script = @"
                $(document).ready(function() {
                    $('#modalReprogramar').modal('show');
                });";
                ScriptManager.RegisterStartupScript(this, GetType(), "openModal", script, true);
            }
            else
            {
                MostrarError("No se pudo cargar el turno seleccionado.");
            }
        }

        protected void txtFechaReprogramar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int turnoId = int.Parse(hiddenTurnoId.Value);

                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

                if (turno == null)
                {
                    MostrarError("No se pudo encontrar el turno seleccionado.");
                    return;
                }

                DateTime fechaSeleccionada = DateTime.Parse(txtFechaReprogramar.Text);

                List<TimeSpan> horariosDisponibles = turnoNegocio.ObtenerHorariosDisponibles(turno.Medico.MedicoId, fechaSeleccionada);

                ddlHoraReprogramar.Items.Clear();

                foreach (var hora in horariosDisponibles)
                {
                    ddlHoraReprogramar.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                ddlHoraReprogramar.Enabled = true;
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar horarios disponibles: " + ex.Message);
            }
        }

        protected void btnConfirmarReprogramar_Click(object sender, EventArgs e)
        {
            try
            {
                int turnoId = int.Parse(hiddenTurnoId.Value);

                DateTime nuevaFecha = DateTime.Parse(txtFechaReprogramar.Text);
                TimeSpan nuevaHora = TimeSpan.Parse(ddlHoraReprogramar.SelectedValue);
                string nuevaObservacion = txtObservacionReprogramar.Text;

                if (nuevaFecha < DateTime.Today)
                {
                    MostrarError("La fecha seleccionada no puede ser anterior a la fecha actual.");
                    return;
                }

                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

                if (turno == null)
                {
                    MostrarError("No se pudo encontrar el turno seleccionado.");
                    return;
                }

                turno.Fecha = nuevaFecha;
                turno.HoraInicio = nuevaHora;
                turno.Observaciones = nuevaObservacion;
                turno.Estado = EstadoTurno.Reprogramado;

                turnoNegocio.ActualizarTurno(turno);

                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                Paciente paciente = pacienteNegocio.ObtenerPacientePorId(turno.PacienteId);

                EmailService emailService = new EmailService();
                string destinatario = paciente.Email;
                string asunto = emailService.CrearAsuntoTurnoReprogramado();
                string cuerpo = emailService.CrearTemplateTurnoReprogramado(paciente.Nombre, nuevaFecha, nuevaHora, nuevaObservacion);
                emailService.EnviarCorreo(destinatario, asunto, cuerpo);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#modalReprogramar').modal('hide');", true);

                CargarTurnos();

                MostrarMensaje("Turno reprogramado correctamente.", true);
            }
            catch (Exception ex)
            {
                MostrarError("Error al reprogramar el turno: " + ex.Message);
            }
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            lblMensaje.Text = mensaje;

            if (esExito)
            {
                lblMensaje.CssClass = "alert alert-success";
            }
            else
            {
                lblMensaje.CssClass = "alert alert-danger";
            }

            lblMensaje.Visible = true;
        }

        protected void ValidarReprogramacion(object sender, EventArgs e)
        {
            bool esValido = true;

            DateTime fechaSeleccionada;
            if (DateTime.TryParse(txtFechaReprogramar.Text, out fechaSeleccionada))
            {
                if (fechaSeleccionada < DateTime.Today)
                {
                    lblErrorFecha.Text = "La fecha debe ser posterior a la actual.";
                    lblErrorFecha.Visible = true;
                    esValido = false;
                }
                else
                {
                    lblErrorFecha.Visible = false;
                }
            }

            // Validación de Observación
            if (string.IsNullOrWhiteSpace(txtObservacionReprogramar.Text))
            {
                lblErrorObservacion.Text = "La observación no puede estar vacía.";
                lblErrorObservacion.Visible = true;
                esValido = false;
            }
            else
            {
                lblErrorObservacion.Visible = false;
            }

            // Habilitar o deshabilitar el botón de confirmar
            btnConfirmarReprogramar.Enabled = esValido;
        }


        protected void btnAltaTurno_Click(object sender, EventArgs e)
        {
            Response.Redirect("AltaTurno.aspx");
        }
    }
}