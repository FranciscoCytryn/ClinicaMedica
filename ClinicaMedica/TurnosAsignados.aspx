<%@ Page Title="Turnos Asignados" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TurnosAsignados.aspx.cs" Inherits="ClinicaMedica.TurnosAsignados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-12 text-center">
                <h2>Turnos Asignados</h2>
                <asp:Label ID="lblMensajeError" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <asp:GridView ID="gvTurnosAsignados" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="HoraInicio" HeaderText="Hora" DataFormatString="{0:hh\:mm}" />
                        <asp:BoundField DataField="Paciente.Nombre" HeaderText="Paciente" />
                        <asp:BoundField DataField="Especialidad.Nombre" HeaderText="Especialidad" />
                        <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>