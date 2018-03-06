using Common.Infrastructure.Log;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Factorys
{
    public class HelperLog
    {
        public ILog log { get; set; } 

        public HelperLog()
        {
            ConfigContainer.Container();
            this.log = ConfigContainer.Container().GetInstance<ILog>();
        }


        private static class ConfigContainer
        {

            private static Container container = new Container();

            static ConfigContainer()
            {
                container.Register<ILog, LogArquivoComponente>();
                container.Verify();

            }

            public static Container Container()
            {
                return container;
            }

        }
    }
   
}
