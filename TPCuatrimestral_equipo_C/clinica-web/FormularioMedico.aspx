<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioMedico.aspx.cs" Inherits="clinica_web.FormularioMedico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hfId" runat="server" />

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Registro -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">
                <asp:Label ID="lblTitulo" runat="server" Text="Registrar Nuevo Medico"></asp:Label></h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej. Roberto"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Ej. Fernández"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtMatricula" class="form-label">Matrícula</label>
                    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" placeholder="Ej. MN 12345"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Especialidades</label>
                    <div class="especialidades-container">
                        <asp:CheckBoxList ID="cblEspecialidades" runat="server"
                            CssClass="especialidades-lista"
                            RepeatDirection="Vertical"
                            RepeatLayout="UnorderedList">
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="turnoMedico" class="form-label">Turno de trabajo</label>
                    <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione un turno..." Value="" Selected="True"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="d-flex justify-content-end gap-2 mt-2">
                <a href="Medicos.aspx" class="btn btn-outline-secondary px-4">Volver</a>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>


</asp:Content>
