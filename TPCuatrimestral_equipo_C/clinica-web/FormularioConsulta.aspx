<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FormularioConsulta.aspx.cs" Inherits="clinica_web.FormularioConsulta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:HiddenField ID="hfIdTurno" runat="server" />

    <div class="card shadow-sm mb-4">
        <div class="card-header bg-white">
            <h5 class="mb-0 pt-1">Consulta del turno</h5>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-md-4 mb-3">
                    <label class="form-label">Paciente</label>
                    <asp:TextBox ID="txtPaciente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-4 mb-3">
                    <label class="form-label">Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-4 mb-3">
                    <label class="form-label">Hora</label>
                    <asp:TextBox ID="txtHora" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-12 mb-3">
                    <label class="form-label">Motivo</label>
                    <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>

                <!-- Historial de consultas del paciente -->
                <div class="col-12 mb-3">
                    <h5 class="text-primary">Historial del paciente</h5>

                    <div class="table-responsive">
                        <asp:GridView
                            ID="dgvHistorialPaciente"
                            runat="server"
                            CssClass="table table-striped table-bordered"
                            AutoGenerateColumns="false"
                            EmptyDataText="El paciente no posee consultas anteriores.">

                            <Columns>
                                <asp:BoundField DataField="Fecha"
                                    HeaderText="Fecha"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:BoundField DataField="Especialidad.Nombre"
                                    HeaderText="Especialidad" />

                                <asp:BoundField DataField="Medico.NombreCompleto"
                                    HeaderText="Médico" />

                                <asp:BoundField DataField="Consulta.Motivo"
                                    HeaderText="Motivo" />

                                <asp:BoundField DataField="Consulta.Diagnostico"
                                    HeaderText="Diagnóstico" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </div>
                <div class="col-12 mb-3">
                    <label for="txtDiagnostico" class="form-label">Diagnóstico</label>
                    <asp:TextBox
                        ID="txtDiagnostico"
                        runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="4">
                    </asp:TextBox>
                </div>
            </div>

            <div class="text-end mt-2">
                <a href="~/MisTurnos.aspx" runat="server" class="btn btn-outline-secondary me-2">Volver</a>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar diagnóstico" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
            </div>
        </div>
    </div>

</asp:Content>
