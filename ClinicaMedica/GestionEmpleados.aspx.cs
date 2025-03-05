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
    public partial class GestionEmpleados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    Response.End();
                    return;
                }

                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            EmpleadoNegocio negocio = new EmpleadoNegocio();
            gvEmpleados.DataSource = negocio.ListarEmpleados();
            gvEmpleados.DataBind();
        }

        protected void gvEmpleados_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmpleados.EditIndex = e.NewEditIndex;
            CargarEmpleados();
        }

        protected void gvEmpleados_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmpleados.EditIndex = -1;
            CargarEmpleados();
        }

        protected void gvEmpleados_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                EmpleadoNegocio negocio = new EmpleadoNegocio();
                GridViewRow row = gvEmpleados.Rows[e.RowIndex];

                int usuarioId = Convert.ToInt32(gvEmpleados.DataKeys[e.RowIndex].Value);
                string nombre = (row.FindControl("txtNombre") as TextBox).Text;
                string email = (row.FindControl("txtEmail") as TextBox).Text;
                string telefono = (row.FindControl("txtTelefono") as TextBox).Text;

                Usuario empleado = new Usuario
                {
                    UsuarioId = usuarioId,
                    Nombre = nombre,
                    Email = email,
                    Telefono = telefono
                };

                try
                {
                    negocio.ModificarEmpleado(empleado);
                    gvEmpleados.EditIndex = -1;
                    CargarEmpleados();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", $"alert('Error al modificar empleado: {ex.Message}');", true);
                }
            }
        }

        protected void gvEmpleados_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int usuarioId = Convert.ToInt32(gvEmpleados.DataKeys[e.RowIndex].Value);
            EmpleadoNegocio negocio = new EmpleadoNegocio();

            try
            {
                negocio.EliminarEmpleado(usuarioId);
                CargarEmpleados();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", $"alert('Error al eliminar empleado: {ex.Message}');", true);
            }
        }

        protected void btnAbrirModal_Click(object sender, EventArgs e)
        {
            txtNombreNuevo.Text = string.Empty;
            txtEmailNuevo.Text = string.Empty;
            txtTelefonoNuevo.Text = string.Empty;
            txtPasswordNuevo.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirModal", "$('#modalAltaEmpleado').modal('show');", true);
        }

        protected void btnAltaEmpleado_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                EmpleadoNegocio negocio = new EmpleadoNegocio();

                Usuario nuevoEmpleado = new Usuario
                {
                    Nombre = txtNombreNuevo.Text,
                    Email = txtEmailNuevo.Text,
                    Telefono = txtTelefonoNuevo.Text,
                    Password = txtPasswordNuevo.Text,
                    Rol = "Recepcionista",
                    Activo = true
                };

                try
                {
                    negocio.AgregarEmpleado(nuevoEmpleado);
                    CargarEmpleados(); 
                    ScriptManager.RegisterStartupScript(this, GetType(), "CerrarModal", "$('#modalAltaEmpleado').modal('hide');", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Error", $"alert('Error al agregar empleado: {ex.Message}');", true);
                }
            }
        }
    }
}