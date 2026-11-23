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
            throw new NotImplementedException();
        }

        private void cargarDvg()
        {
            TurnoMedicoNegocio negocio = new TurnoMedicoNegocio();
            DateTime fecha = DateTime.Parse(txtFecha.Text);

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
                t.Observaciones
            }).ToList();

            dgvTurnos.DataSource = listaMostrar;
            dgvTurnos.DataBind();
        }
    }
}