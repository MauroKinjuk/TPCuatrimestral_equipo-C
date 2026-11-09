SELECT
    T.id AS IdTurnoTrabajo,
    T.id_medico AS IdMedico,
    T.dia_semana,
    T.hora_inicio,
    T.hora_fin,
    M.nombre,
    M.apellido,
    M.matricula
FROM turnos_trabajo T
INNER JOIN medicos M ON T.id_medico = M.id;


ALTER TABLE turnos_trabajo
ADD activo BIT NOT NULL DEFAULT 1;


SELECT id, id_medico, dia_semana, hora_inicio, hora_fin, activo
FROM turnos_trabajo
WHERE activo = 0;