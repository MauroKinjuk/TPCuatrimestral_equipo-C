using dominio;
using negocio;
using System;
using System.Web.UI.WebControls;


namespace clinica_web
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.TieneAcceso(Session["usuario"], TipoUsuario.Administrador))
            {
                Response.Redirect("~/Default.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                UsuarioNegocio negocio = new UsuarioNegocio();

                dgvUsuarios.DataSource = negocio.Listar();
                dgvUsuarios.DataBind();
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            // Obtenemos el botón Eliminar que se presionó.
            LinkButton boton = (LinkButton)sender;

            // Obtenemos el ID del usuario seleccionado.
            int id = int.Parse(boton.CommandArgument);

            // Realizamos la baja lógica del usuario.
            UsuarioNegocio negocio = new UsuarioNegocio();
            negocio.Eliminar(id);

            // Actualizamos el listado para mostrar solamente usuarios activos.
            dgvUsuarios.DataSource = negocio.Listar();
            dgvUsuarios.DataBind();
        }

        // Filtramos los usuarios según el texto ingresado.
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            UsuarioNegocio negocio = new UsuarioNegocio();

            dgvUsuarios.DataSource = negocio.Filtrar(txtFiltro.Text.Trim());
            dgvUsuarios.DataBind();
        }
    }
}