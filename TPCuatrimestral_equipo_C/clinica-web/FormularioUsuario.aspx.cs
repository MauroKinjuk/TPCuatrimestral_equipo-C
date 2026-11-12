using dominio;
using negocio;
using System;
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

                if (Request.QueryString["id"] != null)
                {
                    int id = int.Parse(Request.QueryString["id"]);
                    CargarUsuario(id);
                }
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

        private void CargarUsuario(int id)
        {
            // Al editar, la contraseña no es obligatoria porque conservamos la actual
            rfvContrasenia.Enabled = false;

            UsuarioNegocio negocio = new UsuarioNegocio();
            Usuario usuario = negocio.BuscarPorId(id);

            // Cambia el título cuando estamos editando un usuario.
            lblTitulo.Text = "Modificar usuario";

            if (usuario != null)
            {
                txtNombreUsuario.Text = usuario.NombreUsuario;
                ddlTipo.SelectedValue = usuario.Tipo.ToString();//selecciona automáticamente su tipo

                if (usuario.Medico != null)
                {
                    divMedico.Visible = true;
                    ddlMedico.SelectedValue = usuario.Medico.Id.ToString();
                }
            }
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

                // // Si la URL contiene un ID, estamos editando un usuario existente
                if (Request.QueryString["id"] != null)
                {
                    // Obtenemos el ID de la URL y lo convertimos a enterox
                    usuario.Id = int.Parse(Request.QueryString["id"]);

                    // Actualizamos los datos del usuario en la base de datos
                    negocio.Modificar(usuario);
                }
                else
                {     // Si no hay ID en la URL, agregamos un usuario nuevo.
                    negocio.Agregar(usuario);
                }
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