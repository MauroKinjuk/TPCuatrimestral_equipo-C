using dominio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class TurnoMedicoNegocio
    {

        public List<TurnoMedico> ListarPorMedico(int idMedico)
        {
            List<TurnoMedico> lista = new List<TurnoMedico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, id_medico, id_estado_turno,
                         fecha_turno, hora_turno, observaciones
                        FROM turnos
                        WHERE id_medico = @idMedico
                        ORDER BY fecha_turno, hora_turno");

                datos.SetearParametro("@idMedico", idMedico);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoMedico turno = new TurnoMedico();

                    turno.Medico = new Medico();
                    turno.Estado = new EstadoTurno();

                    turno.Id = (int)datos.Lector["id"];
                    turno.Medico.Id = (int)datos.Lector["id_medico"];
                    turno.Estado.Id = (int)datos.Lector["id_estado_turno"];
                    turno.Fecha = (DateTime)datos.Lector["fecha_turno"];
                    turno.Hora = (TimeSpan)datos.Lector["hora_turno"];

                    turno.Observaciones = datos.Lector["observaciones"] == DBNull.Value
                        ? null
                        : (string)datos.Lector["observaciones"];

                    lista.Add(turno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos del médico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<TurnoMedico> ListarPorFecha(DateTime fecha)
        {
            List<TurnoMedico> lista = new List<TurnoMedico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT 
                        t.id,
                        t.id_paciente,
                        p.nombre AS nombre_paciente,
                        p.apellido AS apellido_paciente,
                        t.id_especialidad,
                        e.nombre AS nombre_especialidad,
                        t.id_medico,
                        m.nombre AS nombre_medico,
                        m.apellido AS apellido_medico,
                        t.id_estado_turno,
                        et.nombre AS nombre_estado,
                        t.fecha_turno,
                        t.hora_turno,
                        t.observaciones
                    FROM turnos t
                    INNER JOIN pacientes p ON p.id = t.id_paciente
                    INNER JOIN especialidades e ON e.id = t.id_especialidad
                    INNER JOIN medicos m ON m.id = t.id_medico
                    INNER JOIN estados_turno et ON et.id = t.id_estado_turno
                    WHERE t.fecha_turno = @fecha
                    ORDER BY t.fecha_turno, t.hora_turno");
                datos.SetearParametro("@fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoMedico turno = new TurnoMedico();

                    turno.Id = (int)datos.Lector["id"];
                    turno.Fecha = (DateTime)datos.Lector["fecha_turno"];
                    turno.Hora = (TimeSpan)datos.Lector["hora_turno"];
                    turno.Observaciones = datos.Lector["observaciones"] == DBNull.Value
                        ? null
                        : (string)datos.Lector["observaciones"];

                    turno.Paciente = new Paciente();
                    turno.Paciente.Id = (int)datos.Lector["id_paciente"];
                    turno.Paciente.Nombre = (string)datos.Lector["nombre_paciente"];
                    turno.Paciente.Apellido = (string)datos.Lector["apellido_paciente"];

                    turno.Especialidad = new Especialidad();
                    turno.Especialidad.Id = (int)datos.Lector["id_especialidad"];
                    turno.Especialidad.Nombre = (string)datos.Lector["nombre_especialidad"];

                    turno.Medico = new Medico();
                    turno.Medico.Id = (int)datos.Lector["id_medico"];
                    turno.Medico.Nombre = (string)datos.Lector["nombre_medico"];
                    turno.Medico.Apellido = (string)datos.Lector["apellido_medico"];

                    turno.Estado = new EstadoTurno();
                    turno.Estado.Id = (int)datos.Lector["id_estado_turno"];
                    turno.Estado.Nombre = (string)datos.Lector["nombre_estado"];

                    lista.Add(turno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos medicos del dia", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<TimeSpan> ListarHorariosDisponibles(
            DateTime fecha,
            List<TurnoTrabajo> horariosTrabajo,
            List<TurnoMedico> turnosMedico)
        {
            List<TimeSpan> horariosDisponibles = new List<TimeSpan>();

            // Obtiene el nombre del día y lo formatea para que coincida con el que guardamos en la BD.
            // NOTA: Si cambiamos el formato de los dias en la BD, este metodo tiene que cambiar.
            string diaSemana = fecha.ToString("dddd", new CultureInfo("es-AR"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            // Obtiene los horarios de trabajo del medico correspondientes al dia.
            List<TurnoTrabajo> horariosDelDia = new List<TurnoTrabajo>();
            foreach (TurnoTrabajo turno in horariosTrabajo)
            {
                if (turno.DiaSemana == diaSemana)
                {
                    horariosDelDia.Add(turno);
                }
            }

            foreach (TurnoTrabajo horario in horariosDelDia)
            {
                // Recorre los horarios del medico, empezando por el inicial y agregando 1h por loop.
                for (TimeSpan hora = horario.HoraInicio;
                     hora.Add(TimeSpan.FromHours(1)) <= horario.HoraFin;
                     hora = hora.Add(TimeSpan.FromHours(1)))
                {
                    // Si la fecha es hoy, descarta los horarios que ya pasaron.
                    if (fecha.Date == DateTime.Today && hora <= DateTime.Now.TimeOfDay)
                        continue;

                    // Valida si el turno con la fecha y hora del loop esta ocupado.
                    bool ocupado = false;
                    foreach (TurnoMedico turno in turnosMedico)
                    {
                        if (turno.Fecha.Date == fecha.Date &&
                            turno.Hora == hora &&
                            // Los turnos cancelados (id 3) siguen estando disponibles.
                            turno.Estado.Id != 3)
                        {
                            ocupado = true;
                            break;
                        }
                    }

                    if (!ocupado)
                        horariosDisponibles.Add(hora);
                }
            }

            return horariosDisponibles;
        }


        // Busca los tres turnos disponibles más próximos
        // según la especialidad y el paciente seleccionado.
        public List<TurnoMedico> ListarTurnosSugeridos(int idEspecialidad, int idPaciente)
        {
            List<TurnoMedico> sugerencias = new List<TurnoMedico>();

            MedicoNegocio negocioMedicos = new MedicoNegocio();
            TurnoTrabajoNegocio negocioTrabajo = new TurnoTrabajoNegocio();

            // Obtiene los médicos asociados a la especialidad.
            List<Medico> medicos = negocioMedicos.ListarPorEspecialidad(idEspecialidad);

            foreach (Medico medico in medicos)
            {
                // Obtiene los horarios de trabajo y los turnos del médico.
                List<TurnoTrabajo> horariosTrabajo = negocioTrabajo.ListarPorMedico(medico.Id);

                List<TurnoMedico> turnosMedico = ListarPorMedico(medico.Id);

                // Recorre los próximos 30 días, incluyendo hoy.
                for (int i = 0; i < 30; i++)
                {
                    DateTime fecha = DateTime.Today.AddDays(i);

                    List<TimeSpan> horarios = ListarHorariosDisponibles(fecha, horariosTrabajo, turnosMedico);

                    foreach (TimeSpan hora in horarios.Distinct())
                    {
                        // Verifica que el paciente no tenga otro turno
                        // en la misma fecha y horario.
                        if (!ExisteTurnoPaciente(idPaciente, fecha, hora))
                        {
                            TurnoMedico turno = new TurnoMedico();

                            turno.Medico = medico;
                            turno.Fecha = fecha;
                            turno.Hora = hora;

                            sugerencias.Add(turno);
                        }
                    }
                }
            }

            // Ordena los turnos por fecha y hora
            // y devuelve solamente los primeros tres.
            return sugerencias
                .OrderBy(t => t.Fecha)
                .ThenBy(t => t.Hora)
                .Take(3)
                .ToList();
        }
        public void Agregar(TurnoMedico turno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.IniciarTransaccion();

                datos.SetearConsulta(
                    @"INSERT INTO turnos
                    (id_paciente, id_especialidad, id_medico, id_estado_turno,
                     fecha_turno, hora_turno, observaciones)
                    VALUES
                    (@idPaciente, @idEspecialidad, @idMedico, @idEstado,
                     @fecha, @hora, @observaciones);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);");

                datos.SetearParametro("@idPaciente", turno.Paciente.Id);
                datos.SetearParametro("@idEspecialidad", turno.Especialidad.Id);
                datos.SetearParametro("@idMedico", turno.Medico.Id);
                datos.SetearParametro("@idEstado", turno.Estado.Id);
                datos.SetearParametro("@fecha", turno.Fecha.Date);
                datos.SetearParametro("@hora", turno.Hora);
                datos.SetearParametro("@observaciones", turno.Observaciones.Trim());

                turno.Id = (int)datos.EjecutarScalar();

                datos.SetearConsulta(
                    @"INSERT INTO consultas (id_turno, motivo)
                    VALUES (@idTurno, @motivo)");

                datos.SetearParametro("@idTurno", turno.Id);
                datos.SetearParametro("@motivo", turno.Observaciones.Trim());

                datos.EjecutarAccion();
                datos.ConfirmarTransaccion();
            }
            catch (Exception ex)
            {
                datos.RevertirTransaccion();
                throw new Exception("Error al agregar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteTurnoPaciente(int idPaciente, DateTime fecha, TimeSpan hora)

        // Evita que un paciente tenga dos turnos simultáneos.

        {
            AccesoDatos datos = new AccesoDatos();
            bool existe = false;

            try
            {
                datos.SetearConsulta(
                    @"SELECT 1 FROM turnos
                    WHERE id_paciente = @idPaciente
                    AND id_estado_turno <> 3
                    AND fecha_turno = @fecha
                    AND hora_turno = @hora");
                datos.SetearParametro("@idPaciente", idPaciente);
                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@hora", hora);

                existe = datos.EjecutarScalar() != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar turnos del paciente.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return existe;
        }


        public bool ExisteTurnoMedico(int idMedico, DateTime fecha, TimeSpan hora)

        // Permite detectar si un médico ya tiene otro paciente asignado en ese horario.
        {
            AccesoDatos datos = new AccesoDatos();
            bool existe = false;

            try
            {
                datos.SetearConsulta(
                    @"SELECT 1 FROM turnos
                      WHERE id_medico = @idMedico
                      AND id_estado_turno <> 3                        
                      AND fecha_turno = @fecha
                      AND hora_turno = @hora");

                datos.SetearParametro("@idMedico", idMedico);
                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@hora", hora);

                existe = datos.EjecutarScalar() != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar turnos del médico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return existe;
        }

        public void CancelarTurno(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Cancela el turno seleccionado si está Nuevo o Reprogramado
                // y su fecha y hora todavía no han llegado.
                datos.SetearConsulta(
                    @"UPDATE turnos
                      SET id_estado_turno = 3
                      WHERE id = @idTurno
                      AND id_estado_turno IN (1, 2)
                      AND (fecha_turno > CAST(GETDATE() AS DATE)
                      OR (fecha_turno = CAST(GETDATE() AS DATE)
                      AND hora_turno > CAST(GETDATE() AS TIME)))");

                datos.SetearParametro("@idTurno", idTurno);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void MarcarNoAsistio(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"UPDATE turnos
                      SET id_estado_turno = 4
                      WHERE id = @idTurno
                      AND id_estado_turno IN (1, 2)
                      AND (fecha_turno < CAST(GETDATE() AS DATE)
                      OR (fecha_turno = CAST(GETDATE() AS DATE)
                      AND hora_turno < CAST(GETDATE() AS TIME)))");

                // Cambia el estado del turno seleccionado a "No Asistió" (ID 4),
                // siempre que su estado sea Nuevo o Reprogramado (ID 1 o 2)
                // y que la fecha y hora del turno ya hayan pasado.

                datos.SetearParametro("@idTurno", idTurno);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al marcar el turno como No Asistió.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        // Busca un turno por su ID y devuelve sus datos.
        public TurnoMedico BuscarPorId(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, id_paciente, id_especialidad,
                      id_medico, id_estado_turno, fecha_turno,
                      hora_turno, observaciones
                      FROM turnos
                      WHERE id = @idTurno");

                datos.SetearParametro("@idTurno", idTurno);
                datos.EjecutarLectura();

                // Si encuentra el turno, carga sus datos.
                if (datos.Lector.Read())
                {
                    TurnoMedico turno = new TurnoMedico();

                    turno.Id = (int)datos.Lector["id"];

                    turno.Paciente = new Paciente();
                    turno.Paciente.Id = (int)datos.Lector["id_paciente"];

                    turno.Especialidad = new Especialidad();
                    turno.Especialidad.Id = (int)datos.Lector["id_especialidad"];

                    turno.Medico = new Medico();
                    turno.Medico.Id = (int)datos.Lector["id_medico"];

                    turno.Estado = new EstadoTurno();
                    turno.Estado.Id = (int)datos.Lector["id_estado_turno"];

                    turno.Fecha = (DateTime)datos.Lector["fecha_turno"];
                    turno.Hora = (TimeSpan)datos.Lector["hora_turno"];

                    turno.Observaciones = datos.Lector["observaciones"] == DBNull.Value
                        ? null
                        : (string)datos.Lector["observaciones"];

                    return turno;
                }

                // Si no encuentra el turno, devuelve null.
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Modifica la fecha, hora y observaciones de un turno existente
        // y cambia su estado a Reprogramado.
        public void ReprogramarTurno(TurnoMedico turno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.IniciarTransaccion();

                // Reprograma un turno vigente, verificando que el nuevo
                // horario sea futuro y no esté ocupado por otro turno.
                datos.SetearConsulta(
                    @"UPDATE turnos
                    SET fecha_turno = @fecha,
                      hora_turno = @hora,
                      observaciones = @observaciones,
                      id_estado_turno = 2
                  WHERE id = @idTurno
                  AND id_estado_turno IN (1, 2)
                  AND (fecha_turno > CAST(GETDATE() AS DATE)
                  OR (fecha_turno = CAST(GETDATE() AS DATE)
                  AND hora_turno > CAST(GETDATE() AS TIME)))
                  AND (@fecha > CAST(GETDATE() AS DATE)
                  OR (@fecha = CAST(GETDATE() AS DATE)
                  AND @hora > CAST(GETDATE() AS TIME)))
                  AND NOT EXISTS (
                      SELECT 1
                      FROM turnos t
                      WHERE t.id <> @idTurno
                      AND t.id_estado_turno <> 3
                      AND t.fecha_turno = @fecha
                      AND t.hora_turno = @hora
                      AND (t.id_medico = turnos.id_medico
                      OR t.id_paciente = turnos.id_paciente)
                      );

                    IF @@ROWCOUNT = 0
                        THROW 50001, 'No se pudo reprogramar el turno.', 1;");


                datos.SetearParametro("@fecha", turno.Fecha.Date);
                datos.SetearParametro("@hora", turno.Hora);
                datos.SetearParametro("@observaciones", turno.Observaciones.Trim());
                datos.SetearParametro("@idTurno", turno.Id);

                datos.EjecutarAccion();

                datos.SetearConsulta(
                    @"IF EXISTS (SELECT 1 FROM consultas WHERE id_turno = @idTurno)
                        UPDATE consultas
                        SET motivo = @motivo
                        WHERE id_turno = @idTurno;
                    ELSE
                        INSERT INTO consultas (id_turno, motivo)
                        VALUES (@idTurno, @motivo);");

                datos.SetearParametro("@idTurno", turno.Id);
                datos.SetearParametro("@motivo", turno.Observaciones.Trim());

                datos.EjecutarAccion();
                datos.ConfirmarTransaccion();
            }
            catch (Exception ex)
            {
                datos.RevertirTransaccion();
                throw new Exception("Error al reprogramar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Sobrecarga: Cuenta todos los turnos históricos con el estado especifico.
        public int ContarPorEstado(int idEstado)
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE id_estado_turno = @idEstado");

                datos.SetearParametro("@idEstado", idEstado);

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos por estado.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

        // Sobrecarga: Cuenta todos los turnos de una fecha determinada con el estado especifico.
        public int ContarPorEstado(DateTime fecha, int idEstado)
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE fecha_turno = @fecha
                    AND id_estado_turno = @idEstado");

                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@idEstado", idEstado);

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos por estado.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

        // Sobrecarga: Cuenta todos los turnos de una fecha determinada con el estado y medico especifico.
        public int ContarPorEstado(DateTime fecha, int idEstado, int idMedico)
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE fecha_turno = @fecha
                    AND id_estado_turno = @idEstado
                    AND id_medico = @idMedico");

                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@idEstado", idEstado);
                datos.SetearParametro("@idMedico", idMedico);

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos del médico por estado.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

        // Sobrecarga: Cuenta todos los turnos pendientes históricos.
        public int ContarPendientes()
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE id_estado_turno IN (1, 2)");

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos pendientes.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

        // Sobrecarga: Cuenta los turnos pendientes de una fecha determinada.
        public int ContarPendientes(DateTime fecha)
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE fecha_turno = @fecha
                    AND id_estado_turno IN (1, 2)");

                datos.SetearParametro("@fecha", fecha.Date);

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos pendientes.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

        // Sobrecarga: Cuenta los turnos pendientes de una fecha y medico determinado.
        public int ContarPendientes(DateTime fecha, int idMedico)
        {
            int cantidad = 0;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                    FROM turnos
                    WHERE fecha_turno = @fecha
                    AND id_medico = @idMedico
                    AND id_estado_turno IN (1, 2)");

                datos.SetearParametro("@fecha", fecha.Date);
                datos.SetearParametro("@idMedico", idMedico);

                cantidad = Convert.ToInt32(datos.EjecutarScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar los turnos pendientes del médico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return cantidad;
        }

    }
}
