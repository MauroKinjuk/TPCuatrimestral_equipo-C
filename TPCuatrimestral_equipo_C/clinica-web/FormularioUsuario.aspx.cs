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
    public partial class FormularioUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMedicos();
            }
        }

        private void CargarMedicos()
        {
            MedicoNegocio negocio = new MedicoNegocio();

            ddlMedico.DataSource = negocio.Listar();
            ddlMedico.DataTextField = "Apellido";
            ddlMedico.DataValueField = "Id";
            ddlMedico.DataBind();

            ddlMedico.Items.Insert(0, new ListItem("Seleccione un médico", ""));
            // agrega la opción «Seleccione un médico» al principio
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            divMedico.Visible = ddlTipo.SelectedValue == "Medico";
            //muestra o no      //obtiene el valor seleccionado del desplegable

         //Si seleccionamos Médico, el campo aparece. Si seleccionamos Administrador o Recepcionista, desaparece
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            } // Verifica que el formulario cumpla con las validaciones antes de guardar
            Usuario usuario = new Usuario();

            usuario.NombreUsuario = txtNombreUsuario.Text.Trim();
            usuario.Contrasenia = txtContrasenia.Text;
            usuario.Tipo = (TipoUsuario)Enum.Parse(
                typeof(TipoUsuario),
                ddlTipo.SelectedValue
            );
            if (usuario.Tipo == TipoUsuario.Medico)
            {

                if (string.IsNullOrEmpty(ddlMedico.SelectedValue))
                {                    //comprueba si el desplegable no tiene ningún valor seleccionado
                                     // ej: «Seleccione un médico»
                    return;
                }

                usuario.Medico = new Medico();
                usuario.Medico.Id = int.Parse(ddlMedico.SelectedValue);
            }

            try
            {
                UsuarioNegocio negocio = new UsuarioNegocio();
                negocio.Agregar(usuario);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.InnerException?.Message ?? ex.Message;
                lblError.Visible = true;
                return;
            }

            Response.Redirect("Usuarios.aspx");
        }
    }

   
    }