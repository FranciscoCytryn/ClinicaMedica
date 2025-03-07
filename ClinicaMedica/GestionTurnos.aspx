<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionTurnos.aspx.cs" Inherits="ClinicaMedica.GestionTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="col-md-12">
        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>
    <asp:Button ID="btnAltaTurno" runat="server" Text="Alta Turno" CssClass="btn btn-success mb-3" 
        OnClick="btnAltaTurno_Click" />
    <asp:GridView ID="gvTurnos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
    DataKeyNames="TurnoId" EnableViewState="true" OnRowDataBound="gvTurnos_RowDataBound" OnRowUpdating="gvTurnos_RowUpdating" OnRowCancelingEdit="gvTurnos_RowCancelingEdit">
    <Columns>
        <asp:TemplateField HeaderText="Paciente">
            <ItemTemplate>
                <asp:Label ID="lblPacienteGrid" runat="server" Text='<%# Eval("Paciente.Nombre") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Fecha">
            <ItemTemplate>
                <asp:Label ID="lblFechaGrid" runat="server" Text='<%# Eval("Fecha", "{0:dd/MM/yyyy}") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Horario">
            <ItemTemplate>
                <asp:Label ID="lblHorarioGrid" runat="server" Text='<%# Eval("HoraInicio", "{0:hh\\:mm}") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Médico">
            <ItemTemplate>
                <asp:Label ID="lblMedicoGrid" runat="server" Text='<%# Eval("Medico.Usuario.Nombre") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Especialidad">
            <ItemTemplate>
                <asp:Label ID="lblEspecialidadGrid" runat="server" Text='<%# Eval("Especialidad.Nombre") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Estado">
            <ItemTemplate>
                <asp:Label ID="lblEstadoGrid" runat="server" 
                    Text='<%# Eval("Estado").ToString() == "NoAsistio" ? "No Asistió" : Eval("Estado") %>'>
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Observaciones">
            <ItemTemplate>
                <asp:Label ID="lblObservaciones" runat="server" Text='<%# Eval("Observaciones") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtObservaciones" runat="server" Text='<%# Bind("Observaciones") %>' TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server" ControlToValidate="txtObservaciones"
                    ErrorMessage="La observación es obligatoria." Display="Dynamic" CssClass="text-danger" />
        
                <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="btn btn-success"
                    CommandName="Update" />
                <asp:Button ID="btnCancelarEdicion" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                    CommandName="Cancel" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>
                <asp:Panel ID="pnlAcciones" runat="server">
                    <asp:Button ID="btnReprogramar" runat="server" Text="Reprogramar" CssClass="btn btn-primary"
                        Visible="false" CommandArgument='<%# Eval("TurnoId") %>' OnClick="btnReprogramar_Click" />

                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Turno" CssClass="btn btn-danger"
                        OnClick="btnCancelarTurno_Click" CommandArgument='<%# Eval("TurnoId") %>' Visible="false" />

                    <asp:Button ID="btnCerrar" runat="server" Text="Marcar como Cerrado" CssClass="btn btn-success"
                        OnClick="btnCerrarTurno_Click" CommandArgument='<%# Eval("TurnoId") %>' Visible="false" />

                    <asp:Button ID="btnNoAsistio" runat="server" Text="Marcar como No Asistió" CssClass="btn btn-warning"
                        OnClick="btnNoAsistioTurno_Click" CommandArgument='<%# Eval("TurnoId") %>' Visible="false" />
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

     <div class="mt-3">
        <asp:Label ID="Label1" runat="server" CssClass="alert alert-success" Visible="false"></asp:Label>
    </div>

   <div class="modal fade" id="modalReprogramar" tabindex="-1" aria-labelledby="modalReprogramarLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalReprogramarLabel">Reprogramar Turno</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="hiddenTurnoId" runat="server" />
                        
                        <div class="form-group">
                            <label for="txtFechaReprogramar">Fecha:</label>
                            <asp:TextBox ID="txtFechaReprogramar" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="ValidarReprogramacion"></asp:TextBox>
                            <asp:Label ID="lblErrorFecha" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </div>

                        <div class="form-group">
                            <label for="ddlHoraReprogramar">Hora:</label>
                            <asp:DropDownList ID="ddlHoraReprogramar" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label for="txtObservacionReprogramar">Observación:</label>
                            <asp:TextBox ID="txtObservacionReprogramar" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" AutoPostBack="true" OnTextChanged="ValidarReprogramacion"></asp:TextBox>
                            <asp:Label ID="lblErrorObservacion" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="btnConfirmarReprogramar" runat="server" Text="Confirmar" CssClass="btn btn-primary" OnClick="btnConfirmarReprogramar_Click" Enabled="false" />
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
