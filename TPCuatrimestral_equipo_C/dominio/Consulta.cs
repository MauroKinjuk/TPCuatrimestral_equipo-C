using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Consulta
    {
        public int Id { get; set; }
        public TurnoMedico Turno { get; set; }
        public string Motivo { get; set; }
        public string Diagnostico { get; set; }
    }
}
