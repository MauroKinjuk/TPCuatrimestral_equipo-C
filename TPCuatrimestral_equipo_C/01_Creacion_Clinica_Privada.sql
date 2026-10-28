
CREATE DATABASE clinica_privada;
GO

USE clinica_privada;
GO

CREATE TABLE especialidades (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(MAX) NULL
);
GO

CREATE TABLE obras_sociales (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE
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
    matricula VARCHAR(50) NOT NULL UNIQUE
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

    CONSTRAINT fk_turnostrabajo_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id)
);
GO

CREATE TABLE turnos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_medico INT NOT NULL,
    id_estado_turno INT NOT NULL,
    fecha_turno DATE NOT NULL,
    hora_turno TIME NOT NULL,

    CONSTRAINT fk_turnos_pacientes
        FOREIGN KEY (id_paciente)
        REFERENCES pacientes (id),

    CONSTRAINT fk_turnos_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id),

    CONSTRAINT fk_turnos_estados
        FOREIGN KEY (id_estado_turno)
        REFERENCES estados_turno (id),

    CONSTRAINT uq_turnos_medico_fecha_hora
        UNIQUE (id_medico, fecha_turno, hora_turno),

    CONSTRAINT uq_turnos_paciente_fecha_hora
        UNIQUE (id_paciente, fecha_turno, hora_turno)
);
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
    contrasenia VARCHAR(50) NOT NULL,
    tipo VARCHAR(20) NOT NULL
        CONSTRAINT ck_usuarios_tipo
        CHECK (tipo IN ('Administrador', 'Recepcionista', 'Medico')),
    id_medico INT NULL,

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
('Sofía', 'Benítez', 'MP-103678');
GO

INSERT INTO medicos_especialidades (id_medico, id_especialidad) VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 1),
(3, 4),
(4, 5),
(5, 6);
GO

INSERT INTO pacientes (id_obra_social, nombre, apellido, dni, fecha_nacimiento, telefono, email, direccion, nro_afiliado) VALUES
(1, 'Martín', 'Alvarez', '30123456', '1985-03-14', '11-5555-1001', 'martin.alvarez@mail.com', 'Av. Corrientes 1234, CABA', 'OS-100234'),
(2, 'Camila', 'Rojas', '38234567', '1992-07-22', '11-5555-1002', 'camila.rojas@mail.com', 'Belgrano 456, CABA', 'SM-203456'),
(3, 'Diego', 'Fernández', '35345678', '1988-11-05', '11-5555-1003', 'diego.fernandez@mail.com', 'San Martín 789, Morón', 'GA-304567'),
(NULL, 'Lucía', 'Mendez', '42456789', '1999-01-30', '11-5555-1004', 'lucia.mendez@mail.com', 'Rivadavia 4321, CABA', NULL),
(4, 'Jorge', 'Castillo', '28567890', '1978-09-17', '11-5555-1005', 'jorge.castillo@mail.com', 'Mitre 234, Haedo', 'PA-405678'),
(5, 'Ana', 'Torres', '40678901', '1995-05-08', '11-5555-1006', 'ana.torres@mail.com', 'Yrigoyen 567, Ramos Mejía', 'IO-506789');
GO

INSERT INTO antecedentes_medicos (id_paciente, tipo, descripcion) VALUES
(1, 'Alergia', 'Alérgico a la penicilina'),
(2, 'Cirugía', 'Apendicectomía en 2015'),
(5, 'Enfermedad', 'Hipertensión controlada con medicación'),
(6, 'Alergia', 'Alérgica a ibuprofeno');
GO

INSERT INTO turnos_trabajo (id_medico, dia_semana, hora_inicio, hora_fin) VALUES
(1, 'Lunes', '08:00', '12:00'),
(1, 'Miércoles', '08:00', '12:00'),
(2, 'Martes', '14:00', '18:00'),
(2, 'Jueves', '14:00', '18:00'),
(3, 'Lunes', '09:00', '13:00'),
(3, 'Viernes', '09:00', '13:00'),
(4, 'Martes', '08:00', '12:00'),
(4, 'Jueves', '08:00', '12:00'),
(5, 'Miércoles', '14:00', '18:00'),
(5, 'Viernes', '14:00', '18:00');
GO

INSERT INTO turnos (id_paciente, id_medico, id_estado_turno, fecha_turno, hora_turno) VALUES
(1, 1, 5, '2026-10-19', '09:00'),
(2, 2, 4, '2026-10-22', '15:00'),
(3, 3, 2, '2026-10-26', '10:00'),
(4, 4, 1, '2026-10-29', '10:00'),
(5, 5, 1, '2026-10-30', '16:00'),
(6, 1, 1, '2026-11-02', '11:00');
GO

INSERT INTO consultas (id_turno, motivo, diagnostico) VALUES
(1, 'Control de presión arterial', 'Presión dentro de valores normales. Control en 6 meses.'),
(2, 'Palpitaciones y mareos', NULL),
(3, 'Erupción en brazos', NULL),
(4, 'Limpieza y control dental', NULL),
(5, 'Dolor de rodilla tras correr', NULL),
(6, 'Chequeo general anual', NULL);
GO

INSERT INTO usuarios (nombre_usuario, contrasenia, tipo, id_medico) VALUES
('admin', 'admin', 'Administrador', NULL),
('recepcion', 'recepcion', 'Recepcionista', NULL),
('lgomez', 'lgomez', 'Medico', 1);
GO
