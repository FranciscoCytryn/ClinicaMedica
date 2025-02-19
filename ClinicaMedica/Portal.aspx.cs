using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace ClinicaMedica
{
    public partial class Portal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Seguridad.SesionActiva(Session["usuario"]))
                {
                    Usuario usuario = (Usuario)Session["usuario"];
                    lblNombreUsuario.Text = usuario.Nombre;
                    ConfigurarBotonesSegunRol(usuario);
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void ConfigurarBotonesSegunRol(Usuario usuario)
        {
            btnGestionUsuarios.Visible = Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario);
            btnGestionTurnos.Visible = Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario);
            btnInformes.Visible = Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario);
            btnTurnosAsignados.Visible = true;
        }

        protected void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionUsuarios.aspx");
        }

        protected void btnGestionTurnos_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionTurnos.aspx");
        }

        protected void btnInformes_Click(object sender, EventArgs e)
        {
            Response.Redirect("Informes.aspx");
        }

        protected void btnTurnosAsignados_Click(object sender, EventArgs e)
        {
            Response.Redirect("TurnosAsignados.aspx");
        }
    }
}
