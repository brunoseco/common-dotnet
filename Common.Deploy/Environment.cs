using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public static class Environment
    {


        public static string Config(string environment)
        {

            if (environment == Environment.Homolog)
                return "Homolog";

            if (environment == Environment.Validation)
                return "Validation";

            if (environment == Environment.Implantation)
                return "Implantation";

            if (environment == Environment.PreProduction)
                return "Pre-Production";

            if (environment == Environment.Azure)
                return "Azure";


            return "Release";
        
        }

        public static string Development
        {
            get
            {
                return "DEVELOPMENT";
            }
        }

        public static string Homolog
        {
            get
            {
                return "HOMOLOG";
            }
        }

        public static string Validation
        {
            get
            {
                return "VALIDATION";
            }
        }

        public static string Implantation
        {
            get
            {
                return "IMPLANTATION";
            }
        }

        public static string PreProduction
        {
            get
            {
                return "PRE-PRODUCTION";
            }
        }

        public static string Azure
        {
            get
            {
                return "AZURE";
            }
        }

        public static string Production
        {
            get
            {
                return "PRODUCTION";
            }
        }

        

    }
}
