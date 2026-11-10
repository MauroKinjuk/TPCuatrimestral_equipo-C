
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

        private void cargarGrilla()
        {
            ObraSocialNegocio negocio = new ObraSocialNegocio();

            List<ObraSocial> lista = negocio.Listar();

            dgvObrasSociales.DataSource = lista;
            dgvObrasSociales.DataBind();
        }
    }
}
