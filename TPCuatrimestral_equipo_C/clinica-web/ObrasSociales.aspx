<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ObrasSociales.aspx.cs" Inherits="clinica_web.ObrasSociales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Registro -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1"><asp:Label ID="lblTitulo" runat="server" Text="Agregar Nueva Obra Social"></asp:Label></h5>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingresar nombre de la obra social"></asp:TextBox>
            </div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
        </div>
    </div>

    <!-- Tabla -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvObrasSociales" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Nombre" HeaderText="Obra Social" />
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
