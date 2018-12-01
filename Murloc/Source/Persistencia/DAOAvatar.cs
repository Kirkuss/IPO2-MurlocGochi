using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Persistencia
{
    class DAOAvatar
    {
        public void Read(Avatar a)
        {
            String sql = "SELECT * FROM Avatar;";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                a.Nombre = Convert.ToString(reader["Nombre"]);
                a.Paisaje = Convert.ToString(reader["Paisaje"]);
                a.Apetito = Convert.ToInt32(reader["Apetito"]);
                a.Diversion = Convert.ToInt32(reader["Diversion"]);
                a.Energia = Convert.ToInt32(reader["Energia"]);
                a.Experiencia = Convert.ToInt32(reader["Experiencia"]);
                a.Nivel = Convert.ToInt32(reader["Nivel"]);
                a.Oro = Convert.ToInt32(reader["Oro"]);
                a.Traje = Convert.ToInt32(reader["Traje"]);
            }
            reader.Close();
        }

        public int count()
        {
            int count = 0;
            String sql = "SELECT * FROM Avatar;";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                count++;
            }
            return count;
        }

        public int insert(Avatar a)
        {
            String sql = "INSERT INTO Avatar VALUES ('" + a.Nombre + "', " + a.Apetito + ", " + a.Diversion + ", " + a.Energia + ", " + a.Experiencia + ", " + a.Nivel + ", " + a.Oro + ", " + a.Traje + ", '" + a.Paisaje + "');";
            return BDConector.getDB().Query(sql);
        }

        public int update(Avatar a)
        {
            String sql = "UPDATE Avatar SET Apetito=" + a.Apetito + ", Diversion=" + a.Diversion + ", Energia=" + a.Energia + ", Experiencia=" + a.Experiencia + ", Nivel=" + a.Nivel + ", Oro =" + a.Oro + ", Traje=" + a.Traje + ", Paisaje='" + a.Paisaje + "' WHERE Nombre='" + a.Nombre + "';";
            return BDConector.getDB().Query(sql);
        }

        public int delete()
        {
            String sql = "DELETE FROM Avatar;";
            return BDConector.getDB().Query(sql);
        }
    }
}
