<%@ Page Title="Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="ClinicaMedica.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-12 text-center">
                <h2>Bienvenido, <asp:Label ID="lblNombreUsuario" runat="server" CssClass="fw-bold"></asp:Label></h2>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnGestionUsuarios" runat="server" Text="Gestión de Usuarios" CssClass="btn btn-primary m-2" OnClick="btnGestionUsuarios_Click" Visible="false" />
                <asp:Button ID="btnGestionTurnos" runat="server" Text="Gestión de Turnos" CssClass="btn btn-primary m-2" OnClick="btnGestionTurnos_Click" Visible="false" />
                <asp:Button ID="btnInformes" runat="server" Text="Informes" CssClass="btn btn-primary m-2" OnClick="btnInformes_Click" Visible="false" />
                <asp:Button ID="btnTurnosAsignados" runat="server" Text="Turnos Asignados" CssClass="btn btn-primary m-2" OnClick="btnTurnosAsignados_Click" Visible="false" />
            </div>
        </div>
    </div>
</asp:Content>