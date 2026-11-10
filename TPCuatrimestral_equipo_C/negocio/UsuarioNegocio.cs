using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class UsuarioNegocio
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, nombre_usuario, tipo, id_medico
                       FROM usuarios
                       WHERE activo = 1");//Listamos solamente los usuarios activos, excluyendo los dados de baja.

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario();

                    usuario.Id = (int)datos.Lector["id"];
                    usuario.NombreUsuario = (string)datos.Lector["nombre_usuario"];

                    // Convierte el texto de SQL Server al enum TipoUsuario
                    usuario.Tipo = (TipoUsuario)Enum.Parse(
                            typeof(TipoUsuario),
                            (string)datos.Lector["tipo"]
                    );


                    if (datos.Lector["id_medico"] != DBNull.Value)
                    {
                        usuario.Medico = new Medico();
                        usuario.Medico.Id = (int)datos.Lector["id_medico"];
                    }



                    lista.Add(usuario);
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios.", ex);
            }

            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }


        public void Agregar(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (ExisteNombreUsuario(usuario.NombreUsuario))
                {
                    throw new Exception("El nombre de usuario ya existe.");
                }

                datos.SetearConsulta("INSERT INTO usuarios (nombre_usuario, contrasenia, tipo, id_medico)" +
                    " VALUES (@nombre_usuario, @contrasenia, @tipo, @id_medico)");

                datos.SetearParametro("@nombre_usuario", usuario.NombreUsuario);
                datos.SetearParametro("@contrasenia", usuario.Contrasenia);
                datos.SetearParametro("@tipo", usuario.Tipo.ToString());
                datos.SetearParametro("@id_medico",
                    usuario.Medico != null ? (object)usuario.Medico.Id : DBNull.Value);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el usuario.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(
                    "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = @nombre_usuario");

                datos.SetearParametro("@nombre_usuario", nombreUsuario);

                int cantidad = (int)datos.EjecutarScalar();

                return cantidad > 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Usuario BuscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Usuario usuario = null; // Porque todavía no sabemos si el usuario existe

            try
            {
                datos.SetearConsulta(
                    @"SELECT id, nombre_usuario, tipo, id_medico
                       FROM usuarios
                       WHERE id = @id");

                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuario();

                    usuario.Id = (int)datos.Lector["id"];
                    usuario.NombreUsuario = (string)datos.Lector["nombre_usuario"];

                    usuario.Tipo = (TipoUsuario)Enum.Parse(
                        typeof(TipoUsuario),
                        (string)datos.Lector["tipo"]);

                    if (datos.Lector["id_medico"] != DBNull.Value)
                    {
                        usuario.Medico = new Medico();
                        usuario.Medico.Id = (int)datos.Lector["id_medico"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return usuario;
        }


        public void Modificar(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {

                if (ExisteNombreUsuario(usuario.NombreUsuario, usuario.Id))
                {
                    throw new Exception("El nombre de usuario ya existe.");
                }

                datos.SetearConsulta(@"UPDATE usuarios
                               SET nombre_usuario = @nombre_usuario,
                                   tipo = @tipo,
                                   id_medico = @id_medico
                               WHERE id = @id");

                datos.SetearParametro("@nombre_usuario", usuario.NombreUsuario);
                datos.SetearParametro("@tipo", usuario.Tipo.ToString());
                datos.SetearParametro("@id_medico",
                    usuario.Medico != null ? (object)usuario.Medico.Id : DBNull.Value);
                datos.SetearParametro("@id", usuario.Id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el usuario.", ex);
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
                datos.SetearConsulta(@"UPDATE usuarios
                               SET activo = 0
                               WHERE id = @id");

                datos.SetearParametro("@id", id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public bool ExisteNombreUsuario(string nombreUsuario, int id) // sobrecarga de metodo- mismo nombre
                                                                      // distinto parametro
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"SELECT COUNT(*)
                               FROM usuarios
                               WHERE nombre_usuario = @nombre_usuario
                               AND id <> @id"); //excluye al usuario que estamos modificando de la búsqueda

                datos.SetearParametro("@nombre_usuario", nombreUsuario);
                datos.SetearParametro("@id", id);

                int cantidad = (int)datos.EjecutarScalar();

                return cantidad > 0;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
