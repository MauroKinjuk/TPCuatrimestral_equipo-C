
using dominio;
using negocio;
using System;
using System.Collections.Generic;

namespace clinica_web
{
    public partial class ObrasSociales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarGrilla();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = "El nombre es obligatorio.";
                return;
            }

            try
            {
                ObraSocial obraSocial = new ObraSocial();
                ObraSocialNegocio negocio = new ObraSocialNegocio();

                obraSocial.Nombre = nombre;

                if (string.IsNullOrEmpty(hfId.Value))
                {
                    negocio.Agregar(obraSocial);
                }
                else
                {
                    obraSocial.Id = int.Parse(hfId.Value);
                    negocio.Modificar(obraSocial);
                }

                limpiarFormulario();
                cargarGrilla();
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        protected void dgvObrasSociales_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            ObraSocialNegocio negocio = new ObraSocialNegocio();

            if (e.CommandName == "Editar")
            {
                ObraSocial obraSocial = negocio.Listar().Find(x => x.Id == id);

                if (obraSocial != null)
                {
                    hfId.Value = obraSocial.Id.ToString();
                    txtNombre.Text = obraSocial.Nombre;
                    lblTitulo.Text = "Editar Obra Social";
                    btnGuardar.Text = "Guardar cambios";
                    btnCancelar.Visible = true;
                    pnlMensaje.Visible = false;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    negocio.Eliminar(id);
                    limpiarFormulario();
                    cargarGrilla();
                }
                catch (Exception ex)
                {
                    pnlMensaje.Visible = true;
                    lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
        }

        private void limpiarFormulario()
        {
            hfId.Value = "";
            txtNombre.Text = "";
            lblTitulo.Text = "Agregar Nueva Obra Social";
            btnGuardar.Text = "Guardar";
            btnCancelar.Visible = false;
            pnlMensaje.Visible = false;
        }

        private void cargarGrilla()
        {
            ObraSocialNegocio negocio = new ObraSocialNegocio();

            List<ObraSocial> lista = negocio.Listar();

            dgvObrasSociales.DataSource = lista;
            dgvObrasSociales.DataBind();
        }
    }
}
