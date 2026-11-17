using System;
using dominio;

namespace clinica_web
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool esLogin = Request.AppRelativeCurrentExecutionFilePath
                .Equals("~/Login.aspx", StringComparison.OrdinalIgnoreCase);

            if (!esLogin && Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            // Ocultamos el menú de navegación solamente en la pantalla de Login.
            navPrincipal.Visible = !esLogin;

            Usuario usuario = Session["usuario"] as Usuario;
            if (usuario != null)
            {
                liIniciarSesion.Visible = false;
                liUsuario.Visible = true;
                lblNombreUsuario.Text = usuario.NombreUsuario;

                if (usuario.Tipo == TipoUsuario.Recepcionista)
                {
                    liUsuarios.Visible = false;
                    liTurnosTrabajo.Visible = false;
                    liEspecialidades.Visible = false;
                }
                else if (usuario.Tipo == TipoUsuario.Medico)
                {
                    liUsuarios.Visible = false;
                    liTurnosTrabajo.Visible = false;
                    liEspecialidades.Visible = false;
                    liPacientes.Visible = false;
                    liObrasSociales.Visible = false;
                    liMedicos.Visible = false;

                    linkTurnos.HRef = "~/MisTurnos.aspx";
                }
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
