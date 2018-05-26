using Common.Infrastructure.Log;
using Common.Infrastructure.ORM.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    public class ServerService
    {

        public static void StartServiceDevelopmentToHomolog(DeploySettings deploy)
        {
            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            var deployName = deploy.DeployName;

            var resultDeploysToDo = AdoNetHelper.ExecuteReader("Select * from DeployQueue where Status=@Status", connectionStringDeploy, new
            {
                DeployName = deployName,
                Status = (int)EStatusDeploy.DevelopmentToHomolog
            }, commandType: CommandType.Text);

            foreach (var deployToDo in resultDeploysToDo)
            {              
                if (deployToDo.HomologToPreProductionAprove)
                {
                    FactoryLog.GetInstace().Debug(string.Format("Deploy {0} em Pré-produção iniciado", deployToDo.DeployName));
                    
                    deploy.SetPackagingName(deployToDo.DeployName);
                    if (deployToDo.Flow == (int)EFlow.Updates)
                        deploy.Flow = EFlow.Updates;
                    if (deployToDo.Flow == (int)EFlow.FixInProduction)
                        deploy.Flow = EFlow.FixInProduction;
                    DeployProcess.HomologToPreProduction(deploy);

                    FactoryLog.GetInstace().Debug(string.Format("Deploy {0} em Pré-produção finalizado", deployToDo.DeployName));
                }
            }
        }

        public static void StartServicePreProductionToProduction(DeploySettings deploy)
        {
            var connectionStringDeploy = deploy.ConnectionStringDeploy;
            var deployName = deploy.DeployName;

            var resultDeploysToDo = AdoNetHelper.ExecuteReader("Select * from DeployQueue where Status=@Status", connectionStringDeploy, new
            {
                DeployName = deployName,
                Status = (int)EStatusDeploy.HomologToPreProduction
            }, commandType: CommandType.Text);

            foreach (var deployToDo in resultDeploysToDo)
            {                
                if (deployToDo.PreProductionToProductionAprove)
                {
                    FactoryLog.GetInstace().Debug(string.Format("Deploy {0} em Producao iniciado", deployToDo.DeployName));

                    deploy.SetPackagingName(deployToDo.DeployName);

                    if (deployToDo.Flow == (int)EFlow.Updates)
                        deploy.Flow = EFlow.Updates;

                    if (deployToDo.Flow == (int)EFlow.FixInProduction)
                        deploy.Flow = EFlow.FixInProduction;

                    DeployProcess.PreProductionToProduction(deploy);
                    DeployProcess.PreProductionToImplantation(deploy);

                    FactoryLog.GetInstace().Debug(string.Format("Deploy {0} em Producao finalizado", deployToDo.DeployName));
                }

                
            }

        }

    }
}
