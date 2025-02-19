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
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!IsLoginPage())
                {
                    if (!Seguridad.SesionActiva(Session["usuario"]))
                    {
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        Usuario usuario = (Usuario)Session["usuario"];
                        lblUsuario.Text = $"{usuario.Nombre}";
                    }
                }

            }
        }

        private bool IsLoginPage()
        {
            string paginaActual = Request.Url.AbsolutePath;
            return paginaActual.EndsWith("/Login", StringComparison.OrdinalIgnoreCase) ||
                   paginaActual.EndsWith("/Login.aspx", StringComparison.OrdinalIgnoreCase);
        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Portal.aspx");
        }
    }
}