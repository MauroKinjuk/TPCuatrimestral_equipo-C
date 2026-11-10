<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioTurnoTrabajo.aspx.cs" Inherits="clinica_web.FormularioTurnoTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Formulario para registrar turnos de trabajo -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">
                <asp:Label ID="lblTitulo" runat="server"
                    Text="Registrar turno de trabajo" />
            </h5>
        </div>


        <div class="card-body">
            <div class="row">

                <div class="col-md-6 mb-3">
                    <label class="form-label">Médico</label>

                    <asp:DropDownList ID="ddlMedicos"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Día de la semana</label>

                    <asp:DropDownList ID="ddlDiaSemana"
                        runat="server"
                        CssClass="form-select">

                        <asp:ListItem Text="Seleccione un día" Value="" />
                        <asp:ListItem Text="Lunes" Value="Lunes" />
                        <asp:ListItem Text="Martes" Value="Martes" />
                        <asp:ListItem Text="Miércoles" Value="Miércoles" />
                        <asp:ListItem Text="Jueves" Value="Jueves" />
                        <asp:ListItem Text="Viernes" Value="Viernes" />
                        <asp:ListItem Text="Sábado" Value="Sábado" />
                        <asp:ListItem Text="Domingo" Value="Domingo" />

                    </asp:DropDownList>
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora de entrada</label>
                    <asp:TextBox ID="txtHoraInicio"
                        runat="server"
                        TextMode="Time"
                        CssClass="form-control">
                    </asp:TextBox>
                </div>

                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora de salida</label>

                    <asp:TextBox ID="txtHoraFin"
                        runat="server"
                        TextMode="Time"
                        CssClass="form-control">
                    </asp:TextBox>

                </div>
                <!-- Cierra el campo Hora de salida -->

            </div>
            <!-- Cierra la fila de los cuatro campos -->

            <div class="mt-3">
                <asp:Button ID="btnGuardar"
                    runat="server"
                    Text="Guardar turno de trabajo"
                    CssClass="btn btn-primary"
                    OnClick="btnGuardar_Click" />

                <a href="TurnosTrabajo.aspx" class="btn btn-secondary">Cancelar </a>

                <asp:Label ID="lblMensaje"
                    runat="server"
                    CssClass="d-block mt-2 text-danger">
                </asp:Label>

            </div>

        </div>
        <!-- Cierra card-body -->
    </div>
    <!-- Cierra la tarjeta del formulario -->
</asp:Content>
