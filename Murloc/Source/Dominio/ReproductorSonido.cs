using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Murloc_Tamagochi.Source.Dominio
{
    class ReproductorSonido
    {
        MediaPlayer reproductor;
        private String path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Sonidos");

        public ReproductorSonido()
        {
            reproductor = new MediaPlayer();
        }

        public void confuso()
        {
            reproductor.Open(new Uri(path + "\\Confuso.mp3"));
            reproductor.Play();
        }
        
        public void sonidoAleatorio(int n)
        {
            switch (n)
            {
                case 0:
                    reproductor.Open(new Uri(path + "\\No_1.mp3"));
                    reproductor.Play();
                    break;
                case 1:
                    reproductor.Open(new Uri(path + "\\No_2.mp3"));
                    reproductor.Play();
                    break;
                case 2:
                    reproductor.Open(new Uri(path + "\\Discrepo.mp3"));
                    reproductor.Play();
                    break;
                case 3:
                    reproductor.Open(new Uri(path + "\\Uff.mp3"));
                    reproductor.Play();
                    break;

            }
        }

        public void saludar()
        {
            reproductor.Open(new Uri(path + "\\Saludo.mp3"));
            reproductor.Play();
        }

        public void roncar()
        {
            reproductor.Open(new Uri(path + "\\roncar.mp3"));
            reproductor.Play();
        }

        public void comer()
        {
            reproductor.Open(new Uri(path + "\\comer.mp3"));
            reproductor.Play();
        }

        public void logro()
        {
            reproductor.Open(new Uri(path + "\\Completado.mp3"));
            reproductor.Play();
        }

        public void intro()
        {
            reproductor.Open(new Uri(path + "\\intro.mp3"));
            reproductor.Play();
        }

        public void juego()
        {
            reproductor.Open(new Uri(path + "\\juego.mp3"));
            reproductor.MediaEnded += loopJuego;
            reproductor.Play();
        }

        public void loopJuego(Object sender, EventArgs e)
        {
            juego();
        }

        public void Golpe()
        {
            reproductor.Open(new Uri(path + "\\Golpe.mp3"));
            reproductor.Play();
        }

        public void coger()
        {
            reproductor.Open(new Uri(path + "\\Bolsa.mp3"));
            reproductor.Play();
        }

        public void morir()
        {
            reproductor.Open(new Uri(path + "\\Muerte.mp3"));
            reproductor.Play();
        }

        public void pararReproductor()
        {
            reproductor.Stop();
        }

        public void setVolumen(double volumen)
        {
            reproductor.Volume = volumen;
        }

        public void comprar()
        {
            reproductor.Open(new Uri(path + "\\Compra.mp3"));
            reproductor.Play();
        }
    }
}
