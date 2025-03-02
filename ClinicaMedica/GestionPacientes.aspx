<%@ Page Title="Gestión de Pacientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="ClinicaMedica.GestionPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-12 text-center">
                <h2>Gestión de Pacientes</h2>
            </div>
        </div>
       <div class="row mt-4">
            <div class="col-md-12">
                <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row mt-4">
            <div class="col-md-12">
                <asp:GridView ID="gvPacientes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped"
                    OnRowEditing="gvPacientes_RowEditing" OnRowCancelingEdit="gvPacientes_RowCancelingEdit"
                    OnRowUpdating="gvPacientes_RowUpdating" OnRowDeleting="gvPacientes_RowDeleting" DataKeyNames="PacienteId">
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre y apellido">
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

                        <asp:TemplateField HeaderText="Fecha de Nacimiento">
                            <ItemTemplate>
                                <asp:Label ID="lblFechaNacimiento" runat="server" Text='<%# Eval("FechaNacimiento", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFechaNacimiento" runat="server" Text='<%# Bind("FechaNacimiento", "{0:dd/MM/yyyy}") %>' CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server"
                                    ControlToValidate="txtFechaNacimiento"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger" />
                                <asp:CompareValidator ID="cvFechaNacimiento" runat="server"
                                    ControlToValidate="txtFechaNacimiento"
                                    Operator="DataTypeCheck"
                                    Type="Date"
                                    Display="Dynamic"
                                    ErrorMessage="Formato de fecha inválido (dd/MM/yyyy)."
                                    CssClass="text-danger" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Dirección">
                            <ItemTemplate>
                                <asp:Label ID="lblDireccion" runat="server" Text='<%# Eval("Direccion") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDireccion" runat="server" Text='<%# Bind("Direccion") %>' CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDireccion" runat="server"
                                    ControlToValidate="txtDireccion"
                                    Display="Dynamic"
                                    ErrorMessage="Este campo es obligatorio."
                                    CssClass="text-danger" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnEditar" runat="server" Text="Editar" CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Delete" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('¿Estás seguro de eliminar este paciente?');" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Update" CssClass="btn btn-success btn-sm" Enabled='<%# Page.IsValid %>' />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CommandName="Cancel" CssClass="btn btn-secondary btn-sm"  CausesValidation="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>    
                    </Columns>
                </asp:GridView>
            </div>
        </div>
            <div class="row mt-4">
                <div class="col-md-12 text-center">
                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#modalAltaPaciente">
                        Alta Paciente
                    </button>
                </div>
            </div>
        </div>
    <div class="modal fade" id="modalAltaPaciente" tabindex="-1" aria-labelledby="modalAltaPacienteLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalAltaPacienteLabel">Alta de Paciente</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtNombreNuevo">Nombre y apellido</label>
                        <asp:TextBox ID="txtNombreNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombreNuevo" runat="server"
                            ControlToValidate="txtNombreNuevo"
                            Display="Dynamic"
                            ErrorMessage="Este campo es obligatorio."
                            CssClass="text-danger"
                            ValidationGroup="AltaPaciente"/>
                    </div>
                    <div class="form-group">
                        <label for="txtEmailNuevo">Email</label>
                        <asp:TextBox ID="txtEmailNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmailNuevo" runat="server"
                            ControlToValidate="txtEmailNuevo"
                            Display="Dynamic"
                            ErrorMessage="Este campo es obligatorio."
                            CssClass="text-danger"
                            ValidationGroup="AltaPaciente"/>
                        <asp:RegularExpressionValidator ID="revEmailNuevo" runat="server"
                            ControlToValidate="txtEmailNuevo"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                            Display="Dynamic"
                            ErrorMessage="Formato de email inválido."
                            CssClass="text-danger"
                            ValidationGroup="AltaPaciente"/>
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
                            ValidationGroup="AltaPaciente"/>
                    </div>
                    <div class="form-group">
                        <label for="txtFechaNacimientoNuevo">Fecha de Nacimiento</label>
                        <asp:TextBox ID="txtFechaNacimientoNuevo" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFechaNacimientoNuevo" runat="server"
                            ControlToValidate="txtFechaNacimientoNuevo"
                            Display="Dynamic"
                            ErrorMessage="Este campo es obligatorio."
                            CssClass="text-danger"
                            ValidationGroup="AltaPaciente"/>
                    </div>
                    <div class="form-group">
                        <label for="txtDireccionNuevo">Dirección</label>
                        <asp:TextBox ID="txtDireccionNuevo" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDireccionNuevo" runat="server"
                            ControlToValidate="txtDireccionNuevo"
                            Display="Dynamic"
                            ErrorMessage="Este campo es obligatorio."
                            CssClass="text-danger"
                            ValidationGroup="AltaPaciente"/>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button ID="btnAltaPaciente" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnAltaPaciente_Click"  ValidationGroup="AltaPaciente" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
    function soloNumeros(event) {

        var charCode = (event.which) ? event.which : event.keyCode;
        if ((charCode < 48 || charCode > 57) && (charCode !== 8 && charCode !== 46 && charCode !== 9 && charCode !== 37 && charCode !== 39))
        {
            event.preventDefault();
            return false;
        }
        return true;
    }
    </script>
</asp:Content>