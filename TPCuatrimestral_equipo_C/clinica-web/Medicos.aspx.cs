using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        // TODO = Cambiar DataSource
        List<Medico> listaEjemplo = new List<Medico>
        {
            new Medico {Id = 1, Nombre = "Roberto", Apellido = "Fernández", Matricula = "MN 45892", Especialidades = null, TurnosTrabajo = null},
            new Medico {Id = 2, Nombre = "Laura", Apellido = "Martínez", Matricula = "MN 89341", Especialidades = null, TurnosTrabajo = null},
            new Medico {Id = 3, Nombre = "Diego", Apellido = "López", Matricula = "MN 74125", Especialidades = null, TurnosTrabajo = null}
        };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgvMedicos.DataSource = listaEjemplo;
                dgvMedicos.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}