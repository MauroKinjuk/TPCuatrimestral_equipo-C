using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class HistorialPaciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idPaciente;

                if (int.TryParse(Request.QueryString["id"], out idPaciente))
                {
                    PacienteNegocio negocio = new PacienteNegocio();
                    Paciente paciente = negocio.Obtener(idPaciente);

                    if (paciente != null)
                    {
                        lblNombrePaciente.Text = paciente.NombreCompleto;
                        lblDniPaciente.Text = paciente.Dni;

                        TurnoNegocio turnoNegocio = new TurnoNegocio();
                        dgvHistorial.DataSource = turnoNegocio.ListarPorPaciente(idPaciente);
                        dgvHistorial.DataBind();
                    }
                    else
                    {
                        Response.Redirect("Pacientes.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Pacientes.aspx");
                }
            }
        }
    }
}