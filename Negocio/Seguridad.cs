using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public static class Seguridad
    {
        public static bool SesionActiva(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            return usuario != null;
        }

        public static bool EsAdmin(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            return usuario != null && usuario.Rol == "Administrador";
        }

        public static bool EsRecepcionista(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            return usuario != null && usuario.Rol == "Recepcionista";
        }

        public static bool EsMedico(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            return usuario != null && usuario.Rol == "Médico";
        }
    }
}
