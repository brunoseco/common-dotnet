using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Helpers
{
    public static class AdoNetHelper
    {
        public static int ExecuteNonQuery(string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var result = default(int);
            var conn = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                var sqlParameters = Parameters.PrepareArguments(commandText, parameters).Item2;
                foreach (var param in sqlParameters)
                    cmd.Parameters.Add(param);
                conn.Open();
                result = cmd.ExecuteNonQuery();
                conn.Close();
            }

            return result;
        }

        public static IEnumerable<dynamic> ExecuteReader(string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            FactoryLog.GetInstace().Info(string.Format("{0}", commandText));

            var table = new List<Dictionary<string, object>>();
            var conn = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                var sqlParameters = Parameters.PrepareArguments(commandText, parameters).Item2;
                foreach (var param in sqlParameters)
                    cmd.Parameters.Add(param);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader[i];
                        table.Add(row);
                    }
                }

                conn.Close();
            }

            var result = table.DictionaryToObject();
            return result;
        }

        public static IEnumerable<dynamic> ExecuteReaderMARS(string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var tables = new List<List<Dictionary<string, object>>>();
            var resultComplex = new List<dynamic>();
            var conn = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                var sqlParameters = Parameters.PrepareArguments(commandText, parameters).Item2;
                foreach (var param in sqlParameters)
                    cmd.Parameters.Add(param);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    TableMatch(tables, reader);

                    while (reader.NextResult())
                    {
                        TableMatch(tables, reader);
                    }
                }
                conn.Close();
            }

            foreach (var table in tables)
            {
                resultComplex.Add(new
                {
                    table = table.DictionaryToObject()
                });
            }

            return resultComplex;
        }

        private static void TableMatch(List<List<Dictionary<string, object>>> tables, SqlDataReader reader)
        {
            var subTable = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader[i];
                subTable.Add(row);
            }
            tables.Add(subTable);
        }
    }
}
