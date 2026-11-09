
USE clinica_privada;
GO

-- Ampliar la columna contrasenia para permitir hashes seguros
ALTER TABLE usuarios
ALTER COLUMN contrasenia VARCHAR(255) NOT NULL;
GO