CREATE PROCEDURE sp_ValidarCredenciales
    @Email NVARCHAR(100),
    @Password NVARCHAR(MAX)
AS
BEGIN
    SELECT 
        UsuarioId, 
        Nombre, 
        Email, 
        Password, 
        Rol, 
        Telefono
    FROM 
        Usuarios
    WHERE 
        Email = @Email 
        AND Password = @Password;
END