using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

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

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpia los medicos anteriores
            ddlMedico.Items.Clear();

            // Limpia los dias
            rptDiasDisponibles.DataSource = null;
            rptDiasDisponibles.DataBind();

            // Limpia los horarios
            rptHorariosDisponibles.DataSource = null;
            rptHorariosDisponibles.DataBind();

            // Verifica que este seleccionada una especialidad
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                return;
            }

            CargarMedicos();
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Limpia los dias
            rptDiasDisponibles.DataSource = null;
            rptDiasDisponibles.DataBind();

            // Limpia los horarios
            rptHorariosDisponibles.DataSource = null;
            rptHorariosDisponibles.DataBind();

            if (string.IsNullOrEmpty(ddlMedico.SelectedValue))
            {
                return;
            }

            CargarDiasDisponibles();
        }

        private void CargarDiasDisponibles()
        {
            int idMedico = int.Parse(ddlMedico.SelectedValue);

            TurnoTrabajoNegocio negocioTrabajo = new TurnoTrabajoNegocio();
            TurnoMedicoNegocio negocioTurnos = new TurnoMedicoNegocio();

            List<TurnoTrabajo> horariosTrabajo = negocioTrabajo.ListarPorMedico(idMedico);
            List<TurnoMedico> turnosMedico = negocioTurnos.ListarPorMedico(idMedico);

            List<object> diasDisponibles = new List<object>();

            DateTime hoy = DateTime.Today;

            // Recorre los próximos 30 días, incluyendo hoy
            for (int i = 0; i < 30; i++)
            {
                DateTime fecha = hoy.AddDays(i);

                List<TimeSpan> horarios = negocioTurnos.ListarHorariosDisponibles(
                    fecha,
                    horariosTrabajo,
                    turnosMedico);

                if (horarios.Count > 0)
                {
                    diasDisponibles.Add(new
                    {
                        Texto = fecha.ToString("ddd dd", new CultureInfo("es-AR")).ToUpper(),
                        Fecha = fecha.ToString("dd-MM-yyyy")
                    });
                }
            }

            // Carga las fechas disponibles en el Repeater
            rptDiasDisponibles.DataSource = diasDisponibles;
            rptDiasDisponibles.DataBind();
        }

        protected void rptDiasDisponibles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarDia")
            {
                DateTime fechaSeleccionada = DateTime.ParseExact(
                    e.CommandArgument.ToString(),
                    "dd-MM-yyyy",
                    CultureInfo.InvariantCulture);

                ViewState["FechaSeleccionada"] = fechaSeleccionada;

                ViewState["HoraSeleccionada"] = null;

                CargarDiasDisponibles();
                CargarHorariosDisponibles(fechaSeleccionada);
            }
        }

        private void CargarHorariosDisponibles(DateTime fecha)
        {
            int idMedico = int.Parse(ddlMedico.SelectedValue);

            TurnoTrabajoNegocio negocioTrabajo = new TurnoTrabajoNegocio();
            TurnoMedicoNegocio negocioTurnos = new TurnoMedicoNegocio();

            List<TurnoTrabajo> horariosTrabajo = negocioTrabajo.ListarPorMedico(idMedico);
            List<TurnoMedico> turnosMedico = negocioTurnos.ListarPorMedico(idMedico);

            // Obtiene los horarios de trabajo correspondientes al día seleccionado
            List<TimeSpan> horariosDisponibles = negocioTurnos.ListarHorariosDisponibles(
                fecha,
                horariosTrabajo,
                turnosMedico);

            List<object> listaMostrar = new List<object>();

            foreach (TimeSpan hora in horariosDisponibles)
            {
                listaMostrar.Add(new
                {
                    Texto = hora.ToString(@"hh\:mm"),
                    Hora = hora
                });
            }

            rptHorariosDisponibles.DataSource = listaMostrar;
            rptHorariosDisponibles.DataBind();
        }

        protected void rptHorariosDisponibles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarHorario")
            {
                ViewState["HoraSeleccionada"] = e.CommandArgument.ToString();

                DateTime fechaSeleccionada = (DateTime)ViewState["FechaSeleccionada"];

                CargarHorariosDisponibles(fechaSeleccionada);
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> errores = validar();

                if (errores.Count > 0)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = string.Join("<br />", errores);
                    return;
                }

                int idPaciente = int.Parse(ddlPaciente.SelectedValue);
                int idMedico = int.Parse(ddlMedico.SelectedValue);
                DateTime fecha = (DateTime)ViewState["FechaSeleccionada"];
                TimeSpan hora = TimeSpan.Parse(ViewState["HoraSeleccionada"].ToString());

                TurnoMedico turno = new TurnoMedico();

                turno.Paciente = new Paciente();
                turno.Paciente.Id = idPaciente;

                turno.Medico = new Medico();
                turno.Medico.Id = idMedico;

                turno.Estado = new EstadoTurno();
                turno.Estado.Id = 1; // Id nuevo turno

                turno.Fecha = fecha;
                turno.Hora = hora;
                turno.Observaciones = txtObservaciones.Text;

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
                negocio.Agregar(turno);

                /*
                 * TODO = hacer el redirect a la vista con la lista de turnos.
                 * Actualmente solo recarga el formulario.
                 */
                Response.Redirect("FormularioTurno.aspx");
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        private List<string> validar()
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(ddlPaciente.SelectedValue))
            {
                errores.Add("Debe seleccionar un paciente.");
            }

            if (string.IsNullOrWhiteSpace(ddlEspecialidad.SelectedValue))
            {
                errores.Add("Debe seleccionar una especialidad");
            }

            if (string.IsNullOrWhiteSpace(ddlMedico.SelectedValue))
            {
                errores.Add("Debe seleccionar un médico.");
            }

            if (ViewState["FechaSeleccionada"] == null)
            {
                errores.Add("Debe seleccionar una fecha.");
            }

            if (ViewState["HoraSeleccionada"] == null)
            {
                errores.Add("Debe seleccionar un horario.");
            }

            return errores;
        }
    }
}