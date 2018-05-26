using System;
using System.Linq;
using System.Collections.Generic;
using Util.Data;
using Util.Numero;
using Common.Cna.Domain.Enums;

namespace Common.Cna.Domain.Cache
{
    [Serializable]
    public class ParametersCache
    {
        public Dictionary<string, object> source { get; set; }

        public ParametersCache(Dictionary<string, object> source)
        {
            this.source = source;
        }

        public string FORUM_COMPETENTE
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.FORUM_COMPETENTE.ToString()).IsAny())
                    return this.source[EParametro.FORUM_COMPETENTE.ToString()].ToString();

                return string.Empty;
            }
        }

        public bool COBRAR_TAXA_CANCELAMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.COBRAR_TAXA_CANCELAMENTO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.COBRAR_TAXA_CANCELAMENTO.ToString()].ToString());

                return false;
            }
        }
        public bool COBRAR_TAXA_TRANCAMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.COBRAR_TAXA_TRANCAMENTO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.COBRAR_TAXA_TRANCAMENTO.ToString()].ToString());

                return false;
            }
        }
        public bool COBRAR_TAXA_TRANSFERENCIA_TURMA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.COBRAR_TAXA_TRANSFERENCIA_TURMA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.COBRAR_TAXA_TRANSFERENCIA_TURMA.ToString()].ToString());

                return false;
            }
        }
        public bool COBRAR_TAXA_TRANSFERENCIA_ESCOLA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.COBRAR_TAXA_TRANSFERENCIA_ESCOLA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.COBRAR_TAXA_TRANSFERENCIA_ESCOLA.ToString()].ToString());

                return false;
            }
        }
        public bool COBRAR_TAXA_SEGUNDA_CHAMADA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.COBRAR_TAXA_SEGUNDA_CHAMADA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.COBRAR_TAXA_SEGUNDA_CHAMADA.ToString()].ToString());

                return false;
            }
        }
        public bool PERMITE_BONUS_PARA_EQUIPE
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.PERMITE_BONUS_PARA_EQUIPE.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.PERMITE_BONUS_PARA_EQUIPE.ToString()].ToString());

                return false;
            }
        }
        public bool CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA.ToString()].ToString());

                return false;
            }
        }
        public bool CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO.ToString()].ToString());

                return false;
            }
        }


        public bool PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES.ToString()].ToString());

                return true;
            }
        }
        public bool ESCOLA_TEM_INSCRICAO_ESTADUAL
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.ESCOLA_TEM_INSCRICAO_ESTADUAL.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.ESCOLA_TEM_INSCRICAO_ESTADUAL.ToString()].ToString());

                return true;
            }
        }
        public bool ESCOLA_ATUA_COM_TAXA_DE_CONTRATO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.ESCOLA_ATUA_COM_TAXA_DE_CONTRATO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.ESCOLA_ATUA_COM_TAXA_DE_CONTRATO.ToString()].ToString());

                return false;
            }
        }
        public bool PERMITE_COBRAR_TAXA_EMISSAO_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.PERMITE_COBRAR_TAXA_EMISSAO_BOLETO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.PERMITE_COBRAR_TAXA_EMISSAO_BOLETO.ToString()].ToString());

                return false;
            }
        }
        public bool ESCOLA_TRABALHA_COM_MADRINHA_TURMA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.ESCOLA_TRABALHA_COM_MADRINHA_TURMA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.ESCOLA_TRABALHA_COM_MADRINHA_TURMA.ToString()].ToString());

                return false;
            }
        }
        public bool LIBERAR_NOTIFICACAO_COACHING_AGENDADO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIBERAR_NOTIFICACAO_COACHING_AGENDADO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.LIBERAR_NOTIFICACAO_COACHING_AGENDADO.ToString()].ToString());

                return false;
            }
        }
        public bool LIBERAR_MIGRACAO_MANUAL
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIBERAR_MIGRACAO_MANUAL.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.LIBERAR_MIGRACAO_MANUAL.ToString()].ToString());

                return false;
            }
        }
        
        public int DIAS_PROSPECT_SEM_CONTATO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.DIAS_PROSPECT_SEM_CONTATO.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.DIAS_PROSPECT_SEM_CONTATO.ToString()].ToString());

                return 0;
            }
        }
        public int DIAS_CADASTRO_SEM_CONTATO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.DIAS_CADASTRO_SEM_CONTATO.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.DIAS_CADASTRO_SEM_CONTATO.ToString()].ToString());

                return 0;
            }
        }
        public int NUMERO_REPIQUES_POR_DIA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.NUMERO_REPIQUES_POR_DIA.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.NUMERO_REPIQUES_POR_DIA.ToString()].ToString());

                return 0;
            }
        }
        public int DIAS_PARA_AVISO_INICIO_AULA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.DIAS_PARA_AVISO_INICIO_AULA.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.DIAS_PARA_AVISO_INICIO_AULA.ToString()].ToString());

                return 0;
            }
        }
        public int MINUTOS_DURACAO_AULA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MINUTOS_DURACAO_AULA.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.MINUTOS_DURACAO_AULA.ToString()].ToString());

                return 0;
            }
        }
        public int QUANTIDADE_MAXIMA_AULAS_EXCEDENTES
        {
            get
            {

                return NumeroHelper.intTryParse(this.source[EParametro.QUANTIDADE_MAXIMA_AULAS_EXCEDENTES.ToString()].ToString());
            }
        }
        public int DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE.ToString()].ToString());

                return 0;
            }
        }
        public int CONTA_ESCOLA_ID
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.CONTA_ESCOLA_ID.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.CONTA_ESCOLA_ID.ToString()].ToString());

                return 0;
            }
        }
        public int CONTA_CHEQUE_ID
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.CONTA_CHEQUE_ID.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.CONTA_CHEQUE_ID.ToString()].ToString());

                return 0;
            }
        }
        public int CONTA_CREDITO_ID
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.CONTA_CREDITO_ID.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.CONTA_CREDITO_ID.ToString()].ToString());

                return 0;
            }
        }
        public int DURACAO_TESTE_NIVEL
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.DURACAO_TESTE_NIVEL.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.DURACAO_TESTE_NIVEL.ToString()].ToString());

                return 0;
            }
        }
        public int LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA.ToString()].ToString());

                return 0;
            }
        }
        public int TIPO_LAYOUT_RECIBO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.TIPO_LAYOUT_RECIBO.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.TIPO_LAYOUT_RECIBO.ToString()].ToString());

                return 0;
            }
        }

        public int TEMPO_COMPROMISSO_SINALIZAR
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.TEMPO_COMPROMISSO_SINALIZAR.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.TEMPO_COMPROMISSO_SINALIZAR.ToString()].ToString());

                return 0;

            }
        }

        public decimal VALOR_FIXO_TAXA_CANCELAMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_FIXO_TAXA_CANCELAMENTO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_FIXO_TAXA_CANCELAMENTO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_FIXO_TAXA_TRANCAMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_FIXO_TAXA_TRANCAMENTO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_FIXO_TAXA_TRANCAMENTO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_FIXO_TAXA_SEGUNDA_CHAMADA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_FIXO_TAXA_SEGUNDA_CHAMADA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_FIXO_TAXA_SEGUNDA_CHAMADA.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_PADRAO_MULTA_CONTRATO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_PADRAO_MULTA_CONTRATO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_PADRAO_MULTA_CONTRATO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_PADRAO_JUROS_CONTRATO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_PADRAO_JUROS_CONTRATO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_PADRAO_JUROS_CONTRATO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VARIACAO_MINIMA_VALOR_RECEBIMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VARIACAO_MINIMA_VALOR_RECEBIMENTO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VARIACAO_MINIMA_VALOR_RECEBIMENTO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VARIACAO_MAXIMA_VALOR_RECEBIMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VARIACAO_MAXIMA_VALOR_RECEBIMENTO.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VARIACAO_MAXIMA_VALOR_RECEBIMENTO.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_SERVICO_CNA_TALK
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_SERVICO_CNA_TALK.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_SERVICO_CNA_TALK.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_LICENCA_USO_CNABOX
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_LICENCA_USO_CNABOX.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_LICENCA_USO_CNABOX.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_POR_ENVIO_SMS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_POR_ENVIO_SMS.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_POR_ENVIO_SMS.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_LICENCA_ONMAPS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_LICENCA_ONMAPS.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_LICENCA_ONMAPS.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_LICENCA_CNATALK
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_LICENCA_CNATALK.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_LICENCA_CNATALK.ToString()].ToString());

                return 0;
            }
        }
        public decimal VALOR_HORA_AVULSA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.VALOR_HORA_AVULSA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.VALOR_HORA_AVULSA.ToString()].ToString());

                return 0;
            }
        }

        public string MENSAGEM_LINHA1_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MENSAGEM_LINHA1_BOLETO.ToString()).IsAny())
                    return this.source[EParametro.MENSAGEM_LINHA1_BOLETO.ToString()].ToString();

                return string.Empty;
            }
        }
        public string MENSAGEM_LINHA2_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MENSAGEM_LINHA2_BOLETO.ToString()).IsAny())
                    return this.source[EParametro.MENSAGEM_LINHA2_BOLETO.ToString()].ToString();

                return string.Empty;
            }
        }
        public string MENSAGEM_LINHA3_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MENSAGEM_LINHA3_BOLETO.ToString()).IsAny())
                    return this.source[EParametro.MENSAGEM_LINHA3_BOLETO.ToString()].ToString();

                return string.Empty;
            }
        }
        public string MENSAGEM_DESCONTO_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MENSAGEM_DESCONTO_BOLETO.ToString()).IsAny())
                    return this.source[EParametro.MENSAGEM_DESCONTO_BOLETO.ToString()].ToString();

                return string.Empty;
            }
        }
        public string MENSAGEM_SEGUNDA_VIA_BOLETO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.MENSAGEM_SEGUNDA_VIA_BOLETO.ToString()).IsAny())
                    return this.source[EParametro.MENSAGEM_SEGUNDA_VIA_BOLETO.ToString()].ToString();

                return string.Empty;
            }
        }
        public string BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO.ToString()).IsAny())
                    return this.source[EParametro.BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO.ToString()].ToString();

                return string.Empty;
            }
        }

        public bool SOMENTE_MODULO_CONSULTIVO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.SOMENTE_MODULO_CONSULTIVO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.SOMENTE_MODULO_CONSULTIVO.ToString()].ToString());

                return false;
            }
        }
        public bool LIBERAR_SMS_NOTIFICACAO_AUTOMATICA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIBERAR_SMS_NOTIFICACAO_AUTOMATICA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.LIBERAR_SMS_NOTIFICACAO_AUTOMATICA.ToString()].ToString());

                return false;
            }
        }
        public bool LIBERAR_ENVIO_SMS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIBERAR_ENVIO_SMS.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.LIBERAR_ENVIO_SMS.ToString()].ToString());

                return false;
            }
        }
        public bool ESCOLA_TRABALHA_SHOPPING_CNA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.ESCOLA_TRABALHA_SHOPPING_CNA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.ESCOLA_TRABALHA_SHOPPING_CNA.ToString()].ToString());

                return false;
            }
        }
        public bool LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO.ToString()].ToString());

                return false;
            }
        }

        public decimal LIMITE_MENSAGEM_ENVIO_COMUNICADOR
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIMITE_MENSAGEM_ENVIO_COMUNICADOR.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.LIMITE_MENSAGEM_ENVIO_COMUNICADOR.ToString()].ToString());

                return 0;
            }
        }
        
        public bool PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA.ToString()].ToString());

                return false;
            }
        }

        public int LIMITE_ENVIO_SMS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIMITE_ENVIO_SMS.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.LIMITE_ENVIO_SMS.ToString()].ToString());

                return 0;
            }
        }

        public decimal LIMITE_CONTRATACAO_HORA_AVULSA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIMITE_CONTRATACAO_HORA_AVULSA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.LIMITE_CONTRATACAO_HORA_AVULSA.ToString()].ToString());

                return 0;
            }
        }

        public decimal LIMITE_CONTRATACAO_CARGA_HORARIA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.LIMITE_CONTRATACAO_CARGA_HORARIA.ToString()).IsAny())
                    return NumeroHelper.DecimalTryParse(this.source[EParametro.LIMITE_CONTRATACAO_CARGA_HORARIA.ToString()].ToString());

                return 0;
            }
        }

        public bool BOLETO_COM_INFORMACAO_ALUNO
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.BOLETO_COM_INFORMACAO_ALUNO.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.BOLETO_COM_INFORMACAO_ALUNO.ToString()].ToString());

                return false;
            }
        }

        public bool HABILITAR_CONTAS_CORRENTES_INATIVAS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.HABILITAR_CONTAS_CORRENTES_INATIVAS.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.HABILITAR_CONTAS_CORRENTES_INATIVAS.ToString()].ToString());

                return false;
            }
        }

        public bool HABILITAR_VISUALIZAR_TODAS_AGENDAS
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.HABILITAR_VISUALIZAR_TODAS_AGENDAS.ToString()).IsAny())
                    return NumeroHelper.BoolTryParse(this.source[EParametro.HABILITAR_VISUALIZAR_TODAS_AGENDAS.ToString()].ToString());

                return false;
            }
        }


        public int QUANTIDADE_CARNE_POR_FOLHA
        {
            get
            {
                if (this.source.Where(_ => _.Key == EParametro.QUANTIDADE_CARNE_POR_FOLHA.ToString()).IsAny())
                    return NumeroHelper.intTryParse(this.source[EParametro.QUANTIDADE_CARNE_POR_FOLHA.ToString()].ToString());

                return 0;
            }
        }


    }
}
