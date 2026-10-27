using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public enum TipoUsuario
    {
        Administrador,
        Recepcionista,
        Medico
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public TipoUsuario Tipo { get; set; }
        public Medico Medico { get; set; }
    }
}
