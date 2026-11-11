USE clinica_privada
GO

-- Se agrega columna activo si no existe (para eliminacion logica)
IF COL_LENGTH('dbo.medicos', 'activo') IS NULL
BEGIN
	ALTER TABLE dbo.medicos
	ADD activo BIT NOT NULL DEFAULT 1;
END;
GO

-- Se agrega la restriccion UNIQUE a matricula
ALTER TABLE dbo.medicos
ADD CONSTRAINT UQ_medicos_matricula UNIQUE (matricula);
GO