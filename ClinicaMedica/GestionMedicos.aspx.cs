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
    public partial class GestionMedicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    Response.End();
                    return;
                }

                CargarMedicos();
            }
        }

        private void CargarMedicos()
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            List<Medico> medicos = medicoNegocio.Listar();

            foreach (var medico in medicos)
            {
                if (medico.TurnosTrabajo != null && medico.TurnosTrabajo.Count > 0)
                {
                    medico.HoraEntrada = medico.TurnosTrabajo[0].HoraEntrada;
                    medico.HoraSalida = medico.TurnosTrabajo[0].HoraSalida;
                }
                else
                {
                    medico.HoraEntrada = TimeSpan.Zero;
                    medico.HoraSalida = TimeSpan.Zero;
                }
            }

            gvMedicos.DataSource = medicos;
            gvMedicos.DataBind();
        }

        protected string ObtenerEspecialidadesPorMedico(List<Dominio.Especialidad> especialidades)
        {
            return string.Join(", ", especialidades.Select(e => e.Nombre));
        }


        protected void gvMedicos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMedicos.EditIndex = e.NewEditIndex;
            CargarMedicos();

            GridViewRow row = gvMedicos.Rows[e.NewEditIndex];
            CheckBoxList cblEspecialidades = (CheckBoxList)row.FindControl("cblEspecialidades");

            if (cblEspecialidades != null)
            {
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                List<Dominio.Especialidad> todasEspecialidades = especialidadNegocio.Listar();

                cblEspecialidades.DataSource = todasEspecialidades;
                cblEspecialidades.DataTextField = "Nombre";
                cblEspecialidades.DataValueField = "EspecialidadId";
                cblEspecialidades.DataBind();

                int medicoId = Convert.ToInt32(gvMedicos.DataKeys[e.NewEditIndex].Value);

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                List<Dominio.Especialidad> especialidadesMedico = medicoNegocio.ObtenerEspecialidadesPorMedico(medicoId);

                foreach (Dominio.Especialidad especialidad in especialidadesMedico)
                {
                    ListItem item = cblEspecialidades.Items.FindByValue(especialidad.EspecialidadId.ToString());
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void gvMedicos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMedicos.EditIndex = -1;
            CargarMedicos();
        }

        protected void gvMedicos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int medicoId = Convert.ToInt32(gvMedicos.DataKeys[e.RowIndex].Value);

                GridViewRow row = gvMedicos.Rows[e.RowIndex];
                string nombre = ((TextBox)row.FindControl("txtNombre")).Text;
                string email = ((TextBox)row.FindControl("txtEmail")).Text;
                string telefono = ((TextBox)row.FindControl("txtTelefono")).Text;

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                medicoNegocio.EditarMedico(medicoId, nombre, email, telefono);

                CheckBoxList cblEspecialidades = (CheckBoxList)row.FindControl("cblEspecialidades");
                List<Dominio.Especialidad> especialidadesSeleccionadas = new List<Dominio.Especialidad>();
                foreach (ListItem item in cblEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        especialidadesSeleccionadas.Add(new Dominio.Especialidad
                        {
                            EspecialidadId = int.Parse(item.Value),
                            Nombre = item.Text
                        });
                    }
                }
                medicoNegocio.ActualizarEspecialidadesMedico(medicoId, especialidadesSeleccionadas);

                TextBox txtHoraEntrada = (TextBox)row.FindControl("txtHoraEntrada");
                TextBox txtHoraSalida = (TextBox)row.FindControl("txtHoraSalida");

                if (txtHoraEntrada != null && txtHoraSalida != null &&
                    !string.IsNullOrEmpty(txtHoraEntrada.Text) && !string.IsNullOrEmpty(txtHoraSalida.Text))
                {
                    TimeSpan horaEntrada = TimeSpan.Parse(txtHoraEntrada.Text);
                    TimeSpan horaSalida = TimeSpan.Parse(txtHoraSalida.Text);

                    if (horaEntrada >= horaSalida)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showerror", "alert('La hora de entrada debe ser menor que la hora de salida.');", true);
                        return;
                    }

                    ActualizarTurnoTrabajo(medicoId, horaEntrada, horaSalida);
                }

                gvMedicos.EditIndex = -1;
                CargarMedicos();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showerror", $"alert('Error al actualizar el médico: {ex.Message}');", true);
            }
        }

        protected void gvMedicos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int medicoId = Convert.ToInt32(gvMedicos.DataKeys[e.RowIndex].Value);

            MedicoNegocio medicoNegocio = new MedicoNegocio();
            medicoNegocio.BajaMedico(medicoId);

            CargarMedicos();
        }

        private void LimpiarFormulario()
        {
            txtNombreNuevo.Text = string.Empty;
            txtEmailNuevo.Text = string.Empty;
            txtTelefonoNuevo.Text = string.Empty;
            txtPasswordNuevo.Text = string.Empty;

            foreach (ListItem item in cblEspecialidadesNuevo.Items)
            {
                item.Selected = false;
            }
        }

        protected void btnAltaMedico_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario())
                {
                    return; 
                }

                string nombre = txtNombreNuevo.Text;
                string email = txtEmailNuevo.Text;
                string telefono = txtTelefonoNuevo.Text;
                string password = txtPasswordNuevo.Text;

                List<Dominio.Especialidad> especialidades = new List<Dominio.Especialidad>();
                foreach (ListItem item in cblEspecialidadesNuevo.Items)
                {
                    if (item.Selected)
                    {
                        especialidades.Add(new Dominio.Especialidad
                        {
                            EspecialidadId = int.Parse(item.Value),
                            Nombre = item.Text
                        });
                    }
                }

                List<TurnoTrabajo> turnosTrabajo = new List<TurnoTrabajo>();
                if (!string.IsNullOrEmpty(txtHoraEntradaNuevo.Text) && !string.IsNullOrEmpty(txtHoraSalidaNuevo.Text))
                {
                    TimeSpan horaEntrada = TimeSpan.Parse(txtHoraEntradaNuevo.Text);
                    TimeSpan horaSalida = TimeSpan.Parse(txtHoraSalidaNuevo.Text);

                    turnosTrabajo.Add(new TurnoTrabajo
                    {
                        HoraEntrada = horaEntrada,
                        HoraSalida = horaSalida
                    });
                }

                Medico nuevoMedico = new Medico
                {
                    Usuario = new Usuario
                    {
                        Nombre = nombre,
                        Email = email,
                        Telefono = telefono,
                        Password = password,
                        Rol = "Médico",
                        Activo = true
                    },
                    Especialidades = especialidades,
                    TurnosTrabajo = turnosTrabajo 
                };

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                medicoNegocio.AltaMedico(nuevoMedico);

                EmailService emailService = new EmailService();
                string asunto = emailService.CrearAsuntoAltaMedico();
                string especialidadesFormateadas = string.Join(", ", especialidades);
                string cuerpo = emailService.CrearTemplateAltaMedico(nombre, email, especialidadesFormateadas, txtHoraEntradaNuevo.Text, txtHoraSalidaNuevo.Text);
                emailService.EnviarCorreo(email, asunto, cuerpo);

                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Médico dado de alta correctamente.');", true);

                LimpiarFormulario();
                CargarMedicos();

                ScriptManager.RegisterStartupScript(this, GetType(), "cerrarModal", "$('#modalAltaMedico').modal('hide');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showerror", $"alert('Error al dar de alta el médico: {ex.Message}');", true);
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrEmpty(txtNombreNuevo.Text) ||
                string.IsNullOrEmpty(txtEmailNuevo.Text) ||
                string.IsNullOrEmpty(txtTelefonoNuevo.Text) ||
                string.IsNullOrEmpty(txtPasswordNuevo.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showerror", "alert('Todos los campos son obligatorios.');", true);
                return false;
            }

            if (!string.IsNullOrEmpty(txtHoraEntradaNuevo.Text) || !string.IsNullOrEmpty(txtHoraSalidaNuevo.Text))
            {
                if (!TimeSpan.TryParse(txtHoraEntradaNuevo.Text, out TimeSpan horaEntrada) ||
                    !TimeSpan.TryParse(txtHoraSalidaNuevo.Text, out TimeSpan horaSalida))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showerror", "alert('Formato de hora inválido. Use HH:mm.');", true);
                    return false;
                }

                if (horaEntrada >= horaSalida)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showerror", "alert('La hora de entrada debe ser menor que la hora de salida.');", true);
                    return false;
                }
            }

            return true; 
        }

        protected void cvEspecialidades_ServerValidate(object source, ServerValidateEventArgs args)
        {
            GridViewRow row = gvMedicos.Rows[gvMedicos.EditIndex];
            CheckBoxList cblEspecialidades = (CheckBoxList)row.FindControl("cblEspecialidades");

            bool alMenosUnaSeleccionada = false;
            foreach (ListItem item in cblEspecialidades.Items)
            {
                if (item.Selected)
                {
                    alMenosUnaSeleccionada = true;
                    break;
                }
            }

            args.IsValid = alMenosUnaSeleccionada;
        }

        private void CargarEspecialidadesEnModal()
        {
            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            List<Dominio.Especialidad> especialidades = especialidadNegocio.Listar();

            cblEspecialidadesNuevo.DataSource = especialidades;
            cblEspecialidadesNuevo.DataTextField = "Nombre";
            cblEspecialidadesNuevo.DataValueField = "EspecialidadId";
            cblEspecialidadesNuevo.DataBind();
        }

        protected void btnAbrirModal_Click(object sender, EventArgs e)
        {
            CargarEspecialidadesEnModal();
            ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModal", "$('#modalAltaMedico').modal('show');", true);
        }

        protected string ObtenerTurnosTrabajoFormateados(List<TurnoTrabajo> turnosTrabajo)
        {
            if (turnosTrabajo == null || turnosTrabajo.Count == 0)
                return "Sin turnos asignados";

            var turnosFormateados = turnosTrabajo
                .Select(t => $"{t.HoraEntrada:hh\\:mm} - {t.HoraSalida:hh\\:mm}")
                .ToList();

            return string.Join("<br />", turnosFormateados);
        }
        private void ActualizarTurnoTrabajo(int medicoId, TimeSpan horaEntrada, TimeSpan horaSalida)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ActualizarTurnoTrabajo");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@HoraEntrada", horaEntrada);
                datos.SetearParametro("@HoraSalida", horaSalida);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el turno de trabajo", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
