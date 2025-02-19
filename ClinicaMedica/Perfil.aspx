<%@ Page Title="Perfil" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="ClinicaMedica.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-8 offset-md-2">
                <h2 class="text-center mb-4">Perfil de Usuario</h2>
                <div class="card">
                    <div class="card-body">
                        <div class="form-group">
                            <label><strong>Nombre y Apellido:</strong></label>
                            <asp:Label ID="lblNombre" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label><strong>Email:</strong></label>
                            <asp:Label ID="lblEmail" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label><strong>Teléfono:</strong></label>
                            <asp:Label ID="lblTelefono" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label><strong>Rol:</strong></label>
                            <asp:Label ID="lblRol" runat="server" CssClass="form-control-plaintext"></asp:Label>
                        </div>
                        <asp:Panel ID="pnlEspecialidades" runat="server" Visible="false">
                            <div class="form-group">
                                <label><strong>Especialidades:</strong></label>
                                <asp:Label ID="lblEspecialidades" runat="server" CssClass="form-control-plaintext"></asp:Label>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>