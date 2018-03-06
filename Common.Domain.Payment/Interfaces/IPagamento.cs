using Common.Domain.Payment.Models;
using System;
using System.Threading.Tasks;

namespace Common.Domain.Payment
{
    public interface IPagamento : IDisposable
    {
        Task<Fatura> CriarFatura(Fatura model);
    }
}
