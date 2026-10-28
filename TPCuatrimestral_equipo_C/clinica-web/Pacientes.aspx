<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="clinica_web.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Registro -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Registrar Nuevo Paciente</h5>
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
                    <label for="txtEmail" class="form-label">Correo Electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="correo@ejemplo.com" TextMode="Email"></asp:TextBox>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej. 2954 123456"></asp:TextBox>
                </div>
            </div>
            <div class="text-end mt-2">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>

    <!-- Tabla -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvPacientes" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Dni" HeaderText="DNI" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />

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
