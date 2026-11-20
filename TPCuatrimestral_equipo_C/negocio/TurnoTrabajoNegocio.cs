using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using negocio;
using dominio;

namespace negocio
{
    public class TurnoTrabajoNegocio
    {

        public List<TurnoTrabajo> Listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT T.id AS IdTurnoTrabajo, T.id_medico AS IdMedico,
                             T.dia_semana, T.hora_inicio, T.hora_fin,
                             M.nombre, M.apellido, M.matricula
                     FROM turnos_trabajo T
                     INNER JOIN medicos M ON T.id_medico = M.id
                     WHERE T.activo = 1
                     ORDER BY M.apellido, M.nombre, T.hora_inicio");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo turnoTrabajo = new TurnoTrabajo();

                    turnoTrabajo.Medico = new Medico();

                    turnoTrabajo.Id = (int)datos.Lector["IdTurnoTrabajo"];
                    turnoTrabajo.Medico.Id = (int)datos.Lector["IdMedico"];
                    turnoTrabajo.Medico.Nombre = (string)datos.Lector["nombre"];
                    turnoTrabajo.Medico.Apellido = (string)datos.Lector["apellido"];
                    turnoTrabajo.Medico.Matricula = (string)datos.Lector["matricula"];
                    turnoTrabajo.DiaSemana = (string)datos.Lector["dia_semana"];
                    turnoTrabajo.HoraInicio = (TimeSpan)datos.Lector["hora_inicio"];
                    turnoTrabajo.HoraFin = (TimeSpan)datos.Lector["hora_fin"];
                    

                    lista.Add(turnoTrabajo);

                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos de trabajo.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<TurnoTrabajo> ListarPorMedico(int idMedico)
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT T.id AS IdTurnoTrabajo, T.id_medico AS IdMedico,
                     T.dia_semana, T.hora_inicio, T.hora_fin,
                     M.nombre, M.apellido, M.matricula
                    FROM turnos_trabajo T
                    INNER JOIN medicos M ON T.id_medico = M.id
                    WHERE T.activo = 1
                    AND T.id_medico = @idMedico
                    ORDER BY T.hora_inicio");

                datos.SetearParametro("@idMedico", idMedico);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo turnoTrabajo = new TurnoTrabajo();
                    turnoTrabajo.Medico = new Medico();

                    turnoTrabajo.Id = (int)datos.Lector["IdTurnoTrabajo"];
                    turnoTrabajo.Medico.Id = (int)datos.Lector["IdMedico"];
                    turnoTrabajo.Medico.Nombre = (string)datos.Lector["nombre"];
                    turnoTrabajo.Medico.Apellido = (string)datos.Lector["apellido"];
                    turnoTrabajo.Medico.Matricula = (string)datos.Lector["matricula"];
                    turnoTrabajo.DiaSemana = (string)datos.Lector["dia_semana"];
                    turnoTrabajo.HoraInicio = (TimeSpan)datos.Lector["hora_inicio"];
                    turnoTrabajo.HoraFin = (TimeSpan)datos.Lector["hora_fin"];

