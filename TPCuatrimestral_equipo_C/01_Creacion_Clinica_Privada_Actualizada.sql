USE master;
GO

IF DB_ID('clinica_privada') IS NOT NULL
BEGIN
    ALTER DATABASE clinica_privada
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP DATABASE clinica_privada;
END
GO

CREATE DATABASE clinica_privada
GO

USE clinica_privada;
GO

CREATE TABLE especialidades (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(MAX) NULL,
    activo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE obras_sociales (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    activo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE estados_turno (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(150) NULL
);
GO

CREATE TABLE medicos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    matricula VARCHAR(50) NOT NULL UNIQUE,
    activo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE medicos_especialidades (
    id_medico INT NOT NULL,
    id_especialidad INT NOT NULL,

    CONSTRAINT pk_medicos_especialidades
        PRIMARY KEY (id_medico, id_especialidad),

    CONSTRAINT fk_medesp_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id),

    CONSTRAINT fk_medesp_especialidades
        FOREIGN KEY (id_especialidad)
        REFERENCES especialidades (id)
);
GO

CREATE TABLE pacientes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_obra_social INT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    fecha_nacimiento DATE NOT NULL,
    telefono VARCHAR(30) NULL,
    email VARCHAR(150) NOT NULL,
    direccion VARCHAR(255) NULL,
    nro_afiliado VARCHAR(50) NULL,
    activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT fk_pacientes_obras_sociales
        FOREIGN KEY (id_obra_social)
        REFERENCES obras_sociales (id)
);
GO

CREATE TABLE antecedentes_medicos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(MAX) NOT NULL,

    CONSTRAINT fk_antecedentes_pacientes
        FOREIGN KEY (id_paciente)
        REFERENCES pacientes (id)
);
GO

CREATE TABLE turnos_trabajo (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_medico INT NOT NULL,
    dia_semana VARCHAR(20) NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT fk_turnostrabajo_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id)
);
GO


CREATE TABLE turnos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_medico INT NOT NULL,
    id_especialidad INT NOT NULL,
    id_estado_turno INT NOT NULL,
    fecha_turno DATE NOT NULL,
    hora_turno TIME NOT NULL,
    observaciones VARCHAR(500) NULL,

    CONSTRAINT fk_turnos_pacientes
        FOREIGN KEY (id_paciente)
        REFERENCES pacientes (id),

    CONSTRAINT fk_turnos_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id),

    CONSTRAINT fk_turnos_especialidades
        FOREIGN KEY (id_especialidad)
        REFERENCES especialidades (id),

    CONSTRAINT fk_turnos_estados
        FOREIGN KEY (id_estado_turno)
        REFERENCES estados_turno (id)

    
);
GO
-- Evita turnos duplicados del medico,
-- ignorando los turnos cancelados.
CREATE UNIQUE INDEX uq_turnos_medico_fecha_hora
ON dbo.turnos (id_medico, fecha_turno, hora_turno)
WHERE id_estado_turno <> 3;
GO

-- Evita turnos duplicados del paciente,
-- ignorando los turnos cancelados.
CREATE UNIQUE INDEX uq_turnos_paciente_fecha_hora
ON dbo.turnos (id_paciente, fecha_turno, hora_turno)
WHERE id_estado_turno <> 3;
GO


CREATE TABLE consultas (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_turno INT NOT NULL UNIQUE,
    motivo VARCHAR(255) NOT NULL,
    diagnostico VARCHAR(MAX) NULL,

    CONSTRAINT fk_consultas_turnos
        FOREIGN KEY (id_turno)
        REFERENCES turnos (id)
);
GO

CREATE TABLE usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    contrasenia VARCHAR(255) NOT NULL,
    tipo VARCHAR(20) NOT NULL
        CONSTRAINT ck_usuarios_tipo
        CHECK (tipo IN ('Administrador', 'Recepcionista', 'Medico')),
    id_medico INT NULL,
    activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT fk_usuarios_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id)
);
GO

INSERT INTO estados_turno (nombre, descripcion) VALUES
('Nuevo', 'Turno agendado a la espera de la fecha u horario de atención'),
('Reprogramado', 'El turno fue movido a otra fecha u horario'),
('Cancelado', 'El turno fue dado de baja por el paciente o la clínica'),
('No Asistió', 'El paciente no se presentó al turno'),
('Cerrado', 'El paciente asistió y el turno quedó cerrado');
GO

INSERT INTO obras_sociales (nombre) VALUES
('OSDE'),
('Swiss Medical'),
('Galeno'),
('PAMI'),
('IOMA');
GO

INSERT INTO especialidades (nombre, descripcion) VALUES
('Clínica Médica', 'Atención médica general y controles de salud'),
('Pediatría', 'Atención de niños y adolescentes'),
('Cardiología', 'Diagnóstico y tratamiento del corazón'),
('Dermatología', 'Diagnóstico y tratamiento de la piel'),
('Odontología', 'Salud bucal y tratamientos dentales'),
('Traumatología', 'Lesiones del sistema óseo y muscular');
GO

