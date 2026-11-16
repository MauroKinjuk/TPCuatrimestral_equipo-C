using System;
using dominio;
using negocio;

namespace clinica_web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            Usuario usuario = new Usuario();

            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasenia.Text))
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = "Completá usuario y contraseña.";
                    return;
                }

                usuario.NombreUsuario = txtUsuario.Text;
                usuario.Contrasenia = txtContrasenia.Text;

                if (negocio.Loguear(usuario))
                {
                    Session.Add("usuario", usuario);
                    Response.Redirect("Default.aspx", false);
                }
                else
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }
    }
}
