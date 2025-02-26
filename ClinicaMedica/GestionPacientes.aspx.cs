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
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    Response.End(); 
                    return;
                }

                CargarPacientes();
            }
        }

        private void CargarPacientes()
        {
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            List<Paciente> pacientes = pacienteNegocio.Listar();

            gvPacientes.DataSource = pacientes;
            gvPacientes.DataBind();
            lblMensaje.Visible = false;
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
            if (pacienteNegocio.ExisteEmail(email, pacienteId))
            {
                lblMensaje.Text = "El email ya está registrado.";
                lblMensaje.Visible = true;
                return;
            }

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

        protected void btnAltaPaciente_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string nombre = txtNombreNuevo.Text;
                    string email = txtEmailNuevo.Text;
                    string telefono = txtTelefonoNuevo.Text;
                    DateTime fechaNacimiento = DateTime.Parse(txtFechaNacimientoNuevo.Text);
                    string direccion = txtDireccionNuevo.Text;

                    PacienteNegocio pacienteNegocio = new PacienteNegocio();
                    if (pacienteNegocio.ExisteEmail(email))
                    {
                        MostrarMensaje("El email ya está registrado.", true);
                        return;
                    }

                    Paciente nuevoPaciente = new Paciente
                    {
                        Nombre = nombre,
                        Email = email,
                        Telefono = telefono,
                        FechaNacimiento = fechaNacimiento,
                        Direccion = direccion
                    };

                    pacienteNegocio.AltaPaciente(nuevoPaciente);

                    ScriptManager.RegisterStartupScript(this, GetType(), "CerrarModal", "$('#modalAltaPaciente').modal('hide');", true);
                    CargarPacientes();
                    MostrarMensaje("Paciente dado de alta correctamente.", false);
                }
            }
            catch (FormatException ex)
            {
                MostrarMensaje("Formato de fecha inválido. Use el formato dd/MM/yyyy.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al dar de alta al paciente: " + ex.Message, true);
            }
        }

        private void MostrarMensaje(string mensaje, bool esError)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = esError ? "text-danger" : "text-success";
            lblMensaje.Visible = true;
        }
    }
}