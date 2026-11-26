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

                // Verifica si se recibió el ID de un turno por la URL.
                if (Request.QueryString["id"] != null)
                {
                    int idTurno;

                    // Verifica que el ID recibido sea un número entero.
                    if (int.TryParse(Request.QueryString["id"], out idTurno))
                    {
                        CargarTurnoParaReprogramar(idTurno);
                    }
                    else
                    {
                        Response.Redirect("TurnosMedicos.aspx");
                    }
                }
            }
        }

        // Recupera los datos del turno que se desea reprogramar
        // y los carga automáticamente en el formulario.
        private void CargarTurnoParaReprogramar(int idTurno)
        {
            TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
            TurnoMedico turno = negocio.BuscarPorId(idTurno);

            // Verifica que el turno exista, esté Nuevo o Reprogramado
            // y que su fecha y hora todavía no hayan llegado.
            if (turno == null ||
                (turno.Estado.Id != 1 && turno.Estado.Id != 2) ||
                turno.Fecha.Date.Add(turno.Hora) <= DateTime.Now)
            {
                Response.Redirect("TurnosMedicos.aspx");
                return;
            }

            // Selecciona el paciente y la especialidad del turno original.
            ddlPaciente.SelectedValue = turno.Paciente.Id.ToString();
            ddlEspecialidad.SelectedValue = turno.Especialidad.Id.ToString();

            // Carga los médicos de la especialidad y selecciona el original.
            CargarMedicos();
            ddlMedico.SelectedValue = turno.Medico.Id.ToString();

            // Recupera las observaciones del turno.
            txtObservaciones.Text = turno.Observaciones;

            // Guarda el ID del turno que estamos reprogramando.
            ViewState["IdTurnoReprogramar"] = turno.Id;

            // Carga los días disponibles del médico.
            CargarDiasDisponibles();

            // Deshabilita los campos que no se pueden modificar
            // al reprogramar un turno.
            ddlPaciente.Enabled = false;
            ddlEspecialidad.Enabled = false;
            ddlMedico.Enabled = false;

            // Cambia el texto del botón para indicar que
            // se está reprogramando un turno existente.
            btnAsignar.Text = "Reprogramar Turno";


            // Cambia el título del formulario.
            tituloFormulario.InnerText = "Reprogramar Turno";
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





        // Carga los tres turnos sugeridos según el paciente
        // y la especialidad seleccionada.
        private void CargarTurnosSugeridos()
        {
            // Oculta el panel inicialmente.
            pnlTurnosSugeridos.Visible = false;

            // Verifica que se haya seleccionado un paciente
            // y una especialidad.
            if (string.IsNullOrEmpty(ddlPaciente.SelectedValue) ||
                string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                return;
            }

            // Obtiene los ID seleccionados.
            int idPaciente = int.Parse(ddlPaciente.SelectedValue);
            int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);

            TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();

            // Obtiene los tres turnos disponibles más próximos.
            List<TurnoMedico> sugerencias = negocio.ListarTurnosSugeridos(idEspecialidad, idPaciente);

            // Prepara los datos que se mostrarán en las tarjetas.
            rptTurnosSugeridos.DataSource = sugerencias.Select(t => new
            {
                Medico = t.Medico.NombreCompleto,
                Fecha = t.Fecha.ToString("dd/MM/yyyy"),
                Hora = t.Hora.ToString(@"hh\:mm"),

                DatosTurno = t.Medico.Id + "|" +
                             t.Fecha.ToString("dd-MM-yyyy") + "|" +
                             t.Hora.ToString(@"hh\:mm")
            }).ToList();

            // Carga los datos en el Repeater.
            rptTurnosSugeridos.DataBind();

            // Muestra el panel si existen turnos sugeridos.
            pnlTurnosSugeridos.Visible = sugerencias.Count > 0;
        }



        // Permite seleccionar uno de los turnos sugeridos.
        protected void rptTurnosSugeridos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarTurno")
            {
                // Separa los datos del turno seleccionado.
                string[] datos = e.CommandArgument.ToString().Split('|');
                // Split() divide esa cadena y guarda cada dato en una posición diferente del vector.


                int idMedico = int.Parse(datos[0]);

                DateTime fecha = DateTime.ParseExact(datos[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

                TimeSpan hora = TimeSpan.Parse(datos[2]);

                // Selecciona automáticamente el médico.
                ddlMedico.SelectedValue = idMedico.ToString();

                // Guarda la fecha y hora seleccionadas.
                ViewState["FechaSeleccionada"] = fecha;
                ViewState["HoraSeleccionada"] = hora;

                // Carga los días y horarios disponibles del médico.
                CargarDiasDisponibles();
                CargarHorariosDisponibles(fecha);
            }
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

            // Limpia la fecha y hora seleccionadas anteriormente.
            ViewState["FechaSeleccionada"] = null;
            ViewState["HoraSeleccionada"] = null;

            // Oculta las sugerencias anteriores.
            pnlTurnosSugeridos.Visible = false;

            // Verifica que este seleccionada una especialidad
            if (string.IsNullOrEmpty(ddlEspecialidad.SelectedValue))
            {
                return;
            }

            CargarMedicos();

            // Carga los turnos sugeridos automáticamente.
            CargarTurnosSugeridos();
        }



        // Actualiza los turnos sugeridos al cambiar de paciente.
        protected void ddlPaciente_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            // Limpia el médico seleccionado anteriormente.
            ddlMedico.ClearSelection();

            // Limpia la fecha y hora seleccionadas.
            ViewState["FechaSeleccionada"] = null;
            ViewState["HoraSeleccionada"] = null;

            // Limpia los días disponibles.
            rptDiasDisponibles.DataSource = null;
            rptDiasDisponibles.DataBind();

            // Limpia los horarios disponibles.
            rptHorariosDisponibles.DataSource = null;
            rptHorariosDisponibles.DataBind();

            // Actualiza los turnos sugeridos.
            CargarTurnosSugeridos();
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
                List<string> errores = Validar();

                if (errores.Count > 0)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = string.Join("<br />", errores);
                    return;
                }

                int idPaciente = int.Parse(ddlPaciente.SelectedValue);
                int idEspecialidad = int.Parse(ddlEspecialidad.SelectedValue);
                int idMedico = int.Parse(ddlMedico.SelectedValue);
                DateTime fecha = (DateTime)ViewState["FechaSeleccionada"];
                TimeSpan hora = TimeSpan.Parse(ViewState["HoraSeleccionada"].ToString());

                TurnoMedico turno = new TurnoMedico();

                turno.Paciente = new Paciente();
                turno.Paciente.Id = idPaciente;

                turno.Especialidad = new Especialidad();
                turno.Especialidad.Id = idEspecialidad;

                turno.Medico = new Medico();
                turno.Medico.Id = idMedico;

                // TODO = Si se pasa un id por url para reprogramar el turno se tiene que guardar ese estado
                turno.Estado = new EstadoTurno();
                turno.Estado.Id = 1; // Id nuevo turno

                turno.Fecha = fecha;
                turno.Hora = hora;
                turno.Observaciones = txtObservaciones.Text;

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();

                // Verifica si se está reprogramando un turno existente.
                if (ViewState["IdTurnoReprogramar"] != null)
                {
                    // Recupera el ID del turno original.
                    turno.Id = Convert.ToInt32(ViewState["IdTurnoReprogramar"]);

                    // Cambia el estado a Reprogramado.
                    turno.Estado.Id = 2;

                    // Actualiza el turno existente en la base de datos.
                    negocio.ReprogramarTurno(turno);
                }
                else
                {
                    // Si no se está reprogramando, registra un turno nuevo.
                    negocio.Agregar(turno);
                }

                // Regresa al listado de turnos médicos.
                Response.Redirect("TurnosMedicos.aspx");
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        private List<string> Validar()
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

            if (!string.IsNullOrWhiteSpace(ddlPaciente.SelectedValue) &&
                ViewState["FechaSeleccionada"] != null &&
                ViewState["HoraSeleccionada"] != null)
            {
                int idPaciente = int.Parse(ddlPaciente.SelectedValue);
                DateTime fecha = (DateTime)ViewState["FechaSeleccionada"];
                TimeSpan hora = TimeSpan.Parse(ViewState["HoraSeleccionada"].ToString());

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();

                if (negocio.ExisteTurnoPaciente(idPaciente, fecha, hora))
                {
                    errores.Add("El paciente ya tiene un turno asignado en esa fecha y horario.");
                }
            }

            if (!string.IsNullOrWhiteSpace(ddlMedico.SelectedValue) &&
                ViewState["FechaSeleccionada"] != null &&
                ViewState["HoraSeleccionada"] != null)
            {
                int idMedico = int.Parse(ddlMedico.SelectedValue);
                DateTime fecha = (DateTime)ViewState["FechaSeleccionada"];
                TimeSpan hora = TimeSpan.Parse(ViewState["HoraSeleccionada"].ToString());

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();

                if (negocio.ExisteTurnoMedico(idMedico, fecha, hora))
                {
                    errores.Add("El médico ya tiene un turno asignado en esa fecha y horario.");
                }
            }

            // Verifica que la fecha y hora seleccionadas sean futuras.
            // Recupera la fecha y la hora que el usuario seleccionó
            // previamente y que se guardaron en el ViewState.
            // Luego verifica que el turno no corresponda a un horario pasado.
            if (ViewState["FechaSeleccionada"] != null &&
                ViewState["HoraSeleccionada"] != null)
            {
                DateTime fecha = (DateTime)ViewState["FechaSeleccionada"];
                TimeSpan hora = TimeSpan.Parse(
                    ViewState["HoraSeleccionada"].ToString());

                if (fecha.Date.Add(hora) <= DateTime.Now)
                {
                    errores.Add("La fecha y hora del turno deben ser futuras.");
                }
            }

            return errores;
        }
    }
}