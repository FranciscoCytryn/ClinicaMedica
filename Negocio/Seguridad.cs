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
            if (usuario != null)
                return true;
            else
                return false;
        }

        public static bool EsAdmin(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            if (usuario.Rol == "Administrador")
                return true;
            else
                return false;
        }

        public static bool EsRecepcionista(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            if (usuario.Rol == "Recepcionista")
                return true;
            else
                return false;
        }

        public static bool EsMedico(object user)
        {
            Usuario usuario = user != null ? (Usuario)user : null;
            if (usuario.Rol == "Médico")
                return true;
            else
                return false;
        }
    }
}
