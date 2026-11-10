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
                datos.SetearConsulta("SELECT id, nombre, apellido, matricula FROM medicos");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico medico = new Medico();

                    medico.Id = (int)datos.Lector["id"];
                    medico.Nombre = (string)datos.Lector["nombre"];
                    medico.Apellido = (string)datos.Lector["apellido"];
                    medico.Matricula = (string)datos.Lector["matricula"];

                    lista.Add(medico);
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

        // Agregar

        // Modificar

        // Eliminar (baja logica?)

        // TODO = Hacer matricula UNIQUE en la DB
        public bool ExisteMatricula(string matricula, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
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
