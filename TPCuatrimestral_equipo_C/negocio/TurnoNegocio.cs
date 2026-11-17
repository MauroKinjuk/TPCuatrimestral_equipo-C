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
                     P.id AS id_paciente, P.nombre AS nombre_paciente, P.apellido AS apellido_paciente,
                     E.id AS id_estado, E.nombre AS nombre_estado,
                     C.id AS id_consulta, C.motivo, C.diagnostico
              FROM turnos T
              INNER JOIN pacientes P ON T.id_paciente = P.id
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
