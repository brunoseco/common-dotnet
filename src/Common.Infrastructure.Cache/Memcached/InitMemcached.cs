using Common.NoSql;
using Common.NoSql.Mongo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.Cache
{
    public static class InitMemcached
    {

        public static void Run()
        {
            try
            {
                InstanceMemCached("11211", "512");
                InitMongo.Run();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Erro ao inicializar o cache {[0]}", ex.Message));
            }

        }

        private static void InstanceMemCached(string port, string memoryMB)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            var fileName = @"C:\memcached\memcached.exe";
            var arguments = string.Format("-p {0} -m {1}", port, memoryMB);
            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;

            if (File.Exists(fileName))
            {
                var started = Process.GetProcessesByName("memcached").Any();
                if (!started)
                {
                    ClearSeedDataAuth();
                    Process.Start(startInfo);

                }
            }
        }

        private static void ClearSeedDataAuth()
        {
            var seed = ConfigurationManager.AppSettings["pathSeed"];
            if (File.Exists(seed))
                File.Delete(seed);
        }

        public static bool MemcachedIsUp()
        {
            try
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = @"C:\memcached\memcached.exe";

                if (File.Exists(startInfo.FileName))
                    return Process.GetProcessesByName("memcached").Any();


                return false;
            }
            catch
            {
                return false;
            }


        }

    }
}
