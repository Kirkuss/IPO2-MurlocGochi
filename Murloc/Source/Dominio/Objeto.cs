using Murloc_Tamagochi.Source.Persistencia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Dominio
{
    class Objeto
    {
        private DAOObjeto DAO = new DAOObjeto();   

        private String nombre;
        private int coste;
        private int obtenido;
        private String imagen;
        private int nivel;
        private String tipo;

        public string Nombre { get => nombre; set => nombre = value; }
        public int Coste { get => coste; set => coste = value; }
        public int Obtenido { get => obtenido; set => obtenido = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public int Nivel { get => nivel; set => nivel = value; }

        public Objeto() { }

        public Boolean comprar(Avatar a)
        {
            if(a.Oro >= this.coste && a.Nivel >= this.nivel && this.obtenido != 1)
            {
                a.Oro -= this.coste;
                this.obtenido = 1;
                this.update();
                return true;
            }
            return false;
        }

        public int update()
        {
            return DAO.update(this);
        }

        public void Read()
        {
            DAO.Read(this);
        }

        public int reset()
        {
            return DAO.reset();
        }

        public ArrayList ReadAllFondos()  
        {
            return DAO.ReadAllFondos();
        }

        public ArrayList ReadAllTrajes()
        {
            return DAO.ReadAllTrajes();
        }

    }
}
