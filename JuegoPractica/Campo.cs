using System.Collections.Generic;

namespace JuegoPractica
{
    internal class Campo
    {
        public List<Carta> slotsJugador;
        public List<Carta> slotsIA;

        public Campo()
        {
            slotsJugador = new List<Carta> { null, null, null, null,null };
            slotsIA = new List<Carta> { null, null, null, null,null };
        }

        public void ColocarCartaJugador(Carta carta, int posicion)
        {
            slotsJugador[posicion] = carta;
        }

        public void ColocarCartaIA(Carta carta, int posicion)
        {
            slotsIA[posicion] = carta;
        }

        public void EliminarCartaJugador(int posicion)
        {
            slotsJugador[posicion] = null;
        }

        public void EliminarCartaIA(int posicion)
        {
            slotsIA[posicion] = null;
        }
    }
}
