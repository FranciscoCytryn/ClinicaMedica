using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    [Serializable]
    public class GestionPasos
    {
        public List<Paso> Pasos { get; set; }

        public GestionPasos()
        {
            Pasos = new List<Paso>();
        }

        public void AgregarPaso(string id)
        {
            Pasos.Add(new Paso { Id = id, Habilitado = false });
        }

        public void HabilitarPaso(string id)
        {
            var paso = Pasos.FirstOrDefault(p => p.Id == id);
            if (paso != null)
            {
                paso.Habilitado = true;
            }
        }

        public void DeshabilitarPaso(string id)
        {
            var paso = Pasos.FirstOrDefault(p => p.Id == id);
            if (paso != null)
            {
                paso.Habilitado = false;
            }
        }

        public Paso ObtenerPaso(string id)
        {
            return Pasos.FirstOrDefault(p => p.Id == id);
        }

        public string ObtenerSiguientePaso(string idPasoActual)
        {
            int indiceActual = Pasos.FindIndex(p => p.Id == idPasoActual);
            if (indiceActual < Pasos.Count - 1)
            {
                return Pasos[indiceActual + 1].Id;
            }
            return null;
        }

        public string ObtenerPasoAnterior(string idPasoActual)
        {
            int indiceActual = Pasos.FindIndex(p => p.Id == idPasoActual);
            if (indiceActual > 0)
            {
                return Pasos[indiceActual - 1].Id;
            }
            return null;
        }
    }
}
