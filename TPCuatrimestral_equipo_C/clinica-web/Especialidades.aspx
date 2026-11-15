<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="clinica_web.WebForm3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Buscador y alta -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div class="d-flex gap-2">
            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                Style="width: 280px;"
                placeholder="Buscar por nombre de especialidad">
            </asp:TextBox>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary" OnClick="btnBuscar_Click" />
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nueva Especialidad" CssClass="btn btn-primary" OnClick="btnNuevo_Click" />
    </div>

    <!-- Tabla -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvEspecialidades" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True"
                    OnRowCommand="dgvEspecialidades_RowCommand">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Nombre" HeaderText="Especialidad" />

                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="text-end pe-4" ItemStyle-CssClass="text-end pe-4">
                            <ItemTemplate>
                                <asp:Button ID="btnDescripcion" runat="server" Text="Ver descripcion" CssClass="btn btn-sm btn-outline-secondary me-1"
                                    CommandName="Descripcion" CommandArgument='<%# Eval("Id") %>' />
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-sm btn-outline-secondary me-1"
                                    CommandName="Editar" CommandArgument='<%# Eval("Id") %>' />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-sm btn-outline-danger"
                                    CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
