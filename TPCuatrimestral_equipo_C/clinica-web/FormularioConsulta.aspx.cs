using dominio;
using negocio;
using System;
using System.Collections.Generic;

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
                if (string.IsNullOrWhiteSpace(txtDiagnostico.Text))
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = "Debe cargar un diagnóstico.";
                    return;
                }

                Usuario usuario = (Usuario)Session["usuario"];
                ConsultaNegocio negocio = new ConsultaNegocio();
                negocio.ModificarDiagnostico(
                    int.Parse(hfIdTurno.Value),
                    usuario.Medico.Id,
                    txtDiagnostico.Text.Trim());
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

            // El medico solo puede cargar diagnóstico en sus turnos vigentes
            if (turno == null || usuario.Medico == null || turno.Medico.Id != usuario.Medico.Id ||
                (turno.Estado.Id != 1 && turno.Estado.Id != 2))
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

            // Cargamos el historial clínico del paciente
            cargarHistorialPaciente(turno);
        }


        private void cargarHistorialPaciente(TurnoMedico turnoActual)
        {
            TurnoNegocio negocio = new TurnoNegocio();

            // Obtenemos todos los turnos del pacient
            List<TurnoMedico> historial =
                negocio.ListarPorPaciente(turnoActual.Paciente.Id);

            List<TurnoMedico> consultasAnteriores = new List<TurnoMedico>();

            // Recorremos los turnos del paciente
            foreach (TurnoMedico turno in historial)
            {
                // Verificamos que sea una consulta anterior y cerrada
                if (turno.Id != turnoActual.Id && turno.Estado.Id == 5 && turno.Consulta != null &&
                    turno.Fecha.Date.Add(turno.Hora) < turnoActual.Fecha.Date.Add(turnoActual.Hora))
                {
                    consultasAnteriores.Add(turno);
                }
            }

            // Mostramos las consultas anteriores en la tabla
            dgvHistorialPaciente.DataSource = consultasAnteriores;
            dgvHistorialPaciente.DataBind();
        }
    }
}