                    lista.Add(turnoTrabajo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos de trabajo del médico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<TurnoTrabajo> Filtrar(string texto)
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT T.id AS IdTurnoTrabajo, T.id_medico AS IdMedico,
                     T.dia_semana, T.hora_inicio, T.hora_fin,
                     M.nombre, M.apellido, M.matricula
              FROM turnos_trabajo T
              INNER JOIN medicos M ON T.id_medico = M.id
              WHERE T.activo = 1
             AND (M.nombre COLLATE Latin1_General_CI_AI LIKE @filtro
                 OR M.apellido COLLATE Latin1_General_CI_AI LIKE @filtro
                 OR T.dia_semana COLLATE Latin1_General_CI_AI LIKE @filtro)
              ORDER BY M.apellido, M.nombre, T.hora_inicio");

                datos.SetearParametro("@filtro", "%" + texto + "%");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo turnoTrabajo = new TurnoTrabajo();

                    turnoTrabajo.Medico = new Medico();

                    turnoTrabajo.Id = (int)datos.Lector["IdTurnoTrabajo"];
                    turnoTrabajo.Medico.Id = (int)datos.Lector["IdMedico"];
                    turnoTrabajo.Medico.Nombre = (string)datos.Lector["nombre"];
                    turnoTrabajo.Medico.Apellido = (string)datos.Lector["apellido"];
                    turnoTrabajo.Medico.Matricula = (string)datos.Lector["matricula"];
                    turnoTrabajo.DiaSemana = (string)datos.Lector["dia_semana"];
                    turnoTrabajo.HoraInicio = (TimeSpan)datos.Lector["hora_inicio"];
                    turnoTrabajo.HoraFin = (TimeSpan)datos.Lector["hora_fin"];

                    lista.Add(turnoTrabajo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar los turnos de trabajo.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }


        public TurnoTrabajo BuscarPorId(int id)
        {
            List<TurnoTrabajo> lista = Listar();

            foreach (TurnoTrabajo turno in lista)
            {
                if (turno.Id == id)
                {
                    return turno;
                }
            }

            return null;
        }


        public void Agregar(TurnoTrabajo turnoTrabajo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.SetearConsulta("INSERT INTO turnos_trabajo (id_medico, dia_semana, hora_inicio, hora_fin)" +
                    " VALUES (@idMedico, @diaSemana, @horaInicio, @horaFin)");

                datos.SetearParametro("@idMedico", turnoTrabajo.Medico.Id);
                datos.SetearParametro("@diaSemana", turnoTrabajo.DiaSemana);
                datos.SetearParametro("@horaInicio", turnoTrabajo.HoraInicio);
                datos.SetearParametro("@horaFin", turnoTrabajo.HoraFin);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Modificar(TurnoTrabajo turnoTrabajo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                     @"UPDATE turnos_trabajo
                    SET id_medico = @idMedico,
                    dia_semana = @diaSemana,
                    hora_inicio = @horaInicio,
                    hora_fin = @horaFin
                    WHERE id = @id");

                datos.SetearParametro("@idMedico", turnoTrabajo.Medico.Id);
                datos.SetearParametro("@diaSemana", turnoTrabajo.DiaSemana);
                datos.SetearParametro("@horaInicio", turnoTrabajo.HoraInicio);
                datos.SetearParametro("@horaFin", turnoTrabajo.HoraFin);
                datos.SetearParametro("@id", turnoTrabajo.Id);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }



        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Baja lógica: no borramos el registro de la base de datos.
                // Solamente cambiamos su estado de activo (1) a inactivo (0).
                datos.SetearConsulta(
                    @"UPDATE turnos_trabajo
                      SET activo = 0
                      WHERE id = @id");

                // Indicamos qué turno de trabajo queremos dar de baja.
                datos.SetearParametro("@id", id);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public bool ExisteSuperposicion(TurnoTrabajo turnoTrabajo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Busca horarios del mismo médico y día que se superpongan
                // con el horario que queremos agregar o modificar.
                datos.SetearConsulta(
                    @"SELECT COUNT(*)
                      FROM turnos_trabajo
                      WHERE id_medico = @idMedico
                      AND dia_semana = @diaSemana
                      AND hora_inicio < @horaFin
                      AND hora_fin > @horaInicio
                      AND id <> @id 
                      AND activo = 1");
                
                // Enviamos los datos del turno que queremos guardar.
                datos.SetearParametro("@idMedico", turnoTrabajo.Medico.Id);
                datos.SetearParametro("@diaSemana", turnoTrabajo.DiaSemana);
                datos.SetearParametro("@horaInicio", turnoTrabajo.HoraInicio);
                datos.SetearParametro("@horaFin", turnoTrabajo.HoraFin);

                // Excluimos el mismo turno cuando estamos modificando.
                // Si es un turno nuevo, su Id es 0 y no excluye ninguno.
                datos.SetearParametro("@id", turnoTrabajo.Id);

                datos.EjecutarLectura();


                // COUNT devuelve la cantidad de horarios superpuestos
                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];

                    // Si encontró al menos uno, existe superposición.
                    if (cantidad > 0)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                datos.CerrarConexion();
            }

            return false;
        }


        public List<TurnoTrabajo> ListarPorMedicoYDia(int idMedico, string diaSemana)
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT T.id AS IdTurnoTrabajo,
                             T.id_medico AS IdMedico,
                             T.dia_semana,
                             T.hora_inicio,
                             T.hora_fin,
                             M.nombre,
                             M.apellido,
                             M.matricula
                      FROM turnos_trabajo T
                      INNER JOIN medicos M ON T.id_medico = M.id
                      WHERE T.id_medico = @idMedico
                        AND T.dia_semana = @diaSemana
                        AND T.activo = 1
                        AND M.activo = 1
                      ORDER BY T.hora_inicio");

                datos.SetearParametro("@idMedico", idMedico);
                datos.SetearParametro("@diaSemana", diaSemana);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo turno = new TurnoTrabajo();

                    turno.Id = (int)datos.Lector["IdTurnoTrabajo"];

                    turno.Medico = new Medico();

                    turno.Medico.Id = (int)datos.Lector["IdMedico"];
                    turno.Medico.Nombre = (string)datos.Lector["nombre"];
                    turno.Medico.Apellido = (string)datos.Lector["apellido"];
                    turno.Medico.Matricula = (string)datos.Lector["matricula"];

                    turno.DiaSemana = (string)datos.Lector["dia_semana"];
                    turno.HoraInicio = (TimeSpan)datos.Lector["hora_inicio"];
                    turno.HoraFin = (TimeSpan)datos.Lector["hora_fin"];

                    lista.Add(turno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los horarios del médico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }
    }
}
