using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using negocio;
using dominio;

namespace negocio
{
    public class MedicoNegocio
    {

        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT m.id, m.nombre, m.apellido, m.matricula, e.id AS id_especialidad, e.nombre AS especialidad
                    FROM medicos m
                    INNER JOIN medicos_especialidades me ON m.id = me.id_medico
                    INNER JOIN especialidades e ON me.id_especialidad = e.id");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    int idMedico = (int)datos.Lector["id"];
                    Medico medico = lista.Find(x => x.Id == idMedico);

                    if (medico == null)
                    {
                        medico = LeerMedico(datos.Lector);
                        lista.Add(medico);
                    }

                    AgregarEspecialidad(medico, datos.Lector);
                }
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<Medico> Filtrar(string texto)
        {
            List<Medico> medicos = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT m.id, m.nombre, m.apellido, m.matricula, e.id AS id_especialidad, e.nombre AS especialidad
                    FROM medicos m
                    INNER JOIN medicos_especialidades me ON m.id = me.id_medico
                    INNER JOIN especialidades e ON me.id_especialidad = e.id
                    WHERE m.nombre LIKE @filtro
                    OR m.apellido LIKE @filtro
                    OR m.matricula LIKE @filtro
                    OR e.nombre LIKE @filtro");
                datos.SetearParametro("@filtro", "%" + texto + "%");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    int idMedico = (int)datos.Lector["id"];
                    // Busca si ya existe un medico con ese id en la lista
                    Medico medico = medicos.Find(x => x.Id == idMedico);

                    // Si no existe lo crea
                    if (medico == null)
                    {
                        medico = LeerMedico(datos.Lector);
                        medicos.Add(medico);
                    }

                    // Se agregan las especialidades al medico
                    AgregarEspecialidad(medico, datos.Lector);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar los medicos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return medicos;
        }

        public Medico BuscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Medico medico = null;

            try
            {
                datos.SetearConsulta(
                    @"SELECT m.id, m.nombre, m.apellido, m.matricula, e.id AS id_especialidad, e.nombre AS especialidad
                    FROM medicos m
                    INNER JOIN medicos_especialidades me ON m.id = me.id_medico
                    INNER JOIN especialidades e ON me.id_especialidad = e.id
                    WHERE m.id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                // Se usa while porque la consulta puede devolver tantas filas como especialidades tenga el medico
                while (datos.Lector.Read())
                {
                    if (medico == null)
                    {
                        medico = LeerMedico(datos.Lector);
                    }

                    AgregarEspecialidad(medico, datos.Lector);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener Medico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return medico;
        }

        public void Agregar(Medico medico)
        {
            if (ExisteMatricula(medico.Matricula, medico.Id))
            {
                throw new Exception("Ya existe un medico registrado con esa matricula.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.IniciarTransaccion();
                // Agrega el medico a la DB y devuelve el id que se le asigno
                datos.SetearConsulta(
                    @"INSERT INTO medicos (nombre, apellido, matricula)
                    VALUES (@nombre, @apellido, @matricula);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);");

                CargarParametros(datos, medico);

                medico.Id = (int)datos.EjecutarScalar();

                // Se guardan todas las especialidades del medico
                foreach (Especialidad e in medico.Especialidades)
                {
                    datos.SetearConsulta(
                        @"INSERT INTO medicos_especialidades (id_medico, id_especialidad)
                        VALUES (@idMedico, @idEspecialidad);");

                    datos.SetearParametro("@idMedico", medico.Id);
                    datos.SetearParametro("@idEspecialidad", e.Id);

                    datos.EjecutarAccion();
                }

                datos.ConfirmarTransaccion();
            }
            catch (Exception ex)
            {
                // Si hay un error revertimos los inserts anteriores
                datos.RevertirTransaccion();
                throw new Exception("Error al agregar medico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Medico medico)
        {
            if (ExisteMatricula(medico.Matricula, medico.Id))
            {
                throw new Exception("Ya existe un medico registrado con esa matricula.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.IniciarTransaccion();
                // Modifica datos de medico
                datos.SetearConsulta(
                    @"UPDATE medicos SET
                        nombre = @nombre,
                        apellido = @apellido,
                        matricula = @matricula
                    WHERE id = @id");

                CargarParametros(datos, medico);
                datos.SetearParametro("@id", medico.Id);
                datos.EjecutarAccion();

                // Elimina las especialidades actuales del medico
                datos.SetearConsulta("DELETE FROM medicos_especialidades WHERE id_medico = @idMedico");
                datos.SetearParametro("@idMedico", medico.Id);
                datos.EjecutarAccion();

                // Agrega las nuevas especialidades al medico
                foreach (Especialidad e in medico.Especialidades)
                {
                    datos.SetearConsulta(
                        @"INSERT INTO medicos_especialidades (id_medico, id_especialidad)
                        VALUES (@idMedico, @idEspecialidad);");

                    datos.SetearParametro("@idMedico", medico.Id);
                    datos.SetearParametro("@idEspecialidad", e.Id);

                    datos.EjecutarAccion();
                }

                datos.ConfirmarTransaccion();
            }
            catch (Exception ex)
            {
                datos.RevertirTransaccion();
                throw new Exception("Error al modificar medico.", ex);
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
                datos.SetearConsulta(
                    @"UPDATE medicos 
                    SET activo = 0
                    WHERE id = @id");

                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el medico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteMatricula(string matricula, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Devuelve el numero de matriculas que coincide con la matricula del medico excluyendo la de el
                datos.SetearConsulta("SELECT COUNT(*) FROM medicos WHERE matricula = @matricula AND id <> @idExcluir");
                datos.SetearParametro("@matricula", matricula);
                datos.SetearParametro("@idExcluir", idExcluir);

                return (int)datos.EjecutarScalar() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar la matricula del medico.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        private Medico LeerMedico(SqlDataReader lector)
        {
            Medico medico = new Medico();

            medico.Id = (int)lector["id"];
            medico.Nombre = (string)lector["nombre"];
            medico.Apellido = (string)lector["apellido"];
            medico.Matricula = (string)lector["matricula"];

            return medico;
        }

        private void CargarParametros(AccesoDatos datos, Medico medico)
        {
            datos.SetearParametro("@nombre", medico.Nombre);
            datos.SetearParametro("@apellido", medico.Apellido);
            datos.SetearParametro("@matricula", medico.Matricula);
        }

        private void AgregarEspecialidad(Medico medico, SqlDataReader lector)
        {
            Especialidad especialidad = LeerEspecialidad(lector);
            medico.Especialidades.Add(especialidad);
        }

        private Especialidad LeerEspecialidad(SqlDataReader lector)
        {
            Especialidad especialidad = new Especialidad();
            especialidad.Id = (int)lector["id_especialidad"];
            especialidad.Nombre = (string)lector["especialidad"];

            return especialidad;
        }
    }
}
