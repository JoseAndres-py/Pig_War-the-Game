using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Game.Models
{
    public class ListadoPositions
    {
        public List<Negocio.elementGame> Barrera;
        public Negocio.elementGame Enemigo;
        public List<Negocio.elementGame> Disparos;
        public int Puntaje;
    }
}