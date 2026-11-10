<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="clinica_web.Usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <h2>Administración de usuarios</h2>

    <a href="FormularioUsuario.aspx" class="btn btn-primary mb-3">
         Nuevo usuario
   </a>

    <asp:GridView ID="dgvUsuarios" runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-striped">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
        </Columns>

    </asp:GridView>

</asp:Content>
