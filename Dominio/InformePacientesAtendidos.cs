using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class InformePacientesAtendidos
    {
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int CantidadAtenciones { get; set; }
    }
}
