using System.Collections.Generic;
using Common.Cna.Domain.Cache;
using Common.Domain;
using Common.Domain.Interfaces;
using Common.Cna.Domain.Enums;

namespace Common.Cna.Domain.Helpers
{
    public abstract class HelperParametro : DomainBase
    {
        protected IRepository rep;

        protected ParametersCache BuilderConfigCache()
        {
            var source = new Dictionary<string, object>
            {
                { EParametro.LIMITE_MENSAGEM_ENVIO_COMUNICADOR.ToString(), GetValorChave(EParametro.LIMITE_MENSAGEM_ENVIO_COMUNICADOR.ToString()) },
                { EParametro.ESCOLA_ATUA_COM_TAXA_DE_CONTRATO.ToString(), GetValorChave(EParametro.ESCOLA_ATUA_COM_TAXA_DE_CONTRATO.ToString()) },
                { EParametro.DIAS_PROSPECT_SEM_CONTATO.ToString(), GetValorChave(EParametro.DIAS_PROSPECT_SEM_CONTATO.ToString()) },
                { EParametro.DIAS_CADASTRO_SEM_CONTATO.ToString(), GetValorChave(EParametro.DIAS_CADASTRO_SEM_CONTATO.ToString()) },
                { EParametro.NUMERO_REPIQUES_POR_DIA.ToString(), GetValorChave(EParametro.NUMERO_REPIQUES_POR_DIA.ToString()) },
                { EParametro.COBRAR_TAXA_CANCELAMENTO.ToString(), GetValorChave(EParametro.COBRAR_TAXA_CANCELAMENTO.ToString()) },
                { EParametro.VALOR_FIXO_TAXA_CANCELAMENTO.ToString(), GetValorChave(EParametro.VALOR_FIXO_TAXA_CANCELAMENTO.ToString()) },
                { EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO.ToString(), GetValorChave(EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO.ToString()) },
                { EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS.ToString(), GetValorChave(EParametro.VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS.ToString()) },
                { EParametro.COBRAR_TAXA_TRANCAMENTO.ToString(), GetValorChave(EParametro.COBRAR_TAXA_TRANCAMENTO.ToString()) },
                { EParametro.VALOR_FIXO_TAXA_TRANCAMENTO.ToString(), GetValorChave(EParametro.VALOR_FIXO_TAXA_TRANCAMENTO.ToString()) },
                { EParametro.COBRAR_TAXA_TRANSFERENCIA_TURMA.ToString(), GetValorChave(EParametro.COBRAR_TAXA_TRANSFERENCIA_TURMA.ToString()) },
                { EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA.ToString(), GetValorChave(EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA.ToString()) },
                { EParametro.COBRAR_TAXA_TRANSFERENCIA_ESCOLA.ToString(), GetValorChave(EParametro.COBRAR_TAXA_TRANSFERENCIA_ESCOLA.ToString()) },
                { EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA.ToString(), GetValorChave(EParametro.VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA.ToString()) },
                { EParametro.COBRAR_TAXA_SEGUNDA_CHAMADA.ToString(), GetValorChave(EParametro.COBRAR_TAXA_SEGUNDA_CHAMADA.ToString()) },
                { EParametro.VALOR_FIXO_TAXA_SEGUNDA_CHAMADA.ToString(), GetValorChave(EParametro.VALOR_FIXO_TAXA_SEGUNDA_CHAMADA.ToString()) },
                { EParametro.PERMITE_BONUS_PARA_EQUIPE.ToString(), GetValorChave(EParametro.PERMITE_BONUS_PARA_EQUIPE.ToString()) },
                { EParametro.CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA.ToString(), GetValorChave(EParametro.CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA.ToString()) },
                { EParametro.DIAS_PARA_AVISO_INICIO_AULA.ToString(), GetValorChave(EParametro.DIAS_PARA_AVISO_INICIO_AULA.ToString()) },
                { EParametro.MINUTOS_DURACAO_AULA.ToString(), GetValorChave(EParametro.MINUTOS_DURACAO_AULA.ToString()) },
                { EParametro.QUANTIDADE_MAXIMA_AULAS_EXCEDENTES.ToString(), GetValorChave(EParametro.QUANTIDADE_MAXIMA_AULAS_EXCEDENTES.ToString()) },
                { EParametro.DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE.ToString(), GetValorChave(EParametro.DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE.ToString()) },
                { EParametro.VALOR_PADRAO_MULTA_CONTRATO.ToString(), GetValorChave(EParametro.VALOR_PADRAO_MULTA_CONTRATO.ToString()) },
                { EParametro.VALOR_PADRAO_JUROS_CONTRATO.ToString(), GetValorChave(EParametro.VALOR_PADRAO_JUROS_CONTRATO.ToString()) },
                { EParametro.VARIACAO_MINIMA_VALOR_RECEBIMENTO.ToString(), GetValorChave(EParametro.VARIACAO_MINIMA_VALOR_RECEBIMENTO.ToString()) },
                { EParametro.VARIACAO_MAXIMA_VALOR_RECEBIMENTO.ToString(), GetValorChave(EParametro.VARIACAO_MAXIMA_VALOR_RECEBIMENTO.ToString()) },
                { EParametro.CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO.ToString(), GetValorChave(EParametro.CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO.ToString()) },
                { EParametro.PERMITE_COBRAR_TAXA_EMISSAO_BOLETO.ToString(), GetValorChave(EParametro.PERMITE_COBRAR_TAXA_EMISSAO_BOLETO.ToString()) },
                { EParametro.CONTA_ESCOLA_ID.ToString(), GetValorChave(EParametro.CONTA_ESCOLA_ID.ToString()) },
                { EParametro.CONTA_CHEQUE_ID.ToString(), GetValorChave(EParametro.CONTA_CHEQUE_ID.ToString()) },
                { EParametro.CONTA_CREDITO_ID.ToString(), GetValorChave(EParametro.CONTA_CREDITO_ID.ToString()) },
                { EParametro.DURACAO_TESTE_NIVEL.ToString(), GetValorChave(EParametro.DURACAO_TESTE_NIVEL.ToString()) },
                { EParametro.ESCOLA_TRABALHA_COM_MADRINHA_TURMA.ToString(), GetValorChave(EParametro.ESCOLA_TRABALHA_COM_MADRINHA_TURMA.ToString()) },
                { EParametro.LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA.ToString(), GetValorChave(EParametro.LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA.ToString()) },
                { EParametro.LIBERAR_NOTIFICACAO_COACHING_AGENDADO.ToString(), GetValorChave(EParametro.LIBERAR_NOTIFICACAO_COACHING_AGENDADO.ToString()) },
                { EParametro.VALOR_SERVICO_CNA_TALK.ToString(), GetValorChave(EParametro.VALOR_SERVICO_CNA_TALK.ToString()) },
                { EParametro.VALOR_LICENCA_USO_CNABOX.ToString(), GetValorChave(EParametro.VALOR_LICENCA_USO_CNABOX.ToString()) },
                { EParametro.VALOR_POR_ENVIO_SMS.ToString(), GetValorChave(EParametro.VALOR_POR_ENVIO_SMS.ToString()) },
                { EParametro.VALOR_LICENCA_ONMAPS.ToString(), GetValorChave(EParametro.VALOR_LICENCA_ONMAPS.ToString()) },
                { EParametro.VALOR_LICENCA_CNATALK.ToString(), GetValorChave(EParametro.VALOR_LICENCA_CNATALK.ToString()) },
                { EParametro.LIBERAR_MIGRACAO_MANUAL.ToString(), GetValorChave(EParametro.LIBERAR_MIGRACAO_MANUAL.ToString()) },
                { EParametro.MENSAGEM_LINHA1_BOLETO.ToString(), GetValorChave(EParametro.MENSAGEM_LINHA1_BOLETO.ToString()) },
                { EParametro.MENSAGEM_LINHA2_BOLETO.ToString(), GetValorChave(EParametro.MENSAGEM_LINHA2_BOLETO.ToString()) },
                { EParametro.MENSAGEM_LINHA3_BOLETO.ToString(), GetValorChave(EParametro.MENSAGEM_LINHA3_BOLETO.ToString()) },
                { EParametro.MENSAGEM_DESCONTO_BOLETO.ToString(), GetValorChave(EParametro.MENSAGEM_DESCONTO_BOLETO.ToString()) },
                { EParametro.MENSAGEM_SEGUNDA_VIA_BOLETO.ToString(), GetValorChave(EParametro.MENSAGEM_SEGUNDA_VIA_BOLETO.ToString()) },
                { EParametro.SOMENTE_MODULO_CONSULTIVO.ToString(), GetValorChave(EParametro.SOMENTE_MODULO_CONSULTIVO.ToString()) },
                { EParametro.LIBERAR_SMS_NOTIFICACAO_AUTOMATICA.ToString(), GetValorChave(EParametro.LIBERAR_SMS_NOTIFICACAO_AUTOMATICA.ToString()) },
                { EParametro.LIBERAR_ENVIO_SMS.ToString(), GetValorChave(EParametro.LIBERAR_ENVIO_SMS.ToString()) },
                { EParametro.ESCOLA_TRABALHA_SHOPPING_CNA.ToString(), GetValorChave(EParametro.ESCOLA_TRABALHA_SHOPPING_CNA.ToString()) },
                { EParametro.TIPO_LAYOUT_RECIBO.ToString(), GetValorChave(EParametro.TIPO_LAYOUT_RECIBO.ToString()) },
                { EParametro.BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO.ToString(), GetValorChave(EParametro.BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO.ToString()) },
                { EParametro.LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO.ToString(), GetValorChave(EParametro.LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO.ToString()) },
                { EParametro.VALOR_HORA_AVULSA.ToString(), GetValorChave(EParametro.VALOR_HORA_AVULSA.ToString()) },
                { EParametro.ESCOLA_TEM_INSCRICAO_ESTADUAL.ToString(), GetValorChave(EParametro.ESCOLA_TEM_INSCRICAO_ESTADUAL.ToString()) },
                { EParametro.PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES.ToString(), GetValorChave(EParametro.PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES.ToString()) },
                { EParametro.PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA.ToString(), GetValorChave(EParametro.PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA.ToString()) },
                { EParametro.LIMITE_ENVIO_SMS.ToString(), GetValorChave(EParametro.LIMITE_ENVIO_SMS.ToString()) },
                { EParametro.BOLETO_COM_INFORMACAO_ALUNO.ToString(), GetValorChave(EParametro.BOLETO_COM_INFORMACAO_ALUNO.ToString()) },
                { EParametro.FORUM_COMPETENTE.ToString(), GetValorChave(EParametro.FORUM_COMPETENTE.ToString()) },
                { EParametro.HABILITAR_CONTAS_CORRENTES_INATIVAS.ToString(), GetValorChave(EParametro.HABILITAR_CONTAS_CORRENTES_INATIVAS.ToString()) },
                { EParametro.HABILITAR_VISUALIZAR_TODAS_AGENDAS.ToString(), GetValorChave(EParametro.HABILITAR_VISUALIZAR_TODAS_AGENDAS.ToString()) },
                { EParametro.LIMITE_CONTRATACAO_CARGA_HORARIA.ToString(), GetValorChave(EParametro.LIMITE_CONTRATACAO_CARGA_HORARIA.ToString()) },
                { EParametro.TEMPO_COMPROMISSO_SINALIZAR.ToString(), GetValorChave(EParametro.TEMPO_COMPROMISSO_SINALIZAR.ToString()) },
                { EParametro.LIMITE_CONTRATACAO_HORA_AVULSA.ToString(), GetValorChave(EParametro.LIMITE_CONTRATACAO_HORA_AVULSA.ToString()) },
                { EParametro.QUANTIDADE_CARNE_POR_FOLHA.ToString(), GetValorChave(EParametro.QUANTIDADE_CARNE_POR_FOLHA.ToString()) },
            };

            var retorno = new ParametersCache(source);
            return retorno;
        }

        public abstract string GetValorChave(string chave, bool verificar = true);

        public override void Dispose()
        {

        }
    }

}
