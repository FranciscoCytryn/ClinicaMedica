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

CREATE PROCEDURE [dbo].[sp_ListarTurnos]
AS
BEGIN
    SELECT 
        t.TurnoId, 
        p.Nombre AS NombrePaciente,
        t.Fecha,
        u.Nombre AS NombreMedico,
        e.Nombre AS NombreEspecialidad,  
        t.Estado,
        t.Observaciones,
        t.HoraInicio
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.PacienteId
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    INNER JOIN Especialidades e ON t.EspecialidadId = e.EspecialidadId  
    ORDER BY t.Fecha DESC;  
END


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
    @Telefono NVARCHAR(20)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Usuarios
        SET 
            Nombre = @Nombre,
            Email = @Email,
            Telefono = @Telefono
        WHERE 
            UsuarioId = (SELECT UsuarioId FROM Medicos WHERE MedicoId = @MedicoId);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;

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


CREATE PROCEDURE [dbo].[sp_ListarPacientes]
AS
BEGIN
    SELECT PacienteId, Nombre, Email, Telefono, FechaNacimiento, Direccion
    FROM Pacientes
    WHERE Activo = 1;
END
GO



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

CREATE PROCEDURE sp_ListarEmpleados
AS
BEGIN
    SELECT UsuarioId, Nombre, Email, Telefono, Activo
    FROM Usuarios
    WHERE Rol = 'Recepcionista' AND Activo = 1;
END;

CREATE PROCEDURE sp_AgregarEmpleado
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(50),
    @Contraseña NVARCHAR(100),
    @Activo BIT,
    @Rol NVARCHAR(50)
AS
BEGIN
    INSERT INTO Usuarios (Nombre, Email, Telefono, Password, Activo, Rol)
    VALUES (@Nombre, @Email, @Telefono, @Contraseña, @Activo, @Rol);
END;

CREATE PROCEDURE sp_ModificarEmpleado
    @Id INT,
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @Telefono NVARCHAR(50),
    @Activo BIT
AS
BEGIN
    UPDATE Usuarios
    SET Nombre = @Nombre,
        Email = @Email,
        Telefono = @Telefono,
        Activo = @Activo
    WHERE UsuarioId = @Id;
END;

CREATE PROCEDURE sp_EliminarEmpleado
    @Id INT
AS
BEGIN
    UPDATE Usuarios
    SET Activo = 0
    WHERE UsuarioId = @Id;
END;

CREATE PROCEDURE sp_ObtenerTurnosTrabajoPorMedico
    @MedicoId INT
AS
BEGIN
    SELECT 
        TurnoTrabajoId,
        HoraEntrada,
        HoraSalida
    FROM TurnosTrabajo
    WHERE MedicoId = @MedicoId
    ORDER BY HoraEntrada;
END

CREATE PROCEDURE sp_ListarTurnos
AS
BEGIN
    SELECT 
        t.TurnoId, 
        p.Nombre AS NombrePaciente,
        t.Fecha,
        u.Nombre AS NombreMedico,  
        t.Estado,
        t.Observaciones,
        t.HoraInicio,
        me.EspecialidadId  
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.PacienteId
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId  
    INNER JOIN MedicoEspecialidad me ON m.MedicoId = me.MedicoId  
END

CREATE PROCEDURE ObtenerTurnoPorId
    @TurnoId INT
AS
BEGIN
    SELECT 
        t.TurnoId,
        t.PacienteId,
        p.Nombre AS PacienteNombre,
        t.MedicoId,
        u.Nombre AS MedicoNombre,  
        t.Fecha,
        t.HoraInicio,
        t.Estado,
        t.Observaciones,
        t.EspecialidadId
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.PacienteId   
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    WHERE t.TurnoId = @TurnoId;
END

CREATE PROCEDURE sp_ReprogramarTurno
    @TurnoId INT,
    @NuevaFecha DATETIME,
    @NuevaHora TIME
AS
BEGIN
    UPDATE Turnos
    SET Fecha = @NuevaFecha,
        HoraInicio = @NuevaHora,
        Estado = 'Reprogramado'
    WHERE TurnoId = @TurnoId;
END

CREATE PROCEDURE sp_CancelarTurno
    @TurnoId INT
AS
BEGIN
    UPDATE Turnos
    SET Estado = 'Cancelado'
    WHERE TurnoId = @TurnoId;
END

CREATE PROCEDURE sp_NoAsistioTurno
    @TurnoId INT
AS
BEGIN
    UPDATE Turnos
    SET Estado = 'No Asistió'
    WHERE TurnoId = @TurnoId
    AND Fecha < GETDATE();
