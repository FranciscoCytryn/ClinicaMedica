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
                // Obtener el objeto Turno asociado a la fila actual
                Turno turno = (Turno)e.Row.DataItem;

                // Buscar el contenedor de los botones (si existe)
                Panel pnlAcciones = (Panel)e.Row.FindControl("pnlAcciones");

                if (pnlAcciones != null)
                {
                    // Buscar los botones dentro del contenedor
                    Button btnReprogramar = (Button)pnlAcciones.FindControl("btnReprogramar");
                    Button btnCancelar = (Button)pnlAcciones.FindControl("btnCancelar");
                    Button btnCerrar = (Button)pnlAcciones.FindControl("btnCerrar");
                    Button btnNoAsistio = (Button)pnlAcciones.FindControl("btnNoAsistio");

                    // Verificar si los botones se encontraron correctamente
                    if (btnReprogramar != null && btnCancelar != null && btnCerrar != null && btnNoAsistio != null)
                    {
                        DateTime fechaTurno = turno.Fecha;
                        DateTime fechaActual = DateTime.Now.Date;

                        // Verificar si el estado es "Cancelado"
                        bool esCancelado = turno.Estado == EstadoTurno.Cancelado;

                        if (esCancelado)
                        {
                            // Si el estado es "Cancelado", ocultar todos los botones
                            btnReprogramar.Visible = false;
                            btnCancelar.Visible = false;
                            btnCerrar.Visible = false;
                            btnNoAsistio.Visible = false;
                        }
                        else
                        {
                            // Si el estado no es "Cancelado", aplicar las reglas anteriores
                            bool esTurnoPasado = fechaTurno < fechaActual;
                            bool esEstadoValido = turno.Estado == EstadoTurno.Nuevo || turno.Estado == EstadoTurno.Reprogramado;

                            btnReprogramar.Visible = !esTurnoPasado; // Solo visible para turnos futuros
                            btnCancelar.Visible = !esTurnoPasado;    // Solo visible para turnos futuros
                            btnCerrar.Visible = esTurnoPasado && esEstadoValido; // Solo visible para turnos pasados con estado válido
                            btnNoAsistio.Visible = esTurnoPasado && esEstadoValido; // Solo visible para turnos pasados con estado válido
                        }
                    }
                    else
                    {
                        // Mostrar un mensaje de error si los botones no se encontraron
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

            // Almacenar la acción en una variable de sesión
            Session["AccionTurno"] = "Cancelado";

            // Poner la fila en modo de edición
            gvTurnos.EditIndex = row.RowIndex;

            // Recargar la GridView para reflejar el modo de edición
            CargarTurnos();
        }
        protected void btnCerrarTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Almacenar la acción en una variable de sesión
            Session["AccionTurno"] = "Cerrado";

            // Poner la fila en modo de edición
            gvTurnos.EditIndex = row.RowIndex;

            // Recargar la GridView para reflejar el modo de edición
            CargarTurnos();
        }

        protected void btnNoAsistioTurno_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Almacenar la acción en una variable de sesión
            Session["AccionTurno"] = "NoAsistio";

            // Poner la fila en modo de edición
            gvTurnos.EditIndex = row.RowIndex;

            // Recargar la GridView para reflejar el modo de edición
            CargarTurnos();
        }

        protected void btnConfirmarAccion_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int turnoId = Convert.ToInt32(btn.CommandArgument);

            // Obtener la observación
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            TextBox txtObservacion = (TextBox)row.FindControl("txtObservacion");

            if (string.IsNullOrEmpty(txtObservacion.Text))
            {
                MostrarError("La observación es obligatoria.");
                return;
            }

            // Actualizar el estado y la observación
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            try
            {
                // Actualizar la observación
                turnoNegocio.ActualizarObservacion(turnoId, txtObservacion.Text);

                // Cambiar el estado a "Cancelado"
                turnoNegocio.ActualizarEstado(turnoId, "Cancelado");

                // Recargar la GridView para reflejar los cambios
                CargarTurnos();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cancelar el turno: " + ex.Message);
            }
        }

        protected string GetCommandName(System.Web.UI.IDataItemContainer container)
        {
            // Convertir el contenedor a GridViewRow
            GridViewRow row = (GridViewRow)container;

            // Buscar los botones en la fila actual
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
            // Obtener el ID del turno
            int turnoId = Convert.ToInt32(gvTurnos.DataKeys[e.RowIndex].Value);

            // Obtener la fila actual en modo de edición
            GridViewRow row = gvTurnos.Rows[e.RowIndex];

            // Obtener la observación editada
            TextBox txtObservaciones = (TextBox)row.FindControl("txtObservaciones");

            if (string.IsNullOrEmpty(txtObservaciones.Text))
            {
                MostrarError("La observación es obligatoria.");
                return;
            }

            // Obtener la acción almacenada en la variable de sesión
            string accion = Session["AccionTurno"] as string;
            if (string.IsNullOrEmpty(accion))
            {
                MostrarError("No se pudo determinar la acción a realizar.");
                return;
            }

            // Actualizar el estado y la observación
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            try
            {
                // Actualizar la observación
                turnoNegocio.ActualizarObservacion(turnoId, txtObservaciones.Text);

                // Cambiar el estado según la acción
                switch (accion)
                {
                    case "Cancelado":
                        turnoNegocio.ActualizarEstado(turnoId, "Cancelado");
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

                // Limpiar la variable de sesión
                Session["AccionTurno"] = null;

                // Salir del modo de edición
                gvTurnos.EditIndex = -1;

                // Recargar la GridView para reflejar los cambios
                CargarTurnos();
            }
            catch (Exception ex)
            {
                MostrarError("Error al actualizar el turno: " + ex.Message);
            }
        }

        protected void gvTurnos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Limpiar la variable de sesión
            Session["AccionTurno"] = null;

            // Salir del modo de edición
            gvTurnos.EditIndex = -1;

            // Recargar la GridView
            CargarTurnos();
        }

        protected void btnReprogramar_Click(object sender, EventArgs e)
        {
            // Obtener el ID del turno desde el CommandArgument
            Button btn = (Button)sender;
            int turnoId = Convert.ToInt32(btn.CommandArgument);

            // Obtener el turno desde la base de datos
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

            if (turno != null)
            {
                // Cargar los datos del turno en el modal
                hiddenTurnoId.Value = turnoId.ToString();
                txtFechaReprogramar.Text = turno.Fecha.ToString("yyyy-MM-dd");
                txtObservacionReprogramar.Text = turno.Observaciones;

                // Obtener los horarios disponibles para la fecha actual
                List<TimeSpan> horariosDisponibles = turnoNegocio.ObtenerHorariosDisponibles(turno.Medico.MedicoId, turno.Fecha);

                // Agregar el horario actual del turno a la lista de horarios disponibles
                if (!horariosDisponibles.Contains(turno.HoraInicio))
                {
                    horariosDisponibles.Add(turno.HoraInicio);
                }

                // Limpiar el DropDownList antes de cargar nuevos datos
                ddlHoraReprogramar.Items.Clear();

                // Ordenar los horarios disponibles
                horariosDisponibles.Sort();

                // Cargar los horarios disponibles en el DropDownList
                foreach (var hora in horariosDisponibles)
                {
                    ddlHoraReprogramar.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                // Seleccionar la hora actual del turno
                ddlHoraReprogramar.SelectedValue = turno.HoraInicio.ToString();

                // Abrir el modal usando JavaScript
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
                // Obtener el ID del turno desde el HiddenField
                int turnoId = int.Parse(hiddenTurnoId.Value);

                // Obtener el turno desde la base de datos
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

                if (turno == null)
                {
                    MostrarError("No se pudo encontrar el turno seleccionado.");
                    return;
                }

                // Obtener la fecha seleccionada
                DateTime fechaSeleccionada = DateTime.Parse(txtFechaReprogramar.Text);

                // Obtener los horarios disponibles para el médico en la fecha seleccionada
                List<TimeSpan> horariosDisponibles = turnoNegocio.ObtenerHorariosDisponibles(turno.Medico.MedicoId, fechaSeleccionada);

                // Limpiar el DropDownList antes de cargar nuevos datos
                ddlHoraReprogramar.Items.Clear();

                // Cargar los horarios disponibles en el DropDownList
                foreach (var hora in horariosDisponibles)
                {
                    ddlHoraReprogramar.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString()));
                }

                // Habilitar el DropDownList de horas
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
                // Obtener el ID del turno desde el HiddenField
                int turnoId = int.Parse(hiddenTurnoId.Value);

                // Obtener los nuevos valores del modal
                DateTime nuevaFecha = DateTime.Parse(txtFechaReprogramar.Text);
                TimeSpan nuevaHora = TimeSpan.Parse(ddlHoraReprogramar.SelectedValue);
                string nuevaObservacion = txtObservacionReprogramar.Text;

                // Validar que la nueva fecha no sea anterior a la fecha actual
                if (nuevaFecha < DateTime.Today)
                {
                    MostrarError("La fecha seleccionada no puede ser anterior a la fecha actual.");
                    return;
                }

                // Obtener el turno desde la base de datos
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno turno = turnoNegocio.ObtenerTurnoPorId(turnoId);

                if (turno == null)
                {
                    MostrarError("No se pudo encontrar el turno seleccionado.");
                    return;
                }

                // Actualizar el turno
                turno.Fecha = nuevaFecha;
                turno.HoraInicio = nuevaHora;
                turno.Observaciones = nuevaObservacion;
                turno.Estado = EstadoTurno.Reprogramado;

                turnoNegocio.ActualizarTurno(turno);

                // Cerrar el modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#modalReprogramar').modal('hide');", true);

                // Recargar la GridView para reflejar los cambios
                CargarTurnos();

                // Mostrar mensaje de éxito
                MostrarMensaje("Turno reprogramado correctamente.", true);
            }
            catch (Exception ex)
            {
                MostrarError("Error al reprogramar el turno: " + ex.Message);
            }
        }

        private void MostrarMensaje(string mensaje, bool esExito)
        {
            // Asignar el mensaje al Label
            lblMensaje.Text = mensaje;

            // Cambiar el estilo según el tipo de mensaje (éxito o error)
            if (esExito)
            {
                lblMensaje.CssClass = "alert alert-success";
            }
            else
            {
                lblMensaje.CssClass = "alert alert-danger";
            }

            // Hacer visible el Label
            lblMensaje.Visible = true;
        }

        protected void ValidarReprogramacion(object sender, EventArgs e)
        {
            bool esValido = true;

            // Validación de Fecha
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