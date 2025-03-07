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
    public partial class TurnosAsignados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["usuario"] as Usuario;

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !Seguridad.EsMedico(usuario))
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                CargarTurnosAsignados(usuario.UsuarioId);
            }
        }

        private void CargarTurnosAsignados(int idMedico)
        {
            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                List<Turno> turnosAsignados = turnoNegocio.ListarTurnosPorMedico(idMedico);

                gvTurnosAsignados.DataSource = turnosAsignados;
                gvTurnosAsignados.DataBind();
            }
            catch (Exception ex)
            {
                lblMensajeError.Text = "Ocurrió un error al cargar los turnos: " + ex.Message;
                lblMensajeError.Visible = true;
            }
        }
    }
}