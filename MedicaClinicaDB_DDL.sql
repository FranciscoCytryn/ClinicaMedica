CREATE DATABASE ClinicaMedica;
GO
USE ClinicaMedica;
GO

CREATE TABLE Usuarios (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(MAX) NOT NULL,
    Rol NVARCHAR(50) NOT NULL CHECK (Rol IN ('Administrador', 'Recepcionista', 'M�dico')),
    Telefono NVARCHAR(20)
);

CREATE TABLE Especialidades (
    EspecialidadId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Medicos (
    MedicoId INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT UNIQUE, -- Relaci�n 1 a 1 con Usuarios
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

CREATE TABLE MedicoEspecialidad (
    MedicoId INT,
    EspecialidadId INT,
    PRIMARY KEY (MedicoId, EspecialidadId),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(MedicoId),
    FOREIGN KEY (EspecialidadId) REFERENCES Especialidades(EspecialidadId)
);

CREATE TABLE Pacientes (
    PacienteId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(20),
    FechaNacimiento DATE,
    Direccion NVARCHAR(200)
);

CREATE TABLE TurnosTrabajo (
    TurnoTrabajoId INT PRIMARY KEY IDENTITY(1,1),
    MedicoId INT,
    HoraEntrada TIME NOT NULL,
    HoraSalida TIME NOT NULL,
    FOREIGN KEY (MedicoId) REFERENCES Medicos(MedicoId)
);

CREATE TABLE Turnos (
    TurnoId INT PRIMARY KEY IDENTITY(1,1),
    PacienteId INT,
    MedicoId INT,
    Fecha DATE NOT NULL,
    HoraInicio TIME NOT NULL,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Nuevo', 'Reprogramado', 'Cancelado', 'No Asisti�', 'Cerrado')),
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (PacienteId) REFERENCES Pacientes(PacienteId),
    FOREIGN KEY (MedicoId) REFERENCES Medicos(MedicoId)
);

ALTER TABLE Turnos
ADD EspecialidadId INT NULL;

ALTER TABLE Turnos
ADD CONSTRAINT FK_Turnos_Especialidades FOREIGN KEY (EspecialidadId)
REFERENCES Especialidades(EspecialidadId);

UPDATE Turnos
SET EspecialidadId = (
    SELECT TOP 1 EspecialidadId
    FROM MedicoEspecialidad
    WHERE MedicoEspecialidad.MedicoId = Turnos.MedicoId
)
WHERE EspecialidadId IS NULL;


