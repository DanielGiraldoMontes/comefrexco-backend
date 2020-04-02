using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComeFrexco.Models
{

    public class CargarVereda
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public object departamento { get; set; }
        public object municipio { get; set; }
    }
    public class AgregarVereda
    {
        public string id { get; set; }
        public string descripcion { get; set; }
        public int municipio { get; set; }
    }
}
