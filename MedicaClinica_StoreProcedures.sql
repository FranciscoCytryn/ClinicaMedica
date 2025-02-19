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

CREATE PROCEDURE sp_ObtenerEspecialidadesPorMedico
    @MedicoId INT
AS
BEGIN
    SELECT e.Nombre
    FROM Especialidades e
    INNER JOIN MedicoEspecialidad me ON e.EspecialidadId = me.EspecialidadId
    WHERE me.MedicoId = @MedicoId;
END

CREATE PROCEDURE sp_ListarPacientes
AS
BEGIN
    SELECT PacienteId, Nombre, Email, Telefono, FechaNacimiento, Direccion
    FROM Pacientes
    WHERE Activo = 1;
END
GO

CREATE PROCEDURE sp_ActualizarPaciente
    @PacienteId INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @FechaNacimiento DATE,
    @Direccion NVARCHAR(200)
AS
BEGIN
    UPDATE Pacientes
    SET Nombre = @Nombre,
        Email = @Email,
        Telefono = @Telefono,
        FechaNacimiento = @FechaNacimiento,
        Direccion = @Direccion
    WHERE PacienteId = @PacienteId;
END
GO

CREATE PROCEDURE sp_EliminarPaciente
    @PacienteId INT
AS
BEGIN
    UPDATE Pacientes
    SET Activo = 0
    WHERE PacienteId = @PacienteId;
END
GO