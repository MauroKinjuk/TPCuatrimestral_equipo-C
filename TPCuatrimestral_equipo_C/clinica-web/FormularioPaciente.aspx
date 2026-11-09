<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioPaciente.aspx.cs" Inherits="clinica_web.FormularioPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hfId" runat="server" />

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1"><asp:Label ID="lblTitulo" runat="server" Text="Registrar Nuevo Paciente"></asp:Label></h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. Juan"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ej. Pérez"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtDni" class="form-label">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" placeholder="Sin puntos ni espacios"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtFechaNacimiento" class="form-label">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtEmail" class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="correo@ejemplo.com" TextMode="Email"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej. 2954 123456"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtDireccion" class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" placeholder="Ej. Av. Corrientes 1234, CABA"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="ddlObraSocial" class="form-label">Obra Social</label>
                    <asp:DropDownList ID="ddlObraSocial" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                        <asp:ListItem Text="Sin obra social" Value="" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtNroAfiliado" class="form-label">Nro. de afiliado</label>
                    <asp:TextBox ID="txtNroAfiliado" runat="server" CssClass="form-control" placeholder="Ej. OS-100234"></asp:TextBox>
                </div>
            </div>
            <div class="d-flex justify-content-end gap-2 mt-2">
                <a href="Pacientes.aspx" class="btn btn-outline-secondary px-4">Volver</a>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
