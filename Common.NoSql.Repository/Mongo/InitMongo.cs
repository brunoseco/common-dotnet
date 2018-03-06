using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Common.NoSql.Mongo
{
    public static class InitMongo
    {

        public static void Run()
        {
            try
            {
                InstanceMongo();
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException(string.Format("Erro ao inicializar o mongo {[0]}", ex.Message));
            }

        }

        
        private static void InstanceMongo()
        {
            var files = new string[] { @"C:\Program Files\MongoDB\Server\3.0\bin\mongod.exe", @"C:\Program Files\MongoDB\Server\3.2\bin\mongod.exe", @"C:\Program Files\MongoDB\Server\3.4\bin\mongod.exe" };
            foreach (var file in files)
                if (StartUp(file))
                    break;
        }

        private static bool StartUp(string fileName)
        {
            if (File.Exists(fileName))
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = fileName;

                bool started = ProcessIsUp();
                if (!started)
                {
                    Process.Start(startInfo);
                    return true;
                }
            }

            return false;
        }

        public static bool ProcessIsUp()
        {
            return Process.GetProcessesByName("mongod").Any();
        }

    }
}
