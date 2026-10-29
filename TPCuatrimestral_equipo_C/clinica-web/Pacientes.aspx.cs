
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace clinica_web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PacienteNegocio negocio = new PacienteNegocio();

                List<Paciente> lista = negocio.Listar();

                dgvPacientes.DataSource = lista;
                dgvPacientes.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}
