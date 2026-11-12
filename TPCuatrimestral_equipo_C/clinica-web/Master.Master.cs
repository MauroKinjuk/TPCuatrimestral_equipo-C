using System;

namespace clinica_web
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ocultamos el menú de navegación solamente en la pantalla de Login.
            navPrincipal.Visible = !Request.AppRelativeCurrentExecutionFilePath
                .Equals("~/Login.aspx", StringComparison.OrdinalIgnoreCase);
        }
    }
}
