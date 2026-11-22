<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="HistorialPaciente.aspx.cs" Inherits="clinica_web.HistorialPaciente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Historial de turnos del paciente</h2>

        <a href="Pacientes.aspx" class="btn btn-outline-secondary">Volver a pacientes
        </a>
    </div>
    <div class="card shadow-sm border-0 mb-4">
        <div class="card-body">

            <h5 class="card-title mb-3">Datos del paciente</h5>

            <p>
                <strong>Nombre y apellido:</strong>
                <asp:Label ID="lblNombrePaciente" runat="server"></asp:Label>
            </p>

            <p>
                <strong>DNI:</strong>
                <asp:Label ID="lblDniPaciente" runat="server"></asp:Label>
            </p>

        </div>


        <div class="card shadow-sm border-0">
            <div class="card-body">

                <h5 class="card-title mb-3">Historial de turnos</h5>

                <div class="table-responsive">
                    <asp:GridView ID="dgvHistorial" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-striped align-middle"
                        GridLines="None"
                        EmptyDataText="Este paciente no tiene turnos registrados.">

                        <HeaderStyle CssClass="table-dark" />

                        <Columns>
                            <asp:BoundField DataField="Fecha"
                                HeaderText="Fecha"
                                DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <%# ((TimeSpan)Eval("Hora")).ToString(@"hh\:mm") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Médico">
                                <ItemTemplate>
                                    <%# Eval("Medico.NombreCompleto") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <%# Eval("Estado.Nombre") %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Motivo de consulta">
                                <ItemTemplate>
                                    <%# Eval("Consulta") != null
                                        ? Eval("Consulta.Motivo"): "Sin consulta registrada" %>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Observaciones">
                                <ItemTemplate>
                                    <%# string.IsNullOrWhiteSpace(Eval("Observaciones")?.ToString())
                                        ? "Sin observaciones": Eval("Observaciones") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                </div>

            </div>
        </div>

    </div>

</asp:Content>
