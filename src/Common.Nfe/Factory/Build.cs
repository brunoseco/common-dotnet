using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.Linq;
using Common.Nfe.Model.Rps;
using Common.Nfe.Model;
using Common.Nfe.Layout;
using System.Linq.Expressions;
using Common.Nfe.Enum;

namespace Common.Nfe.Factory
{
    internal sealed class Build
    {
        private string filePath = ConfigurationManager.AppSettings["CaminhoRPS"];
        private string fileName = String.Empty;
        private StringBuilder dataValues;
        private LayoutPadrao layout;

        public Build()
        {
            layout = new LayoutSP();
            dataValues = new StringBuilder();
            fileName = string.Concat(GenerateNameForFile(), ".txt");
        }

        public Build(int? TemplateNFeId)
        {
            if (!TemplateNFeId.HasValue)
                layout = new LayoutSP();
            else
            {
                if (TemplateNFeId.Value == (int)ETemplateNFe.LayoutSP)
                    layout = new LayoutSP();
                else
                    layout = new LayoutRJ();
            }


            dataValues = new StringBuilder();
            fileName = string.Concat(GenerateNameForFile(), ".txt");
        }

        public byte[] BuildToStream(RPSDocument document)
        {
            try
            {

                var fileContent = new StringBuilder();

                BuildHeader(document);
                fileContent.AppendLine(dataValues.ToString().Trim());

                BuildDetails(document);
                fileContent.AppendLine(dataValues.ToString().Trim());

                BuildFooter(document);
                fileContent.AppendLine(dataValues.ToString().Trim());

                var info = Encoding.GetEncoding("iso-8859-1").GetBytes(fileContent.ToString());

                return info;

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel gerar o arquivo no caminho " + filePath + " especificado.\nException: {0}", ex);
            }
        }

