using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class InformeMedicosConTurnosCerrados
    {
        public int MedicoId { get; set; }
        public string MedicoNombre { get; set; }
        public string Especialidades { get; set; }
        public int CantidadTurnosCerrados { get; set; }
    }
}
