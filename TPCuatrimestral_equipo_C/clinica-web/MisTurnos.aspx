<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="clinica_web.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Mis turnos</h2>

    <asp:GridView ID="dgvTurnos" runat="server"
        EmptyDataText="No tenés turnos asignados.">
    </asp:GridView>

</asp:Content>
