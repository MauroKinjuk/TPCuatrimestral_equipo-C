using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        private SqlTransaction transaccion;


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
            comando.Parameters.Clear();
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void AbrirConexion()
        {
            if (conexion.State == ConnectionState.Closed)
                conexion.Open();
        }

        public void IniciarTransaccion()
        {
            AbrirConexion();

            transaccion = conexion.BeginTransaction();
            comando.Transaction = transaccion;
        }

        public void ConfirmarTransaccion()
        {
            transaccion.Commit();

            // Desvinculacion de la transaccion y liberacion de recursos
            comando.Transaction = null;
            transaccion.Dispose();
            transaccion = null;

            CerrarConexion();
        }

        public void RevertirTransaccion()
        {
            if (transaccion != null)
            {
                transaccion.Rollback();

                // Desvinculacion de la transaccion y liberacion de recursos
                comando.Transaction = null;
                transaccion.Dispose();
                transaccion = null;
            }

            CerrarConexion();
        }

        public void EjecutarLectura()
        {
            conexion.Open();
            lector = comando.ExecuteReader();
        }

        public void EjecutarAccion()
        {
            conexion.Open();
            comando.ExecuteNonQuery();
        }


        public object EjecutarScalar()
        {
            conexion.Open();
            return comando.ExecuteScalar();
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
