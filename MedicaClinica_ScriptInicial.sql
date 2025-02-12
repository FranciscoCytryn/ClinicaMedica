INSERT INTO Usuarios (Nombre, Email, Password, Rol, Telefono)
VALUES
-- Administrador
('admin admin', 'admin@clinica.com', 'password1', 'Administrador', '123456789'),

-- Recepcionistas (nombres de boxeadores famosos)
('Muhammad Ali', 'muhammad.ali@clinica.com', 'password2', 'Recepcionista', '111111111'),
('Mike Tyson', 'mike.tyson@clinica.com', 'password3', 'Recepcionista', '222222222'),
('Sugar Ray Leonard', 'sugar.ray@clinica.com', 'password4', 'Recepcionista', '333333333'),
('Floyd Mayweather Jr.', 'floyd.mayweather@clinica.com', 'password5', 'Recepcionista', '444444444'),
('Manny Pacquiao', 'manny.pacquiao@clinica.com', 'password6', 'Recepcionista', '555555555'),

-- Médicos (nombres de boxeadores famosos)
('Joe Frazier', 'joe.frazier@clinica.com', 'password7', 'Médico', '666666666'),
('George Foreman', 'george.foreman@clinica.com', 'password8', 'Médico', '777777777'),
('Evander Holyfield', 'evander.holyfield@clinica.com', 'password9', 'Médico', '888888888'),
('Oscar De La Hoya', 'oscar.delahoya@clinica.com', 'password10', 'Médico', '999999999'),
('Roberto Durán', 'roberto.duran@clinica.com', 'password11', 'Médico', '101010101'),
('Marvin Hagler', 'marvin.hagler@clinica.com', 'password12', 'Médico', '121212121'),
('Lennox Lewis', 'lennox.lewis@clinica.com', 'password13', 'Médico', '131313131'),
('Rocky Marciano', 'rocky.marciano@clinica.com', 'password14', 'Médico', '141414141'),
('Jack Dempsey', 'jack.dempsey@clinica.com', 'password15', 'Médico', '151515151'),
('Sonny Liston', 'sonny.liston@clinica.com', 'password16', 'Médico', '161616161'),
('Vitali Klitschko', 'vitali.klitschko@clinica.com', 'password17', 'Médico', '171717171'),
('Wladimir Klitschko', 'wladimir.klitschko@clinica.com', 'password18', 'Médico', '181818181'),
('Canelo Álvarez', 'canelo.alvarez@clinica.com', 'password19', 'Médico', '191919191'),
('Gennady Golovkin', 'gennady.golovkin@clinica.com', 'password20', 'Médico', '202020202');

-- 3. Insertar datos en la tabla Especialidades
INSERT INTO Especialidades (Nombre)
VALUES
('Cardiología'),
('Dermatología'),
('Pediatría'),
('Ginecología'),
('Traumatología'),
('Oftalmología'),
('Neurología'),
('Psiquiatría'),
('Odontología'),
('Kinesiología');

-- 4. Insertar datos en la tabla Medicos
INSERT INTO Medicos (UsuarioId)
SELECT UsuarioId
FROM Usuarios
WHERE Rol = 'Médico';

-- 5. Insertar datos en la tabla MedicoEspecialidad
INSERT INTO MedicoEspecialidad (MedicoId, EspecialidadId)
VALUES
-- Joe Frazier: Cardiología y Neurología
(1, 1), (1, 7),
-- George Foreman: Dermatología y Psiquiatría
(2, 2), (2, 8),
-- Evander Holyfield: Pediatría y Odontología
(3, 3), (3, 9),
-- Oscar De La Hoya: Ginecología y Kinesiología
(4, 4), (4, 10),
-- Roberto Durán: Traumatología y Oftalmología
(5, 5), (5, 6),
-- Marvin Hagler: Cardiología y Dermatología
(6, 1), (6, 2),
-- Lennox Lewis: Pediatría y Ginecología
(7, 3), (7, 4),
-- Rocky Marciano: Traumatología y Neurología
(8, 5), (8, 7),
-- Jack Dempsey: Oftalmología y Psiquiatría
(9, 6), (9, 8),
-- Sonny Liston: Odontología y Kinesiología
(10, 9), (10, 10);

-- 6. Insertar datos en la tabla Pacientes (nombres genéricos)
INSERT INTO Pacientes (Nombre, Email, Telefono, FechaNacimiento, Direccion)
VALUES
('Juan Pérez', 'juan.perez@example.com', '212121212', '1980-05-15', '123 Calle Principal, Ciudad'),
('María González', 'maria.gonzalez@example.com', '232323232', '1985-08-20', '456 Avenida Central, Ciudad'),
('Carlos López', 'carlos.lopez@example.com', '242424242', '1990-02-10', '789 Calle Secundaria, Ciudad'),
('Ana Martínez', 'ana.martinez@example.com', '252525252', '1975-11-25', '101 Avenida Norte, Ciudad'),
('Luis Rodríguez', 'luis.rodriguez@example.com', '262626262', '1988-07-30', '202 Calle Sur, Ciudad'),
('Sofía Fernández', 'sofia.fernandez@example.com', '272727272', '1995-04-12', '303 Avenida Este, Ciudad'),
('Pedro Sánchez', 'pedro.sanchez@example.com', '282828282', '1982-09-18', '404 Calle Oeste, Ciudad'),
('Laura Gómez', 'laura.gomez@example.com', '292929292', '1978-12-05', '505 Avenida Principal, Ciudad'),
('Jorge Díaz', 'jorge.diaz@example.com', '303030303', '1992-03-22', '606 Calle Central, Ciudad'),
('Marta Ruiz', 'marta.ruiz@example.com', '313131313', '1987-06-14', '707 Avenida Secundaria, Ciudad');

-- 7. Insertar datos en la tabla TurnosTrabajo
INSERT INTO TurnosTrabajo (MedicoId, HoraEntrada, HoraSalida)
VALUES
-- Joe Frazier
(1, '08:00', '16:00'),
-- George Foreman
(2, '09:00', '17:00'),
-- Evander Holyfield
(3, '10:00', '18:00'),
-- Oscar De La Hoya
(4, '08:30', '16:30'),
-- Roberto Durán
(5, '07:00', '15:00');

-- 8. Insertar datos en la tabla Turnos
INSERT INTO Turnos (PacienteId, MedicoId, Fecha, HoraInicio, Estado, Observaciones)
VALUES
-- Turnos para Juan Pérez
(1, 1, '2023-10-15', '10:00', 'Nuevo', 'Consulta de rutina'),
(1, 2, '2023-10-16', '11:00', 'Reprogramado', 'Seguimiento dermatológico'),
-- Turnos para María González
(2, 3, '2023-10-17', '12:00', 'Nuevo', 'Control pediátrico'),
(2, 4, '2023-10-18', '13:00', 'Cerrado', 'Consulta ginecológica'),
-- Turnos para Carlos López
(3, 5, '2023-10-19', '14:00', 'Nuevo', 'Revisión traumatológica'),
(3, 6, '2023-10-20', '15:00', 'Cancelado', 'Consulta cancelada por el paciente'),
-- Turnos para Ana Martínez
(4, 7, '2023-10-21', '16:00', 'Nuevo', 'Control pediátrico'),
(4, 8, '2023-10-22', '17:00', 'No Asistió', 'Paciente no se presentó'),
-- Turnos para Luis Rodríguez
(5, 9, '2023-10-23', '18:00', 'Nuevo', 'Consulta oftalmológica'),
(5, 10, '2023-10-24', '19:00', 'Cerrado', 'Revisión odontológica');