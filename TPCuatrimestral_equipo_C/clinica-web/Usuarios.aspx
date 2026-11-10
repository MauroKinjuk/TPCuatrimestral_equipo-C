<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="clinica_web.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="card">
        <div class="card-header">
            <h4 class="mb-0">Usuarios</h4>
        </div>

        <div class="card-body">

            <a href="FormularioUsuario.aspx" class="btn btn-primary mb-3">Nuevo usuario
            </a>

            <asp:GridView ID="dgvUsuarios" runat="server"
                AutoGenerateColumns="false"
                CssClass="table table-striped">

                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="ID" />
                    <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>

                            <a href='<%# "FormularioUsuario.aspx?id=" + Eval("Id") %>'
                                class="btn btn-outline-secondary btn-sm">Editar
                            </a>

                             <%-- Botón para realizar la baja lógica del usuario seleccionado --%>
                            <asp:LinkButton ID="btnEliminar" runat="server"
                                Text="Eliminar"
                                CssClass="btn btn-outline-danger btn-sm"
                                CommandArgument='<%# Eval("Id") %>'
                                OnClick="btnEliminar_Click"
                                OnClientClick="return confirm('¿Está seguro de eliminar este usuario?');"
                                CausesValidation="false" />

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>

        </div>
    </div>

</asp:Content>
