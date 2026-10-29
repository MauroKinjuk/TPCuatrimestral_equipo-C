using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// TODO = Agregar FechaNacimiento, Direccion, ObraSocial, ¿Antecedentes?

namespace clinica_web
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        // TODO = Cambiar DataSource
        List<Paciente> listaEjemplo = new List<Paciente>
        {
            new Paciente {Id = 1, Nombre = "María", Apellido = "Gómez", Dni = "32145678", Email = "maria.gomez@email.com", Telefono = "2954 987654"},
            new Paciente {Id = 2, Nombre = "Carlos", Apellido = "Rodríguez", Dni = "12345678", Email = "carlos.r@email.com", Telefono = "2954 456789"}
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dgvPacientes.DataSource = listaEjemplo;
                dgvPacientes.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}