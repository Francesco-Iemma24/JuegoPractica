using System.Windows.Forms;
using System.Media;

namespace JuegoPractica
{
    public partial class Form1 : Form
    {
        private Duelo duelo;
        private Carta cartaSeleccionadaJugador = null;
        private PictureBox[] campoJugador;
        private PictureBox[] campoIA;
        private List<PictureBox> CartasVisuales = new List<PictureBox>();
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();


            campoJugador = new PictureBox[] { picCampoJ1, picCampoJ2, picCampoJ3, picCampoJ4, picCampoJ5 };
            campoIA = new PictureBox[] { pictCampoIA1, pictCampoIA2, pictCampoIA3, pictCampoIA4, pictCampoIA5 };
            CartasVisuales.AddRange(new PictureBox[] { picCarta1, picCarta2, picCarta3, picCarta4, picCarta5 });


            foreach (PictureBox pic in campoJugador.Concat(campoIA))
            {
                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.BackColor = Color.FromArgb(30, 30, 30);

                pic.MouseEnter += (s, e) => pic.BorderStyle = BorderStyle.Fixed3D;
                pic.MouseLeave += (s, e) => pic.BorderStyle = BorderStyle.FixedSingle;
            }

            foreach (PictureBox pic in CartasVisuales)
            {
                pic.BorderStyle = BorderStyle.FixedSingle;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.BackColor = Color.FromArgb(30, 30, 30);
                pic.MouseEnter += (s, e) => pic.BorderStyle = BorderStyle.Fixed3D;
                pic.MouseLeave += (s, e) => pic.BorderStyle = BorderStyle.FixedSingle;
            }
        




            DeshabilitarCartas();


            string rutaSonido = Path.Combine(Application.StartupPath, "Sonidos", "Inicio.wav");
            if (File.Exists(rutaSonido))
            {
                SoundPlayer player = new SoundPlayer(rutaSonido);
                player.Play();
            }


        }


