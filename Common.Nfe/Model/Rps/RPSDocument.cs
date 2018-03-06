using Common.Nfe.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Nfe.Model.Rps
{
    public class RPSDocument
    {

        private Build build;
        public RPSDocument(int? TemplateNFeId)
        {
            this.TemplateNFe = TemplateNFeId;
            this.build = new Build(TemplateNFeId);
        }
        public int? TemplateNFe { get; set; }
        public string TipoDeRegistroHeader { get; set; }
        public string VersaoDoArquivo { get; set; }
        public string IdentificacaoDoContribuinte { get; set; }
        public string CpfCnpjDoContribuinte { get; set; }
        public string InscricaoMunicipalDoContribuinte { get; set; }
        public string DataDeInicioDoPeriodoTranferido { get; set; }
        public string DataDeFimDoPeriodoTranferido { get; set; }
        public IEnumerable<RpsItem> CollectionRpsItem { get; set; }
        public string TipoDeRegistroRodape { get; set; }
        public string NumeroDeLinhas { get; set; }
        public string ValorTotalDosServicos { get; set; }
        public string ValorTotalDasDeducoes { get; set; }
        public string ValorTotalDosDescontosCondicionados { get; set; }
        public string ValorTotalDosDescontosIncondicionados { get; set; }

        public string FileName
        {
            get
            {
                return build.GenerateNameForFile();
            }
        }

        public byte[] Gerar()
        {

            return this.build.BuildToStream(this);
        }
    }
}
