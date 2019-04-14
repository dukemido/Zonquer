using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServer.Tables
{
    public class Config
    {
        static object obj = new object();
        public static int GetLastUID()
        {
            return 1;
            lock (obj)
            {
                int uid = 0;
                using (var conn = new SqlConnection(Program.connectionString))
                using (var cmd = new SqlCommand("GetLastUID", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                        if (reader.Read())
                            uid = int.Parse(reader["uid"].ToString());
                }
                return uid;
            }
        }
    }
}
