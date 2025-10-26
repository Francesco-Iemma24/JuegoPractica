using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuegoPractica
{
    internal class Jugador
    {
        public string nombre { get; set; }
        public int vida { get; set; } = 4000;
        public List<Carta> Cartas { get; set; }

        public Jugador(string nombre, List<Carta> Cartas)
        {
            this.nombre = nombre;
            this.Cartas = Cartas;

        }

        public void recibirDaño(int daño)
        {
            vida -= daño;
            if (vida < 0) vida = 0;
        }
    }
}
