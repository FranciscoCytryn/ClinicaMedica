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
    public partial class Perfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Seguridad.SesionActiva(Session["usuario"]))
                {
                    Usuario usuario = (Usuario)Session["usuario"];
                    MostrarDatosUsuario(usuario);

                    if (Seguridad.EsMedico(usuario))
                    {
                        MostrarEspecialidades(usuario.UsuarioId);
                        MostrarHorarioTrabajo(usuario.UsuarioId);
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void MostrarDatosUsuario(Usuario usuario)
        {
            lblNombre.Text = usuario.Nombre;
            lblEmail.Text = usuario.Email;
            lblTelefono.Text = usuario.Telefono;
            lblRol.Text = usuario.Rol;
        }

        private void MostrarEspecialidades(int usuarioId)
        {
            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            var especialidades = especialidadNegocio.ListarPorMedico(usuarioId);

            pnlEspecialidades.Visible = true;
            lblEspecialidades.Text = string.Join(", ", especialidades);
        }

        private void MostrarHorarioTrabajo(int usuarioId)
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            List<TurnoTrabajo> turnosTrabajo = medicoNegocio.ObtenerTurnosTrabajoPorMedico(usuarioId);

            if (turnosTrabajo != null && turnosTrabajo.Count > 0)
            {
                pnlHorarioTrabajo.Visible = true;
                lblHorarioTrabajo.Text = string.Join("<br />", turnosTrabajo.Select(t => $"{t.HoraEntrada:hh\\:mm} - {t.HoraSalida:hh\\:mm}"));
            }
        }
    }
}