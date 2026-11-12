using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
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
            Response.Redirect("FormularioMedico.aspx");
        }

        protected void dgvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Editar")
            {
                Response.Redirect("FormularioMedico.aspx?id=" + id);
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    MedicoNegocio negocio = new MedicoNegocio();
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

        private void cargarDgv()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            string texto = txtBuscar.Text.Trim();

            List<Medico> medicos = string.IsNullOrEmpty(texto)
                // TODO = cambiar para que muestre los resultados por pagina en lugar de solo 10
                ? negocio.Listar().Take(10).ToList()
                : negocio.Filtrar(texto);

            // Transforma la lista de medicos en una lista de objetos anonimos con las propiedades seleccionadas
            var listaMostrar = medicos.Select(m => new
            {
                m.Id,
                m.Nombre,
                m.Apellido,
                m.Matricula,
                // Obtiene los nombres de las especialidades y los concatena en un solo texto
                Especialidades = string.Join(", ", m.Especialidades.Select(e => e.Nombre))
            }).ToList();

            dgvMedicos.DataSource = listaMostrar;
            dgvMedicos.DataBind();
        }
    }
}