INSERT INTO medicos (nombre, apellido, matricula) VALUES
('Laura', 'Gómez', 'MN-100234'),
('Carlos', 'Pereyra', 'MN-100567'),
('Marina', 'Suárez', 'MN-101890'),
('Julián', 'Ferreyra', 'MN-102345'),
('Sofía', 'Benítez', 'MP-103678'),
('Pablo', 'Ruiz', 'MN-104567'),
('Valentina', 'Castro', 'MN-105678'),
('Andrés', 'Molina', 'MP-106789');
GO

INSERT INTO medicos_especialidades (id_medico, id_especialidad) VALUES
-- Laura Gómez
(1, 1),
(1, 2),
-- Carlos Pereyra
(2, 1),
(2, 3),
-- Marina Suárez
(3, 4),
-- Julián Ferreyra
(4, 5),
-- Sofía Benítez
(5, 6),
-- Pablo Ruiz
(6, 2),
(6, 3),
-- Valentina Castro
(7, 1),
(7, 4),
-- Andrés Molina
(8, 5),
(8, 6);
GO

INSERT INTO pacientes
(id_obra_social, nombre, apellido, dni, fecha_nacimiento,
 telefono, email, direccion, nro_afiliado)
VALUES
(1, 'Martín', 'Alvarez', '30123456', '1985-03-14',
 '11-5555-1001', 'martin.alvarez@mail.com',
 'Av. Corrientes 1234, CABA', 'OS-100234'),
(2, 'Camila', 'Rojas', '38234567', '1992-07-22',
 '11-5555-1002', 'camila.rojas@mail.com',
 'Belgrano 456, CABA', 'SM-203456'),
(3, 'Diego', 'Fernández', '35345678', '1988-11-05',
 '11-5555-1003', 'diego.fernandez@mail.com',
 'San Martín 789, Morón', 'GA-304567'),
(NULL, 'Lucía', 'Mendez', '42456789', '1999-01-30',
 '11-5555-1004', 'lucia.mendez@mail.com',
 'Rivadavia 4321, CABA', NULL),
(4, 'Jorge', 'Castillo', '28567890', '1978-09-17',
 '11-5555-1005', 'jorge.castillo@mail.com',
 'Mitre 234, Haedo', 'PA-405678'),
(5, 'Ana', 'Torres', '40678901', '1995-05-08',
 '11-5555-1006', 'ana.torres@mail.com',
 'Yrigoyen 567, Ramos Mejía', 'IO-506789'),
(1, 'Federico', 'Ramírez', '33456789', '1986-12-11',
 '11-5555-1007', 'federico.ramirez@mail.com',
 'Av. Rivadavia 2345, CABA', 'OS-107890'),
(2, 'Valeria', 'Morales', '39765432', '1994-04-19',
 '11-5555-1008', 'valeria.morales@mail.com',
 'Belgrano 890, Morón', 'SM-208901'),
(3, 'Nicolás', 'Herrera', '36789012', '1990-08-25',
 '11-5555-1009', 'nicolas.herrera@mail.com',
 'Sarmiento 567, Haedo', 'GA-309012'),
(NULL, 'Florencia', 'Díaz', '41567890', '1997-06-03',
 '11-5555-1010', 'florencia.diaz@mail.com',
 'Italia 345, Ramos Mejía', NULL);
GO

INSERT INTO antecedentes_medicos (id_paciente, tipo, descripcion) VALUES
(1, 'Alergia', 'Alérgico a la penicilina'),
(2, 'Cirugía', 'Apendicectomía realizada en 2015'),
(3, 'Enfermedad', 'Hipertensión controlada con medicación'),
(5, 'Alergia', 'Alergia al polen'),
(6, 'Enfermedad', 'Asma leve'),
(8, 'Cirugía', 'Cirugía de rodilla realizada en 2020'),
(9, 'Alergia', 'Alergia al ibuprofeno');
GO

INSERT INTO turnos_trabajo
(id_medico, dia_semana, hora_inicio, hora_fin)
VALUES
(1, 'Lunes', '06:00', '14:00'),
(2, 'Lunes', '08:00', '16:00'),
(3, 'Lunes', '16:00', '00:00'),
(4, 'Martes', '06:00', '14:00'),
(5, 'Martes', '08:00', '16:00'),
(6, 'Martes', '16:00', '00:00'),
(7, 'Miércoles', '06:00', '14:00'),
(8, 'Miércoles', '08:00', '16:00'),
(1, 'Miércoles', '16:00', '00:00'),
(2, 'Jueves', '06:00', '14:00'),
(3, 'Jueves', '08:00', '16:00'),
(4, 'Jueves', '16:00', '00:00'),
(5, 'Viernes', '06:00', '14:00'),
(6, 'Viernes', '08:00', '16:00'),
(7, 'Viernes', '16:00', '00:00'),
(8, 'Sábado', '06:00', '14:00'),
(1, 'Sábado', '08:00', '16:00'),
(2, 'Sábado', '16:00', '00:00'),
(3, 'Domingo', '06:00', '14:00'),
(4, 'Domingo', '08:00', '16:00'),
(5, 'Domingo', '16:00', '00:00');
GO

