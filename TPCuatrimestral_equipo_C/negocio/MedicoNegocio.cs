using System;
using System.Collections.Generic;
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
    }
}
