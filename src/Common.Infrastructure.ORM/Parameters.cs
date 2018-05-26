using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    class Parameters
    {

        public static Tuple<string, object[]> PrepareArguments(string storedProcedure, object parameters)
        {
            var parameterNames = new List<string>();
            var parameterParameters = new List<object>();

            if (parameters != null)
            {
                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    string name = "@" + propertyInfo.Name;
                    object value = propertyInfo.GetValue(parameters, null);

                    var IsDefaultValue_ = IsDefaultValue(propertyInfo, value);
                    if (!IsDefaultValue_)
                    {
                        parameterNames.Add(name);
                        parameterParameters.Add(new SqlParameter(name, value ?? DBNull.Value));
                    }
                }
            }

            if (parameterNames.Count > 0)
                storedProcedure += " " + string.Join(", ", parameterNames);

            return new Tuple<string, object[]>(storedProcedure, parameterParameters.ToArray());
        }

        private static bool IsDefaultValue(PropertyInfo propertyInfo, object value)
        {
            var IsDefaultValue_ = false;
            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                var castValue = Convert.ToDateTime(value);
                if (castValue == default(DateTime))
                    IsDefaultValue_ = true;
            }
            return IsDefaultValue_;
        }

    }
}
