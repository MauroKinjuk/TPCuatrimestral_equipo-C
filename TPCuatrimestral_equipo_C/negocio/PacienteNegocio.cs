using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;


namespace negocio
{
    public class PacienteNegocio
    {
        public List<Paciente> Listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"SELECT p.id, p.nombre, p.apellido, p.dni, p.fecha_nacimiento, p.telefono, p.email, p.direccion, p.nro_afiliado,
                                            os.id AS id_obra_social, os.nombre AS obra_social
                                     FROM pacientes p
                                     LEFT JOIN obras_sociales os ON os.id = p.id_obra_social");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(leerPaciente(datos.Lector));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los pacientes.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return lista;
        }


        public Paciente Obtener(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Paciente paciente = null;

            try
            {
                datos.SetearConsulta(@"SELECT p.id, p.nombre, p.apellido, p.dni, p.fecha_nacimiento, p.telefono, p.email, p.direccion, p.nro_afiliado,
                                            os.id AS id_obra_social, os.nombre AS obra_social
                                     FROM pacientes p
                                     LEFT JOIN obras_sociales os ON os.id = p.id_obra_social
                                     WHERE p.id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    paciente = leerPaciente(datos.Lector);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el paciente.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return paciente;
        }


        public void Agregar(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"INSERT INTO pacientes (id_obra_social, nombre, apellido, dni, fecha_nacimiento, telefono, email, direccion, nro_afiliado)
                                     VALUES (@idObraSocial, @nombre, @apellido, @dni, @fechaNacimiento, @telefono, @email, @direccion, @nroAfiliado)");
                cargarParametros(datos, paciente);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el paciente.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        public void Modificar(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"UPDATE pacientes SET
                                        id_obra_social = @idObraSocial,
                                        nombre = @nombre,
                                        apellido = @apellido,
                                        dni = @dni,
                                        fecha_nacimiento = @fechaNacimiento,
                                        telefono = @telefono,
                                        email = @email,
                                        direccion = @direccion,
                                        nro_afiliado = @nroAfiliado
                                     WHERE id = @id");
                cargarParametros(datos, paciente);
                datos.SetearParametro("@id", paciente.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el paciente.", ex);
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
                datos.SetearConsulta("DELETE FROM pacientes WHERE id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el paciente.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        private Paciente leerPaciente(SqlDataReader lector)
        {
            Paciente paciente = new Paciente();

            paciente.Id = (int)lector["id"];
            paciente.Nombre = (string)lector["nombre"];
            paciente.Apellido = (string)lector["apellido"];
            paciente.Dni = (string)lector["dni"];
            paciente.FechaNacimiento = (DateTime)lector["fecha_nacimiento"];
            paciente.Email = (string)lector["email"];
            paciente.Telefono = lector["telefono"] is DBNull ? null : (string)lector["telefono"];
            paciente.Direccion = lector["direccion"] is DBNull ? null : (string)lector["direccion"];
            paciente.NroAfiliado = lector["nro_afiliado"] is DBNull ? null : (string)lector["nro_afiliado"];

            if (!(lector["id_obra_social"] is DBNull))
            {
                paciente.ObraSocial = new ObraSocial();
                paciente.ObraSocial.Id = (int)lector["id_obra_social"];
                paciente.ObraSocial.Nombre = (string)lector["obra_social"];
            }

            return paciente;
        }


        private void cargarParametros(AccesoDatos datos, Paciente paciente)
        {
            datos.SetearParametro("@idObraSocial", paciente.ObraSocial != null ? (object)paciente.ObraSocial.Id : DBNull.Value);
            datos.SetearParametro("@nombre", paciente.Nombre);
            datos.SetearParametro("@apellido", paciente.Apellido);
            datos.SetearParametro("@dni", paciente.Dni);
            datos.SetearParametro("@fechaNacimiento", paciente.FechaNacimiento);
            datos.SetearParametro("@email", paciente.Email);
            datos.SetearParametro("@telefono", string.IsNullOrEmpty(paciente.Telefono) ? (object)DBNull.Value : paciente.Telefono);
            datos.SetearParametro("@direccion", string.IsNullOrEmpty(paciente.Direccion) ? (object)DBNull.Value : paciente.Direccion);
            datos.SetearParametro("@nroAfiliado", string.IsNullOrEmpty(paciente.NroAfiliado) ? (object)DBNull.Value : paciente.NroAfiliado);
        }
    }
}
