<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TurnosMedicos.aspx.cs" Inherits="clinica_web.TurnosMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Buscador y alta -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div class="d-flex gap-2 align-items-center">
            <asp:TextBox ID="txtFecha" runat="server" TextMode="Date" CssClass="form-control" Style="width: 200px;"></asp:TextBox>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary" OnClick="btnBuscar_OnClick" />
        </div>
        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo Turno" CssClass="btn btn-primary" OnClick="btnNuevo_OnClick" />
    </div>

    <!-- Tabla -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvTurnos" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True"
                    OnRowCommand="dgvTurnos_RowCommand"
                    EmptyDataText="No hay turnos registrados para la fecha seleccionada.">
                    <HeaderStyle CssClass="table-dark" />
                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy}"
                            HeaderStyle-CssClass="ps-3"
                            ItemStyle-CssClass="ps-3" />
                        <asp:BoundField DataField="Hora" HeaderText="Hora" DataFormatString="{0:hh\:mm}" />
                        <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                        <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                        <asp:BoundField DataField="Medico" HeaderText="Médico" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                        <asp:TemplateField HeaderText="Acciones"
                            HeaderStyle-CssClass="text-center"
                            ItemStyle-CssClass="text-center">

                            <ItemTemplate>

                                <!-- Mantiene los botones alineados horizontalmente -->
                                <div class="d-flex flex-nowrap gap-2 justify-content-center">

                                    <asp:Button ID="btnReprogramar"
                                        runat="server"
                                        Text="Reprogramar"
                                        CssClass="btn btn-sm btn-outline-secondary"
                                        CommandName="Reprogramar"
                                        CommandArgument='<%# Eval("Id") %>'
                                        Enabled='<%# Eval("PuedeCancelar") %>' />

                                    <asp:Button ID="btnCancelar"
                                        runat="server"
                                        Text="Cancelar"
                                        CssClass="btn btn-sm btn-outline-danger"
                                        CommandName="Cancelar"
                                        CommandArgument='<%# Eval("Id") %>'
                                        Enabled='<%# Eval("PuedeCancelar") %>'
                                        OnClientClick="return confirm('¿Seguro que querés cancelar este turno?');" />

                                    <asp:Button ID="btnNoAsistio"
                                        runat="server"
                                        Text="No Asistió"
                                        CssClass="btn btn-sm btn-outline-warning"
                                        CommandName="NoAsistio"
                                        CommandArgument='<%# Eval("Id") %>'
                                        Enabled='<%# Eval("PuedeMarcarNoAsistio") %>'
                                        OnClientClick="return confirm('¿Confirmás que el paciente no asistió al turno?');" />

                                </div>

                            </ItemTemplate>

                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
