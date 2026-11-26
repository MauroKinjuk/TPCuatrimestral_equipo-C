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

        public void ModificarDiagnostico(int idTurno, string diagnostico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"IF EXISTS (SELECT 1 FROM consultas WHERE id_turno = @idTurno)
                        UPDATE consultas
                        SET diagnostico = @diagnostico
                        WHERE id_turno = @idTurno;
                    ELSE
                        INSERT INTO consultas (id_turno, motivo, diagnostico)
                        SELECT @idTurno,
                            LEFT(COALESCE(NULLIF(T.observaciones, ''), 'Sin motivo registrado'), 255),
                            @diagnostico
                        FROM turnos T
                        WHERE T.id = @idTurno;");

                datos.SetearParametro("@diagnostico", diagnostico);
                datos.SetearParametro("@idTurno", idTurno);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el diagnóstico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
