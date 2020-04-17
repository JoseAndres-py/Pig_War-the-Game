using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Directorio.Models
{
    public class Persona
    {
        public Negocio.Persona Datos { get; set; }
        public List<Negocio.TipoContacto> TiposContacto { get; set; }
    }
}