        private void InicializarJuego()
        {
            List<Carta> TodaslasCartas = new List<Carta>
            {
                new Carta("Dragón Blanco de Ojos Azules", 3000,2500,"Imagenes/DragonBlanco.jpg"),
                new Carta("Mago Oscuro", 2500,2100,"Imagenes/MagoOscuro.jpg"),
                new Carta("Ciber Dragón", 4500,3800,"Imagenes/DragonBlancoDefinitivo.jpg"),
                new Carta("Slifer El Dragon Del Cielo", 4000, 4000, "Imagenes/Slifer.jpg"),
                new Carta("Guerrero Celta", 1400, 1200, "Imagenes/GuerreroZelta.jpg"),
                new Carta("Maga Oscura", 2000, 1700, "Imagenes/MagaOscura.jpg"),
                new Carta("DarkPaladin", 2900, 2400, "Imagenes/DarkPaladin.jpg"),
                new Carta("Dragon Alado de Ra", 5000, 5000, "Imagenes/DragonAladoDeRa.jpg"),
                new Carta("Dragón Negro de Ojos Rojo", 2400, 2000, "Imagenes/DragonNegroOjosRojos.jpg"),
                new Carta("Espadachin De Llamas", 1800, 1600, "Imagenes/EspadachinDeLlamas.jpg"),
                new Carta("Guerrero Zombie", 1200, 900, "Imagenes/GuerreroZombie.jpg"),
                new Carta("Bestia De ataque", 1900, 1200, "Imagenes/BestiaDeAtaque.jpg"),
                new Carta("Obelisko El Atormentador", 4000, 4000, "Imagenes/Obelisko.jpg"),
                new Carta("Metal Dragon", 1850, 1700, "Imagenes/MetalDragon.jpg"),
                new Carta("Soldado de Brillo Negro", 3000, 2500, "Imagenes/BrilloNegro.jpg"),
                new Carta("Gazella", 1900, 900, "Imagenes/Gazella.jpg"),
                new Carta("Garoozis",1800,1500,"Imagenes/Garoozis.jpg"),
                new Carta("Gaia el Caballero Feroz",2300,2100,"Imagenes/Gaia.jpg"),
                new Carta("Mago del Caos negro",2800,2600,"Imagenes/MagoDelCaos.jpg"),
                new Carta("Guardian de la Reja",3750,3400,"Imagenes/GateGuardian.jpg"),
                new Carta("Caballero Amo Del Dragon",5000,5000,"Imagenes/DragonDefinitivo.jpg"),
                new Carta("Maga Oscura Jinete de Dragon",2600,2100,"Imagenes/MagaOscuraJinete.png"),
                new Carta("Convoca al Craneo",2500,2000,"Imagenes/CraneoInvocado.jgp")

            }.OrderBy(c => random.Next()).ToList();


            List<Carta> cartasJugador = TodaslasCartas.Take(5).ToList();
            List<Carta> cartasIA = TodaslasCartas.Skip(5).Take(5).ToList();

            duelo = new Duelo(new Jugador("Tu", cartasJugador), new Jugador("IA", cartasIA));

            MostrarCartasJugador();
            foreach (var pic in campoJugador) pic.Click -= CampoJugador_Click;
            foreach (var pic in campoIA) pic.Click -= CampoIA_Click;
            foreach (var pic in campoJugador) pic.Click += CampoJugador_Click;
            foreach (var pic in campoIA) pic.Click += CampoIA_Click;


            ActualizarCampoVisual();
            ActualizarVidas();
            lblResultado.Text = "";

        }
        private void CampoJugador_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic?.Tag is Carta carta) cartaSeleccionadaJugador = carta;
        }


        private async void CampoIA_Click(object sender, EventArgs e)
        {
            if (cartaSeleccionadaJugador == null) return;
            PictureBox picIA = sender as PictureBox;
            if (picIA?.Tag is Carta cartaIA)
            {
                int slotJ = duelo.campo.slotsJugador.IndexOf(cartaSeleccionadaJugador);
                int slotIA = duelo.campo.slotsIA.IndexOf(cartaIA);

                PictureBox picJ = campoJugador[slotJ];
                Point ubicacionJugador = picJ.Location;
                Point ubicacionIA = picIA.Location;


                lblResultado.Text = duelo.BatallaIndividual(slotJ, slotIA);

                Point ubicacionFuego = Point.Empty;
                if (duelo.campo.slotsIA[slotIA] == null)
                {
                    ubicacionFuego = ubicacionIA;
                }
                  
                else if (duelo.campo.slotsJugador[slotJ] == null)
                {
                    ubicacionFuego = ubicacionJugador;
                }
                else if (duelo.campo.slotsJugador[slotJ] == null && duelo.campo.slotsIA[slotIA] == null)
                {
                    ubicacionFuego = ubicacionIA; 
                }

               
                if (ubicacionFuego != Point.Empty)
                {
                 
                    int offsetX = (picIA.Width - picFuego.Width) / 2;
                    int offsetY = (picIA.Height - picFuego.Height) / 2;

                    ubicacionFuego.Offset(offsetX, offsetY);

                    
                    await mostrarFuego(ubicacionFuego);
                }
                cartaSeleccionadaJugador = null;

                ActualizarCampoVisual();
                ActualizarVidas();

                if (duelo.DueloTerminado())
                {
                    MessageBox.Show(duelo.resultadoFinal(), "Fin del duelo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReproducirSonido(duelo.resultadoFinal() + ".wav");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void MostrarCartasJugador()
        {
            for (int i = 0; i < CartasVisuales.Count; i++)
            {
                if (i < duelo.jugador.Cartas.Count)
                {
                    var carta = duelo.jugador.Cartas[i];
                    CartasVisuales[i].Image = File.Exists(carta.Imagen) ? Image.FromFile(carta.Imagen) : null;
                    CartasVisuales[i].Tag = carta;
                    CartasVisuales[i].Enabled = true;
                    CartasVisuales[i].SizeMode = PictureBoxSizeMode.Zoom;

                    CartasVisuales[i].Click -= Carta_Click;
                    CartasVisuales[i].Click += Carta_Click;
                }
                else
                {
                    CartasVisuales[i].Image = null;
                    CartasVisuales[i].Enabled = false;
                }
            }
        }
        private async void btnJugar_Click(object sender, EventArgs e)
        {
            InicializarJuego();
            btnJugar.Enabled = false;


            lblResultado.Text = "";

            lblVidaJugador.Text = duelo.jugador.vida.ToString();
            lblVidaIA.Text = duelo.ia.vida.ToString();

            string rutaSonido = Path.Combine(Application.StartupPath, "Sonidos", "Duelo.wav");
            if (File.Exists(rutaSonido))
            {
                SoundPlayer player = new SoundPlayer(rutaSonido);
                player.Play();
            }

            foreach (var pic in CartasVisuales)
                pic.Enabled = true;

        }
        private void Carta_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic?.Tag is Carta carta)
            {
                lblResultado.Text = duelo.ColocarCartaJugador(carta);
                ActualizarCampoVisual();
                MostrarCartasJugador();
            }
        }

        private void ActualizarCampoVisual()
        {
            for (int i = 0; i < CartasVisuales.Count; i++)
            {
                campoJugador[i].Image = duelo.campo.slotsJugador[i] != null && File.Exists(duelo.campo.slotsJugador[i].Imagen)
                    ? Image.FromFile(duelo.campo.slotsJugador[i].Imagen) : null;
                campoJugador[i].Tag = duelo.campo.slotsJugador[i];

                campoIA[i].Image = duelo.campo.slotsIA[i] != null && File.Exists(duelo.campo.slotsIA[i].Imagen)
                    ? Image.FromFile(duelo.campo.slotsIA[i].Imagen) : null;
                campoIA[i].Tag = duelo.campo.slotsIA[i];
            }
        }

        private void ActualizarVidas()
        {
            lblVidaJugador.Text = duelo.jugador.vida.ToString();
            lblVidaIA.Text = duelo.ia.vida.ToString();
        }


        private void DeshabilitarCartas()
        {
            foreach (var pic in CartasVisuales) pic.Enabled = false;
            CartasVisuales.ForEach(pic => pic.Enabled = false);
        }

        private async Task mostrarFuego(Point ubicacion)
        {
            picFuego.Location = ubicacion;
            picFuego.BringToFront();
            picFuego.Visible = true;
            await Task.Delay(350);
            picFuego.Visible = false;
        }

        private void ReproducirSonido(string nombreArchivo)
        {
            string ruta = Path.Combine(Application.StartupPath, "Sonidos", nombreArchivo);
            if (File.Exists(ruta))
            {
                SoundPlayer player = new SoundPlayer(ruta);
                player.Play();
            }
        }



        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (var process in System.Diagnostics.Process.GetProcessesByName("wmplayer"))
                    process.Kill();
            }
            catch { }


            lblResultado.Text = "";
            DeshabilitarCartas();
            CartasVisuales.ForEach(pic => pic.Enabled = false);



            btnJugar.Enabled = true;


            lblVidaJugador.Text = duelo.jugador.vida.ToString();
            lblVidaIA.Text = duelo.ia.vida.ToString();



            string rutaSonido = Path.Combine(Application.StartupPath, "Sonidos", "Inicio.wav");
            if (File.Exists(rutaSonido))
            {
                SoundPlayer player = new SoundPlayer(rutaSonido);
                player.PlayLooping();
            }

            MessageBox.Show("El juego se ha reiniciado correctamente.", "Reinicio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void picFuego_Click(object sender, EventArgs e)
        {

        }
    }
}
