INSERT INTO Usuarios (Nombre, Email, Password, Rol, Telefono)
VALUES
-- Administrador
('admin admin', 'admin@clinica.com', 'password1', 'Administrador', '123456789'),

-- Recepcionistas
('Muhammad Ali', 'muhammad.ali@clinica.com', 'password2', 'Recepcionista', '111111111'),
('Mike Tyson', 'mike.tyson@clinica.com', 'password3', 'Recepcionista', '222222222'),
('Sugar Ray Leonard', 'sugar.ray@clinica.com', 'password4', 'Recepcionista', '333333333'),
('Floyd Mayweather Jr.', 'floyd.mayweather@clinica.com', 'password5', 'Recepcionista', '444444444'),
('Manny Pacquiao', 'manny.pacquiao@clinica.com', 'password6', 'Recepcionista', '555555555'),

-- M�dicos
('Joe Frazier', 'joe.frazier@clinica.com', 'password7', 'M�dico', '666666666'),
('George Foreman', 'george.foreman@clinica.com', 'password8', 'M�dico', '777777777'),
('Evander Holyfield', 'evander.holyfield@clinica.com', 'password9', 'M�dico', '888888888'),
('Oscar De La Hoya', 'oscar.delahoya@clinica.com', 'password10', 'M�dico', '999999999'),
('Roberto Dur�n', 'roberto.duran@clinica.com', 'password11', 'M�dico', '101010101'),
('Marvin Hagler', 'marvin.hagler@clinica.com', 'password12', 'M�dico', '121212121'),
('Lennox Lewis', 'lennox.lewis@clinica.com', 'password13', 'M�dico', '131313131'),
('Rocky Marciano', 'rocky.marciano@clinica.com', 'password14', 'M�dico', '141414141'),
('Jack Dempsey', 'jack.dempsey@clinica.com', 'password15', 'M�dico', '151515151'),
('Sonny Liston', 'sonny.liston@clinica.com', 'password16', 'M�dico', '161616161'),
('Vitali Klitschko', 'vitali.klitschko@clinica.com', 'password17', 'M�dico', '171717171'),
('Wladimir Klitschko', 'wladimir.klitschko@clinica.com', 'password18', 'M�dico', '181818181'),
('Canelo �lvarez', 'canelo.alvarez@clinica.com', 'password19', 'M�dico', '191919191'),
('Gennady Golovkin', 'gennady.golovkin@clinica.com', 'password20', 'M�dico', '202020202');

INSERT INTO Especialidades (Nombre)
VALUES
('Cardiolog�a'),
('Dermatolog�a'),
('Pediatr�a'),
('Ginecolog�a'),
('Traumatolog�a'),
('Oftalmolog�a'),
('Neurolog�a'),
('Psiquiatr�a'),
('Odontolog�a'),
('Kinesiolog�a');

INSERT INTO Medicos (UsuarioId)
SELECT UsuarioId
FROM Usuarios
WHERE Rol = 'M�dico';

INSERT INTO MedicoEspecialidad (MedicoId, EspecialidadId)
VALUES
-- Joe Frazier: Cardiolog�a y Neurolog�a
(1, 1), (1, 7),
-- George Foreman: Dermatolog�a y Psiquiatr�a
(2, 2), (2, 8),
-- Evander Holyfield: Pediatr�a y Odontolog�a
(3, 3), (3, 9),
-- Oscar De La Hoya: Ginecolog�a y Kinesiolog�a
(4, 4), (4, 10),
-- Roberto Dur�n: Traumatolog�a y Oftalmolog�a
(5, 5), (5, 6),
-- Marvin Hagler: Cardiolog�a y Dermatolog�a
(6, 1), (6, 2),
-- Lennox Lewis: Pediatr�a y Ginecolog�a
(7, 3), (7, 4),
-- Rocky Marciano: Traumatolog�a y Neurolog�a
(8, 5), (8, 7),
-- Jack Dempsey: Oftalmolog�a y Psiquiatr�a
(9, 6), (9, 8),
-- Sonny Liston: Odontolog�a y Kinesiolog�a
(10, 9), (10, 10);

INSERT INTO Pacientes (Nombre, Email, Telefono, FechaNacimiento, Direccion)
VALUES
('Juan P�rez', 'juan.perez@example.com', '212121212', '1980-05-15', '123 Calle Principal, Ciudad'),
('Mar�a Gonz�lez', 'maria.gonzalez@example.com', '232323232', '1985-08-20', '456 Avenida Central, Ciudad'),
('Carlos L�pez', 'carlos.lopez@example.com', '242424242', '1990-02-10', '789 Calle Secundaria, Ciudad'),
('Ana Mart�nez', 'ana.martinez@example.com', '252525252', '1975-11-25', '101 Avenida Norte, Ciudad'),
('Luis Rodr�guez', 'luis.rodriguez@example.com', '262626262', '1988-07-30', '202 Calle Sur, Ciudad'),
('Sof�a Fern�ndez', 'sofia.fernandez@example.com', '272727272', '1995-04-12', '303 Avenida Este, Ciudad'),
('Pedro S�nchez', 'pedro.sanchez@example.com', '282828282', '1982-09-18', '404 Calle Oeste, Ciudad'),
('Laura G�mez', 'laura.gomez@example.com', '292929292', '1978-12-05', '505 Avenida Principal, Ciudad'),
('Jorge D�az', 'jorge.diaz@example.com', '303030303', '1992-03-22', '606 Calle Central, Ciudad'),
('Marta Ruiz', 'marta.ruiz@example.com', '313131313', '1987-06-14', '707 Avenida Secundaria, Ciudad');

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
-- Roberto Dur�n
(5, '07:00', '15:00');

INSERT INTO Turnos (PacienteId, MedicoId, Fecha, HoraInicio, Estado, Observaciones)
VALUES
-- Turnos para Juan P�rez
(1, 1, '2023-10-15', '10:00', 'Nuevo', 'Consulta de rutina'),
(1, 2, '2023-10-16', '11:00', 'Reprogramado', 'Seguimiento dermatol�gico'),
-- Turnos para Mar�a Gonz�lez
(2, 3, '2023-10-17', '12:00', 'Nuevo', 'Control pedi�trico'),
(2, 4, '2023-10-18', '13:00', 'Cerrado', 'Consulta ginecol�gica'),
-- Turnos para Carlos L�pez
(3, 5, '2023-10-19', '14:00', 'Nuevo', 'Revisi�n traumatol�gica'),
(3, 6, '2023-10-20', '15:00', 'Cancelado', 'Consulta cancelada por el paciente'),
-- Turnos para Ana Mart�nez
(4, 7, '2023-10-21', '16:00', 'Nuevo', 'Control pedi�trico'),
(4, 8, '2023-10-22', '17:00', 'No Asisti�', 'Paciente no se present�'),
-- Turnos para Luis Rodr�guez
(5, 9, '2023-10-23', '18:00', 'Nuevo', 'Consulta oftalmol�gica'),
(5, 10, '2023-10-24', '19:00', 'Cerrado', 'Revisi�n odontol�gica');