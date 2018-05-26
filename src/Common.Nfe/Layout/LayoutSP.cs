using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Nfe.Layout
{

    internal class LayoutSP : LayoutPadrao
    {
        public LayoutSP()
        {
            field = new Dictionary<string, FieldConfig>();
           
            //Cabeçalho
            field.Add("TipoDeRegistroHeader", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "1" });
            field.Add("VersaoDoArquivo", new FieldConfig { Length = 3, PaddingChar = '0', IsRequired = true, DefaultValue = "001" });

            field.Add("InscricaoMunicipalDoContribuinte", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeInicioDoPeriodoTranferido", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeFimDoPeriodoTranferido", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });

            //Detalhe
            field.Add("TipoDeRegistroItems", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "2" });
            field.Add("TipoDeRPS", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = true, DefaultValue = "RPS" });
            field.Add("SerieDoRPS", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("NumeroDoRPS", new FieldConfig { Length = 12, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeEmissaoRPS", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("SituacaoDoRPS", new FieldConfig { Length = 1, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDosServicos", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDasDeducoes", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("CodigoServicoPrestado", new FieldConfig { Length = 5, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorAlicota", new FieldConfig { Length = 4, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("IssRetidoSimOuNao", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorIdentificadorCPFouCNPJ", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorCPFouCNPJ", new FieldConfig { Length = 14, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorIncricaoMunicipal", new FieldConfig { Length = 8, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorIncricaoEstadual", new FieldConfig { Length = 12, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorRazaoSocial", new FieldConfig { Length = 75, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorTipoEndereco", new FieldConfig { Length = 3, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorEndereco", new FieldConfig { Length = 50, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorNumeroDoEndereco", new FieldConfig { Length = 10, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorComplemento", new FieldConfig { Length = 30, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorBairro", new FieldConfig { Length = 30, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorCidade", new FieldConfig { Length = 50, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorUF", new FieldConfig { Length = 2, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorCEP", new FieldConfig { Length = 8, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorEmail", new FieldConfig { Length = 75, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("DescritivoDosServicos", new FieldConfig { Length = 1000, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            
            //Rodapé
            field.Add("TipoDeRegistroRodape", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "9" });
            field.Add("NumeroDeLinhas", new FieldConfig { Length = 7, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorTotalDosServicos", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorTotalDasDeducoes", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });

        }
    }




}