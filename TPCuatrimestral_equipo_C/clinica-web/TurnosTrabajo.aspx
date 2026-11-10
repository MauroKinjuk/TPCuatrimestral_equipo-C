<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TurnosTrabajo.aspx.cs" Inherits="clinica_web.TurnosTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Listado de turnos de trabajo -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Turnos de trabajo</h5>
        </div>

        <div class="card-body">
            <a href="FormularioTurnoTrabajo.aspx" class="btn btn-primary mb-3">Nuevo turno de trabajo</a>
            <asp:GridView ID="dgvTurnosTrabajo" runat="server"
                CssClass="table table-striped"
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
        </div>
    </div>

</asp:Content>
