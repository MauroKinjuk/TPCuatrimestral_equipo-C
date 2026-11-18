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

            if (!IsPostBack)
            {
                CargarEspecialidades();
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();

            ddlEspecialidad.DataSource = negocio.Listar();
            ddlEspecialidad.DataTextField = "Nombre";
            ddlEspecialidad.DataValueField = "Id";
            ddlEspecialidad.DataBind();

            //Agrega la opción inicial «Seleccione una especialidad.
            ddlEspecialidad.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem(
                    "Seleccione una especialidad...", ""));
        }


        protected void btnAsignar_Click(object sender, EventArgs e)
        {

        }
    }
}