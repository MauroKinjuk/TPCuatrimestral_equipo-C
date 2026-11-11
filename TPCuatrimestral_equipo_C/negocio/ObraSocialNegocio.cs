using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;


namespace negocio
{
    public class ObraSocialNegocio
    {
        public List<ObraSocial> Listar()
        {
            List<ObraSocial> lista = new List<ObraSocial>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT id, nombre FROM obras_sociales WHERE activo = 1 ORDER BY nombre");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    ObraSocial obraSocial = new ObraSocial();

                    obraSocial.Id = (int)datos.Lector["id"];
                    obraSocial.Nombre = (string)datos.Lector["nombre"];

                    lista.Add(obraSocial);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las obras sociales.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }


        public void Agregar(ObraSocial obraSocial)
        {
            if (ExisteNombre(obraSocial.Nombre, 0))
            {
                throw new Exception("Ya existe una obra social con ese nombre.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO obras_sociales (nombre) VALUES (@nombre)");
                datos.SetearParametro("@nombre", obraSocial.Nombre);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la obra social.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Modificar(ObraSocial obraSocial)
        {
            if (ExisteNombre(obraSocial.Nombre, obraSocial.Id))
            {
                throw new Exception("Ya existe otra obra social con ese nombre.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE obras_sociales SET nombre = @nombre WHERE id = @id");
                datos.SetearParametro("@nombre", obraSocial.Nombre);
                datos.SetearParametro("@id", obraSocial.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar la obra social.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Eliminar(int id)
        {
            if (TienePacientes(id))
            {
                throw new Exception("No se puede eliminar la obra social porque tiene pacientes asociados.");
            }

            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Solamente cambiamos su estado de activo (1) a inactivo (0).
                datos.SetearConsulta("UPDATE obras_sociales SET activo = 0 WHERE id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la obra social.", ex);
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
                datos.SetearConsulta("SELECT COUNT(*) FROM obras_sociales WHERE nombre = @nombre AND id <> @idExcluir");
                datos.SetearParametro("@nombre", nombre);
                datos.SetearParametro("@idExcluir", idExcluir);

                return (int)datos.EjecutarScalar() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el nombre de la obra social.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public bool TienePacientes(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM pacientes WHERE id_obra_social = @id");
                datos.SetearParametro("@id", id);

                return (int)datos.EjecutarScalar() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar los pacientes de la obra social.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
