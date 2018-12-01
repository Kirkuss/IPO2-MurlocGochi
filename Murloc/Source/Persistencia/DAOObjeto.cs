using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Murloc_Tamagochi.Source.Persistencia
{
    class DAOObjeto
    {
        public void Read(Objeto o)
        {
            String sql = "SELECT * FROM Objeto WHERE Nombre='" + o.Nombre + "';";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            ArrayList logros = new ArrayList();
            while (reader.Read())
            {
                o.Coste = Convert.ToInt32(reader["Coste"]);
                o.Obtenido = Convert.ToInt32(reader["Obtenido"]);
                o.Imagen = Convert.ToString(reader["Imagen"]);
                o.Nivel = Convert.ToInt32(reader["Nivel"]);
                o.Tipo = Convert.ToString(reader["Tipo"]);
            }
            reader.Close();
        }

        public ArrayList ReadAllFondos()
        {
            Objeto o;
            ArrayList fondos = new ArrayList();
            String sql = "SELECT * FROM Objeto WHERE tipo='fondo';";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                o = new Objeto();
                o.Nombre = Convert.ToString(reader["Nombre"]);
                o.Coste = Convert.ToInt32(reader["Coste"]);
                o.Obtenido = Convert.ToInt32(reader["Obtenido"]);
                o.Imagen = Convert.ToString(reader["Imagen"]);
                o.Nivel = Convert.ToInt32(reader["Nivel"]);
                o.Tipo = Convert.ToString(reader["Tipo"]);
                fondos.Add(o);
            }
            reader.Close();
            return fondos;
        }

        public ArrayList ReadAllTrajes()
        {
            Objeto o;
            ArrayList trajes = new ArrayList();
            String sql = "SELECT * FROM Objeto WHERE tipo='traje';";
            OleDbDataReader reader = BDConector.getDB().Read(sql);
            while (reader.Read())
            {
                o = new Objeto();
                o.Nombre = Convert.ToString(reader["Nombre"]);
                o.Coste = Convert.ToInt32(reader["Coste"]);
                o.Obtenido = Convert.ToInt32(reader["Obtenido"]);
                o.Imagen = Convert.ToString(reader["Imagen"]);
                o.Nivel = Convert.ToInt32(reader["Nivel"]);
                o.Tipo = Convert.ToString(reader["Tipo"]);
                trajes.Add(o);
            }
            reader.Close();
            return trajes;
        }

        public int reset()
        {
            String sql = "UPDATE Objeto SET Obtenido=0 WHERE Nivel<>0;";
            return BDConector.getDB().Query(sql);
                
        }

        public int update(Objeto o)
        {
            String sql = "UPDATE Objeto SET Obtenido=" + o.Obtenido + " WHERE Nombre='" + o.Nombre + "';";
            return BDConector.getDB().Query(sql);
        }
    }
}
