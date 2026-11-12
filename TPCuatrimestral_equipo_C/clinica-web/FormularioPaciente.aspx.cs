
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace clinica_web
{
    public partial class FormularioPaciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarObrasSociales();

                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    hfId.Value = id;
                    cargarPaciente(int.Parse(id));
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
                Paciente paciente = new Paciente();
                PacienteNegocio negocio = new PacienteNegocio();

                paciente.Nombre = txtNombre.Text.Trim();
                paciente.Apellido = txtApellido.Text.Trim();
                paciente.Dni = txtDni.Text.Trim();
                paciente.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                paciente.Email = txtEmail.Text.Trim();
                paciente.Telefono = txtTelefono.Text.Trim();
                paciente.Direccion = txtDireccion.Text.Trim();
                paciente.NroAfiliado = txtNroAfiliado.Text.Trim();

                if (!string.IsNullOrEmpty(ddlObraSocial.SelectedValue))
                {
                    paciente.ObraSocial = new ObraSocial();
                    paciente.ObraSocial.Id = int.Parse(ddlObraSocial.SelectedValue);
                }

                if (string.IsNullOrEmpty(hfId.Value))
                {
                    negocio.Agregar(paciente);
                }
                else
                {
                    paciente.Id = int.Parse(hfId.Value);
                    negocio.Modificar(paciente);
                }

                Response.Redirect("Pacientes.aspx");
            }
            catch (Exception ex)
            {
                pnlMensaje.Visible = true;
                lblMensaje.Text = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        private void cargarObrasSociales()
        {
            ObraSocialNegocio negocio = new ObraSocialNegocio();

            ddlObraSocial.DataSource = negocio.Listar();
            ddlObraSocial.DataTextField = "Nombre";
            ddlObraSocial.DataValueField = "Id";
            ddlObraSocial.DataBind();
        }

        private void cargarPaciente(int id)
        {
            PacienteNegocio negocio = new PacienteNegocio();
            Paciente paciente = negocio.Obtener(id);

            if (paciente == null)
            {
                Response.Redirect("Pacientes.aspx");
                return;
            }

            lblTitulo.Text = "Editar Paciente";
            txtNombre.Text = paciente.Nombre;
            txtApellido.Text = paciente.Apellido;
            txtDni.Text = paciente.Dni;
            txtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd");
            txtEmail.Text = paciente.Email;
            txtTelefono.Text = paciente.Telefono;
            txtDireccion.Text = paciente.Direccion;
            txtNroAfiliado.Text = paciente.NroAfiliado;

            if (paciente.ObraSocial != null)
            {
                ddlObraSocial.SelectedValue = paciente.ObraSocial.Id.ToString();
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

            string dni = txtDni.Text.Trim();
            if (string.IsNullOrWhiteSpace(dni))
            {
                errores.Add("El DNI es obligatorio.");
            }
            else if (!dni.All(char.IsDigit) || dni.Length < 7 || dni.Length > 8)
            {
                errores.Add("El DNI tiene que ser numérico, de 7 u 8 dígitos.");
            }

            DateTime fechaNacimiento;
            if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento))
            {
                errores.Add("La fecha de nacimiento es obligatoria.");
            }
            else if (fechaNacimiento > DateTime.Today)
            {
                errores.Add("La fecha de nacimiento no puede ser futura.");
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errores.Add("El correo electrónico es obligatorio.");
            }
            else if (!esEmailValido(txtEmail.Text.Trim()))
            {
                errores.Add("El correo electrónico no tiene un formato válido.");
            }

            if (!string.IsNullOrEmpty(ddlObraSocial.SelectedValue) && string.IsNullOrWhiteSpace(txtNroAfiliado.Text))
            {
                errores.Add("Si el paciente tiene obra social, el número de afiliado es obligatorio.");
            }

            return errores;
        }

        private bool esEmailValido(string email)
        {
            try
            {
                var direccion = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
