using Common.Infrastructure.ORM.Helpers;
using Common.Infrastructure.ORM.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    static class AdoNetExtensions
    {
        public static int ExecuteNonQuery(this Database self, string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return AdoNetHelper.ExecuteNonQuery(commandText, connectionString, parameters, commandType);        
        }

        public static IEnumerable<dynamic> ExecuteReader(this Database database, string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return ExecuteReader(database, commandText, database.Connection.ConnectionString, parameters, commandType);
        }

        public static IEnumerable<dynamic> ExecuteReader(this Database database, string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return AdoNetHelper.ExecuteReader(commandText, connectionString, parameters, commandType);
        }

        public static IEnumerable<dynamic> ExecuteReaderMARS(this Database database, string commandText, string connectionString, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return AdoNetHelper.ExecuteReaderMARS(commandText, connectionString, parameters, commandType);
        }
    }
}
