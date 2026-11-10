
USE clinica_privada;
GO

-- Agregamos el campo activo para permitir la baja lógica de usuarios.
-- 1 = Activo / 0 = Inactivo.

IF COL_LENGTH('dbo.usuarios', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.usuarios
    ADD activo BIT NOT NULL DEFAULT 1;
END;
GO

 
 -- Agregamos el campo observaciones a la tabla turnos.

-- Verificamos que la columna no exista antes de agregarla.
IF COL_LENGTH('dbo.turnos', 'observaciones') IS NULL
BEGIN
    ALTER TABLE dbo.turnos
    ADD observaciones VARCHAR(500) NULL;
END;
GO