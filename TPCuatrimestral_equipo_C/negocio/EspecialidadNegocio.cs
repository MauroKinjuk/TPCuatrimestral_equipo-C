using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class EspecialidadNegocio
    {
        public List<Especialidad> Listar()
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT id, nombre, descripcion FROM especialidades WHERE activo = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad especialidad = LeerEspecialidad(datos.Lector);
                    lista.Add(especialidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las especialidades.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public List<Especialidad> Filtrar(string texto)
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, nombre, descripcion
                    FROM especialidades
                    WHERE activo = 1
                    AND (
                        nombre LIKE @filtro
                        OR descripcion LIKE @filtro
                    )");
                datos.SetearParametro("@filtro", "%" + texto + "%");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad especialidad = LeerEspecialidad(datos.Lector);
                    lista.Add(especialidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar las especialidades.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }

        public Especialidad BuscarPorId(int id)
        {
            Especialidad especialidad = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, nombre, descripcion
                    FROM especialidades
                    WHERE activo = 1
                    AND id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    especialidad = LeerEspecialidad(datos.Lector);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener especialidad.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return especialidad;
        }

        public void Agregar(Especialidad especialidad)
        {
            if (ExisteNombre(especialidad.Nombre, especialidad.Id))
            {
                throw new Exception("Ya existe una especialidad con ese nombre.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"INSERT INTO especialidades (nombre, descripcion)
                    VALUES (@nombre, @descripcion)");
                CargarParametros(datos, especialidad);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar especialidad.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Especialidad especialidad)
        {
            if (ExisteNombre(especialidad.Nombre, especialidad.Id)) 
            {
                throw new Exception("Ya existe una especialidad con ese nombre.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"UPDATE especialidades SET
                        nombre = @nombre,
                        descripcion = @descripcion
                    WHERE id = @id
                    AND activo = 1");
                CargarParametros(datos, especialidad);
                datos.SetearParametro("@id", especialidad.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar especialidad.", ex);
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
                    @"UPDATE especialidades
                    SET activo = 0
                    WHERE id = @id
                    AND activo = 1"
);
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la especialidad.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteNombre(string nombre, int idExcluir)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Verifica si ya existe otra especialidad con el mismo nombre
                datos.SetearConsulta("SELECT COUNT(*) FROM especialidades WHERE nombre = @nombre AND id <> @idExcluir");
                datos.SetearParametro("@nombre", nombre);
                datos.SetearParametro("@idExcluir", idExcluir);

                return (int)datos.EjecutarScalar() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el nombre de la Especialidad.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        private Especialidad LeerEspecialidad(SqlDataReader lector)
        {
            Especialidad especialidad = new Especialidad();

            especialidad.Id = (int)lector["id"];
            especialidad.Nombre = (string)lector["nombre"];
            especialidad.Descripcion = lector["descripcion"] is DBNull ? null : (string)lector["descripcion"];

            return especialidad;
        }

        private void CargarParametros(AccesoDatos datos, Especialidad especialidad)
        {
            datos.SetearParametro("@nombre", especialidad.Nombre);
            datos.SetearParametro(
                "@descripcion",
                string.IsNullOrEmpty(especialidad.Descripcion) ? (Object)DBNull.Value : especialidad.Descripcion
                );
        }
    }
}
