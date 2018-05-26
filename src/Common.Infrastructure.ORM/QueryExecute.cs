using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using Common.Infrastructure.ORM.Context;
using Common.Infrastructure.ORM.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Context
{
    [Obsolete("Utilize os metodos ExecuteCommand/ExecuteDynamicQuery do repositorio")]
    public class QueryExecute<T> : IDisposable
    {

        private DbContext ctx;
        private string connectionString;
        private ILog log;
        private List<DbParameter> parameters;
        public delegate void ConfigValues(DbDataReader reader,List<T> result);

        public QueryExecute(IUnitOfWork unitOfWork, ILog log)
        {
            this.connectionString = unitOfWork.ConnectionStringComplete();
            this.ctx = unitOfWork as DbContext;
            this.log = log;
            this.parameters = new List<DbParameter>();

        }

        public void AddParametersWithValue(string parameterName, object value)
        {
            parameters.Add(new SqlParameter(parameterName, value));
        }

        public IEnumerable<T> Execute(string commandText, ConfigValues mapper)
        {
            return ExecuteDefault(commandText, mapper, CommandType.StoredProcedure);
        }

        public List<T> ExecuteCommandText(string commandText, ConfigValues mapper)
        {
            return ExecuteDefault(commandText, mapper, CommandType.Text);
        }

        public int ExecuteNonQuery(string commandText)
        {
            FactoryLog.GetInstace().Info(string.Format("{0}", commandText));

            var table = new List<Dictionary<string, object>>();
            var conn = new SqlConnection(connectionString);
            var result = 0;
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;

                foreach (var item in parameters)
                    cmd.Parameters.Add(item);

                conn.Open();
                result = cmd.ExecuteNonQuery();

                conn.Close();
            }


            return result;

        }


        private List<T> ExecuteDefault(string commandText, ConfigValues mapper, CommandType commandType)
        {
            FactoryLog.GetInstace().Info(string.Format("{0}", commandText));

            var table = new List<Dictionary<string, object>>();
            var conn = new SqlConnection(connectionString);
            var result = new List<T>();
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;

                foreach (var item in parameters)
                    cmd.Parameters.Add(item);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mapper(reader, result);
                    }
                }

                conn.Close();
            }


            return result;
        }

        public void Dispose()
        {
            this.ctx.Dispose();
        }
    }
}
