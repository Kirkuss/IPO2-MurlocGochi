using Murloc_Tamagochi.Source.Persistencia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Dominio
{
    class Logro
    {
        private int idLogro;
        private int recompensa;
        private String nombre;
        private String descripcion;
        private int tipo;
        private String imagen;
        private int obtenido;
        private DAOLogro DAO = new DAOLogro();

        public Logro() { }

        public int IdLogro { get => idLogro; set => idLogro = value; }
        public int Recompensa { get => recompensa; set => recompensa = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Imagen { get => imagen; set => imagen = value; }
        public int Obtenido { get => obtenido; set => obtenido = value; }

        public void Read()
        {
            DAO.Read(this);
        }

        public ArrayList ReadAll()
        {
            return DAO.ReadAll();
        }

        public int Update()
        {
            return DAO.update(this);
        }

        public bool completados(ArrayList lista, Logro final)
        {
            Logro l;
            for(int i = 0; i<lista.Count; i++)
            {
                l = (Logro)lista[i];
                if (l.obtenido == 0 && !l.Equals(final))
                {
                    return false;
                }
            }
            return true;
        }

        public int reset()
        {
            return DAO.reset();
        }

        public void Recompensar(Avatar a)
        {
            switch (this.tipo)
            {
                case 0:
                    a.Nivel += this.recompensa;
                    break;
                case 1:
                    a.Oro += this.recompensa;
                    break;
                case 2:
                    a.Nivel += this.recompensa/100;
                    a.Oro += this.recompensa;
                    break;
            }
            this.obtenido = 1;
            DAO.update(this);
        }
    }
}