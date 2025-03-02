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
    SELECT 
        MedicoEspecialidad.EspecialidadId, 
        MedicoEspecialidad.MedicoId,
        Especialidades.Nombre
    FROM 
        MedicoEspecialidad
    INNER JOIN 
        Especialidades ON MedicoEspecialidad.EspecialidadId = Especialidades.EspecialidadId
    WHERE 
        MedicoEspecialidad.MedicoId = @MedicoId;
END

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

CREATE PROCEDURE sp_ListarMedicos
AS
BEGIN
    SELECT 
        m.MedicoId,
        u.UsuarioId,
        u.Nombre,
        u.Email,
        u.Telefono,
        u.Activo
    FROM Medicos m
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    WHERE u.Activo = 1; 

    CREATE PROCEDURE sp_AltaMedico
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Password NVARCHAR(MAX),
    @Rol NVARCHAR(50),
    @Especialidades NVARCHAR(MAX) 
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Usuarios (Nombre, Email, Telefono, Password, Rol, Activo)
        VALUES (@Nombre, @Email, @Telefono, @Password, @Rol, 1);

        DECLARE @UsuarioId INT = SCOPE_IDENTITY();

        INSERT INTO Medicos (UsuarioId)
        VALUES (@UsuarioId);

        DECLARE @MedicoId INT = SCOPE_IDENTITY();

        INSERT INTO MedicoEspecialidad (MedicoId, EspecialidadId)
        SELECT @MedicoId, value
        FROM STRING_SPLIT(@Especialidades, ',');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END

CREATE PROCEDURE sp_EditarMedico
    @MedicoId INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(20),
    @Especialidades NVARCHAR(MAX) 
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Usuarios
        SET Nombre = @Nombre,
            Email = @Email,
            Telefono = @Telefono
        WHERE UsuarioId = (SELECT UsuarioId FROM Medicos WHERE MedicoId = @MedicoId);

        DELETE FROM MedicoEspecialidad
        WHERE MedicoId = @MedicoId;

        INSERT INTO MedicoEspecialidad (MedicoId, EspecialidadId)
        SELECT @MedicoId, value
        FROM STRING_SPLIT(@Especialidades, ',');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END

CREATE PROCEDURE sp_BajaMedico
    @MedicoId INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Usuarios
        SET Activo = 0
        WHERE UsuarioId = (SELECT UsuarioId FROM Medicos WHERE MedicoId = @MedicoId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END

CREATE PROCEDURE sp_ListarEspecialidades
AS
BEGIN
    SELECT EspecialidadId, Nombre
    FROM Especialidades;
END

CREATE PROCEDURE sp_EditarMedico
    @MedicoId INT,
    @Nombre VARCHAR(100),
    @Email VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    UPDATE Medicos
    SET Nombre = @Nombre,
        Email = @Email,
        Telefono = @Telefono
    WHERE MedicoId = @MedicoId;
END

CREATE PROCEDURE sp_EliminarEspecialidadesMedico
    @MedicoId INT
AS
BEGIN
    DELETE FROM MedicoEspecialidad
    WHERE MedicoId = @MedicoId;
END

CREATE PROCEDURE sp_InsertarEspecialidadMedico
    @MedicoId INT,
    @EspecialidadId INT
AS
BEGIN
    INSERT INTO MedicoEspecialidad (MedicoId, EspecialidadId)
    VALUES (@MedicoId, @EspecialidadId);
END

CREATE PROCEDURE sp_ObtenerMedicoPorId
    @MedicoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT M.MedicoId, U.Nombre, U.Email, U.Telefono, U.Activo
    FROM Medicos M
    INNER JOIN Usuarios U ON M.UsuarioId = U.UsuarioId
    WHERE M.MedicoId = @MedicoId;
END;