INSERT INTO turnos
(id_paciente, id_medico, id_especialidad, id_estado_turno,
 fecha_turno, hora_turno)
VALUES
-- LUNES 16
(1, 1, 1, 5, '2026-11-16', '06:00'),
(2, 2, 3, 4, '2026-11-16', '12:00'),
-- MARTES 17
(3, 4, 5, 2, '2026-11-17', '07:00'),
(4, 5, 6, 5, '2026-11-17', '12:00'),
-- MIÉRCOLES 18
(5, 7, 1, 5, '2026-11-18', '07:00'),
(6, 8, 5, 3, '2026-11-18', '12:00'),
-- JUEVES 19
(7, 2, 3, 5, '2026-11-19', '07:00'),
-- VIERNES 20
(8, 5, 6, 2, '2026-11-20', '07:00'),
-- SÁBADO 21
(9, 1, 2, 3, '2026-11-21', '12:00'),
-- DOMINGO 22
(10, 3, 4, 5, '2026-11-22', '07:00'),
-- LUNES 23
(1, 1, 1, 5, '2026-11-23', '06:00'),
(2, 2, 3, 1, '2026-11-23', '12:00'),
(3, 3, 4, 2, '2026-11-23', '18:00'),
-- MARTES 24
(4, 4, 5, 5, '2026-11-24', '07:00'),
(5, 5, 6, 4, '2026-11-24', '12:00'),
(6, 6, 2, 5, '2026-11-24', '18:00'),
-- MIÉRCOLES 25
(7, 7, 1, 5, '2026-11-25', '07:00'),
(8, 8, 5, 1, '2026-11-25', '12:00'),
(9, 1, 2, 5, '2026-11-25', '18:00'),
-- JUEVES 26
(10, 2, 1, 1, '2026-11-26', '07:00'),
(1, 3, 4, 2, '2026-11-26', '12:00'),
(2, 4, 5, 1, '2026-11-26', '18:00'),
-- VIERNES 27
(3, 5, 6, 1, '2026-11-27', '07:00'),
(4, 6, 3, 3, '2026-11-27', '12:00'),
(5, 7, 4, 1, '2026-11-27', '18:00'),
-- SÁBADO 28
(6, 8, 5, 1, '2026-11-28', '07:00'),
(7, 1, 2, 2, '2026-11-28', '12:00'),
(8, 2, 3, 1, '2026-11-28', '18:00'),
-- DOMINGO 29
(9, 3, 4, 1, '2026-11-29', '07:00'),
(10, 4, 5, 3, '2026-11-29', '12:00');
GO

INSERT INTO consultas (id_turno, motivo, diagnostico) VALUES
(1, 'Control general de salud',
 'Paciente en buen estado general. Sin alteraciones.'),
(4, 'Control odontológico',
 'Se realizó control dental. Sin complicaciones.'),
(5, 'Control clínico',
 'Parámetros clínicos dentro de valores normales.'),
(7, 'Control de presión arterial',
 'Presión arterial dentro de valores normales.'),
(10, 'Consulta dermatológica',
 'Dermatitis leve. Se indica tratamiento tópico.'),
(11, 'Control general',
 'Paciente en buen estado general.'),
(14, 'Control odontológico',
 'Se realizó revisión y limpieza dental.'),
(16, 'Control traumatológico',
 'Evolución favorable. Sin complicaciones.'),
(17, 'Consulta clínica',
 'Se recomienda continuar con controles periódicos.'),
(19, 'Control pediátrico',
 'Paciente sin alteraciones relevantes.');
GO


INSERT INTO usuarios
(nombre_usuario, contrasenia, tipo, id_medico)
VALUES
('admin', 'admin', 'Administrador', NULL),
('recepcion', 'recepcion', 'Recepcionista', NULL),
-- MÉDICOS
('lgomez', 'lgomez', 'Medico', 1),
('cpereyra', 'cpereyra', 'Medico', 2),
('msuarez', 'msuarez', 'Medico', 3),
('jferreyra', 'jferreyra', 'Medico', 4),
('sbenitez', 'sbenitez', 'Medico', 5),
('pruiz', 'pruiz', 'Medico', 6),
('vcastro', 'vcastro', 'Medico', 7),
('amolina', 'amolina', 'Medico', 8);
GO
