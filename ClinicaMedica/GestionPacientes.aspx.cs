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
    public partial class GestionPacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Seguridad.SesionActiva(Session["usuario"]))
                {
                    CargarPacientes();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void CargarPacientes()
        {
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            List<Paciente> pacientes = pacienteNegocio.Listar();

            gvPacientes.DataSource = pacientes;
            gvPacientes.DataBind();
        }

        protected void gvPacientes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPacientes.EditIndex = e.NewEditIndex;
            CargarPacientes();
        }

        protected void gvPacientes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPacientes.EditIndex = -1;
            CargarPacientes();
        }

        protected void gvPacientes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int pacienteId = Convert.ToInt32(gvPacientes.DataKeys[e.RowIndex].Value);

            string nombre = (gvPacientes.Rows[e.RowIndex].FindControl("txtNombre") as TextBox).Text;
            string email = (gvPacientes.Rows[e.RowIndex].FindControl("txtEmail") as TextBox).Text;
            string telefono = (gvPacientes.Rows[e.RowIndex].FindControl("txtTelefono") as TextBox).Text;
            string fechaNacimiento = (gvPacientes.Rows[e.RowIndex].FindControl("txtFechaNacimiento") as TextBox).Text;
            string direccion = (gvPacientes.Rows[e.RowIndex].FindControl("txtDireccion") as TextBox).Text;

            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            pacienteNegocio.Actualizar(pacienteId, nombre, email, telefono, DateTime.Parse(fechaNacimiento), direccion);

            gvPacientes.EditIndex = -1;
            CargarPacientes();
        }

        protected void gvPacientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int pacienteId = Convert.ToInt32(gvPacientes.DataKeys[e.RowIndex].Value);

            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            pacienteNegocio.Eliminar(pacienteId);

            CargarPacientes();
        }
    }
}