using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Log
{
    public class FactoryLog
    {
        public static ILog GetInstace()
        {
            return new LogFileComponent();
        }

    }
}
