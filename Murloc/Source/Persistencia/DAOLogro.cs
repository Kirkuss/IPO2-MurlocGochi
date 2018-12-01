using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Murloc_Tamagochi.Source.Persistencia
{
    class DAOLogro
    {
        public void Read(Logro l)
        {
            String sql = "SELECT * FROM Logro WHERE Id='" + l.IdLogro + "';";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                l.Recompensa = Convert.ToInt32(reader["Recompensa"]);
                l.Nombre = Convert.ToString(reader["Nombre"]);
                l.Descripcion = Convert.ToString(reader["Descripcion"]);
                l.Tipo = Convert.ToInt32(reader["Tipo"]);
                l.Imagen = Convert.ToString(reader["Imagen"]);
                l.Obtenido = Convert.ToInt32(reader["Obtenido"]);
            }
            reader.Close();
        }

        public ArrayList ReadAll()
        {
            ArrayList logros = new ArrayList();
            String sql = "SELECT * FROM Logro";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                Logro l = new Logro();
                l.IdLogro = Convert.ToInt32(reader["Id"]);
                l.Recompensa = Convert.ToInt32(reader["Recompensa"]);
                l.Nombre = Convert.ToString(reader["Nombre"]);
                l.Descripcion = Convert.ToString(reader["Descripcion"]);
                l.Tipo = Convert.ToInt32(reader["Tipo"]);
                l.Imagen = Convert.ToString(reader["Imagen"]);
                l.Obtenido = Convert.ToInt32(reader["Obtenido"]);
                logros.Add(l);
            }
            reader.Close();
            return logros;
        }

        public int reset()
        {
            String sql = "UPDATE Logro SET Obtenido=0";
            return BDConector.getDB().Query(sql);
        }

        public int update(Logro l)
        {
            String sql = "UPDATE Logro SET Obtenido=" + l.Obtenido + " WHERE Id=" + l.IdLogro + ";";
            return BDConector.getDB().Query(sql);
        }
    }
}
