using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TurnoTrabajo
    {
        public int TurnoTrabajoId { get; set; }
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
    }
}
