using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Matricula { get; set; }
        public List<Especialidad> Especialidades { get; set; }
        public List<TurnoTrabajo> TurnosTrabajo { get; set; }
    }
}
