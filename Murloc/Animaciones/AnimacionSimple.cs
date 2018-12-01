using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Murloc_Tamagochi.Animaciones
{
    class AnimacionSimple
    {
        private Storyboard animacion;
        private Boolean reproduciendo = false;

        public AnimacionSimple(Storyboard animacion)
        {
            this.Animacion = animacion;
        }

        public bool Reproduciendo { get => reproduciendo; set => reproduciendo = value; }
        public Storyboard Animacion { get => animacion; set => animacion = value; }

        public void Start()
        {
            reproduciendo = true;
            Animacion.Begin();
        }
        public void Stop()
        {
            reproduciendo = false;
            Animacion.Stop();
        }
    }
}
