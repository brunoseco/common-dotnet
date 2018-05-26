using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using Common.Cna.Domain.Cache;
using Common.Domain;
using Common.Domain.CustomExceptions;
using Common.Domain.Interfaces;
using Common.Cna.Domain.Enums;

namespace Common.Cna.Domain.Helpers
{
    public class HelperEscola : DomainBase
    {
        private static HelperEscola GetInstance()
        {
            return new HelperEscola();
        }

        private static ColaboradorLogadoCache ObterColaboradorLogado(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            if (user.IsNull())
                throw new CustomValidationException(string.Format("Usuário invalido."));

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DisableAuth"]) == true)
            {
                return new ColaboradorLogadoCache
                {
                    EscolaLogada = new EscolaCache { EscolaId = 1 }
                };
            }
            if (user.OnlyUser)
            {
                return new ColaboradorLogadoCache
                {
                    EscolaLogada = new EscolaCache { EscolaId = 0 }
                };
            }



            var colaboradorEscola = user.GetUserInfoFromCache<ColaboradorLogadoCache>(contingencyMethod);
            if (colaboradorEscola.IsNull())
                throw new CustomValidationException(string.Format("Escola Não Encontrada para o usuario logado"));

            return colaboradorEscola;
        }



        public static int GetEscolaAdmNacionalId()
        {
            return 276;
        }
        public static EscolaCache GetEscolaLogada(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            return ObterColaboradorLogado(user, contingencyMethod).EscolaLogada;
        }

        public static IEnumerable<int> GetGrupoEscolaLogada(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            return ObterColaboradorLogado(user, contingencyMethod).EscolaLogada.Grupos.Select(_ => _.GrupoId);
        }

        public static bool TemSomentePerfilProfessor(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            var currentUser = ObterColaboradorLogado(user, contingencyMethod);
            if (currentUser.EscolaLogada == null)
                return false;
            if (currentUser.EscolaLogada.Grupos == null)
                return false;

            return !currentUser.EscolaLogada.Grupos
                .Where(_ => _.GrupoId != (int)EGrupo.ProfessorEspanhol)
                .Where(_ => _.GrupoId != (int)EGrupo.ProfessorInglês).Any();
        }

        public static bool TemPerfilDe(CurrentUser user, IEnumerable<EGrupo> Grupo, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            var ids = Grupo.Select(_ => (int)_);

            var escolaLogada = ObterColaboradorLogado(user, contingencyMethod).EscolaLogada;
            if (escolaLogada.Grupos.IsNotNull())
                return escolaLogada.Grupos.Where(_ => ids.Contains(_.GrupoId)).Any();

            return false;
        }

        public static bool TemPerfilDe(CurrentUser user, Func<GrupoCache, bool> criterio, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            var escolaLogada = ObterColaboradorLogado(user, contingencyMethod).EscolaLogada;
            if (escolaLogada.Grupos.IsNotNull())
                return escolaLogada.Grupos.Where(criterio).Any();

            return false;
        }

        public static bool TemPerfilDivulgador(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            return TemPerfilDe(user, _ => _.GrupoId == (int)EGrupo.Divulgador, contingencyMethod) &&
                  !TemPerfilDe(user, _ => _.GrupoId == (int)EGrupo.AssistentedeCoordenaçãoComercial, contingencyMethod) &&
                  !TemPerfilDe(user, _ => _.GrupoId == (int)EGrupo.CoordenadorComercial, contingencyMethod);
        }


        public static int[] StatusTurmaLiberadoPorPerfil(CurrentUser user, Func<CurrentUser, ColaboradorLogadoCache> contingencyMethod)
        {
            var perfilSecretaria = TemPerfilDe(user, new List<EGrupo> { EGrupo.Secretária }, contingencyMethod);

            var gruposMasters = new List<EGrupo> {
                EGrupo.AdministradorPortal,
                EGrupo.AssistentedeCoordenaçãoPedagógica,
                EGrupo.CoordenadorPedagógico,
                EGrupo.Supervisor,
                EGrupo.AssistentedeSupervisão,
                EGrupo.Franqueado
            };

            if (!TemPerfilDe(user, gruposMasters, contingencyMethod))
            {
                if (TemPerfilDe(user, _ => _.GrupoId == (int)EGrupo.ProfessorEspanhol || _.GrupoId == (int)EGrupo.ProfessorInglês, contingencyMethod))
                    return new int[] { (int)EStatusTurma.Andamento, (int)EStatusTurma.Encerrada, (int)EStatusTurma.Formacao };
                else if (perfilSecretaria)
                    return new int[] { (int)EStatusTurma.Andamento, (int)EStatusTurma.Encerrada, (int)EStatusTurma.Formacao };
                else
                    return new int[] { (int)EStatusTurma.Andamento, (int)EStatusTurma.Formacao };
            }

            return null;
        }

        public static ParametersCache GetParametersFromCurrentUser(string token, ICache cache, Func<CurrentUser, ParametersCache> contingencyMethod)
        {
            return GetParametersFromCurrentUser(HelperValidateAuth.ValidateAuthParameters(token, cache), contingencyMethod);
        }

        public static ParametersCache GetParametersFromCurrentUser(CurrentUser userWithParameter, Func<CurrentUser, ParametersCache> contingencyMethod)
        {
            return userWithParameter.GetUserInfoFromCache<ParametersCache>(contingencyMethod);
        }

        public static class Financeiro
        {
            public static int GetContaEscolaId(CurrentUser userWithParameter, Func<CurrentUser, ParametersCache> contingencyMethod)
            {
                return GetParametersFromCurrentUser(userWithParameter, contingencyMethod).CONTA_ESCOLA_ID;
            }

            public static int GetContaChequeId(CurrentUser userWithParameter, Func<CurrentUser, ParametersCache> contingencyMethod)
            {
                return GetParametersFromCurrentUser(userWithParameter, contingencyMethod).CONTA_CHEQUE_ID;
            }

            public static int GetContaCreditoId(CurrentUser userWithParameter, Func<CurrentUser, ParametersCache> contingencyMethod)
            {
                return GetParametersFromCurrentUser(userWithParameter, contingencyMethod).CONTA_CREDITO_ID;
            }
        }

        public static int GetAdministracaoNacionalEscolaId()
        {
            return (int)EEscola.CNA_Administracao_Nacional;
        }

        public override void Dispose()
        {

        }
    }
}