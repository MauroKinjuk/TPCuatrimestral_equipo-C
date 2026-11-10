
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;

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
                negocio.Agregar(obraSocial);

                txtNombre.Text = "";
                pnlMensaje.Visible = false;
                cargarGrilla();
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
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
