
using dominio;
using negocio;
using System;
using System.Collections.Generic;

namespace clinica_web
{
    public partial class Pacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioPaciente.aspx");
        }

        protected void dgvPacientes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Editar")
            {
                Response.Redirect("FormularioPaciente.aspx?id=" + id);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    PacienteNegocio negocio = new PacienteNegocio();
                    negocio.Eliminar(id);
                    cargarGrilla();
                }
                catch (Exception ex)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
        }

        private void cargarGrilla()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            string texto = txtBuscar.Text.Trim();

            List<Paciente> lista = string.IsNullOrEmpty(texto)
                ? negocio.Listar()
                : negocio.Filtrar(texto);

            dgvPacientes.DataSource = lista;
            dgvPacientes.DataBind();
        }
    }
}
