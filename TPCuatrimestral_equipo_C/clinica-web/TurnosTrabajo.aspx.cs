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
    public partial class TurnosTrabajo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
                List<TurnoTrabajo> lista = negocio.Listar();

                dgvTurnosTrabajo.DataSource = lista;
                dgvTurnosTrabajo.DataBind();

            }
        }

        protected void dgvTurnosTrabajo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // verifica que hayamos presionado el botón Editar y no otro botón
            if (e.CommandName == "EditarTurno")
            {
                //obtiene el Id del turno seleccionado, que configuramos anteriormente con Eval("Id")
                int idTurno = Convert.ToInt32(e.CommandArgument);

                //nos lleva al formulario de turnos de trabajo, enviando el identificador por la URL.
                Response.Redirect("FormularioTurnoTrabajo.aspx?id=" + idTurno);
            }


            if (e.CommandName == "EliminarTurno")
            {
                // Obtenemos el ID del horario seleccionado.
                int idTurno = Convert.ToInt32(e.CommandArgument);

                TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();

                // Realizamos la baja lógica (activo = 0).
                negocio.Eliminar(idTurno);

                // Actualizamos el listado para mostrar solo los activos.
                dgvTurnosTrabajo.DataSource = negocio.Listar();
                dgvTurnosTrabajo.DataBind();
            }
        }

    }
}