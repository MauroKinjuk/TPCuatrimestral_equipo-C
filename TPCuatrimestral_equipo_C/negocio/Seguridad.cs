using dominio;
using System.Linq;

namespace negocio
{
    public static class Seguridad
    {
        public static bool SesionActiva(object session)
        {
            Usuario usuario = session as Usuario;
            return usuario != null && usuario.Id != 0;
        }

        public static bool EsAdmin(object session)
        {
            Usuario usuario = session as Usuario;
            return usuario != null && usuario.Tipo == TipoUsuario.Administrador;
        }

        public static bool EsRecepcionista(object session)
        {
            Usuario usuario = session as Usuario;
            return usuario != null && usuario.Tipo == TipoUsuario.Recepcionista;
        }

        public static bool EsMedico(object session)
        {
            Usuario usuario = session as Usuario;
            return usuario != null && usuario.Tipo == TipoUsuario.Medico;
        }

        public static bool TieneAcceso(object session, params TipoUsuario[] tiposPermitidos)
        {
            Usuario usuario = session as Usuario;
            return usuario != null && tiposPermitidos.Contains(usuario.Tipo);
        }
    }
}
