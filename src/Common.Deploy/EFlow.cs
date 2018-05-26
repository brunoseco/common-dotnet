using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Deploy
{
    /// <summary>
    /// Updates - Fluxo completo passando pelos 3 ambientes (homolog, testes e produção). Normalmente usado quando não tem branch.
    /// FixInProduction - Pula o ambiente de homologação e vai direto para testes. Usado normalmente quando se tem um branch de dev e outro de prod.
    /// </summary>
    public enum EFlow
    {
        Updates = 1,
        FixInProduction = 2,
        Validation = 3
    }
}
