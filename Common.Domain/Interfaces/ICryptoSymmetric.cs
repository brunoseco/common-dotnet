using Common.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ICryptoSymmetric
    {
        string Encrypt(string value);
        string Decrypt(string value);
        void SetKey(string value);

    }
}
