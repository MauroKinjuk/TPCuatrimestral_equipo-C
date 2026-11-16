using System;
using dominio;

namespace clinica_web
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ocultamos el menú de navegación solamente en la pantalla de Login.
            navPrincipal.Visible = !Request.AppRelativeCurrentExecutionFilePath
                .Equals("~/Login.aspx", StringComparison.OrdinalIgnoreCase);

            // Si hay un usuario en sesión mostramos su nombre y ocultamos "Iniciar sesión".
            Usuario usuario = Session["usuario"] as Usuario;
            if (usuario != null)
            {
                liIniciarSesion.Visible = false;
                liUsuario.Visible = true;
                lblNombreUsuario.Text = usuario.NombreUsuario;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
