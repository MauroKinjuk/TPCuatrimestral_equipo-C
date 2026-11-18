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
                CargarPacientes();
                CargarEspecialidades();
            }
        }

        private void CargarPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();

            ddlPaciente.DataSource = negocio.Listar();
            ddlPaciente.DataTextField = "NombreCompleto";
            ddlPaciente.DataValueField = "Id";
            ddlPaciente.DataBind();

            ddlPaciente.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem(
                    "Seleccione un paciente...", ""));
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

        private void CargarMedicos()
        {
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
            MedicoNegocio negocio = new MedicoNegocio();

            ddlMedico.DataSource = negocio.ListarPorEspecialidad(idEspecialidad);
            ddlMedico.DataTextField = "NombreCompleto";
            ddlMedico.DataValueField = "Id";
            ddlMedico.DataBind();

            ddlMedico.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem(
                    "Seleccione un Medico...", ""));
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpia los medicos anteriores y sus horarios
            ddlMedico.Items.Clear();
            pnlHorariosDisponibles.Controls.Clear();

            // Verifica que este seleccionada una especialidad
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                return;
            }

            CargarMedicos();
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {

        }
    }
}