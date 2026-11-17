using dominio;
using negocio;
using System;

namespace clinica_web
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.TieneAcceso(Session["usuario"], TipoUsuario.Administrador, TipoUsuario.Recepcionista))
            {
                Response.Redirect("~/Default.aspx", false);
                return;
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {

        }
    }
}