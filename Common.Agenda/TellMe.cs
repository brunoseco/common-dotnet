using Common.Agenda.ClassActivityDataService;
using Common.Agenda.ClassDataService;
using Common.Agenda.StudentDataService;
using Common.Agenda.UserDataService;
using Common.Domain.Enums;
using Common.Domain.Interfaces;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace Common.Agenda
{
    public class TellMe : IAgenda
    {
        private UserDataServiceClient UserData { get; set; }
        private StudentDataServiceClient StudentData { get; set; }
        private ClassDataServiceClient ClassData { get; set; }
        private ClassActivityDataServiceClient ClassActivityData { get; set; }

        public string NomeAluno { get; set; }
        public string NomeUsuario { get; set; }
        public DateTime DataNascimentoAluno { get; set; }
        public DateTime DataNascimentoUsuario { get; set; }
        public string EmailAluno { get; set; }
        public string EmailUsuario { get; set; }
        public UserRole PermissaoUsuario { get; set; }
        public string NomeTurma { get; set; }
        public string DescricaoAtividadeTurma { get; set; }
        public DateTime DataAtividadeTurma { get; set; }
        public string TrabalhoAtividadeTurma { get; set; }
        private string UserName = "cnanacional/integrador.admin";
        private string Password = "xA7Eg";

        public TellMe()
        {
            UserData = new UserDataServiceClient();
            StudentData = new StudentDataServiceClient();
            ClassData = new ClassDataServiceClient();
            ClassActivityData = new ClassActivityDataServiceClient();

            ServicePointManager.ServerCertificateValidationCallback =
                delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
        }

        public void InserirAluno()
        {
            var Aluno = new StudentData();

            Aluno.Name = NomeAluno;
            Aluno.BirthDate = DataNascimentoAluno;
            Aluno.Contacts = new StudentDataService.BaseUserData[1];
            Aluno.Contacts[0] = new StudentDataService.BaseUserData();
            Aluno.Contacts[0].Name = NomeAluno;
            Aluno.Contacts[0].Email = EmailAluno;
            Aluno.Contacts[0].BirthDate = DataNascimentoAluno;

            StudentData.ClientCredentials.UserName.UserName = UserName;
            StudentData.ClientCredentials.UserName.Password = Password;
            StudentData.Open();

            StudentData.Insert(Aluno);
            StudentData.Close();
        }

        public void InserirTurma()
        {
            var Turma = new ClassData();

            Turma.Name = NomeTurma;

            ClassData.ClientCredentials.UserName.UserName = UserName;
            ClassData.ClientCredentials.UserName.Password = Password;
            ClassData.Open();

            ClassData.Insert(Turma);
            ClassData.Close();
        }

        public void InserirUsuario()
        {
            var Usuario = new SchoolUserData();

            Usuario.Name = NomeUsuario;
            Usuario.BirthDate = DataNascimentoUsuario;
            Usuario.Email = EmailUsuario;
            Usuario.UserRole = (UserRoleEnaumData)PermissaoUsuario;

            UserData.ClientCredentials.UserName.UserName = UserName;
            UserData.ClientCredentials.UserName.Password = Password;
            UserData.Open();

            UserData.Insert(Usuario);
            UserData.Close();
        }

        public void InserirAtividadeTurma()
        {
            var AtividadeTurma = new ClassActivityData();

            AtividadeTurma.Description = DescricaoAtividadeTurma;
            AtividadeTurma.Date = DataAtividadeTurma;
            AtividadeTurma.HomeWork = TrabalhoAtividadeTurma;

            ClassActivityData.ClientCredentials.UserName.UserName = UserName;
            ClassActivityData.ClientCredentials.UserName.Password = Password;
            ClassActivityData.Open();

            ClassActivityData.Save(AtividadeTurma);
            ClassActivityData.Close();
        }

    }
}
