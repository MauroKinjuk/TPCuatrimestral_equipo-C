<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="clinica_web.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 class="mb-3">Mis turnos</h2>

    <!-- Tabla -->
    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvTurnos" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True"
                    EmptyDataText="No tenés turnos asignados.">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />

                        <asp:TemplateField HeaderText="Fecha">
                            <ItemTemplate><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Hora">
                            <ItemTemplate><%# Eval("Hora", @"{0:hh\:mm}") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Paciente">
                            <ItemTemplate><%# Eval("Paciente.Nombre") + " " + Eval("Paciente.Apellido") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Motivo">
                            <ItemTemplate><%# Eval("Consulta.Motivo") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate><%# Eval("Estado.Nombre") %></ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Diagnóstico">
                            <ItemTemplate><%# Eval("Consulta.Diagnostico") %></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
