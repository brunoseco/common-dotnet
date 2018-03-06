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
                        this.Descricao = "C�digo do Banco Inv�lido";
                        break;
                    case 02:
                        this.Codigo = 02;
                        this.Descricao = "C�digo do Registro Inv�lido";
                        break;
                    case 03:
                        this.Codigo = 03;
                        this.Descricao = "C�digo do Segmento Inv�lido";
                        break;
                    case 04:
                        this.Codigo = 04;
                        this.Descricao = "C�digo do Movimento n�o Permitido p/ Carteira";
                        break;
                    case 05:
                        this.Codigo = 05;
                        this.Descricao = "C�digo do Movimento Inv�lido";
                        break;
                    case 06:
                        this.Codigo = 06;
                        this.Descricao = "Tipo N�mero Inscri��o Benefici�rio Inv�lido";
                        break;
                    case 07:
                        this.Codigo = 07;
                        this.Descricao = "Agencia/Conta/DV Inv�lidos";
                        break;
                    case 08:
                        this.Codigo = 08;
                        this.Descricao = "Nosso N�mero Inv�lido";
                        break;
                    case 09:
                        this.Codigo = 09;
                        this.Descricao = "Nosso N�mero Duplicado";
                        break;
                    case 10:
                        this.Codigo = 10;
                        this.Descricao = "Carteira Inv�lida";
                        break;
                    case 11:
                        this.Codigo = 11;
                        this.Descricao = "Data de Gera��o Inv�lida";
                        break;
                    case 12:
                        this.Codigo = 12;
                        this.Descricao = "Tipo de Documento Inv�lido";
                        break;
                    case 13:
                        this.Codigo = 13;
                        this.Descricao = "Identif.Da Emiss�o do Boleto Inv�lida";
                        break;
                    case 14:
                        this.Codigo = 14;
                        this.Descricao = "Identif.Da Distribui��o do Boleto Inv�lida";
                        break;
                    case 15:
                        this.Codigo = 15;
                        this.Descricao = "Caracter�sticas Cobran�a Incompat�veis";
                        break;
                    case 16:
                        this.Codigo = 16;
                        this.Descricao = "Data de Vencimento Inv�lida";
                        break;
                    case 17:
                        this.Codigo = 17;
                        this.Descricao = "Data de Vencimento Anterior � Data de Emiss�o";
                        break;
                    case 18:
                        this.Codigo = 18;
                        this.Descricao = "Vencimento fora do prazo de opera��o";
                        break;
                    case 19:
                        this.Codigo = 19;
                        this.Descricao = "T�tulo a Cargo de Bco Correspondentes c/ Vencto Inferior a XX Dias";
                        break;
                    case 20:
                        this.Codigo = 20;
                        this.Descricao = "Valor do T�tulo Inv�lido";
                        break;
                    case 21:
                        this.Codigo = 21;
                        this.Descricao = "Esp�cie do T�tulo Inv�lida";
                        break;
                    case 22:
                        this.Codigo = 22;
                        this.Descricao = "Esp�cie do T�tulo N�o Permitida para a Carteira";
                        break;
                    case 23:
                        this.Codigo = 23;
                        this.Descricao = "Aceite Inv�lido";
                        break;
                    case 24:
                        this.Codigo = 24;
                        this.Descricao = "Data da Emiss�o Inv�lida";
                        break;
                    case 25:
                        this.Codigo = 25;
                        this.Descricao = "Data da Emiss�o Posterior a Data de Entrada";
                        break;
                    case 26:
                        this.Codigo = 26;
                        this.Descricao = "C�digo de Juros de Mora Inv�lido";
                        break;
                    case 27:
                        this.Codigo = 27;
                        this.Descricao = "Valor/Taxa de Juros de Mora Inv�lido";
                        break;
                    case 28:
                        this.Codigo = 28;
                        this.Descricao = "C�digo do Desconto Inv�lido";
                        break;
                    case 29:
                        this.Codigo = 29;
                        this.Descricao = "Valor do Desconto Maior ou Igual ao Valor do T�tulo";
                        break;
                    case 30:
                        this.Codigo = 30;
                        this.Descricao = "Desconto a Conceder N�o Confere";
                        break;
                    case 31:
                        this.Codigo = 31;
                        this.Descricao = "Concess�o de Desconto - J� Existe Desconto Anterior";
                        break;
                    case 32:
                        this.Codigo = 32;
                        this.Descricao = "Valor do IOF Inv�lido";
                        break;
                    case 33:
                        this.Codigo = 33;
                        this.Descricao = "Valor do Abatimento Inv�lido";
                        break;
                    case 34:
                        this.Codigo = 34;
                        this.Descricao = "Valor do Abatimento Maior ou Igual ao Valor do T�tulo";
                        break;
                    case 35:
                        this.Codigo = 35;
                        this.Descricao = "Valor Abatimento a Conceder N�o Confere";
                        break;
                    case 36:
                        this.Codigo = 36;
                        this.Descricao = "Concess�o de Abatimento - J� Existe Abatimento Anterior";
                        break;
                    case 37:
                        this.Codigo = 37;
                        this.Descricao = "C�digo para Protesto Inv�lido";
                        break;
                    case 38:
                        this.Codigo = 38;
                        this.Descricao = "Prazo para Protesto Inv�lido";
                        break;
                    case 39:
                        this.Codigo = 39;
                        this.Descricao = "Pedido de Protesto N�o Permitido para o T�tulo";
                        break;
                    case 40:
                        this.Codigo = 40;
                        this.Descricao = "T�tulo com Ordem de Protesto Emitida";
                        break;
                    case 41:
                        this.Codigo = 41;
                        this.Descricao = "Pedido Cancelamento/Susta��o p/ T�tulos sem Instru��o Protesto";
                        break;
                    case 42:
                        this.Codigo = 42;
                        this.Descricao = "C�digo para Baixa/Devolu��o Inv�lido";
                        break;
                    case 43:
                        this.Codigo = 43;
                        this.Descricao = "Prazo para Baixa/Devolu��o Inv�lido";
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
