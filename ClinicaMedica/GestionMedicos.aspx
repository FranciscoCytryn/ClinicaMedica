<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="ClinicaMedica.GestionMedicos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Botón para abrir el modal de alta de médico -->
            <asp:Button ID="btnAbrirModal" runat="server" Text="Agregar Médico" CssClass="btn btn-primary" OnClick="btnAbrirModal_Click" />

            <!-- GridView para mostrar los médicos -->
            <asp:GridView ID="gvMedicos" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
                OnRowEditing="gvMedicos_RowEditing" OnRowCancelingEdit="gvMedicos_RowCancelingEdit"
                OnRowUpdating="gvMedicos_RowUpdating" OnRowDeleting="gvMedicos_RowDeleting" DataKeyNames="MedicoId" EnableViewState="true">
                <Columns>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Usuario.Nombre") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" Text='<%# Bind("Usuario.Nombre") %>' CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre"
                                Display="Dynamic"
                                ErrorMessage="Este campo es obligatorio."
                                CssClass="text-danger" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Usuario.Email") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Usuario.Email") %>' CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                ControlToValidate="txtEmail"
                                Display="Dynamic"
                                ErrorMessage="Este campo es obligatorio."
                                CssClass="text-danger" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                Display="Dynamic"
                                ErrorMessage="Formato de email inválido."
                                CssClass="text-danger" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Teléfono">
                        <ItemTemplate>
                            <asp:Label ID="lblTelefono" runat="server" Text='<%# Eval("Usuario.Telefono") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTelefono" runat="server" Text='<%# Bind("Usuario.Telefono") %>' CssClass="form-control" MaxLength="10"
                                onkeypress="return soloNumeros(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                                ControlToValidate="txtTelefono"
                                Display="Dynamic"
                                ErrorMessage="Este campo es obligatorio."
                                CssClass="text-danger" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Especialidades">
                        <ItemTemplate>
                            <asp:Label ID="lblEspecialidades" runat="server" Text='<%# ObtenerEspecialidadesPorMedico((List<Dominio.Especialidad>)Eval("Especialidades")) %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBoxList ID="cblEspecialidades" runat="server" CssClass="form-control" DataTextField="Nombre" DataValueField="EspecialidadId">
                            </asp:CheckBoxList>
                            <asp:CustomValidator ID="cvEspecialidades" runat="server"
                                ErrorMessage="Debe seleccionar al menos una especialidad."
                                CssClass="text-danger"
                                Display="Dynamic"
                                OnServerValidate="cvEspecialidades_ServerValidate">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Delete" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('¿Estás seguro de eliminar este médico?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Update" CssClass="btn btn-success btn-sm" Enabled='<%# Page.IsValid %>' />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" CausesValidation="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Modal de Alta de Médico -->
            <div class="modal fade" id="modalAltaMedico" tabindex="-1" aria-labelledby="modalAltaMedicoLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalAltaMedicoLabel">Alta de Médico</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="txtNombreNuevo">Nombre</label>
                                <asp:TextBox ID="txtNombreNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreNuevo" runat="server"
                                    ControlToValidate="txtNombreNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaMedico" />
                            </div>
                            <div class="form-group">
                                <label for="txtEmailNuevo">Email</label>
                                <asp:TextBox ID="txtEmailNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmailNuevo" runat="server"
                                    ControlToValidate="txtEmailNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaMedico" />
                                <asp:RegularExpressionValidator ID="revEmailNuevo" runat="server"
                                    ControlToValidate="txtEmailNuevo"
                                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                    Display="Dynamic"
                                    ErrorMessage="Formato de email inválido."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaMedico" />
                            </div>
                            <div class="form-group">
                                <label for="txtTelefonoNuevo">Teléfono</label>
                                <asp:TextBox ID="txtTelefonoNuevo" runat="server" CssClass="form-control" MaxLength="10"
                                    onkeypress="return soloNumeros(event);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTelefonoNuevo" runat="server"
                                    ControlToValidate="txtTelefonoNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaMedico" />
                            </div>
                            <div class="form-group">
                                <label for="txtPasswordNuevo">Contraseña</label>
                                <asp:TextBox ID="txtPasswordNuevo" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPasswordNuevo" runat="server"
                                    ControlToValidate="txtPasswordNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaMedico" />
                            </div>
                            <div class="form-group">
                                <label for="cblEspecialidadesNuevo">Especialidades</label>
                                <asp:CheckBoxList ID="cblEspecialidadesNuevo" runat="server" CssClass="form-control" DataTextField="Nombre" DataValueField="EspecialidadId">
                                </asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <asp:Button ID="btnAltaMedico" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAltaMedico_Click" ValidationGroup="AltaMedico" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function validarEspecialidades(source, args) {
            var cblEspecialidades = document.getElementById(source.controltovalidate);
            var checkboxes = cblEspecialidades.getElementsByTagName("input");
            var isChecked = false;

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    isChecked = true;
                    break;
                }
            }
            args.IsValid = isChecked;
        }
    </script>
    <script type="text/javascript">
        function soloNumeros(event) {

            var charCode = (event.which) ? event.which : event.keyCode;
            if ((charCode < 48 || charCode > 57) && (charCode !== 8 && charCode !== 46 && charCode !== 9 && charCode !== 37 && charCode !== 39)) {
                event.preventDefault();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>