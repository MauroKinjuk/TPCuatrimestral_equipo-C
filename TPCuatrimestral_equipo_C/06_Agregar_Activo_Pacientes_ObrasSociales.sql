USE clinica_privada
GO

-- Se agrega columna activo si no existe (para eliminacion logica)
IF COL_LENGTH('dbo.pacientes', 'activo') IS NULL
BEGIN
	ALTER TABLE dbo.pacientes
	ADD activo BIT NOT NULL DEFAULT 1;
END;
GO

IF COL_LENGTH('dbo.obras_sociales', 'activo') IS NULL
BEGIN
	ALTER TABLE dbo.obras_sociales
	ADD activo BIT NOT NULL DEFAULT 1;
END;
GO
