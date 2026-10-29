<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="clinica_web.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Registro -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Registrar Nuevo Médico</h5>
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
                    <label for="especialidadMedico" class="form-label">Especialidad</label>
                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione una especialidad..." Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1">Cardiología</asp:ListItem>
                        <asp:ListItem Value="2">Pediatría</asp:ListItem>
                        <asp:ListItem Value="3">Traumatología</asp:ListItem>
                        <asp:ListItem Value="4">Clínica Médica</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label for="turnoMedico" class="form-label">Turno de trabajo</label>
                    <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione un turno..." Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1">Mañana</asp:ListItem>
                        <asp:ListItem Value="2">Tarde</asp:ListItem>
                        <asp:ListItem Value="3">Noche</asp:ListItem>
                    </asp:DropDownList>
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
                <asp:GridView ID="dgvMedicos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped align-middle mb-0" GridLines="None" UseAccessibleHeader="True">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Matricula" HeaderText="Matricula" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Especialidades" HeaderText="Especialidades" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="TurnosTrabajo" HeaderText="Turno" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />

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
