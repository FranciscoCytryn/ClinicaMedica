using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=ClinicaMedica; integrated security=true");
            comando = new SqlCommand();
        }

        public void AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }
        }

        public void CerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();
            }
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public void SetearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
            comando.Parameters.Clear(); // Limpiar parámetros
        }

        public void SetearProcedimiento(string storedProcedure)
        {
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = storedProcedure;
        }

        public void EjecutarConsulta()
        {
            comando.Connection = conexion;
            try
            {
                AbrirConexion();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la consulta", ex);
            }
        }

        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                AbrirConexion();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la acción", ex);
            }
            finally
            {
                CerrarConexion();
            }
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public object EjecutarEscalar(string consulta)
        {
            comando.Connection = conexion;
            try
            {
                AbrirConexion();
                return comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la consulta escalar", ex);
            }
            finally
            {
                CerrarConexion();
            }
        }
    }
}
