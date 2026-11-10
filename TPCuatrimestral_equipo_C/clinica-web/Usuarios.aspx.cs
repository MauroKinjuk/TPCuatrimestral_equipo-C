using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace clinica_web
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
    }
}