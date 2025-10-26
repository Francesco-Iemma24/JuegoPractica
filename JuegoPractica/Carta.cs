using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoPractica
{
    internal class Carta
    {
        public string nombre { get; set; }
        public int ataque { get; set; }
        public int defensa { get; set; }
        public string Imagen { get; set; }

        public Carta(string nombre, int ataque, int defensa, string imagen)
        {
            this.nombre = nombre;
            this.ataque = ataque;
            this.defensa = defensa;
            this.Imagen=imagen;

        }

        public override string ToString()
        {
            return $"{nombre} (ATK: {ataque}, DEF {defensa})";
        }
    }
}
