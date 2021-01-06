using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_Change_Wpf.Service
{
    public static class Sql_Service
    {

        public static DataTable GetHis_IP(string ip = "")
        {
            string sql = string.Format(@"SELECT * FROM IP_Save t {0}where ip ='{1}' ", ip == "" ? "--" : "", ip);

            DataTable dt = DbhelperSQLite.dtQuery(sql);

            return dt;

        }
        public static void InsertIP(string IP, string subnet, string gateway, string dns)
        {
            string sql = string.Format(@"INSERT INTO IP_Save (
	IP,
	subnet,
	gateway,
	dns
)
VALUES
	('{0}', '{1}', '{2}','{3}')", IP, subnet, gateway, dns);

            DbhelperSQLite.ExecuteSql(sql);

        }

        public static void DeletIP(string ID)
        {
            string sql = string.Format(@"DELETE from IP_Save where id='{0}'", ID);

            DbhelperSQLite.ExecuteSql(sql);

        }
    }
}
