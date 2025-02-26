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

CREATE PROCEDURE sp_ExisteEmailPaciente
    @Email NVARCHAR(100),
    @PacienteId INT = NULL 
AS
BEGIN
    IF @PacienteId IS NULL
    BEGIN
        SELECT COUNT(*) FROM Pacientes WHERE Email = @Email AND Activo = 1;
    END
    ELSE
    BEGIN
        SELECT COUNT(*) FROM Pacientes WHERE Email = @Email AND Activo = 1 AND PacienteId <> @PacienteId;
    END
END

CREATE PROCEDURE AltaPaciente
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @FechaNacimiento DATE,
    @Direccion NVARCHAR(200)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM Pacientes WHERE Email = @Email)
        BEGIN
            RAISERROR('El email ya está registrado.', 16, 1);
            RETURN;
        END;

        INSERT INTO Pacientes (Nombre, Email, Telefono, FechaNacimiento, Direccion)
        VALUES (@Nombre, @Email, @Telefono, @FechaNacimiento, @Direccion);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END