END

CREATE PROCEDURE sp_ObtenerEspecialidadPorId
    @Id INT
AS
BEGIN
    SELECT EspecialidadId, Nombre
    FROM Especialidades
    WHERE EspecialidadId = @Id
END

CREATE PROCEDURE sp_ObtenerEspecialidadesPorMedicoId
    @MedicoId INT
AS
BEGIN
    SELECT 
        e.EspecialidadId,
        e.Nombre,
        MIN(t.HoraEntrada) AS HoraInicio, 
        MAX(t.HoraSalida) AS HoraFin
    FROM 
        Especialidades e
    INNER JOIN 
        MedicoEspecialidad me ON e.EspecialidadId = me.EspecialidadId
    INNER JOIN 
        TurnosTrabajo t ON me.MedicoId = t.MedicoId
    WHERE 
        me.MedicoId = @MedicoId
    GROUP BY 
        e.EspecialidadId, e.Nombre
END

CREATE PROCEDURE sp_ObtenerTurnoPorId
    @TurnoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        t.TurnoId,
        t.Fecha,
        t.HoraInicio,
        t.Estado,
        t.PacienteId,
        me.MedicoId,
        u.Nombre AS NombreMedico,
        e.EspecialidadId,
        e.Nombre AS NombreEspecialidad
    FROM Turnos t
    JOIN MedicoEspecialidad me ON t.EspecialidadId = me.EspecialidadId
    JOIN Usuarios u ON me.MedicoId = u.UsuarioId
    JOIN Especialidades e ON t.EspecialidadId = e.EspecialidadId
    WHERE t.TurnoId = @TurnoId;
END;

CREATE PROCEDURE sp_CerrarTurno
    @TurnoId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Turnos
    SET Estado = 'Cerrado'
    WHERE TurnoId = @TurnoId;
END;

CREATE PROCEDURE sp_ActualizarObservacionTurno
    @TurnoId INT,
    @Observaciones NVARCHAR(MAX)
AS
BEGIN
    UPDATE Turnos
    SET Observaciones = @Observaciones
    WHERE TurnoId = @TurnoId;
END

CREATE PROCEDURE sp_ActualizarEstadoTurno
    @TurnoId INT,
    @Estado VARCHAR(50)
AS
BEGIN
    UPDATE Turnos
    SET Estado = @Estado
    WHERE TurnoId = @TurnoId;
END
GO

CREATE PROCEDURE sp_ListarMedicosPorEspecialidad
    @EspecialidadId INT
AS
BEGIN
    SELECT m.MedicoId, u.Nombre
    FROM Medicos m
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    INNER JOIN MedicoEspecialidad me ON m.MedicoId = me.MedicoId
    WHERE me.EspecialidadId = @EspecialidadId
      AND u.Activo = 1; 
END

CREATE PROCEDURE sp_VerificarDisponibilidadMedico
    @MedicoId INT,
    @Fecha DATE,
    @Hora TIME
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Turnos
        WHERE MedicoId = @MedicoId
          AND Fecha = @Fecha
          AND HoraInicio = @Hora
    )
    BEGIN
        SELECT 0 AS Disponible; 
    END
    ELSE
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM TurnosTrabajo
            WHERE MedicoId = @MedicoId
              AND @Hora >= HoraEntrada
              AND @Hora <= HoraSalida
        )
        BEGIN
            SELECT 1 AS Disponible;
        END
        ELSE
        BEGIN
            SELECT 0 AS Disponible; 
        END
    END
END

CREATE PROCEDURE sp_ListarTurnosPorMedicoYFecha
    @MedicoId INT,
    @Fecha DATE
AS
BEGIN
    SELECT 
        t.TurnoId,
        t.Fecha,
        t.HoraInicio,
        t.Estado,
        t.Observaciones,
        p.PacienteId,
        p.Nombre AS NombrePaciente,
        m.MedicoId,
        u.Nombre AS NombreMedico
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.PacienteId
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    WHERE t.MedicoId = @MedicoId
      AND t.Fecha = @Fecha
      AND t.Estado NOT IN ('Cancelado', 'No Asistió');
END

CREATE PROCEDURE sp_GuardarTurno
    @PacienteId INT,
    @MedicoId INT,
    @EspecialidadId INT,
    @Fecha DATE,
    @Hora TIME,
    @Estado NVARCHAR(50),
    @Observaciones NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Turnos (PacienteId, MedicoId, EspecialidadId, Fecha, HoraInicio, Estado, Observaciones)
    VALUES (@PacienteId, @MedicoId, @EspecialidadId, @Fecha, @Hora, @Estado, @Observaciones);
