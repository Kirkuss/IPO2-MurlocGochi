using Murloc_Tamagochi.Source.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Dominio
{ 
    class Avatar
    {
        private String nombre;
        private int apetito;
        private int diversion;
        private int energia;
        private float experiencia;
        private int nivel;
        private int oro;
        private int traje;
        private String paisaje;
        private DAOAvatar DAO = new DAOAvatar();

        public Avatar() { }

        public Avatar(int apetito, int diversion, int energia)
        {
            this.Apetito = apetito;
            this.Diversion = diversion;
            this.Energia = energia;
        }

        public int Apetito { get => apetito; set => apetito = value; }
        public int Diversion { get => diversion; set => diversion = value; }
        public int Energia { get => energia; set => energia = value; }
        public float Experiencia { get => experiencia; set => experiencia = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Oro { get => oro; set => oro = value; }
        public int Traje { get => traje; set => traje = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Paisaje { get => paisaje; set => paisaje = value; }

        public int insert()
        {
            return DAO.insert(this);
        }

        public int update()
        {
            return DAO.update(this);
        }

        public int delete()
        {
            return DAO.delete();
        }

        public void Read()
        {
            DAO.Read(this);
        }

        public int count()
        {
            return DAO.count();
        }
    }

}