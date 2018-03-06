using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Nfe.Layout
{
    internal class LayoutRJ : LayoutPadrao
    {
        public LayoutRJ()
        {
            field = new Dictionary<string, FieldConfig>();

            field.Add("TipoDeRegistroHeader", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "10" });
            field.Add("VersaoDoArquivo", new FieldConfig { Length = 3, PaddingChar = '0', IsRequired = true, DefaultValue = "003" });
            field.Add("IdentificacaoDoContribuinte", new FieldConfig { Length = 8, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("CpfCnpjDoContribuinte", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("InscricaoMunicipalDoContribuinte", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeInicioDoPeriodoTranferido", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeFimDoPeriodoTranferido", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });

            field.Add("TipoDeRegistroItems", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "20" });
            field.Add("TipoDeRPS", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = true, DefaultValue = "RPS" });
            field.Add("SerieDoRPS", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("NumeroDoRPS", new FieldConfig { Length = 12, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("DataDeEmissaoRPS", new FieldConfig { Length = 8, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("SituacaoDoRPS", new FieldConfig { Length = 1, PaddingChar = ' ', IsRequired = true, DefaultValue = "1" });
            field.Add("TomadorIdentificadorCPFouCNPJ", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorCPFouCNPJ", new FieldConfig { Length = 14, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorIncricaoMunicipal", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorIncricaoEstadual", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorRazaoSocial", new FieldConfig { Length = 75, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("TomadorTipoEndereco", new FieldConfig { Length = 3, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorEndereco", new FieldConfig { Length = 50, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorNumeroDoEndereco", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorComplemento", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorBairro", new FieldConfig { Length = 30, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorCidade", new FieldConfig { Length = 50, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorUF", new FieldConfig { Length = 2, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorCEP", new FieldConfig { Length = 8, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorTelefone", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TomadorEmail", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("TipoDeTributacaoDeServico", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("CidadeDaPrestacaoDoServico", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("UfDaPrestacaoDoServico", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("RegimeEspecialDeTributacao", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("OpcaoPeloSimples", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("InsentivoCultural", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("CodigoServicoPrestado", new FieldConfig { Length = 5, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorAlicota", new FieldConfig { Length = 4, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDosServicos", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDasDeducoes", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDescontoCondicionado", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorDescontoIncondicionado", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorCOFIN", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorCSLL", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorINSS", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorIRPJ", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorPisPasep", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorDeOutrasRetencoesFederais", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorISS", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("IssRetidoSimOuNao", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("DataDaCompentencia", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("CodigoDaObra", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("AnotacaoDeResponsabilidadeTecnica", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("SerieRpsSubstituido", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("NumeroDoRpsSubstituido", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("EspacoReservado", new FieldConfig { Length = 6, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("DescritivoDosServicos", new FieldConfig { Length = 999, PaddingChar = ' ', IsRequired = true, DefaultValue = "" });
            field.Add("CodigoDoPais", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });
            field.Add("CodigoDoBeneficio", new FieldConfig { Length = 5, PaddingChar = ' ', IsRequired = false, DefaultValue = "" });

            field.Add("TipoDeRegistroRodape", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = true, DefaultValue = "9" });
            field.Add("NumeroDeLinhas", new FieldConfig { Length = 7, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorTotalDosServicos", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = true, DefaultValue = "" });
            field.Add("ValorTotalDasDeducoes", new FieldConfig { Length = 15, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorTotalDosDescontosCondicionados", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });
            field.Add("ValorTotalDosDescontosIncondicionados", new FieldConfig { Length = 1, PaddingChar = '0', IsRequired = false, DefaultValue = "" });

        }
    }

}