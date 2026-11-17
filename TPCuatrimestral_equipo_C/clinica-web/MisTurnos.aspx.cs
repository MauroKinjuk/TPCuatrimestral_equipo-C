using dominio;
using negocio;
using System;

namespace clinica_web
{
    public partial class MisTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.EsMedico(Session["usuario"]))
            {
                Response.Redirect("~/Default.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                dgvTurnos.DataBind();
            }
        }
    }
}
