
CREATE DATABASE clinica_privada;
GO

USE clinica_privada;
GO

CREATE TABLE especialidades (
    id_especialidad INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    descripcion VARCHAR(MAX) NULL
);
GO

CREATE TABLE obras_sociales (
    id_obra_social INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE
);
GO

CREATE TABLE estados_turno (
    id_estado_turno INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(150) NULL
);
GO


CREATE TABLE medicos (
    id_medico INT IDENTITY(1,1) PRIMARY KEY,
    id_especialidad INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    matricula VARCHAR(50) NOT NULL UNIQUE,

    CONSTRAINT fk_medicos_especialidades
        FOREIGN KEY (id_especialidad)
        REFERENCES especialidades (id_especialidad)
);
GO

CREATE TABLE pacientes (
    id_paciente INT IDENTITY(1,1) PRIMARY KEY,
    id_obra_social INT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    dni VARCHAR(20) NOT NULL UNIQUE,
    fecha_nacimiento DATE NOT NULL,
    telefono VARCHAR(30) NULL,
    email VARCHAR(150) NULL,
    direccion VARCHAR(255) NULL,
    nro_afiliado VARCHAR(50) NULL,

    CONSTRAINT fk_pacientes_obras_sociales
        FOREIGN KEY (id_obra_social)
        REFERENCES obras_sociales (id_obra_social)
);
GO

CREATE TABLE antecedentes_medicos (
    id_antecedente INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    tipo VARCHAR(50) NOT NULL,
    descripcion VARCHAR(MAX) NOT NULL,

    CONSTRAINT fk_antecedentes_pacientes
        FOREIGN KEY (id_paciente)
        REFERENCES pacientes (id_paciente)
);
GO



CREATE TABLE disponibilidades_medicas (
    id_disponibilidad INT IDENTITY(1,1) PRIMARY KEY,
    id_medico INT NOT NULL,
    dia_semana VARCHAR(20) NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,

    CONSTRAINT fk_disponibilidades_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id_medico)
);
GO

CREATE TABLE turnos (
    id_turno INT IDENTITY(1,1) PRIMARY KEY,
    id_paciente INT NOT NULL,
    id_medico INT NOT NULL,
    id_estado_turno INT NOT NULL,
    fecha_turno DATE NOT NULL,
    hora_turno TIME NOT NULL,

    CONSTRAINT fk_turnos_pacientes
        FOREIGN KEY (id_paciente)
        REFERENCES pacientes (id_paciente),

    CONSTRAINT fk_turnos_medicos
        FOREIGN KEY (id_medico)
        REFERENCES medicos (id_medico),

    CONSTRAINT fk_turnos_estados
        FOREIGN KEY (id_estado_turno)
        REFERENCES estados_turno (id_estado_turno)
);
GO


CREATE TABLE consultas (
    id_consulta INT IDENTITY(1,1) PRIMARY KEY,
    id_turno INT NOT NULL UNIQUE,
    motivo VARCHAR(255) NOT NULL,
    diagnostico VARCHAR(MAX) NOT NULL,

    CONSTRAINT fk_consultas_turnos
        FOREIGN KEY (id_turno)
        REFERENCES turnos (id_turno)
);
GO



INSERT INTO estados_turno (nombre, descripcion) VALUES
('Pendiente', 'Turno agendado a la espera de la fecha u horario de atención'),
('Atendido', 'El paciente asistió y fue atendido por el médico'),
('Cancelado', 'El turno fue dado de baja por el paciente o la clínica'),
('Ausente', 'El paciente no se presentó al turno');
GO
