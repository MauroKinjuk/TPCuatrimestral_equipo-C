using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;


        // constructor
        public AccesoDatos()
        {
            conexion = new SqlConnection();

            // Conexion con env
            string conexionConEnv = Environment.GetEnvironmentVariable("DB_CLINICA_CONNECTION", EnvironmentVariableTarget.User);

            // Si env es null utiliza la connection string
            conexion.ConnectionString = conexionConEnv ?? @"Server=localhost\SQLEXPRESS;Database=clinica_privada;Integrated Security=True;";

            comando = new SqlCommand();
            comando.Connection = conexion;
        }

        
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void EjecutarLectura()
        {
            conexion.Open();
            lector = comando.ExecuteReader();
        }


        public SqlDataReader Lector
        {
            get { return lector; }
        }


        public void CerrarConexion()
        {
            if (lector != null && !lector.IsClosed)
            {
                lector.Close();
            }

            if (conexion != null && conexion.State != System.Data.ConnectionState.Closed)
            {
                conexion.Close();
            }
        }
    }
}
