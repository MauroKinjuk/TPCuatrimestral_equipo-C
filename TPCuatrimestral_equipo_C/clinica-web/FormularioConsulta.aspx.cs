using dominio;
using negocio;
using System;

namespace clinica_web
{
    public partial class FormularioConsulta : System.Web.UI.Page
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
                string id = Request.QueryString["id"];
                if (string.IsNullOrEmpty(id))
                {
                    Response.Redirect("~/MisTurnos.aspx", false);
                    return;
                }

                cargarTurno(int.Parse(id));
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultaNegocio negocio = new ConsultaNegocio();
                negocio.ModificarDiagnostico(int.Parse(hfIdTurno.Value), txtDiagnostico.Text.Trim());
                Response.Redirect("~/MisTurnos.aspx", false);
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        private void cargarTurno(int idTurno)
        {
            Usuario usuario = (Usuario)Session["usuario"];
            TurnoNegocio negocio = new TurnoNegocio();
            TurnoMedico turno = negocio.BuscarPorId(idTurno);

            // El medico solo puede ver sus propios turnos
            if (turno == null || usuario.Medico == null || turno.Medico.Id != usuario.Medico.Id)
            {
                Response.Redirect("~/MisTurnos.aspx", false);
                return;
            }

            hfIdTurno.Value = turno.Id.ToString();
            txtPaciente.Text = turno.Paciente.Nombre + " " + turno.Paciente.Apellido;
            txtFecha.Text = turno.Fecha.ToString("dd/MM/yyyy");
            txtHora.Text = turno.Hora.ToString(@"hh\:mm");

            if (turno.Consulta != null)
            {
                txtMotivo.Text = turno.Consulta.Motivo;
                txtDiagnostico.Text = turno.Consulta.Diagnostico;
            }
        }
    }
}
