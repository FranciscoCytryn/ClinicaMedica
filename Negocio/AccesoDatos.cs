using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
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
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=ClinicaMedica; integrated security=true; TrustServerCertificate=True");
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

        public object EjecutarStoredProcedure(string storedProcedure, SqlParameter[] parametros)
        {
            comando.Connection = conexion;
            try
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = storedProcedure;
                comando.Parameters.Clear();

                if (parametros != null)
                {
                    comando.Parameters.AddRange(parametros);
                }

                AbrirConexion();
                return comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el procedimiento", ex);
            }
            finally
            {
                CerrarConexion();
            }
        }

        public Usuario ValidarCredenciales(string email, string password)
        {
            try
            {
                SetearProcedimiento("sp_ValidarCredenciales");
                SetearParametro("@Email", email);
                SetearParametro("@Password", password);
                EjecutarConsulta();

                if (lector.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        UsuarioId = (int)lector["UsuarioId"],
                        Nombre = lector["Nombre"].ToString(),
                        Email = lector["Email"].ToString(),
                        Password = lector["Password"].ToString(),
                        Rol = lector["Rol"].ToString(),
                        Telefono = lector["Telefono"].ToString()
                    };
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CerrarConexion();
            }
        }
    }
}
