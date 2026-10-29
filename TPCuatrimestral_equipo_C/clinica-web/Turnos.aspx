<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Turnos.aspx.cs" Inherits="clinica_web.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Registro Turno -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Asignar Nuevo Turno</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label for="ddlEspecialidad" class="form-label">Especialidad</label>
                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione una especialidad..." Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1">Cardiología</asp:ListItem>
                        <asp:ListItem Value="2">Pediatría</asp:ListItem>
                        <asp:ListItem Value="3">Traumatología</asp:ListItem>
                        <asp:ListItem Value="4">Clínica Médica</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-6 mb-3">
                    <label for="ddlMedico" class="form-label">Médico</label>
                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Seleccione un médico..." Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1">Dr. Roberto Fernández</asp:ListItem>
                        <asp:ListItem Value="2">Dra. Laura Martínez</asp:ListItem>
                        <asp:ListItem Value="3">Dr. Diego López</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-6 mb-3">
                    <label for="txtFecha" class="form-label">Fecha y Hora</label>
                    <div class="d-flex gap-2">
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Seleccione un horario..." Value="" Selected="True" Enabled="False"></asp:ListItem>
                            <asp:ListItem Text="06:00" Value="06:00:00"></asp:ListItem>
                            <asp:ListItem Text="07:00" Value="07:00:00"></asp:ListItem>
                            <asp:ListItem Text="08:00" Value="08:00:00"></asp:ListItem>
                            <asp:ListItem Text="09:00" Value="09:00:00"></asp:ListItem>
                            <asp:ListItem Text="10:00" Value="10:00:00"></asp:ListItem>
                            <asp:ListItem Text="11:00" Value="11:00:00"></asp:ListItem>
                            <asp:ListItem Text="12:00" Value="12:00:00"></asp:ListItem>
                            <asp:ListItem Text="13:00" Value="13:00:00"></asp:ListItem>
                            <asp:ListItem Text="14:00" Value="14:00:00"></asp:ListItem>
                            <asp:ListItem Text="15:00" Value="15:00:00"></asp:ListItem>
                            <asp:ListItem Text="16:00" Value="16:00:00"></asp:ListItem>
                            <asp:ListItem Text="17:00" Value="17:00:00"></asp:ListItem>
                            <asp:ListItem Text="18:00" Value="18:00:00"></asp:ListItem>
                            <asp:ListItem Text="19:00" Value="19:00:00"></asp:ListItem>
                            <asp:ListItem Text="20:00" Value="20:00:00"></asp:ListItem>
                            <asp:ListItem Text="21:00" Value="21:00:00"></asp:ListItem>
                            <asp:ListItem Text="22:00" Value="22:00:00"></asp:ListItem>
                            <asp:ListItem Text="23:00" Value="23:00:00"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-6 mb-3">
                    <label for="ddlEstadoTurno" class="form-label">Estado</label>
                    <asp:DropDownList ID="ddlEstadoTurno" runat="server" CssClass="form-select">
                        <asp:ListItem Value="1" Selected="True">Nuevo</asp:ListItem>
                        <asp:ListItem Value="2">Reprogramado</asp:ListItem>
                        <asp:ListItem Value="3">Cancelado</asp:ListItem>
                        <asp:ListItem Value="4">No Asistió</asp:ListItem>
                        <asp:ListItem Value="5">Cerrado</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="text-end mt-2">
                <asp:Button ID="btnAsignar" runat="server" Text="Asignar Turno" CssClass="btn btn-primary px-4" OnClick="btnAsignar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
