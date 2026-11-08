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
    }
}
