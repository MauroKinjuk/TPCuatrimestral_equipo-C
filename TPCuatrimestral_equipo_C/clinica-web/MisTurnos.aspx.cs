using dominio;
using negocio;
using System;
using System.Web.UI.WebControls;

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
                Usuario usuario = (Usuario)Session["usuario"];

                if (usuario.Medico != null)
                {
                    TurnoNegocio negocio = new TurnoNegocio();
                    dgvTurnos.DataSource = negocio.ListarPorMedico(usuario.Medico.Id);
                }

                dgvTurnos.DataBind();
            }
        }

        protected void dgvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Diagnostico")
            {
                Response.Redirect("FormularioConsulta.aspx?id=" + e.CommandArgument);
            }
        }
    }
}
