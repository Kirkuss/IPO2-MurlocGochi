using Murloc_Tamagochi.Source.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Dominio
{
    class Estadisticas
    {
        private int tiempoAvatar;
        private int clickJugar;
        private int clicksDormir;
        private int clicksComer;
        private int clicksBolsas;
        private int clicksPeces;
        private String avatar;
        private DAOEstadisticas DAO = new DAOEstadisticas();

        public Estadisticas()
        {
            tiempoAvatar = 0;
        }

        public Estadisticas(String avatar)
        {
            this.Avatar = avatar;
            tiempoAvatar = 0;
            clickJugar = 0;
            clicksDormir = 0;
            clicksComer = 0;
            clicksBolsas = 0;
            ClicksPeces = 0;
        }

        public int TiempoAvatar { get => tiempoAvatar; set => tiempoAvatar = value; }
        public int ClickJugar { get => clickJugar; set => clickJugar = value; }
        public int ClicksDormir { get => clicksDormir; set => clicksDormir = value; }
        public int ClicksComer { get => clicksComer; set => clicksComer = value; }
        public int ClicksBolsas { get => clicksBolsas; set => clicksBolsas = value; }
        public int ClicksPeces { get => clicksPeces; set => clicksPeces = value; }
        public string Avatar { get => avatar; set => avatar = value; }

        public void Read()
        {
            DAO.Read(this);
        }

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
    }
}
