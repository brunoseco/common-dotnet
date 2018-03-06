using System;
using System.Collections.Generic;
using System.Text;

namespace BoletoNet
{



    public class CodigoRejeicao_Caixa : AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores

        public CodigoRejeicao_Caixa()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public CodigoRejeicao_Caixa(string codigo)
        {
            try
            {
                this.carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        private void carregar(string codigo)
        {
            try
            {
                int _codigo;

                this.Banco = new Banco_Caixa();

                Int32.TryParse(codigo, out _codigo);

                switch (_codigo)
                {
                    case 01:
                        this.Codigo = 01;
                        this.Descricao = "Código do Banco Inválido";
                        break;
                    case 02:
                        this.Codigo = 02;
                        this.Descricao = "Código do Registro Inválido";
                        break;
                    case 03:
                        this.Codigo = 03;
                        this.Descricao = "Código do Segmento Inválido";
                        break;
                    case 04:
                        this.Codigo = 04;
                        this.Descricao = "Código do Movimento não Permitido p/ Carteira";
                        break;
                    case 05:
                        this.Codigo = 05;
                        this.Descricao = "Código do Movimento Inválido";
                        break;
                    case 06:
                        this.Codigo = 06;
                        this.Descricao = "Tipo Número Inscrição Beneficiário Inválido";
                        break;
                    case 07:
                        this.Codigo = 07;
                        this.Descricao = "Agencia/Conta/DV Inválidos";
                        break;
                    case 08:
                        this.Codigo = 08;
                        this.Descricao = "Nosso Número Inválido";
                        break;
                    case 09:
                        this.Codigo = 09;
                        this.Descricao = "Nosso Número Duplicado";
                        break;
                    case 10:
                        this.Codigo = 10;
                        this.Descricao = "Carteira Inválida";
                        break;
                    case 11:
                        this.Codigo = 11;
                        this.Descricao = "Data de Geração Inválida";
                        break;
                    case 12:
                        this.Codigo = 12;
                        this.Descricao = "Tipo de Documento Inválido";
                        break;
                    case 13:
                        this.Codigo = 13;
                        this.Descricao = "Identif.Da Emissão do Boleto Inválida";
                        break;
                    case 14:
                        this.Codigo = 14;
                        this.Descricao = "Identif.Da Distribuição do Boleto Inválida";
                        break;
                    case 15:
                        this.Codigo = 15;
                        this.Descricao = "Características Cobrança Incompatíveis";
                        break;
                    case 16:
                        this.Codigo = 16;
                        this.Descricao = "Data de Vencimento Inválida";
                        break;
                    case 17:
                        this.Codigo = 17;
                        this.Descricao = "Data de Vencimento Anterior à Data de Emissão";
                        break;
                    case 18:
                        this.Codigo = 18;
                        this.Descricao = "Vencimento fora do prazo de operação";
                        break;
                    case 19:
                        this.Codigo = 19;
                        this.Descricao = "Título a Cargo de Bco Correspondentes c/ Vencto Inferior a XX Dias";
                        break;
                    case 20:
                        this.Codigo = 20;
                        this.Descricao = "Valor do Título Inválido";
                        break;
                    case 21:
                        this.Codigo = 21;
                        this.Descricao = "Espécie do Título Inválida";
                        break;
                    case 22:
                        this.Codigo = 22;
                        this.Descricao = "Espécie do Título Não Permitida para a Carteira";
                        break;
                    case 23:
                        this.Codigo = 23;
                        this.Descricao = "Aceite Inválido";
                        break;
                    case 24:
                        this.Codigo = 24;
                        this.Descricao = "Data da Emissão Inválida";
                        break;
                    case 25:
                        this.Codigo = 25;
                        this.Descricao = "Data da Emissão Posterior a Data de Entrada";
                        break;
                    case 26:
                        this.Codigo = 26;
                        this.Descricao = "Código de Juros de Mora Inválido";
                        break;
                    case 27:
                        this.Codigo = 27;
                        this.Descricao = "Valor/Taxa de Juros de Mora Inválido";
                        break;
                    case 28:
                        this.Codigo = 28;
                        this.Descricao = "Código do Desconto Inválido";
                        break;
                    case 29:
                        this.Codigo = 29;
                        this.Descricao = "Valor do Desconto Maior ou Igual ao Valor do Título";
                        break;
                    case 30:
                        this.Codigo = 30;
                        this.Descricao = "Desconto a Conceder Não Confere";
                        break;
                    case 31:
                        this.Codigo = 31;
                        this.Descricao = "Concessão de Desconto - Já Existe Desconto Anterior";
                        break;
                    case 32:
                        this.Codigo = 32;
                        this.Descricao = "Valor do IOF Inválido";
                        break;
                    case 33:
                        this.Codigo = 33;
                        this.Descricao = "Valor do Abatimento Inválido";
                        break;
                    case 34:
                        this.Codigo = 34;
                        this.Descricao = "Valor do Abatimento Maior ou Igual ao Valor do Título";
                        break;
                    case 35:
                        this.Codigo = 35;
                        this.Descricao = "Valor Abatimento a Conceder Não Confere";
                        break;
                    case 36:
                        this.Codigo = 36;
                        this.Descricao = "Concessão de Abatimento - Já Existe Abatimento Anterior";
                        break;
                    case 37:
                        this.Codigo = 37;
                        this.Descricao = "Código para Protesto Inválido";
                        break;
                    case 38:
                        this.Codigo = 38;
                        this.Descricao = "Prazo para Protesto Inválido";
                        break;
                    case 39:
                        this.Codigo = 39;
                        this.Descricao = "Pedido de Protesto Não Permitido para o Título";
                        break;
                    case 40:
                        this.Codigo = 40;
                        this.Descricao = "Título com Ordem de Protesto Emitida";
                        break;
                    case 41:
                        this.Codigo = 41;
                        this.Descricao = "Pedido Cancelamento/Sustação p/ Títulos sem Instrução Protesto";
                        break;
                    case 42:
                        this.Codigo = 42;
                        this.Descricao = "Código para Baixa/Devolução Inválido";
                        break;
                    case 43:
                        this.Codigo = 43;
                        this.Descricao = "Prazo para Baixa/Devolução Inválido";
                        break;
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    default:
                        this.Codigo = 0;
                        this.Descricao = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }


        #endregion
    }
}
