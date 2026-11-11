USE clinica_privada
GO

-- Se agrega columna activo si no existe (para eliminacion logica)
IF COL_LENGTH('dbo.especialidades', 'activo') IS NULL
BEGIN
	ALTER TABLE dbo.especialidades
	ADD activo BIT NOT NULL DEFAULT 1;
END;
GO