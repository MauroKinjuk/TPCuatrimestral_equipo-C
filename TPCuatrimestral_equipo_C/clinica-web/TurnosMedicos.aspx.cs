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
    public partial class TurnosMedicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                cargarDvg();
            }
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            cargarDvg();
        }

        protected void btnNuevo_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("FormularioTurnoMedico.aspx");
        }


        protected void dgvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancelar")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
                negocio.CancelarTurno(idTurno);

                cargarDvg();
            }

            else if (e.CommandName == "NoAsistio")
            {
                int idTurno = Convert.ToInt32(e.CommandArgument);

                TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
                negocio.MarcarNoAsistio(idTurno);

                cargarDvg();
            }

            else if (e.CommandName == "Reprogramar")
            {
                // Obtiene el ID del turno seleccionado.
                int idTurno = Convert.ToInt32(e.CommandArgument);

                // Abre el formulario de turnos enviando el ID
                // del turno que se desea reprogramar.
                Response.Redirect("FormularioTurnoMedico.aspx?id=" + idTurno);
            }

        }


        // Obtiene los turnos de la fecha seleccionada y los muestra
        // en la tabla. También determina qué acciones se pueden
        // realizar según el estado, la fecha y la hora de cada turno.
        private void cargarDvg()
        {
            TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
            DateTime fecha;

            if (!DateTime.TryParse(txtFecha.Text, out fecha))
            {
                fecha = DateTime.Today;
                txtFecha.Text = fecha.ToString("yyyy-MM-dd");
            }

            List<TurnoMedico> turnos = negocio.ListarPorFecha(fecha.Date);

            var listaMostrar = turnos.Select(t => new
            {
                t.Id,
                t.Fecha,
                t.Hora,
                Paciente = t.Paciente.NombreCompleto,
                Especialidad = t.Especialidad.Nombre,
                Medico = t.Medico.NombreCompleto,
                Estado = t.Estado.Nombre,
                t.Observaciones,

                // Permite cancelar un turno si su estado es Nuevo o Reprogramado
                // y todavía no llegó la fecha y hora del turno
                PuedeCancelar = (t.Estado.Id == 1 || t.Estado.Id == 2)
                     && t.Fecha.Date.Add(t.Hora) > DateTime.Now,


                // Permite marcar al paciente como ausente si el turno está
                // Nuevo o Reprogramado y su fecha y hora ya pasaron
                PuedeMarcarNoAsistio = (t.Estado.Id == 1 || t.Estado.Id == 2)
                          && t.Fecha.Date.Add(t.Hora) < DateTime.Now   //--> Obtiene la fecha y hora actuales 

            }).ToList();

            dgvTurnos.DataSource = listaMostrar;
            dgvTurnos.DataBind();
        }
    }
}