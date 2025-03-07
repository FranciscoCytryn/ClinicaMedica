<%@ Page Title="Gestión de Empleados" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionEmpleados.aspx.cs" Inherits="ClinicaMedica.GestionEmpleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnAbrirModal" runat="server" Text="Agregar Empleado" CssClass="btn btn-primary" OnClick="btnAbrirModal_Click" />

            <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
                OnRowEditing="gvEmpleados_RowEditing" OnRowCancelingEdit="gvEmpleados_RowCancelingEdit"
                OnRowUpdating="gvEmpleados_RowUpdating" OnRowDeleting="gvEmpleados_RowDeleting" DataKeyNames="UsuarioId" EnableViewState="true">
                <Columns>
                    <asp:TemplateField HeaderText="Nombre">
                        <ItemTemplate>
                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" Text='<%# Bind("Nombre") %>' CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre"
                                Display="Dynamic"
                                ErrorMessage="Este campo es obligatorio."
                                CssClass="text-danger" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control"></asp:TextBox>
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
                            <asp:Label ID="lblTelefono" runat="server" Text='<%# Eval("Telefono") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtTelefono" runat="server" Text='<%# Bind("Telefono") %>' CssClass="form-control" MaxLength="10"
                                onkeypress="return soloNumeros(event);"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                                ControlToValidate="txtTelefono"
                                Display="Dynamic"
                                ErrorMessage="Este campo es obligatorio."
                                CssClass="text-danger" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Delete" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('¿Estás seguro de eliminar este empleado?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Update" CssClass="btn btn-success btn-sm" Enabled='<%# Page.IsValid %>' />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CommandName="Cancel" CssClass="btn btn-secondary btn-sm" CausesValidation="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="modal fade" id="modalAltaEmpleado" tabindex="-1" aria-labelledby="modalAltaEmpleadoLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalAltaEmpleadoLabel">Alta de Empleado</h5>
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
                                    ValidationGroup="AltaEmpleado" />
                            </div>
                            <div class="form-group">
                                <label for="txtEmailNuevo">Email</label>
                                <asp:TextBox ID="txtEmailNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmailNuevo" runat="server"
                                    ControlToValidate="txtEmailNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaEmpleado" />
                                <asp:RegularExpressionValidator ID="revEmailNuevo" runat="server"
                                    ControlToValidate="txtEmailNuevo"
                                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                                    Display="Dynamic"
                                    ErrorMessage="Formato de email inválido."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaEmpleado" />
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
                                    ValidationGroup="AltaEmpleado" />
                            </div>
                            <div class="form-group">
                                <label for="txtPasswordNuevo">Contraseña</label>
                                <asp:TextBox ID="txtPasswordNuevo" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPasswordNuevo" runat="server"
                                    ControlToValidate="txtPasswordNuevo"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger"
                                    ValidationGroup="AltaEmpleado" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <asp:Button ID="btnAltaEmpleado" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAltaEmpleado_Click" ValidationGroup="AltaEmpleado" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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