<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="clinica_web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">

        <!-- Encabezado global -->
        <div class="mb-4">
            <h2 class="text-primary mb-1">
                <asp:Label ID="lblSaludo" runat="server"></asp:Label>
            </h2>
            <p class="text-muted mb-0">
                Tipo de usuario:
                <strong>
                    <asp:Label ID="lblTipoUsuario" runat="server"></asp:Label>
                </strong>
            </p>
        </div>


        <!-- PANEL ADMINISTRADOR -->
        <asp:Panel ID="pnlAdministrador" runat="server" Visible="false">

            <!-- SECCIÓN: PERSONAS Y USUARIOS -->
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white">
                    <h5 class="mb-0 text-primary">Personas y usuarios</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        
                        <!-- PACIENTES -->
                        <div class="col-md-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadPacientes" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Pacientes</span>
                            </div>
                        </div>
                        
                        <!-- MEDICOS -->
                        <div class="col-md-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadMedicos" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Médicos</span>
                            </div>
                        </div>
                        
                        <!-- RECEPCIONISTAS -->
                        <div class="col-md-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadRecepcionistas" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Recepcionistas</span>
                            </div>
                        </div>
                        
                        <!-- ADMINISTRADORES -->
                        <div class="col-md-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadAdministradores" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Administradores</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- SECCIÓN: CATÁLOGOS -->
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white">
                    <h5 class="mb-0 text-primary">Especialidades y cobertura</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        
                        <!-- ESPECIALIDADES -->
                        <div class="col-md-6 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadEspecialidades" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Especialidades</span>
                            </div>
                        </div>
                        
                        <!-- OBRAS SOCIALES -->
                        <div class="col-md-6 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblCantidadObrasSociales" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Obras sociales</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- SECCIÓN: TURNOS -->
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white">
                    <h5 class="mb-0 text-primary">Estado de los turnos</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        
                        <!-- COMPLETOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-success rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-success">
                                    <asp:Label ID="lblTurnosCompletos" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Turnos completos</span>
                            </div>
                        </div>
                        
                        <!-- PENDIENTES -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblTurnosPendientesAdmin" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Turnos pendientes</span>
                            </div>
                        </div>
                        
                        <!-- CANCELADOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-danger rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-danger">
                                    <asp:Label ID="lblTurnosCanceladosAdmin" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Turnos cancelados</span>
                            </div>
                        </div>
                        
                        <!-- NO ASISTIDOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-warning rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-warning">
                                    <asp:Label ID="lblTurnosNoAsistidosAdmin" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Turnos no asistidos</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- PANEL RECEPCIONISTA -->
        <asp:Panel ID="pnlRecepcionista" runat="server" Visible="false">
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white">
                    <h5 class="mb-0 text-primary">Turnos de hoy</h5>
                    <small class="text-muted">Resumen de los turnos programados para el día actual.</small>
                </div>
                <div class="card-body">
                    <div class="row">

                        <!-- PENDIENTES -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-primary rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-primary">
                                    <asp:Label ID="lblTurnosPendientes" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Pendientes</span>
                            </div>
                        </div>

                        <!-- CANCELADOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-danger rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-danger">
                                    <asp:Label ID="lblTurnosCancelados" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Cancelados</span>
                            </div>
                        </div>

                        <!-- NO ASISTIDOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-warning rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-warning">
                                    <asp:Label ID="lblTurnosNoAsistidos" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">No asistidos</span>
                            </div>
                        </div>

                        <!-- CERRADOS -->
                        <div class="col-md-6 col-lg-3 mb-3">
                            <div class="border-start border-4 border-success rounded shadow-sm p-3 h-100">
                                <h3 class="mb-1 text-success">
                                    <asp:Label ID="lblTurnosCerrados" runat="server" Text="0"></asp:Label>
                                </h3>
                                <span class="text-muted">Completados</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- PANEL MEDICO -->
        <asp:Panel ID="pnlMedico" runat="server" Visible="false">

            <!-- TURNOS DEL DIA -->
            <div class="card shadow-sm rounded">
                <div class="card-body">
                    <h5 class="text-primary mb-1">Resumen de hoy</h5>
                    <p class="text-muted mb-3">
                        Estado de tus turnos para el día de hoy.
                    </p>
                    
                    <!-- PENDIENTES -->
                    <div class="row g-3">
                        <div class="col-md-3">
                            <div class="border-start border-4 border-primary rounded p-3 shadow-sm">
                                <p class="text-muted mb-1">Pendientes</p>
                                <h3 class="text-primary mb-0">
                                    <asp:Label ID="lblTurnosPendientesMedico" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>
                        
                        <!-- COMPLETADOS -->
                        <div class="col-md-3">
                            <div class="border-start border-4 border-success rounded p-3 shadow-sm">
                                <p class="text-muted mb-1">Completados</p>
                                <h3 class="text-success mb-0">
                                    <asp:Label ID="lblTurnosCompletadosMedico" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>
                        
                        <!-- CANCELADOS -->
                        <div class="col-md-3">
                            <div class="border-start border-4 border-danger rounded p-3 shadow-sm">
                                <p class="text-muted mb-1">Cancelados</p>
                                <h3 class="text-danger mb-0">
                                    <asp:Label ID="lblTurnosCanceladosMedico" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>
                        
                        <!-- NO ASISTIDOS -->
                        <div class="col-md-3">
                            <div class="border-start border-4 border-warning rounded p-3 shadow-sm">
                                <p class="text-muted mb-1">No asistieron</p>
                                <h3 class="text-warning mb-0">
                                    <asp:Label ID="lblTurnosNoAsistidosMedico" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- TURNOS DE LA SEMANA -->
            <div class="card shadow-sm rounded mt-4">
                <div class="card-body">
                    <h5 class="text-primary mb-1">Turnos de la semana</h5>
                    <p class="text-muted mb-3">
                        Cantidad de turnos pendientes por día.
                    </p>

                    <!-- LUNES -->
                    <div class="row g-3">
                        <div class="col">
                            <asp:Panel ID="pnlLunes" runat="server" class="border rounded p-3 text-center">
                                <strong>Lunes</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadLunes" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>

                        <!-- MARTES -->
                        <div class="col">
                            <asp:Panel ID="pnlMartes" runat="server" class="border rounded p-3 text-center">
                                <strong>Martes</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadMartes" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                        
                        <!-- MIERCOLES -->
                        <div class="col">
                            <asp:Panel ID="pnlMiercoles" runat="server" class="border rounded p-3 text-center">
                                <strong>Miércoles</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadMiercoles" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                        
                        <!-- JUEVES -->
                        <div class="col">
                            <asp:Panel ID="pnlJueves" runat="server" class="border rounded p-3 text-center">
                                <strong>Jueves</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadJueves" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                        
                        <!-- VIERNES -->
                        <div class="col">
                            <asp:Panel ID="pnlViernes" runat="server" class="border rounded p-3 text-center">
                                <strong>Viernes</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadViernes" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                        
                        <!-- SABADO -->
                        <div class="col">
                            <asp:Panel ID="pnlSabado" runat="server" class="border rounded p-3 text-center">
                                <strong>Sábado</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadSabado" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                        
                        <!-- DOMINGO -->
                        <div class="col">
                            <asp:Panel ID="pnlDomingo" runat="server" class="border rounded p-3 text-center">
                                <strong>Domingo</strong>
                                <h4 class="mb-0">
                                    <asp:Label ID="lblCantidadDomingo" runat="server" Text="0"></asp:Label>
                                </h4>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
