using dominio;
using negocio;
using System;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si es un usuario no logeado lo retornamos a la pantalla de login
            if (!Seguridad.SesionActiva(Session["usuario"]))
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarInicio();
            }
        }

        private void CargarInicio()
        {
            Usuario usuario = (Usuario)Session["usuario"];

            lblSaludo.Text = "¡Hola, " + usuario.NombreUsuario + "!";
            lblTipoUsuario.Text = usuario.Tipo.ToString();

            // Depende del tipo de usuario hacemos visible su correspondiente panel y lo cargamos
            if (Seguridad.EsAdmin(usuario))
            {
                pnlAdministrador.Visible = true;
                CargarAdministrador();
            }
            else if (Seguridad.EsRecepcionista(usuario))
            {
                pnlRecepcionista.Visible = true;
                CargarRecepcionista();
            }
            else if (Seguridad.EsMedico(usuario))
            {
                pnlMedico.Visible = true;
            }
        }

        private void CargarAdministrador()
        {
            PacienteNegocio negocioPacientes = new PacienteNegocio();
            MedicoNegocio negocioMedicos = new MedicoNegocio();
            UsuarioNegocio negocioUsuarios = new UsuarioNegocio();
            ObraSocialNegocio negocioObrasSociales = new ObraSocialNegocio();
            EspecialidadNegocio negocioEspecialidades = new EspecialidadNegocio();
            TurnoMedicoNegocio negocioTurnos = new TurnoMedicoNegocio();

            lblCantidadPacientes.Text = negocioPacientes.Contar().ToString();
            lblCantidadMedicos.Text = negocioMedicos.Contar().ToString();
            lblCantidadRecepcionistas.Text = negocioUsuarios.ContarPorTipo(TipoUsuario.Recepcionista).ToString();
            lblCantidadAdministradores.Text = negocioUsuarios.ContarPorTipo(TipoUsuario.Administrador).ToString();

            lblCantidadObrasSociales.Text = negocioObrasSociales.Contar().ToString();
            lblCantidadEspecialidades.Text = negocioEspecialidades.Contar().ToString();

            lblTurnosCompletos.Text = negocioTurnos.ContarPorEstado(5).ToString();
            lblTurnosPendientesAdmin.Text = negocioTurnos.ContarPendientes().ToString();
            lblTurnosCanceladosAdmin.Text = negocioTurnos.ContarPorEstado(3).ToString();
            lblTurnosNoAsistidosAdmin.Text = negocioTurnos.ContarPorEstado(4).ToString();
        }

        private void CargarRecepcionista()
        {
            TurnoMedicoNegocio negocioTurnos = new TurnoMedicoNegocio();

            DateTime hoy = DateTime.Today;

            lblTurnosPendientes.Text = negocioTurnos.ContarPendientes(hoy).ToString();
            lblTurnosCancelados.Text = negocioTurnos.ContarPorEstado(hoy, 3).ToString();
            lblTurnosNoAsistidos.Text = negocioTurnos.ContarPorEstado(hoy, 4).ToString();
            lblTurnosCerrados.Text = negocioTurnos.ContarPorEstado(hoy, 5).ToString();
        }

        private void CargarMedico()
        {
            Usuario usuario = (Usuario)Session["usuario"];
            int idMedico = usuario.Medico.Id;

            DateTime hoy = DateTime.Today;

            TurnoMedicoNegocio negocioTurnos = new TurnoMedicoNegocio();

            // Resumen de hoy
            lblTurnosPendientesMedico.Text = negocioTurnos.ContarPendientes(hoy, idMedico).ToString();
            lblTurnosCompletadosMedico.Text = negocioTurnos.ContarPorEstado(hoy, 5, idMedico).ToString();
            lblTurnosCanceladosMedico.Text = negocioTurnos.ContarPorEstado(hoy, 3, idMedico).ToString();
            lblTurnosNoAsistidosMedico.Text = negocioTurnos.ContarPorEstado(hoy, 4, idMedico).ToString();

            // Semana actual
            int diferencia = ((int)hoy.DayOfWeek + 6) % 7;
            DateTime lunes = hoy.AddDays(-diferencia);

            Label[] cantidades = {
                lblCantidadLunes,
                lblCantidadMartes,
                lblCantidadMiercoles,
                lblCantidadJueves,
                lblCantidadViernes,
                lblCantidadSabado,
                lblCantidadDomingo
            };

            for (int i = 0; i < 7; i++)
            {
                DateTime fecha = lunes.AddDays(i);

                int cantidad = negocioTurnos.ContarPendientes(fecha, idMedico);

                cantidades[i].Text = cantidad.ToString();
            }
        }
    }
}
