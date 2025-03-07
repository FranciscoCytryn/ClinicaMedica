<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AltaTurno.aspx.cs" Inherits="ClinicaMedica.AltaTurno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Alta de Turno</h2>

        <div class="form-group">
            <label for="ddlPaciente">Seleccionar Paciente:</label>
            <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-control" AutoPostBack="true"
                OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnNuevoPaciente" runat="server" Text="Nuevo Paciente" CssClass="btn btn-secondary"
                OnClick="btnNuevoPaciente_Click" />
        </div>

        <div class="form-group">
            <label for="ddlEspecialidad">Seleccionar Especialidad:</label>
            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" AutoPostBack="true"
                OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="ddlMedico">Seleccionar Médico:</label>
            <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-control" AutoPostBack="true"
                OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtFecha">Fecha:</label>
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true"
                OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="ddlHora">Hora:</label>
            <asp:DropDownList ID="ddlHora" runat="server" CssClass="form-control" AutoPostBack="true"
                OnSelectedIndexChanged="ddlHora_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtObservacion">Observación:</label>
            <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"
                Enabled="false"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Button ID="btnVolverPasoAnterior" runat="server" Text="Volver al paso anterior" CssClass="btn btn-warning"
                OnClick="btnVolverPasoAnterior_Click" Visible="false" />
        </div>

        <div class="form-group">
            <asp:Button ID="btnConfirmarTurno" runat="server" Text="Confirmar Turno" CssClass="btn btn-primary"
                OnClick="btnConfirmarTurno_Click" Enabled="false" />
        </div>
        <asp:Button ID="btnConfirmarHidden" runat="server" Style="display: none;" OnClick="btnConfirmarHidden_Click" />
    </div>
    <div class="mt-3">
        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>
</asp:Content>
