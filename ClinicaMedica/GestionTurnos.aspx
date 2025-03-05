<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionTurnos.aspx.cs" Inherits="ClinicaMedica.GestionTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="col-md-12">
        <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
    </div>
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
        
                    <!-- Botones de Confirmar y Cancelar -->
                    <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="btn btn-success"
                        CommandName="Update" />
                    <asp:Button ID="btnCancelarEdicion" runat="server" Text="Cancelar" CssClass="btn btn-secondary"
                        CommandName="Cancel" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Acciones">
                 <ItemTemplate>
                    <!-- Panel para contener los botones -->
                    <asp:Panel ID="pnlAcciones" runat="server">
                        <!-- Botones -->
                        <asp:Button ID="btnReprogramar" runat="server" Text="Reprogramar" CssClass="btn btn-primary"
                            Visible="false" CommandArgument='<%# Eval("TurnoId") %>' />

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

    <div class="modal fade" id="modalEditarTurno" tabindex="-1" aria-labelledby="modalEditarTurnoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalEditarTurnoLabel">Editar Turno</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hiddenTurnoId" runat="server" />
                    <div class="form-group">
                        <label for="lblPacienteModal">Paciente:</label>
                        <asp:Label ID="lblPacienteModal" runat="server" Text="" CssClass="form-control" ReadOnly="true"></asp:Label>
                    </div>
                    <div class="form-group">
                        <label for="txtFechaModal">Fecha:</label>
                        <asp:TextBox ID="txtFechaModal" runat="server" CssClass="form-control" TextMode="Date" />
                    </div>
                    <div class="form-group">
                        <label for="txtHoraModal">Hora:</label>
                        <asp:TextBox ID="txtHoraModal" runat="server" CssClass="form-control" TextMode="Time" />
                    </div>
                    <div class="form-group">
                        <label for="ddlEstadoModal">Estado:</label>
                        <asp:DropDownList ID="ddlEstadoModal" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Nuevo">Nuevo</asp:ListItem>
                            <asp:ListItem Value="Reprogramado">Reprogramado</asp:ListItem>
                            <asp:ListItem Value="Cancelado">Cancelado</asp:ListItem>
                            <asp:ListItem Value="NoAsistio">No Asistió</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnGuardarModal" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-success" />
                        <asp:Button ID="btnCancelarModal" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-secondary" data-bs-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
