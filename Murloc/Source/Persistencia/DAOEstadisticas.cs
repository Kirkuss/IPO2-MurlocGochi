using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Persistencia
{
    class DAOEstadisticas
    {
        public void Read(Estadisticas e)
        {
            String sql = "SELECT * FROM Estadisticas WHERE Avatar='" + e.Avatar + "';";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                e.TiempoAvatar = Convert.ToInt32(reader["TiempoJugado"]);
                e.ClickJugar = Convert.ToInt32(reader["ClicksJugar"]);
                e.ClicksDormir = Convert.ToInt32(reader["ClicksDormir"]);
                e.ClicksComer = Convert.ToInt32(reader["ClicksComer"]);
                e.ClicksBolsas = Convert.ToInt32(reader["ClicksBolsas"]);
                e.ClicksPeces = Convert.ToInt32(reader["ClicksPeces"]);
            }
            reader.Close();
        }

        public int insert(Estadisticas e)
        {
            String sql = "INSERT INTO Estadisticas VALUES ('" + e.Avatar + "', '" + e.TiempoAvatar + "', '" + e.ClickJugar + "', '" + e.ClicksDormir + "', '" + e.ClicksComer + "', '" + e.ClicksBolsas + "', '" + e.ClicksPeces + "');";
            return BDConector.getDB().Query(sql);
        }

        public int update(Estadisticas e)
        {
            String sql = "UPDATE Estadisticas SET TiempoJugado='" + e.TiempoAvatar + "', ClicksJugar='" + e.ClickJugar + "', ClicksDormir='" + e.ClicksDormir + "', ClicksComer='" + e.ClicksComer + "', ClicksBolsas='" + e.ClicksBolsas + "', ClicksPeces='" + e.ClicksPeces + "' WHERE Avatar='" + e.Avatar + "';";
            return BDConector.getDB().Query(sql);
        }

        public int delete()
        {
            String sql = "DELETE FROM Estadisticas;";
            return BDConector.getDB().Query(sql);
        }
    }
}
