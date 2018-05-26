using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Common.Gen
{
    public static class HelperFlow
    {

        public enum Eflows
        {

            AtualizarAplicacao = 1,
            AtualizarRepository = 2,
            GerarCodigo,
            GerarCodigoEspecifico,
            AtualizarAplicacaoSemcopiar,
            AbrirLocalDoProjeto,
            FixFileNameToExtInApp = 98,
            Sair = 99

        }

        public static void Flow(string[] args, HelperSysObjectsBase sysObject)
        {
            var executeFlow = FlowOptionsClassic(args, sysObject);

            if (!executeFlow)
                PrinstScn.WriteLine("Fluxo Não implementado");

            Flow(args, sysObject);
        }

        public static void Flow(string[] args, Func<IEnumerable<ExternalResource>> GetConfigExternarReources, HelperSysObjectsBase sysObject)
        {
            var executeFlow = FlowOptions(args, GetConfigExternarReources, sysObject);

            if (!executeFlow)
                PrinstScn.WriteLine("Fluxo Não implementado");

            Flow(args, GetConfigExternarReources, sysObject);
        }

        private static bool FlowOptions(string[] args, Func<IEnumerable<ExternalResource>> GetConfigExternarReources, HelperSysObjectsBase sysObject)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Escolha as opções;");
            Console.WriteLine("[{0} = >> Clonar e Copiar para aplicação]", (int)Eflows.AtualizarAplicacao);
            Console.WriteLine("[{0} = >> Atualizar repositorio local com arquivos da aplicação]", (int)Eflows.AtualizarRepository);
            Console.WriteLine("[{0} = >> Gerar Código ]", (int)Eflows.GerarCodigo);
            Console.WriteLine("[{0} = >> Gerar Código de um Contexto ou Classe Especifica]", (int)Eflows.GerarCodigoEspecifico);
            Console.WriteLine("[{0} = >> Clonar apenas]", (int)Eflows.AtualizarAplicacaoSemcopiar);
            Console.WriteLine("[{0} = >> Abrir local do projeto]", (int)Eflows.AbrirLocalDoProjeto);
            Console.WriteLine("[{0} = >> Sair", (int)Eflows.Sair);

            var flow = Console.ReadLine();

            var result = FlowOptionsClassic(args, sysObject, flow);

            if (flow == ((int)Eflows.AtualizarAplicacao).ToString())
            {
                PrinstScn.WriteLine("Clonar e Copiar para aplicação");
                HelperExternalResources.CloneAndCopy(GetConfigExternarReources());
                result = true;
            }

            if (flow == ((int)Eflows.AtualizarAplicacaoSemcopiar).ToString())
            {
                PrinstScn.WriteLine("Clonar apenas");
                HelperExternalResources.CloneOnly(GetConfigExternarReources());
                result = true;
            }


            if (flow == ((int)Eflows.AtualizarRepository).ToString())
            {
                PrinstScn.WriteLine("Atualizar repositorio local com arquivos da aplicação");
                HelperExternalResources.UpdateLocalRepository(GetConfigExternarReources());
                result = true;
            }

            if (flow == ((int)Eflows.FixFileNameToExtInApp).ToString())
            {
                PrinstScn.WriteLine("Atualizar nomes dos Arquivos da aplicação para .Ext");
                FixRename.Fix(sysObject);
                result = true;
            }


            return result;
        }

        private static bool FlowOptionsClassic(string[] args, HelperSysObjectsBase sysObject)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Escolha as opções;");
            Console.WriteLine("[{0} = >> Gerar Código ]", (int)Eflows.GerarCodigo);
            Console.WriteLine("[{0} = >> Gerar Código de um Contexto ou Classe Especifica]", (int)Eflows.GerarCodigoEspecifico);
            Console.WriteLine("[{0} = >> Abrir local do projeto", (int)Eflows.GerarCodigoEspecifico);
            Console.WriteLine("[{0} = >> Sair", (int)Eflows.Sair);

            var flow = Console.ReadLine();

            return FlowOptionsClassic(args, sysObject, flow);

        }

        private static bool FlowOptionsClassic(string[] args, HelperSysObjectsBase sysObject, string flow)
        {
            var result = false;

            if (flow == ((int)Eflows.GerarCodigo).ToString())
            {
                PrinstScn.WriteLine("Gerar direto na pasta dos projetos? [S=Sim, N=Não]");
                var usePathProjects = Console.ReadLine();

                MainWithOutConfirmation(args, usePathProjects.ToLower() == "s", sysObject);

                result = true;
            }

            if (flow == ((int)Eflows.GerarCodigoEspecifico).ToString())
            {

                PrinstScn.WriteLine("Gerar direto na pasta dos projetos? [S=Sim, N=Não]");
                var usePathProjects = Console.ReadLine();

                MainWithConfirmation(args, usePathProjects.ToLower() == "s", sysObject);

                result = true;
            }

            if (flow == ((int)Eflows.AbrirLocalDoProjeto).ToString())
            {
                HelperCmd.ExecuteCommand(string.Format("explorer '{0}'", AppDomain.CurrentDomain.BaseDirectory.Replace("\\Gerador.Gen\\", "").Replace("bin\\Debug\\", "").Replace("bin\\Release\\", "")), 10000);
                result = true;
            }

            if (flow == ((int)Eflows.Sair).ToString())
            {
                Environment.Exit(0);
                result = true;
            }

            return result;
        }

        private static void MainWithConfirmation(string[] args, bool UsePathProjects, HelperSysObjectsBase sysObject)
        {
            PrinstScn.WriteLine("Atualizando / Criando Contextos ou classes especificas!");

            foreach (var item in sysObject.Contexts)
            {
                PrinstScn.WriteLine("Deseja Atualizar/Criar o Contexto? {0} [S=Sim, N=Não]", item.Namespace);
                var accept = Console.ReadLine();
                if (accept == "s")
                {
                    PrinstScn.WriteLine("Deseja Escolher uma classe, Digite o nome dela?");
                    var className = Console.ReadLine();
                    if (!string.IsNullOrEmpty(className))
                        sysObject.MakeClass(item, className, UsePathProjects);
                    else
                        sysObject.MakeClass(item);
                }

            }

        }

        private static void MainWithOutConfirmation(string[] args, bool UsePathProjects, HelperSysObjectsBase sysObject)
        {
            PrinstScn.WriteLine("Atualizando / Criando todos os Contextos");

            foreach (var item in sysObject.Contexts)
            {
                PrinstScn.WriteLine("{0} - {1}", item.Namespace, item.Arquiteture);
                sysObject.MakeClass(item, UsePathProjects);
            }
        }

    }
}
