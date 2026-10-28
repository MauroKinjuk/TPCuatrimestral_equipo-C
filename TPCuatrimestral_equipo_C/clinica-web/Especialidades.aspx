<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="clinica_web.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Agregar Nueva Especialidad</h5>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label for="txtnombreEspecialidad" class="form-label">Nombre</label>
                <asp:TextBox ID="txtnombreEspecialidad" runat="server" CssClass="form-control" placeholder="Ingresar nombre de la especialidad"></asp:TextBox>
            </div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
        </div>
    </div>

    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvEspecialidades" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />

                        <asp:BoundField DataField="Nombre" HeaderText="Especialidad" />

                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="text-end pe-4" ItemStyle-CssClass="text-end pe-4">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-secondary me-1" />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-outline-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