END

CREATE PROCEDURE [dbo].[sp_ActualizarTurno]
    @TurnoId INT,
    @Fecha DATE,
    @HoraInicio TIME,
    @Observaciones NVARCHAR(MAX),
    @Estado NVARCHAR(50)
AS
BEGIN
    UPDATE Turnos
    SET 
        Fecha = @Fecha,
        HoraInicio = @HoraInicio,
        Observaciones = @Observaciones,
        Estado = @Estado
    WHERE 
        TurnoId = @TurnoId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('No se encontró el turno con el ID especificado.', 16, 1);
        RETURN;
    END
END
GO

CREATE PROCEDURE [dbo].[sp_ListarTurnosPorMedico]
    @MedicoId INT
AS
BEGIN
    SELECT 
        t.TurnoId, 
        p.Nombre AS NombrePaciente,
        t.Fecha,
        u.Nombre AS NombreMedico,
        e.Nombre AS NombreEspecialidad,  
        t.Estado,
        t.Observaciones,
        t.HoraInicio
    FROM Turnos t
    INNER JOIN Pacientes p ON t.PacienteId = p.PacienteId
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    INNER JOIN Especialidades e ON t.EspecialidadId = e.EspecialidadId  
    WHERE t.MedicoId = @MedicoId  
    ORDER BY t.Fecha DESC;  
END

CREATE PROCEDURE [dbo].[sp_ObtenerPacientePorId]
    @PacienteId INT
AS
BEGIN
    SELECT 
        Nombre,
        Email
    FROM Pacientes
    WHERE PacienteId = @PacienteId AND Activo = 1;  
END

CREATE PROCEDURE sp_ActualizarTurnoTrabajo
    @MedicoId INT,
    @HoraEntrada TIME,
    @HoraSalida TIME
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS (SELECT 1 FROM TurnosTrabajo WHERE MedicoId = @MedicoId)
        BEGIN
            UPDATE TurnosTrabajo
            SET 
                HoraEntrada = @HoraEntrada,
                HoraSalida = @HoraSalida
            WHERE 
                MedicoId = @MedicoId;
        END
        ELSE
        BEGIN
            INSERT INTO TurnosTrabajo (MedicoId, HoraEntrada, HoraSalida)
            VALUES (@MedicoId, @HoraEntrada, @HoraSalida);
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;

CREATE PROCEDURE sp_ObtenerPacientesAtendidos
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        p.PacienteId,
        p.Nombre AS PacienteNombre,
        p.Email,
        p.Telefono,
        COUNT(t.TurnoId) AS CantidadAtenciones
    FROM Pacientes p
    INNER JOIN Turnos t ON p.PacienteId = t.PacienteId
    WHERE t.Estado = 'Cerrado' 
      AND t.Fecha BETWEEN @FechaInicio AND @FechaFin
    GROUP BY p.PacienteId, p.Nombre, p.Email, p.Telefono
    ORDER BY CantidadAtenciones DESC;
END;

CREATE PROCEDURE sp_ListarTurnosPorMedico
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        m.MedicoId,
        u.Nombre AS MedicoNombre,
        COUNT(t.TurnoId) AS CantidadTurnos
    FROM Turnos t
    INNER JOIN Medicos m ON t.MedicoId = m.MedicoId
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    WHERE t.Fecha BETWEEN @FechaInicio AND @FechaFin
    GROUP BY m.MedicoId, u.Nombre
    ORDER BY CantidadTurnos DESC;
END;


CREATE PROCEDURE sp_ObtenerInformeMedicosConTurnosCerrados
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SELECT 
        m.MedicoId,
        u.Nombre AS MedicoNombre,
        STRING_AGG(e.Nombre, ', ') AS Especialidades,
        COUNT(t.TurnoId) AS CantidadTurnosCerrados
    FROM Medicos m
    INNER JOIN Usuarios u ON m.UsuarioId = u.UsuarioId
    INNER JOIN MedicoEspecialidad me ON m.MedicoId = me.MedicoId
    INNER JOIN Especialidades e ON me.EspecialidadId = e.EspecialidadId
    LEFT JOIN Turnos t ON m.MedicoId = t.MedicoId AND t.Estado = 'Cerrado' AND t.Fecha BETWEEN @FechaInicio AND @FechaFin
    WHERE u.Activo = 1 
    GROUP BY m.MedicoId, u.Nombre;
END;