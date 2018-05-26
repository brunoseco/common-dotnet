﻿
namespace Common.Cna.Domain.Enums
{
    public enum EParametro
    {
        DIAS_PROSPECT_SEM_CONTATO = 1,
        DIAS_CADASTRO_SEM_CONTATO = 2,
        NUMERO_REPIQUES_POR_DIA = 3,
        COBRAR_TAXA_CANCELAMENTO = 4,
        VALOR_FIXO_TAXA_CANCELAMENTO = 5,
        VALOR_PERCENTUAL_MULTA_CANCELAMENTO_UNICO_ESTAGIO = 6,
        VALOR_PERCENTUAL_MULTA_CANCELAMENTO_VARIOS_ESTAGIOS = 7,
        COBRAR_TAXA_TRANCAMENTO = 8,
        VALOR_FIXO_TAXA_TRANCAMENTO = 9,
        COBRAR_TAXA_TRANSFERENCIA_TURMA = 10,
        VALOR_FIXO_TAXA_TRANSFERENCIA_TURMA = 11,
        COBRAR_TAXA_TRANSFERENCIA_ESCOLA = 12,
        VALOR_FIXO_TAXA_TRANSFERENCIA_ESCOLA = 13,
        COBRAR_TAXA_SEGUNDA_CHAMADA = 14,
        VALOR_FIXO_TAXA_SEGUNDA_CHAMADA = 15,
        PERMITE_BONUS_PARA_EQUIPE = 17,
        CONSIDERAR_ENTREGA_MATERIAL_PRIMEIRO_DIA_AULA = 18,
        DIAS_PARA_AVISO_INICIO_AULA = 19,
        MINUTOS_DURACAO_AULA = 20,
        QUANTIDADE_MAXIMA_AULAS_EXCEDENTES = 21,
        DIAS_PARA_ALUNO_VIRAR_INADIMPLENTE = 22,
        VALOR_PADRAO_MULTA_CONTRATO = 23,
        VALOR_PADRAO_JUROS_CONTRATO = 24,
        VARIACAO_MINIMA_VALOR_RECEBIMENTO = 25,
        VARIACAO_MAXIMA_VALOR_RECEBIMENTO = 26,
        CONSIDERAR_PERCENTUAL_PADRAO_PARA_GERAR_BOLETO = 27,
        ESCOLA_ATUA_COM_TAXA_DE_CONTRATO = 29,
        PERMITE_COBRAR_TAXA_EMISSAO_BOLETO = 30,
        CONTA_ESCOLA_ID = 31,
        CONTA_CHEQUE_ID = 32,
        CONTA_CREDITO_ID = 33,
        DURACAO_TESTE_NIVEL = 34,
        ESCOLA_TRABALHA_COM_MADRINHA_TURMA = 36,
        LIMITE_MAX_AULA_DADAS_INCLUSAO_ALUNO_EM_TURMA = 37,
        VALOR_SERVICO_CNA_TALK = 39,
        VALOR_LICENCA_USO_CNABOX = 40,
        VALOR_POR_ENVIO_SMS = 41,
        VALOR_LICENCA_ONMAPS = 42,
        VALOR_LICENCA_CNATALK = 43,
        ESCOLA_TRABALHA_SHOPPING_CNA = 44,
        LIBERAR_MIGRACAO_MANUAL = 45,
        MENSAGEM_LINHA1_BOLETO = 47,
        MENSAGEM_LINHA2_BOLETO = 48,
        MENSAGEM_LINHA3_BOLETO = 49,
        MENSAGEM_DESCONTO_BOLETO = 51,
        SOMENTE_MODULO_CONSULTIVO = 52,

        LIBERAR_NOTIFICACAO_ALOCACAO_TRANSFERENCIA_TURMA = 67,
        LIBERAR_NOTIFICACAO_COACHING_AGENDADO = 99,

        LIBERAR_SMS_NOTIFICACAO_AUTOMATICA = 100,
        LIBERAR_ENVIO_SMS = 101,

        MENSAGEM_SEGUNDA_VIA_BOLETO = 102,
        TIPO_LAYOUT_RECIBO = 103,
        BAIXAR_BOLETO_APOS_30_DIAS_VENCIMENTO = 104,
        LIBERAR_FINANCEIRO_BOLETO_PORTAL_ALUNO = 106,
        VALOR_HORA_AVULSA = 107,
        LIMITE_MENSAGEM_ENVIO_COMUNICADOR = 110,
        ESCOLA_TEM_INSCRICAO_ESTADUAL = 111,

        PERMISSAO_SECRETARIA_TROCA_PROFESSOR_AULA = 112,
        PERMISSAO_COORD_PEDAGOGICO_APLICAR_TESTENIVEL_INGLES = 115,

        LIMITE_ENVIO_SMS = 116,
        BOLETO_COM_INFORMACAO_ALUNO = 117,

        FORUM_COMPETENTE = 118,

        HABILITAR_CONTAS_CORRENTES_INATIVAS = 119,

        LIMITE_CONTRATACAO_HORA_AVULSA = 120,

        LIMITE_CONTRATACAO_CARGA_HORARIA = 121,

        TEMPO_COMPROMISSO_SINALIZAR = 122,

        HABILITAR_VISUALIZAR_TODAS_AGENDAS = 123,

        QUANTIDADE_CARNE_POR_FOLHA = 124
    }
}
