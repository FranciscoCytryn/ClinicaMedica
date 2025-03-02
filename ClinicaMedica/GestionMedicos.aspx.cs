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
                CheckBoxList cblEspecialidades = (CheckBoxList)gvMedicos.Rows[e.RowIndex].FindControl("cblEspecialidades");

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

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                medicoNegocio.ActualizarEspecialidadesMedico(medicoId, especialidadesSeleccionadas);

                gvMedicos.EditIndex = -1;

                CargarMedicos();
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "showerror", $"alert('Error al actualizar las especialidades: {ex.Message}');", true);
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

                // Crear el objeto Medico
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
                    Especialidades = especialidades
                };

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                medicoNegocio.AltaMedico(nuevoMedico);

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

        protected void gvMedicos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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
    }
}
