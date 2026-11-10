<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioUsuario.aspx.cs" Inherits="clinica_web.FormularioUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card">

        <div class="card-header">
            <h4 class="mb-0">
                <asp:Label ID="lblTitulo" runat="server" Text="Registrar usuario" />
            </h4>
        </div>

        <div class="card-body">

            <div class="row">

                <div class="col-md-6 mb-3">
                    <asp:Label ID="lblNombreUsuario" runat="server"
                        Text="Nombre de usuario" CssClass="form-label" />

                    <asp:TextBox ID="txtNombreUsuario" runat="server"
                        CssClass="form-control" MaxLength="50" />

                    <asp:RequiredFieldValidator ID="rfvNombreUsuario"
                        runat="server"
                        ControlToValidate="txtNombreUsuario"
                        ErrorMessage="Debe ingresar un nombre de usuario."
                        CssClass="text-danger"
                        Display="Dynamic"
                        ValidationGroup="GuardarUsuario" />
                </div>

                <div class="col-md-6 mb-3">
                    <asp:Label ID="lblContrasenia" runat="server"
                        Text="Contraseña" CssClass="form-label" />

                    <asp:TextBox ID="txtContrasenia" runat="server"
                        TextMode="Password"
                        CssClass="form-control" MaxLength="50" />

                    <asp:RequiredFieldValidator ID="rfvContrasenia"
                        runat="server"
                        ControlToValidate="txtContrasenia"
                        ErrorMessage="Debe ingresar una contraseña."
                        CssClass="text-danger"
                        Display="Dynamic"
                        ValidationGroup="GuardarUsuario" />
                </div>

                <div class="col-md-6 mb-3">
                    <asp:Label ID="lblTipo" runat="server"
                        Text="Tipo de usuario" CssClass="form-label" />

                    <asp:DropDownList ID="ddlTipo" runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">

                        <asp:ListItem Text="Seleccione un tipo" Value="" />
                        <asp:ListItem Text="Administrador" Value="Administrador" />
                        <asp:ListItem Text="Recepcionista" Value="Recepcionista" />
                        <asp:ListItem Text="Médico" Value="Medico" />

                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="rfvTipo"
                        runat="server"
                        ControlToValidate="ddlTipo"
                        InitialValue=""
                        ErrorMessage="Debe seleccionar un tipo de usuario."
                        CssClass="text-danger"
                        Display="Dynamic"
                        ValidationGroup="GuardarUsuario" />
                </div>

                <div id="divMedico" runat="server"
                    class="col-md-6 mb-3" visible="false">

                    <asp:Label ID="lblMedico" runat="server"
                        Text="Médico asociado" CssClass="form-label" />

                    <asp:DropDownList ID="ddlMedico" runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="rfvMedico"
                        runat="server"
                        ControlToValidate="ddlMedico"
                        InitialValue=""
                        ErrorMessage="Debe seleccionar un médico."
                        CssClass="text-danger"
                        Display="Dynamic"
                        ValidationGroup="GuardarUsuario" />
                </div>

            </div>

            <asp:Label ID="lblError" runat="server"
                CssClass="text-danger" Visible="false" />

            <div class="mt-4">

                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar usuario"
                    CssClass="btn btn-primary"
                    ValidationGroup="GuardarUsuario"
                    OnClick="btnGuardar_Click" />

                <a href="Usuarios.aspx" class="btn btn-secondary">Cancelar
                </a>

            </div>

        </div>
    </div>

</asp:Content>

