using Common.Deploy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public class DeploySettingsFactory
    {

        public virtual IEnumerable<PathsDeploy> CreateSettings(bool config)
        {

            return new List<PathsDeploy>();
           
        }

    }
}
