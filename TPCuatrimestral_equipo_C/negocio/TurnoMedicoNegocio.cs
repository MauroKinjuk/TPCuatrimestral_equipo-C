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

        public void Agregar(TurnoMedico turno)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"INSERT INTO turnos
                    (id_paciente, id_especialidad, id_medico, id_estado_turno,
                    fecha_turno, hora_turno, observaciones)
                    VALUES
                    (@idPaciente, @idMedico, @idEstado, @idEspecialidad,
                    @fecha, @hora, @observaciones)");

                datos.SetearParametro("@idPaciente", turno.Paciente.Id);
                datos.SetearParametro("@idEspecialidad", turno.Especialidad.Id);
                datos.SetearParametro("@idMedico", turno.Medico.Id);
                datos.SetearParametro("@idEstado", turno.Estado.Id);
                datos.SetearParametro("@fecha", turno.Fecha.Date);
                datos.SetearParametro("@hora", turno.Hora);
                datos.SetearParametro("@observaciones", (object)turno.Observaciones ?? DBNull.Value);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el turno.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteTurnoPaciente(int idPaciente, DateTime fecha, TimeSpan hora)
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
    }
}
