using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Medico
    {
        public int MedicoId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<Dominio.Especialidad> Especialidades { get; set; }
    }
}
