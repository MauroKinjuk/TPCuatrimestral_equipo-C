using System;
using System.Collections.Generic;
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
                datos.SetearConsulta("SELECT * FROM Pacientes");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente paciente = new Paciente();

                    paciente.Id = (int)datos.Lector["Id"];
                    paciente.Nombre = (string)datos.Lector["Nombre"];
                    paciente.Apellido = (string)datos.Lector["Apellido"];
                    paciente.Dni = (string)datos.Lector["Dni"];
                    paciente.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    paciente.Telefono = datos.Lector["Telefono"].ToString();
                    paciente.Email = datos.Lector["Email"].ToString();


                    lista.Add(paciente);

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




    }
}
