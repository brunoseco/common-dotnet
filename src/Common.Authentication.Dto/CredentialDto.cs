using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Authentication.Dto
{
    public class CredentialDto
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string TextCaptcha { get; set; }
        public string IpAccess { get; set; }
        public string Localization { get; set; }
        public byte[] ImageCaptcha { get; set; }
        public int TypeLoginId { get; set; }
    }
}
