<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="clinica_web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- Contenedor principal del formulario de inicio de sesión --%>
    <div class="row justify-content-center mt-5">
        <div class="col-md-5">

            <div class="card">

                <div class="card-header text-center">
                    <h4 class="mb-0">Clínica Privada</h4>
                </div>

                <div class="card-body">

                    <h5 class="text-center mb-4">Iniciar sesión</h5>

                    <%-- Campo para ingresar el nombre de usuario --%>
                    <div class="mb-3">
                        <asp:Label ID="lblUsuario" runat="server"
                            Text="Nombre de usuario"
                            CssClass="form-label" />

                        <asp:TextBox ID="txtUsuario" runat="server"
                            CssClass="form-control"
                            MaxLength="50" />
                    </div>

                    <%-- Campo para ingresar la contraseña --%>
                    <div class="mb-3">
                        <asp:Label ID="lblContrasenia" runat="server"
                            Text="Contraseña"
                            CssClass="form-label" />

                        <asp:TextBox ID="txtContrasenia" runat="server"
                            TextMode="Password"
                            CssClass="form-control" />
                    </div>

                    <%-- Botón de ingreso: por ahora solamente visual --%>
                    <div class="d-grid mt-4">
                        <asp:Button ID="btnIngresar" runat="server"
                            Text="Ingresar"
                            CssClass="btn btn-primary" />
                    </div>
                    <%-- Permite volver al sistema mientras desarrollamos el proyecto --%>
                    <div class="text-center mt-3">
                        <a href="Default.aspx" class="btn btn-outline-secondary">Volver al inicio
                        </a>
                    </div>

                </div>
            </div>

        </div>
    </div>

</asp:Content>

