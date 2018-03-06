
using System.ComponentModel;

namespace Common.Cna.Domain.Enums
{
    public enum EGrupo
    {
        [Description("Administrador do Portal")]
        AdministradorPortal = 6,

        [Description("Visitante")]
        Visitante = 7,

        [Description("Colaborador")]
        Colaborador = 8,

        [Description("Coordenador")]
        Coordenador = 10,

        [Description("Gerente")]
        Gerente = 11,

        [Description("Diretor")]
        Diretor = 12,

        [Description("Presidente")]
        Presidente = 13,

        [Description("Franqueado")]
        Franqueado = 14,

        [Description("Supervisor")]
        Supervisor = 15,

        [Description("Professor de Inglês")]
        ProfessorInglês = 16,

        [Description("Professorde Espanhol")]
        ProfessorEspanhol = 17,

        [Description("Coordenador Pedagógico")]
        CoordenadorPedagógico = 18,

        [Description("Coordenador Comercial")]
        CoordenadorComercial = 19,

        [Description("Divulgador")]
        Divulgador = 20,

        [Description("Auxiliar de Secretaria")]
        Secretária = 21,

        [Description("Monitor de Multimídia")]
        MonitordeMultimídia = 22,

        [Description("Gerentede Operações")]
        GerentedeOperações = 46,

        [Description("Assistente de Coordenação Comercial")]
        AssistentedeCoordenaçãoComercial = 48,

        [Description("Assistente de Coordenação Pedagógica")]
        AssistentedeCoordenaçãoPedagógica = 49,

        [Description("Vigia")]
        Vigia = 51,

        [Description("Auxiliar de Serviços Gerais")]
        AuxiliardeServiçosGerais = 52,

        [Description("Equipede Apoio")]
        EquipedeApoio = 53,

        [Description("Assistente de Supervisão")]
        AssistentedeSupervisão = 54,
    }
}
