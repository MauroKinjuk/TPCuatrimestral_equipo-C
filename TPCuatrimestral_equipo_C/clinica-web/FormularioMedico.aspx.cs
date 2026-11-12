using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace clinica_web
{
    public partial class FormularioMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    hfId.Value = id;
                    cargarMedico(int.Parse(id));
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
                Medico medico = new Medico();
                MedicoNegocio medNegocio = new MedicoNegocio();
                EspecialidadNegocio espNegocio = new EspecialidadNegocio();

                medico.Nombre = txtNombre.Text.Trim();
                medico.Apellido = txtApellido.Text.Trim();
                medico.Matricula = txtMatricula.Text.Trim();

                List<Especialidad> espMedico = new List<Especialidad>();
                List<Especialidad> espTodas = espNegocio.Listar();

                foreach (ListItem item in cblEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        int idEspecialidad = int.Parse(item.Value);
                        Especialidad especialidad = espTodas.Find(esp => esp.Id == idEspecialidad);

                        if (especialidad != null)
                        {
                            espMedico.Add(especialidad);
                        }
                    }
                }

                medico.Especialidades = espMedico;

                if (string.IsNullOrEmpty(hfId.Value))
                {
                    medNegocio.Agregar(medico);
                }
                else
                {
                    medico.Id = int.Parse(hfId.Value);
                    medNegocio.Modificar(medico);
                }

                Response.Redirect("Medicos.aspx");
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                //lblMensaje.Text = ex.Message;
                lblMensaje.Text = ex.GetBaseException().Message;
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();

            cblEspecialidades.DataSource = negocio.Listar();
            cblEspecialidades.DataTextField = "Nombre";
            cblEspecialidades.DataValueField = "Id";
            cblEspecialidades.DataBind();
        }

        private void cargarMedico(int id)
        {
            MedicoNegocio negocio = new MedicoNegocio();
            Medico medico = negocio.BuscarPorId(id);

            if (medico == null)
            {
                Response.Redirect("Medicos.aspx");
                return;
            }

            lblTitulo.Text = "Editar Medico";
            txtNombre.Text = medico.Nombre;
            txtApellido.Text = medico.Apellido;
            txtMatricula.Text = medico.Matricula;

            foreach (Especialidad esp in medico.Especialidades)
            {
                ListItem item = cblEspecialidades.Items.FindByValue(esp.Id.ToString());

                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        private List<string> validar()
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errores.Add("El nombre es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                errores.Add("El apellido es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(txtMatricula.Text))
            {
                errores.Add("La matricula es obligatorio.");
            }

            int idMedico = 0;

            if (Request.QueryString["id"] != null)
            {
                idMedico = int.Parse(Request.QueryString["id"]);
            }

            MedicoNegocio negocio = new MedicoNegocio();
            if (!string.IsNullOrWhiteSpace(txtMatricula.Text) &&
                negocio.ExisteMatricula(txtMatricula.Text.Trim(), idMedico))
            {
                errores.Add("La matricula ingresada pertenece a otro medico.");
            }

            return errores;
        }
    }
}