USE clinica_privada;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    -- Elimina las restricciones anteriores si existen.
    ALTER TABLE dbo.turnos
    DROP CONSTRAINT IF EXISTS uq_turnos_paciente_fecha_hora;

    ALTER TABLE dbo.turnos
    DROP CONSTRAINT IF EXISTS uq_turnos_medico_fecha_hora;

    -- Elimina los indices si ya fueron creados anteriormente.
    DROP INDEX IF EXISTS uq_turnos_paciente_fecha_hora
    ON dbo.turnos;

    DROP INDEX IF EXISTS uq_turnos_medico_fecha_hora
    ON dbo.turnos;

    -- Evita turnos duplicados para un mismo paciente,
    -- sin tener en cuenta los turnos cancelados.
    CREATE UNIQUE INDEX uq_turnos_paciente_fecha_hora
    ON dbo.turnos (id_paciente, fecha_turno, hora_turno)
    WHERE id_estado_turno <> 3;

    -- Evita turnos duplicados para un mismo medico,
    -- sin tener en cuenta los turnos cancelados.
    CREATE UNIQUE INDEX uq_turnos_medico_fecha_hora
    ON dbo.turnos (id_medico, fecha_turno, hora_turno)
    WHERE id_estado_turno <> 3;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    -- Si ocurre un error, deshace los cambios.
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;

GO

-- Agrega la columna activo a especialidades
-- si la base de datos todavia no la tiene.
IF COL_LENGTH('dbo.especialidades', 'activo') IS NULL
BEGIN
    ALTER TABLE dbo.especialidades
    ADD activo BIT NOT NULL DEFAULT 1;
END
GO