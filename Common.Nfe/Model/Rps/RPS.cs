using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Nfe.Model.Rps
{
    public class RPS
    {
        public string TipoDeRegistroHeader {get; set;}
        public string VersaoDoArquivo {get; set;}
        public string IdentificacaoDoContribuinte {get; set;}
        public string CpfCnpjDoContribuinte {get; set;}
        public string InscricaoMunicipalDoContribuinte {get; set;}
        public string DataDeInicioDoPeriodoTranferido {get; set;}
        public string DataDeFimDoPeriodoTranferido {get; set;}
        public string TipoDeRegistroItems {get; set;}
        public string TipoDeRPS {get; set;}
        public string SerieDoRPS {get; set;}
        public string NumeroDoRPS {get; set;}
        public string DataDeEmissaoRPS {get; set;}
        public string SituacaoDoRPS {get; set;}
        public string TomadorIdentificadorCPFouCNPJ {get; set;}
        public string TomadorCPFouCNPJ {get; set;}
        public string TomadorIncricaoMunicipal {get; set;}
        public string TomadorIncricaoEstadual {get; set;}
        public string TomadorRazaoSocial {get; set;}
        public string TomadorTipoEndereco {get; set;}
        public string TomadorEndereco {get; set;}
        public string TomadorNumeroDoEndereco {get; set;}
        public string TomadorComplemento {get; set;}
        public string TomadorBairro {get; set;}
        public string TomadorCidade {get; set;}
        public string TomadorUF {get; set;}
        public string TomadorCEP {get; set;}
        public string TomadorTelefone {get; set;}
        public string TomadorEmail {get; set;}
        public string TipoDeTributacaoDeServico {get; set;}
        public string CidadeDaPrestacaoDoServico {get; set;}
        public string UfDaPrestacaoDoServico {get; set;}
        public string RegimeEspecialDeTributacao {get; set;}
        public string OpcaoPeloSimples {get; set;}
        public string InsentivoCultural {get; set;}
        public string CodigoServicoPrestado {get; set;}
        public string ValorAlicota {get; set;}
        public string valorDosServicos {get; set;}
        public string ValorDasDeducoes {get; set;}
        public string ValorDescontoCondicionado {get; set;}
        public string ValorDescontoIncondicionado {get; set;}
        public string ValorCOFIN {get; set;}
        public string ValorCSLL {get; set;}
        public string ValorINSS {get; set;}
        public string ValorIRPJ {get; set;}
        public string ValorPisPasep {get; set;}
        public string ValorDeOutrasRetencoesFederais {get; set;}
        public string ValorISS {get; set;}
        public string IssRetidoSimOuNao {get; set;}
        public string DataDaCompentencia {get; set;}
        public string CodigoDaObra {get; set;}
        public string AnotacaoDeResponsabilidadeTecnica {get; set;}
        public string SerieRpsSubstituido {get; set;}
        public string NumeroDoRpsSubstituido {get; set;}
        public string EspacoReservado {get; set;}
        public string DescritivoDosServicos {get; set;}
        public string TipoDeRegistroRodape { get; set; }
        public string NumeroDeLinhas {get; set;}
        public string ValorTotalDosServicos {get; set;}
        public string ValorTotalDasDeducoes {get; set;}
        public string ValorTotalDosDescontosCondicionados {get; set;}
        public string ValorTotalDosDescontosIncondicionados { get; set; }

    }
}
