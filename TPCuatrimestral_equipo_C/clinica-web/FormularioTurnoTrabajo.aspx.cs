using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using dominio;
using negocio;

namespace clinica_web
{
    public partial class FormularioTurnoTrabajo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MedicoNegocio medicoNegocio = new MedicoNegocio();

                ddlMedicos.DataSource = medicoNegocio.Listar();
                ddlMedicos.DataTextField = "NombreCompleto";
                ddlMedicos.DataValueField = "Id";
                ddlMedicos.DataBind();

                //Esto agrega la opción «Seleccione un médico» en la primera posición del desplegable.
                ddlMedicos.Items.Insert(0, new ListItem("Seleccione un médico", ""));


                // Obtener el ID desde el formulario
                if (Request.QueryString["id"] != null)
                {
                    int idTurno = int.Parse(Request.QueryString["id"]); //obtiene el valor de la URL

                    TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
                    TurnoTrabajo turno = negocio.BuscarPorId(idTurno);


                    // Mostrar los datos del turno que queremos editar
                    if (turno != null)
                    {
                        ddlMedicos.SelectedValue = turno.Medico.Id.ToString();
                        ddlDiaSemana.SelectedValue = turno.DiaSemana;
                        txtHoraInicio.Text = turno.HoraInicio.ToString(@"hh\:mm");
                        txtHoraFin.Text = turno.HoraFin.ToString(@"hh\:mm");// --> formato 08:00, por eje.


                        // Cambia el texto del botón cuando estamos editando
                        btnGuardar.Text = "Modificar turno de trabajo";

                        // Cambia el título cuando estamos modificando un turno.
                        lblTitulo.Text = "Modificar turno de trabajo";
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            // Limpia el mensaje anterior y prepara los errores en rojo si alguna validacion falla
            lblMensaje.Text = "";
            lblMensaje.CssClass = "d-block mt-2 text-danger";

            if (string.IsNullOrEmpty(ddlMedicos.SelectedValue))
            {
                lblMensaje.Text = "Debe seleccionar un médico.";
                return;
            }
            // si el usuario deja seleccionada la opción vacía y presiona Guardar,
            // evitamos que int.Parse() genere un error.
            int idMedico = int.Parse(ddlMedicos.SelectedValue);

            string diaSemana = ddlDiaSemana.SelectedValue;
            string horaInicio = txtHoraInicio.Text;
            string horaFin = txtHoraFin.Text;

            if (string.IsNullOrEmpty(diaSemana))
            {
                lblMensaje.Text = "Debe seleccionar un día de la semana.";
                return;
            }

            if (string.IsNullOrEmpty(horaInicio) || string.IsNullOrEmpty(horaFin))
            {
                lblMensaje.Text = "Debe completar la hora de entrada y salida.";
                return;
            }

            TimeSpan inicio = TimeSpan.Parse(horaInicio);
            TimeSpan fin = TimeSpan.Parse(horaFin);


            if (fin <= inicio)
            {
                lblMensaje.Text = "La hora de salida debe ser posterior a la hora de entrada.";
                return;
            }

            TurnoTrabajo turnoTrabajo = new TurnoTrabajo();

            turnoTrabajo.Medico = new Medico();

            // Si recibimos un ID por la URL, estamos modificando un turno existente.
            if (Request.QueryString["id"] != null)
            {
                turnoTrabajo.Id = int.Parse(Request.QueryString["id"]);
            }

            turnoTrabajo.Medico.Id = idMedico;
            turnoTrabajo.DiaSemana = diaSemana;
            turnoTrabajo.HoraInicio = inicio;
            turnoTrabajo.HoraFin = fin;


            TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();

            if (negocio.ExisteSuperposicion(turnoTrabajo))
            {
                lblMensaje.Text = "El médico ya tiene un horario de trabajo que se superpone con el seleccionado.";
                return;
            }

            // Si el ID es 0, significa que estamos agregando un turno nuevo.
            // Si tiene un ID, significa que estamos modificando uno existente.
            if (turnoTrabajo.Id == 0)
            {
                negocio.Agregar(turnoTrabajo);
                lblMensaje.Text = "El turno de trabajo se guardó correctamente.";
            }
            else
            {
                negocio.Modificar(turnoTrabajo);
                lblMensaje.Text = "El turno de trabajo se modificó correctamente.";
            }


            // cuando se registre correctamente un horario, aparecerá el mensaje en verde debajo del botón Guardar
            lblMensaje.CssClass = "d-block mt-2 text-success";
           
        }
    }
}