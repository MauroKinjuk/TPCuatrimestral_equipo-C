<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioEspecialidad.aspx.cs" Inherits="clinica_web.FormularioEspecialidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hfId" runat="server" />

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">
                <asp:Label ID="lblTitulo" runat="server" Text="Registrar Nueva Especialidad"></asp:Label>
            </h5>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox
                    ID="txtNombre"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Ingresar nombre de la especialidad">
                </asp:TextBox>
            </div>
            <div class="mb-3">
                <label for="txtDescripcion" class="form-label">Descripción</label>
                <asp:TextBox
                    ID="txtDescripcion"
                    runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="4"
                    MaxLength="500"
                    placeholder="Ingresar una descripción de la especialidad">
            </asp:TextBox>
            </div>
            <div class="d-flex justify-content-end gap-2 mt-4">
                <a href="Especialidades.aspx" class="btn btn-outline-secondary px-4">Volver</a>
                <asp:Button
                    ID="btnGuardar"
                    runat="server"
                    Text="Guardar"
                    CssClass="btn btn-primary px-4" 
                    OnClick="btnGuardar_Click"/>
            </div>
        </div>
    </div>

</asp:Content>
