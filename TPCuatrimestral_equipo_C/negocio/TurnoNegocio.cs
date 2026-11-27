using dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace negocio
{
    public class TurnoNegocio
    {

        private const string consultaTurnos =
            @"SELECT T.id, T.fecha_turno, T.hora_turno, T.observaciones,
                     T.id_medico,
                     T.id_especialidad,
                     ESP.nombre AS nombre_especialidad,
                     M.nombre AS nombre_medico,
                     M.apellido AS apellido_medico,
                     P.id AS id_paciente, P.nombre AS nombre_paciente, P.apellido AS apellido_paciente,
                     E.id AS id_estado, E.nombre AS nombre_estado,
                     C.id AS id_consulta, C.motivo, C.diagnostico
              FROM turnos T
              INNER JOIN pacientes P ON T.id_paciente = P.id
              INNER JOIN medicos M ON T.id_medico = M.id
              INNER JOIN especialidades ESP ON T.id_especialidad = ESP.id
              INNER JOIN estados_turno E ON T.id_estado_turno = E.id
              LEFT JOIN consultas C ON C.id_turno = T.id";

        public List<TurnoMedico> ListarPorMedico(int idMedico)
        {
            List<TurnoMedico> lista = new List<TurnoMedico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(consultaTurnos +
                    @" WHERE T.id_medico = @idMedico
                       ORDER BY T.fecha_turno, T.hora_turno");

                datos.SetearParametro("@idMedico", idMedico);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(MapearTurno(datos));
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

        public TurnoMedico BuscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            TurnoMedico turno = null;

            try
            {
                datos.SetearConsulta(consultaTurnos + " WHERE T.id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    turno = MapearTurno(datos);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return turno;
        }


        public List<TurnoMedico> ListarPorPaciente(int idPaciente)
        {
            List<TurnoMedico> lista = new List<TurnoMedico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(consultaTurnos +
                    @" WHERE T.id_paciente = @idPaciente
               ORDER BY T.fecha_turno DESC, T.hora_turno DESC");

                datos.SetearParametro("@idPaciente", idPaciente);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(MapearTurno(datos));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar el historial del paciente.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }


        public List<TimeSpan> ListarHorariosOcupados(int idMedico, DateTime fecha)
        {
            List<TimeSpan> horarios = new List<TimeSpan>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT T.hora_turno
                      FROM turnos T
                      INNER JOIN estados_turno E
                        ON T.id_estado_turno = E.id
                      WHERE T.id_medico = @idMedico
                        AND T.fecha_turno = @fecha
                        AND E.nombre NOT IN ('Cancelado', 'Reprogramado')");

                datos.SetearParametro("@idMedico", idMedico);
                datos.SetearParametro("@fecha", fecha.Date);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    horarios.Add((TimeSpan)datos.Lector["hora_turno"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los horarios ocupados.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return horarios;
        }


        private static TurnoMedico MapearTurno(AccesoDatos datos)
        {
            TurnoMedico turno = new TurnoMedico();

            turno.Id = (int)datos.Lector["id"];
            turno.Fecha = (DateTime)datos.Lector["fecha_turno"];
            turno.Hora = (TimeSpan)datos.Lector["hora_turno"];

            if (datos.Lector["observaciones"] != DBNull.Value)
            {
                turno.Observaciones = (string)datos.Lector["observaciones"];
            }

            turno.Medico = new Medico();
            turno.Medico.Id = (int)datos.Lector["id_medico"];
            turno.Medico.Nombre = (string)datos.Lector["nombre_medico"];
            turno.Medico.Apellido = (string)datos.Lector["apellido_medico"];

            // Cargamos la especialidad correspondiete al turno
            turno.Especialidad = new Especialidad();
            turno.Especialidad.Id = (int)datos.Lector["id_especialidad"];
            turno.Especialidad.Nombre = (string)datos.Lector["nombre_especialidad"];

            turno.Paciente = new Paciente();
            turno.Paciente.Id = (int)datos.Lector["id_paciente"];
            turno.Paciente.Nombre = (string)datos.Lector["nombre_paciente"];
            turno.Paciente.Apellido = (string)datos.Lector["apellido_paciente"];

            turno.Estado = new EstadoTurno();
            turno.Estado.Id = (int)datos.Lector["id_estado"];
            turno.Estado.Nombre = (string)datos.Lector["nombre_estado"];

            if (datos.Lector["id_consulta"] != DBNull.Value)
            {
                turno.Consulta = new Consulta();
                turno.Consulta.Id = (int)datos.Lector["id_consulta"];
                turno.Consulta.Motivo = (string)datos.Lector["motivo"];

                if (datos.Lector["diagnostico"] != DBNull.Value)
                {
                    turno.Consulta.Diagnostico = (string)datos.Lector["diagnostico"];
                }
            }

            return turno;
        }
    }
}
