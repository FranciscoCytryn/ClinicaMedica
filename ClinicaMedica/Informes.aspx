<%@ Page Title="Informes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Informes.aspx.cs" Inherits="ClinicaMedica.Informes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Informes</h2>
        <hr />

        <div class="form-group">
            <label for="ddlTipoInforme">Seleccione el tipo de informe:</label>
            <asp:DropDownList ID="ddlTipoInforme" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoInforme_SelectedIndexChanged">
                <asp:ListItem Text="-- Seleccione --" Value="" />
                <asp:ListItem Text="Cantidad de turnos atendidos por médico" Value="TurnosPorMedico" />
                <asp:ListItem Text="Cantidad de atenciones por paciente" Value="PacientesAtendidos" />
            </asp:DropDownList>
        </div>

        <asp:Panel ID="pnlFiltros" runat="server" Visible="false">
            <div class="form-group">
                <label for="txtFechaInicio">Fecha de Inicio:</label>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtFechaFin">Fecha de Fin:</label>
                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnGenerarInforme" runat="server" Text="Generar Informe" CssClass="btn btn-primary" OnClick="btnGenerarInforme_Click" />
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlResultados" runat="server" Visible="false">
            <h4>Resultados del Informe</h4>
            <asp:GridView ID="gvResultados" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="true">
            </asp:GridView>
        </asp:Panel>
        <asp:Button ID="btnDescargarPDF" runat="server" Text="Descargar PDF" CssClass="btn btn-danger" OnClick="btnDescargarPDF_Click" />
    </div>
</asp:Content>