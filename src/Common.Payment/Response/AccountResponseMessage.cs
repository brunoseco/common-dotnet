﻿using Newtonsoft.Json;
using System;

namespace Common.Payment.Response
{
    /// <summary>
    /// Requisição para a API de conta 
    /// </summary>
    internal class AccountResponseMessage
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Token de produção
        /// </summary>
        [JsonProperty("live_api_token")]
        public string LiveApiToken { get; set; }

        /// <summary>
        /// Token de teste
        /// </summary>
        [JsonProperty("test_api_token")]
        public string TestApiToken { get; set; }

        /// <summary>
        /// Token do usuário, usado em algumas APIs específicas
        /// </summary>
        [JsonProperty("user_token")]
        public string UserToken { get; set; }
    }
}
