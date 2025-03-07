using System;
using System.Collections.Generic;
using System.Linq;
using Dominio;

namespace Negocio
{
    public class InformeNegocio
    {
        private TurnoNegocio turnoNegocio = new TurnoNegocio();
        private PacienteNegocio pacienteNegocio = new PacienteNegocio();


        public List<InformePacientesAtendidos> ObtenerPacientesAtendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            var pacientes = pacienteNegocio.ObtenerPacientesAtendidos(fechaInicio, fechaFin);

            var resultados = pacientes
                .GroupBy(p => p.PacienteId)  
                .Select(g => new InformePacientesAtendidos
                {
                    PacienteId = g.Key, 
                    PacienteNombre = g.First().PacienteNombre, 
                    Email = g.First().Email, 
                    Telefono = g.First().Telefono, 
                    CantidadAtenciones = g.Count() 
                })
                .ToList();

            return resultados;
        }
    }
}