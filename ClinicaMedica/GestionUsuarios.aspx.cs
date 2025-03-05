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
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    Response.End();
                    return;
                }
                if (usuario != null && Seguridad.SesionActiva(usuario) && Seguridad.EsAdmin(usuario))
                {
                    btnGestionEmpleados.Visible = true;
                }
            }
        }

        protected void btnGestionPacientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionPacientes.aspx");
        }
        protected void btnGestionMedicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionMedicos.aspx");
        }
        protected void btnGestionEmpleados_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionEmpleados.aspx");
        }
    }
}
