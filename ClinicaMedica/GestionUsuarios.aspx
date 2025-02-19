<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="ClinicaMedica.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-12 text-center">
                <h2>Gestión de Usuarios</h2>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnGestionPacientes" runat="server" Text="Gestionar Pacientes" CssClass="btn btn-primary m-2" OnClick="btnGestionPacientes_Click" />
            </div>
        </div>
    </div>
</asp:Content>