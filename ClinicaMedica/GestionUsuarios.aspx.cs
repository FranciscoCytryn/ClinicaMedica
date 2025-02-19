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
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Seguridad.SesionActiva(Session["usuario"]))
                {
                    Usuario usuario = (Usuario)Session["usuario"];
                    if (!Seguridad.EsAdmin(usuario) && !Seguridad.EsRecepcionista(usuario))
                    {
                        Response.Redirect("Portal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected void btnGestionPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionPacientes.aspx");
        }
    }
}
