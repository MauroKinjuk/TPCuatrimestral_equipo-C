USE clinica_privada;
GO

-- Agregar columna activo para eliminación lógica
-- 1 = Activo
-- 0 = Inactivo

IF COL_LENGTH('dbo.turnos_trabajo', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.turnos_trabajo
    ADD activo BIT NOT NULL DEFAULT 1;
END;
GO