using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Authentication.Dto
{
    public class CredentialTokenDto
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int EscolaId { get; set; }
        public int TypeLoginId { get; set; }
    }
}
