using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
    public class EspecialidadNegocio
    {

        public List<Dominio.Especialidad> Listar()
        {
            List<Dominio.Especialidad> especialidades = new List<Dominio.Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ListarEspecialidades");
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    Dominio.Especialidad especialidad = new Dominio.Especialidad
                    {
                        EspecialidadId = (int)datos.Lector["EspecialidadId"],
                        Nombre = datos.Lector["Nombre"].ToString()
                    };
                    especialidades.Add(especialidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las especialidades", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return especialidades;
        }

        public List<string> ListarPorMedico(int medicoId)
        {
            List<string> especialidades = new List<string>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearProcedimiento("sp_ObtenerEspecialidadesPorMedico");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.EjecutarConsulta();

                while (datos.Lector.Read())
                {
                    especialidades.Add(datos.Lector["Nombre"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las especialidades del médico", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }

            return especialidades;
        }


    }
}