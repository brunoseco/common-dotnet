using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.API
{
    public class ErrorMap
    {
        private  IDictionary<string, string> _errorsMap;

        public ErrorMap()
        {

        }
        public ErrorMap(IDictionary<string, string> _errorsMap)
        {
            this._errorsMap = _errorsMap;
        }

        public string GetTraduction(string error)
        {
            if (this._errorsMap.IsNull())
                return error;

            var _traduction = this._errorsMap.Where(_ => error.ToUpper().Contains(_.Key.ToUpper())).IsAny();
            if (_traduction)
                return this._errorsMap.Where(_ => error.ToUpper().Contains(_.Key.ToUpper())).SingleOrDefault().Value;
            return error;
        }
    }
}
