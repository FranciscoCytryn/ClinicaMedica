using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;

namespace ClinicaMedica
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Seguridad.SesionActiva(Session["usuario"]))
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            AccesoDatos datos = new AccesoDatos();
            Usuario usuario = datos.ValidarCredenciales(email, password);

            if (usuario != null)
            {
                Session["usuario"] = usuario;
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblMensaje.Text = "Usuario o contraseña incorrecto.";
            }
        }


    }
}