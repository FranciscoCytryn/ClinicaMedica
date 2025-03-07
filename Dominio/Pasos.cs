using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Serializable]
    public class Paso
    {
        public string Id { get; set; }
        public bool Habilitado { get; set; }
    }
}
