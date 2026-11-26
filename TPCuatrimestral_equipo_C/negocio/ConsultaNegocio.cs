using dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace negocio
{
    public class ConsultaNegocio
    {
        public Consulta BuscarPorTurno(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();
            Consulta consulta = null;

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, id_turno, motivo, diagnostico
                      FROM consultas
                      WHERE id_turno = @idTurno");

                datos.SetearParametro("@idTurno", idTurno);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    consulta = new Consulta();
                    consulta.Id = (int)datos.Lector["id"];

                    consulta.Turno = new TurnoMedico();
                    consulta.Turno.Id = (int)datos.Lector["id_turno"];

                    consulta.Motivo = (string)datos.Lector["motivo"];

                    if (datos.Lector["diagnostico"] != DBNull.Value)
                    {
                        consulta.Diagnostico = (string)datos.Lector["diagnostico"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar la consulta del turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return consulta;
        }

        public void ModificarDiagnostico(int idTurno, int idMedico, string diagnostico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                TurnoMedico turno = turnoNegocio.BuscarPorId(idTurno);

                if (turno == null || turno.Medico.Id != idMedico ||
                    (turno.Estado.Id != 1 && turno.Estado.Id != 2))
                {
                    throw new Exception("No se puede cerrar el turno.");
                }

                Consulta consulta = BuscarPorTurno(idTurno);

                datos.IniciarTransaccion();

                if (consulta == null)
                {
                    string motivo = turno.Observaciones;

                    if (string.IsNullOrWhiteSpace(motivo))
                    {
                        motivo = "Sin motivo registrado";
                    }

                    if (motivo.Length > 255)
                    {
                        motivo = motivo.Substring(0, 255);
                    }

                    datos.SetearConsulta(
                        @"INSERT INTO consultas (id_turno, motivo, diagnostico)
                        VALUES (@idTurno, @motivo, @diagnostico)");

                    datos.SetearParametro("@idTurno", idTurno);
                    datos.SetearParametro("@motivo", motivo);
                    datos.SetearParametro("@diagnostico", diagnostico);
                }
                else
                {
                    datos.SetearConsulta(
                        @"UPDATE consultas
                        SET diagnostico = @diagnostico
                        WHERE id_turno = @idTurno");

                    datos.SetearParametro("@diagnostico", diagnostico);
                    datos.SetearParametro("@idTurno", idTurno);
                }

                datos.EjecutarAccion();

                datos.SetearConsulta(
                    @"UPDATE turnos
                    SET id_estado_turno = 5
                    WHERE id = @idTurno
                    AND id_medico = @idMedico
                    AND id_estado_turno IN (1, 2)");

                datos.SetearParametro("@idTurno", idTurno);
                datos.SetearParametro("@idMedico", idMedico);

                datos.EjecutarAccion();
                datos.ConfirmarTransaccion();
            }
            catch (Exception ex)
            {
                datos.RevertirTransaccion();
                throw new Exception("Error al guardar el diagnóstico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
