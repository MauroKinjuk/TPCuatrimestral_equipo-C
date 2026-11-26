using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class WebForm3 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.TieneAcceso(Session["usuario"], TipoUsuario.Administrador))
            {
                Response.Redirect("~/Default.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                cargarDgv();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarDgv();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormularioEspecialidad.aspx");
        }

        protected void dgvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Descripcion")
            {
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                Especialidad especialidad = negocio.BuscarPorId(id);

                if (especialidad != null)
                {
                    lblNombreDescripcion.Text = especialidad.Nombre;
                    lblDescripcion.Text = string.IsNullOrWhiteSpace(especialidad.Descripcion)
                        ? "Sin descripción cargada."
                        : especialidad.Descripcion;
                    pnlDescripcion.Visible = true;
                }

                cargarDgv();
            }
            else if (e.CommandName == "Editar")
            {
                Response.Redirect("FormularioEspecialidad.aspx?id=" + id);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    EspecialidadNegocio negocio = new EspecialidadNegocio();
                    negocio.Eliminar(id);
                    cargarDgv();
                }
                catch (Exception ex)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
        }

        protected void btnCerrarDescripcion_Click(object sender, EventArgs e)
        {
            pnlDescripcion.Visible = false;
        }

        private void cargarDgv()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            string texto = txtBuscar.Text.Trim();

            List<Especialidad> lista = string.IsNullOrEmpty(texto)
                ? negocio.Listar().Take(10).ToList()
                : negocio.Filtrar(texto);

            dgvEspecialidades.DataSource = lista;
            dgvEspecialidades.DataBind();
        }
    }
}