<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioTurnoMedico.aspx.cs" Inherits="clinica_web.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <!-- Registro Turno -->
    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Asignar Nuevo Turno</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <!-- Paciente -->
                <div class="col-12 mb-3">
                    <label for="ddlPaciente" class="form-label">Paciente</label>
                    <asp:DropDownList
                        ID="ddlPaciente"
                        runat="server"
                        CssClass="form-select">
                    </asp:DropDownList>
                </div>
                <!-- Especialidad -->
                <div class="col-md-6 mb-3">
                    <label for="ddlEspecialidad" class="form-label">Especialidad</label>
                    <asp:DropDownList
                        ID="ddlEspecialidad"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <!-- Médico -->
                <div class="col-md-6 mb-3">
                    <label for="ddlMedico" class="form-label">Médico</label>
                    <asp:DropDownList
                        ID="ddlMedico"
                        runat="server"
                        CssClass="form-select"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <!-- Días disponibles -->
                <div class="col-12 mb-3">
                    <label class="form-label">Días disponibles</label>
                    <div class="d-flex flex-nowrap gap-2 overflow-auto pb-2">
                        <asp:Repeater
                            ID="rptDiasDisponibles"
                            runat="server"
                            OnItemCommand="rptDiasDisponibles_ItemCommand">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnDia"
                                    runat="server"
                                    Text='<%# Eval("Texto") %>'
                                    CommandName="SeleccionarDia"
                                    CommandArgument='<%# Eval("Fecha") %>'
                                    CssClass='<%# (ViewState["FechaSeleccionada"] != null &&
                                                   ((DateTime)ViewState["FechaSeleccionada"]).ToString("dd-MM-yyyy") == Eval("Fecha").ToString())
                                                      ? "btn btn-primary flex-shrink-0"
                                                      : "btn btn-outline-primary flex-shrink-0" %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <!-- Horarios disponibles -->
                <div class="col-12 mb-3">
                    <label class="form-label">Horarios disponibles</label>
                    <div class="d-flex flex-wrap gap-2">
                        <asp:Repeater
                            ID="rptHorariosDisponibles"
                            runat="server"
                            OnItemCommand="rptHorariosDisponibles_ItemCommand">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnHorario"
                                    runat="server"
                                    Text='<%# Eval("Texto") %>'
                                    CommandName="SeleccionarHorario"
                                    CommandArgument='<%# Eval("Hora") %>'
                                    CssClass='<%# (ViewState["HoraSeleccionada"] != null &&
                                                   ViewState["HoraSeleccionada"].ToString() == Eval("Hora").ToString())
                                                      ? "btn btn-primary"
                                                      : "btn btn-outline-primary" %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <!-- Observaciones del turno -->
            <div class="col-12 mb-3">
                <label for="txtObservaciones" class="form-label">Observaciones</label>
                <asp:TextBox
                    ID="txtObservaciones"
                    runat="server"
                    CssClass="form-control"
                    TextMode="MultiLine"
                    Rows="3"
                    MaxLength="500">
                </asp:TextBox>
            </div>
            <div class="text-end mt-2">
                <asp:Button ID="btnAsignar" runat="server" Text="Asignar Turno" CssClass="btn btn-primary px-4" OnClick="btnAsignar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
