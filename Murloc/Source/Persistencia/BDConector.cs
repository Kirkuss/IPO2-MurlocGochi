using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Murloc_Tamagochi.Source.Persistencia
{
    class BDConector
    {
        static String connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=IPOIIDB1.accdb;";
        static OleDbConnection mConn;
        static BDConector instancia = null;

        public BDConector()
        {
            mConn = new OleDbConnection(@"" + connString + "");
            mConn.Open();
        }

        public OleDbDataReader Read(String sql)
        {
            OleDbCommand cmd;
            cmd = mConn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd.ExecuteReader();
        }

        public int Query(String sql)
        {
            OleDbCommand cmd;
            cmd = mConn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd.ExecuteNonQuery();
        }

        public static BDConector getDB()
        {
            if (instancia == null)
            {
                instancia = new BDConector();
            }
            return instancia;
        }
    }
}
