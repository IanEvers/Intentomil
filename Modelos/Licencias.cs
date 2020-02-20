using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intentomil.Modelos
{
    public class Licencias
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string preFechaInicio { get; set; }
        public string preFechaFin{ get; set; }
        public DateTime FechaInicio  { get; set; }
        public DateTime FechaFin{ get; set; }
        public string Tipo { get; set; }
        public string Provincia { get; set; }
        public string Localidad { get; set; }
        public string Direccion { get; set; }
        public string Ordendeldia{ get; set; }
        public int dni { get; set; }
        public string destino { get; set; }
        public string subyofi { get; set; }

    }
}