        public void BuildToFile(RPSDocument document)
        {
            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var fileDirectory = System.IO.Path.Combine(filePath, fileName);

                using (StreamWriter streamWriter = new StreamWriter(fileDirectory, true, Encoding.UTF8))
                {
                    streamWriter.AutoFlush = true;

                    BuildHeader(document);
                    streamWriter.WriteLine(dataValues.ToString().Trim());

                    BuildDetails(document);
                    streamWriter.WriteLine(dataValues.ToString().Trim());

                    BuildFooter(document);
                    streamWriter.WriteLine(dataValues.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel gerar o arquivo no caminho " + filePath + " especificado.\nException: {0}", ex);
            }
        }

        private void BuildHeader(RPSDocument document)
        {
            FieldSettings(layout, GetPropertyName(() => document.TipoDeRegistroHeader), document.TipoDeRegistroHeader);
            FieldSettings(layout, GetPropertyName(() => document.VersaoDoArquivo), document.VersaoDoArquivo);

            if (document.TemplateNFe.Equals((int)ETemplateNFe.LayoutRJ))
            {
                FieldSettings(layout, GetPropertyName(() => document.IdentificacaoDoContribuinte), document.IdentificacaoDoContribuinte);
                FieldSettings(layout, GetPropertyName(() => document.CpfCnpjDoContribuinte), document.CpfCnpjDoContribuinte);
            }

            FieldSettings(layout, GetPropertyName(() => document.InscricaoMunicipalDoContribuinte), document.InscricaoMunicipalDoContribuinte);
            FieldSettings(layout, GetPropertyName(() => document.DataDeInicioDoPeriodoTranferido), document.DataDeInicioDoPeriodoTranferido);
            FieldSettings(layout, GetPropertyName(() => document.DataDeFimDoPeriodoTranferido), document.DataDeFimDoPeriodoTranferido);
        }

        private void BuildDetails(RPSDocument document)
        {
            dataValues.Clear();

            foreach (var item in document.CollectionRpsItem)
            {
                if (!document.TemplateNFe.HasValue)
                    this.PreencheDetalheLayoutSP(item);

                else if (document.TemplateNFe.Equals((int)ETemplateNFe.LayoutSP))
                    this.PreencheDetalheLayoutSP(item);

                else
                    this.PreencheDetalheLayoutRJ(item);

                EndLine();
            }
        }

        private void PreencheDetalheLayoutRJ(RpsItem item)
        {
            FieldSettings(layout, GetPropertyName(() => item.TipoDeRegistroItems), item.TipoDeRegistroItems);
            FieldSettings(layout, GetPropertyName(() => item.TipoDeRPS), item.TipoDeRPS);
            FieldSettings(layout, GetPropertyName(() => item.SerieDoRPS), item.SerieDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.NumeroDoRPS), item.NumeroDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.DataDeEmissaoRPS), item.DataDeEmissaoRPS);
            FieldSettings(layout, GetPropertyName(() => item.SituacaoDoRPS), item.SituacaoDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIdentificadorCPFouCNPJ), item.TomadorIdentificadorCPFouCNPJ);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCPFouCNPJ), item.TomadorCPFouCNPJ);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIncricaoMunicipal), item.TomadorIncricaoMunicipal);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIncricaoEstadual), item.TomadorIncricaoEstadual);
            FieldSettings(layout, GetPropertyName(() => item.TomadorRazaoSocial), item.TomadorRazaoSocial);
            FieldSettings(layout, GetPropertyName(() => item.TomadorTipoEndereco), item.TomadorTipoEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorEndereco), item.TomadorEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorNumeroDoEndereco), item.TomadorNumeroDoEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorComplemento), item.TomadorComplemento);
            FieldSettings(layout, GetPropertyName(() => item.TomadorBairro), item.TomadorBairro);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCidade), item.TomadorCidade);
            FieldSettings(layout, GetPropertyName(() => item.TomadorUF), item.TomadorUF);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCEP), item.TomadorCEP);
            FieldSettings(layout, GetPropertyName(() => item.TomadorTelefone), item.TomadorTelefone);
            FieldSettings(layout, GetPropertyName(() => item.TomadorEmail), item.TomadorEmail);
            FieldSettings(layout, GetPropertyName(() => item.TipoDeTributacaoDeServico), item.TipoDeTributacaoDeServico);
            FieldSettings(layout, GetPropertyName(() => item.CidadeDaPrestacaoDoServico), item.CidadeDaPrestacaoDoServico);
            FieldSettings(layout, GetPropertyName(() => item.UfDaPrestacaoDoServico), item.UfDaPrestacaoDoServico);
            FieldSettings(layout, GetPropertyName(() => item.RegimeEspecialDeTributacao), item.RegimeEspecialDeTributacao);
            FieldSettings(layout, GetPropertyName(() => item.OpcaoPeloSimples), item.OpcaoPeloSimples);
            FieldSettings(layout, GetPropertyName(() => item.InsentivoCultural), item.InsentivoCultural);
            FieldSettings(layout, GetPropertyName(() => item.CodigoServicoPrestado), item.CodigoServicoPrestado);
            FieldSettings(layout, GetPropertyName(() => item.EspacoReservado), item.EspacoReservado);
            FieldSettings(layout, GetPropertyName(() => item.CodigoDoPais), item.CodigoDoPais);
            FieldSettings(layout, GetPropertyName(() => item.CodigoDoBeneficio), item.CodigoDoBeneficio);
            FieldSettings(layout, GetPropertyName(() => item.CodigoServicoPrestado), item.CodigoServicoPrestado);
            FieldSettings(layout, GetPropertyName(() => item.ValorAlicota), item.ValorAlicota.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.ValorDosServicos), item.ValorDosServicos.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.ValorDasDeducoes), item.ValorDasDeducoes.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.ValorDescontoCondicionado), item.ValorDescontoCondicionado);
            FieldSettings(layout, GetPropertyName(() => item.ValorDescontoIncondicionado), item.ValorDescontoIncondicionado);
            FieldSettings(layout, GetPropertyName(() => item.ValorCOFIN), item.ValorCOFIN);
            FieldSettings(layout, GetPropertyName(() => item.ValorCSLL), item.ValorCSLL);
            FieldSettings(layout, GetPropertyName(() => item.ValorINSS), item.ValorINSS);
            FieldSettings(layout, GetPropertyName(() => item.ValorIRPJ), item.ValorIRPJ);
            FieldSettings(layout, GetPropertyName(() => item.ValorPisPasep), item.ValorPisPasep);
            FieldSettings(layout, GetPropertyName(() => item.ValorDeOutrasRetencoesFederais), item.ValorDeOutrasRetencoesFederais);
            FieldSettings(layout, GetPropertyName(() => item.ValorISS), item.ValorISS);
            FieldSettings(layout, GetPropertyName(() => item.IssRetidoSimOuNao), item.IssRetidoSimOuNao);
            FieldSettings(layout, GetPropertyName(() => item.DataDaCompentencia), item.DataDaCompentencia);
            FieldSettings(layout, GetPropertyName(() => item.CodigoDaObra), item.CodigoDaObra);
            FieldSettings(layout, GetPropertyName(() => item.AnotacaoDeResponsabilidadeTecnica), item.AnotacaoDeResponsabilidadeTecnica);
            FieldSettings(layout, GetPropertyName(() => item.SerieRpsSubstituido), item.SerieRpsSubstituido);
            FieldSettings(layout, GetPropertyName(() => item.NumeroDoRpsSubstituido), item.NumeroDoRpsSubstituido);
            FieldSettings(layout, GetPropertyName(() => item.EspacoReservado), item.EspacoReservado);
            FieldSettings(layout, GetPropertyName(() => item.DescritivoDosServicos), item.DescritivoDosServicos);
        }

        private void PreencheDetalheLayoutSP(RpsItem item)
        {
            FieldSettings(layout, GetPropertyName(() => item.TipoDeRegistroItems), item.TipoDeRegistroItems);
            FieldSettings(layout, GetPropertyName(() => item.TipoDeRPS), item.TipoDeRPS);
            FieldSettings(layout, GetPropertyName(() => item.SerieDoRPS), item.SerieDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.NumeroDoRPS), item.NumeroDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.DataDeEmissaoRPS), item.DataDeEmissaoRPS);
            FieldSettings(layout, GetPropertyName(() => item.SituacaoDoRPS), item.SituacaoDoRPS);
            FieldSettings(layout, GetPropertyName(() => item.ValorDosServicos), item.ValorDosServicos.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.ValorDasDeducoes), item.ValorDasDeducoes.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.CodigoServicoPrestado), item.CodigoServicoPrestado);
            FieldSettings(layout, GetPropertyName(() => item.ValorAlicota), item.ValorAlicota.Replace(".", "").Replace(",", ""));
            FieldSettings(layout, GetPropertyName(() => item.IssRetidoSimOuNao), item.IssRetidoSimOuNao);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIdentificadorCPFouCNPJ), item.TomadorIdentificadorCPFouCNPJ);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCPFouCNPJ), item.TomadorCPFouCNPJ);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIncricaoMunicipal), item.TomadorIncricaoMunicipal);
            FieldSettings(layout, GetPropertyName(() => item.TomadorIncricaoEstadual), item.TomadorIncricaoEstadual);
            FieldSettings(layout, GetPropertyName(() => item.TomadorRazaoSocial), item.TomadorRazaoSocial);
            FieldSettings(layout, GetPropertyName(() => item.TomadorTipoEndereco), item.TomadorTipoEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorEndereco), item.TomadorEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorNumeroDoEndereco), item.TomadorNumeroDoEndereco);
            FieldSettings(layout, GetPropertyName(() => item.TomadorComplemento), item.TomadorComplemento);
            FieldSettings(layout, GetPropertyName(() => item.TomadorBairro), item.TomadorBairro);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCidade), item.TomadorCidade);
            FieldSettings(layout, GetPropertyName(() => item.TomadorUF), item.TomadorUF);
            FieldSettings(layout, GetPropertyName(() => item.TomadorCEP), item.TomadorCEP);
            FieldSettings(layout, GetPropertyName(() => item.TomadorEmail), item.TomadorEmail);
            FieldSettings(layout, GetPropertyName(() => item.DescritivoDosServicos), item.DescritivoDosServicos);
        }

        private void BuildFooter(RPSDocument document)
        {
            dataValues.Clear();

            FieldSettings(layout, GetPropertyName(() => document.TipoDeRegistroRodape), document.TipoDeRegistroRodape);
            FieldSettings(layout, GetPropertyName(() => document.NumeroDeLinhas), document.NumeroDeLinhas);
            FieldSettings(layout, GetPropertyName(() => document.ValorTotalDosServicos), document.ValorTotalDosServicos);
            FieldSettings(layout, GetPropertyName(() => document.ValorTotalDasDeducoes), document.ValorTotalDasDeducoes);

            if (document.TemplateNFe.Equals((int)ETemplateNFe.LayoutRJ))
            {
                FieldSettings(layout, GetPropertyName(() => document.ValorTotalDosDescontosCondicionados), document.ValorTotalDosDescontosCondicionados);
                FieldSettings(layout, GetPropertyName(() => document.ValorTotalDosDescontosIncondicionados), document.ValorTotalDosDescontosIncondicionados);
            }
        }

        private void FieldSettings(LayoutPadrao layout, string fieldName, string fieldValue)
        {
            var layoutConfig = layout.field[fieldName];

            if (fieldValue == null)
            {
                fieldValue = (layoutConfig.DefaultValue != string.Empty) ? fieldValue = layoutConfig.DefaultValue : fieldValue;

                if (fieldValue != null)
                    dataValues.Append(fieldValue.Replace(",", "").Replace("-", "").PadLeft(layoutConfig.Length, layoutConfig.PaddingChar));
                else
                    dataValues.Append(string.Empty.PadLeft(layoutConfig.Length, layoutConfig.PaddingChar));
            }
            else
            {
                if (fieldValue.Length > layoutConfig.Length)
                {
                    var startIndex = fieldValue.Length - layoutConfig.Length;
                    dataValues.Append(fieldValue.Substring(startIndex, layoutConfig.Length).Replace(",", "").Replace("-", "").PadLeft(layoutConfig.Length, layoutConfig.PaddingChar));
                }
                else
                    dataValues.Append(fieldValue.Replace(",", "").Replace("-", "").PadLeft(layoutConfig.Length, layoutConfig.PaddingChar));
            }
        }

        private void EndLine()
        {
            dataValues.Append(System.Environment.NewLine);
        }

        private string GetPropertyName<T>(Expression<Func<T>> property)
        {
            //MemberExpression memberExpression = (MemberExpression)property.Body;   
            //return (memberExpression.Member.Name);

            if (property.Body is MemberExpression)
            {
                return ((MemberExpression)property.Body).Member.Name;
            }
            else
            {
                var operand = ((UnaryExpression)property.Body).Operand;
                return ((MemberExpression)operand).Member.Name;
            }
        }

        public string GenerateNameForFile()
        {
            var prefix = "RPS";
            var patternName = "ddMMyyyyhhmmss";

            DateTime dateTime = DateTime.Now;

            return (String.Concat(prefix, dateTime.ToString(patternName), ".txt"));
        }

    }
}
