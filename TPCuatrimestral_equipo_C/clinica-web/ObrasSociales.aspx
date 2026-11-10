<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ObrasSociales.aspx.cs" Inherits="clinica_web.ObrasSociales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="card shadow-sm border-0">
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="dgvObrasSociales" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-striped align-middle mb-0"
                    GridLines="None"
                    UseAccessibleHeader="True">

                    <HeaderStyle CssClass="table-dark" />

                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" HeaderStyle-CssClass="ps-4" ItemStyle-CssClass="ps-4" />
                        <asp:BoundField DataField="Nombre" HeaderText="Obra Social" />
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
