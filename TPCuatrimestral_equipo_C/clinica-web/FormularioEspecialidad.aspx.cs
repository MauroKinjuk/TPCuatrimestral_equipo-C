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
    public partial class FormularioEspecialidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    hfId.Value = id;
                    CargarEspecialidad(int.Parse(id));
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<string> errores = validar();

            if (errores.Count > 0)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = string.Join("<br/>", errores);
                return;
            }

            try
            {
                Especialidad especialidad = new Especialidad();
                EspecialidadNegocio negocio = new EspecialidadNegocio();

                especialidad.Nombre = txtNombre.Text.Trim();

                string descripcion = txtDescripcion.Text.Trim();
                especialidad.Descripcion = string.IsNullOrEmpty(descripcion) ? null : descripcion;

                if (string.IsNullOrEmpty(hfId.Value))
                {
                    negocio.Agregar(especialidad);
                }
                else
                {
                    especialidad.Id = int.Parse(hfId.Value);
                    negocio.Modificar(especialidad);
                }

                Response.Redirect("Especialidades.aspx");
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        private void CargarEspecialidad(int id)
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            Especialidad especialidad = negocio.BuscarPorId(id);

            if (especialidad == null)
            {
                Response.Redirect("Especialidades.aspx");
                return;
            }

            lblTitulo.Text = "Editar Especialidad";
            txtNombre.Text = especialidad.Nombre;
            txtDescripcion.Text = especialidad.Descripcion ?? "";
        }

        private List<string> validar()
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                errores.Add("El nombre es obligatorio");
            }

            int idEspecialidad = 0;

            if (Request.QueryString["id"] != null)
            {
                idEspecialidad = int.Parse(Request.QueryString["id"]);
            }

            EspecialidadNegocio negocio = new EspecialidadNegocio();

            if (!string.IsNullOrEmpty(txtNombre.Text) &&
                negocio.ExisteNombre(txtNombre.Text.Trim(), idEspecialidad))
            {
                errores.Add("El nombre ingresado pertenece a otra especialidad.");
            }

            return errores;
        }
    }
}