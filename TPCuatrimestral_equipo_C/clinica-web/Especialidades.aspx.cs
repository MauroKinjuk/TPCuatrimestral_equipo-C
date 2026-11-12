using dominio;
using System;
using System.Collections.Generic;

namespace clinica_web
{
    public partial class WebForm3 : System.Web.UI.Page
    {

        // TODO = Cambiar DataSource
        List<Especialidad> listaEjemplo = new List<Especialidad>
        {
            new Especialidad { Id = 1, Nombre = "Cardiología" },
            new Especialidad { Id = 2, Nombre = "Pediatría" },
            new Especialidad { Id = 3, Nombre = "Traumatología" }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgvEspecialidades.DataSource = listaEjemplo;
                dgvEspecialidades.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}