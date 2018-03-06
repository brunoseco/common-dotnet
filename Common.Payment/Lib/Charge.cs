using Common.Payment.Request;
using Common.Payment.Response;
using System;
using System.Threading.Tasks;

namespace Common.Payment.Lib
{
    /// <summary>
    /// Cobrança Direta
    /// Podemos efetuar uma cobrança direta de um valor, utilizando um token de cartão de crédito, uma forma de pagamento de cliente ou gerando um boleto bancário.
    /// </summary>
    internal class Charge : APIResource
    {
        public Charge()
        {
            BaseURI = "/charge";
        }

        /// <summary>
        /// Cria uma nova cobrança
        /// </summary>
        /// <param name="request">Parametros para criar uma cobrança</param>
        /// <returns>Uma cobrança do tipo boleto</returns>
        public async Task<ChargeResponseMessage> CreateAsync(ChargeRequestMessage request)
        {
            return await CreateAsync(request, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Cria uma nova cobrança possibilitando envio do token customizado, geralmente de uma subconta, em maketplaces
        /// </summary>
        /// <param name="request">Parametros para criar uma cobrança</param>
        /// <param name="customApiToken">Token customizado/param>
        /// <returns>Uma cobrança do tipo boleto</returns>
        public async Task<ChargeResponseMessage> CreateAsync(ChargeRequestMessage request, string customApiToken)
        {
            var retorno = await PostAsync<ChargeResponseMessage>(request, null, customApiToken).ConfigureAwait(false);
            return retorno;
        }
    }
}
