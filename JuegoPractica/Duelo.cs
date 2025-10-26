using System;
using System.Collections.Generic;

namespace JuegoPractica
{
    internal class Duelo
    {
        public Jugador jugador;
        public Jugador ia;
        private Random random = new Random();
        public Campo campo = new Campo();

        public Duelo(Jugador jugador, Jugador ia)
        {
            this.jugador = jugador;
            this.ia = ia;
        }

        public string ColocarCartaJugador(Carta cartaJugador)
        {
            int slotJugador = campo.slotsJugador.IndexOf(null);
            if (slotJugador == -1) return "No puedes colocar más cartas.";

            campo.ColocarCartaJugador(cartaJugador, slotJugador);
            jugador.Cartas.Remove(cartaJugador);

           
            int slotIA = campo.slotsIA.IndexOf(null);
            if (slotIA != -1 && ia.Cartas.Count > 0)
            {
                Carta cartaIA = ia.Cartas[random.Next(ia.Cartas.Count)];
                ia.Cartas.Remove(cartaIA);
                campo.ColocarCartaIA(cartaIA, slotIA);
            }

            return $"{jugador.nombre} colocó {cartaJugador.nombre}.";
        }

        public string BatallaIndividual(int slotJugador, int slotIA)
        {
            var cartaJ = campo.slotsJugador[slotJugador];
            var cartaI = campo.slotsIA[slotIA];
            

            if (cartaJ == null || cartaI == null) return "No hay carta en uno de los slots.";

            if (cartaJ.ataque > cartaI.ataque)
            {
                campo.EliminarCartaIA(slotIA);
                int daño = cartaJ.ataque - cartaI.ataque;
                ia.recibirDaño(daño);
                return $"Tu {cartaJ.nombre} destruyó a {cartaI.nombre}. IA pierde {daño} LP.";
            }
            else if (cartaI.ataque > cartaJ.ataque)
            {
                campo.EliminarCartaJugador(slotJugador);
                int daño = cartaI.ataque - cartaJ.ataque;
                jugador.recibirDaño(daño);
                return $"IA destruyó tu {cartaJ.nombre}. Pierdes {daño} LP.";
            }
            else
            {
                campo.EliminarCartaJugador(slotJugador);
                campo.EliminarCartaIA(slotIA);
                return $"Empate: {cartaJ.nombre} y {cartaI.nombre} son destruidas.";
            }
        }

        public bool DueloTerminado()
        {
            bool jugadorSinCartas = jugador.Cartas.Count == 0 && campo.slotsJugador.TrueForAll(c => c == null);
            bool iaSinCartas = ia.Cartas.Count == 0 && campo.slotsIA.TrueForAll(c => c == null);

            return jugadorSinCartas || iaSinCartas || jugador.vida <= 0 || ia.vida <= 0;
        }

        public string resultadoFinal()
        {
            if (jugador.vida > ia.vida) return "Victoria";
            else if (ia.vida > jugador.vida) return "Derrota";
            else return "Empate";
        }
    }
}
