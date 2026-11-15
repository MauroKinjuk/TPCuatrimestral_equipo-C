<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TurnosTrabajo.aspx.cs" Inherits="clinica_web.TurnosTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <%-- Buscador de turnos de trabajo --%>
    <div class="d-flex justify-content-between align-items-center mb-3">

        <div class="d-flex gap-2">

            <asp:TextBox ID="txtFiltro" runat="server"
                CssClass="form-control"
                placeholder="Buscar por médico o día">
        </asp:TextBox>

            <asp:Button ID="btnBuscar" runat="server"
                Text="Buscar"
                CssClass="btn btn-outline-secondary"
                OnClick="btnBuscar_Click" />

        </div>

        <a href="FormularioTurnoTrabajo.aspx" class="btn btn-primary">Nuevo turno de trabajo
    </a>

    </div>

    <asp:GridView ID="dgvTurnosTrabajo" runat="server"
        CssClass="table table-striped"
        HeaderStyle-CssClass="table-dark"
        AutoGenerateColumns="false"
        DataKeyNames="Id"
        OnRowCommand="dgvTurnosTrabajo_RowCommand">

        <Columns>
            <asp:TemplateField HeaderText="Médico">
                <ItemTemplate>
                    <%# Eval("Medico.Nombre") %> <%# Eval("Medico.Apellido") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="DiaSemana" HeaderText="Día" />
            <asp:BoundField DataField="HoraInicio" HeaderText="Entrada" DataFormatString="{0:hh\:mm}" />
            <asp:BoundField DataField="HoraFin" HeaderText="Salida" DataFormatString="{0:hh\:mm}" />

            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton ID="btnEditar"
                        runat="server"
                        Text="Editar"
                        CssClass="btn btn-outline-secondary btn-sm"
                        CommandName="EditarTurno"
                        CommandArgument='<%# Eval("Id") %>' />

                    <asp:LinkButton ID="btnEliminar"
                        runat="server"
                        Text="Eliminar"
                        CssClass="btn btn-outline-danger btn-sm"
                        CommandName="EliminarTurno"
                        CommandArgument='<%# Eval("Id") %>'
                        OnClientClick="return confirm('¿Está seguro de que desea eliminar este turno de trabajo?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>

</asp:Content>
