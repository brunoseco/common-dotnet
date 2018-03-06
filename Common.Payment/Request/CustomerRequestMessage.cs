﻿using Common.Payment.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Payment.Request
{
    internal class CustomerRequestMessage
    {
        /// <summary>
        /// E-Mail do Cliente
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Nome do Cliente
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// E-Mail do Cliente
        /// </summary>
        [JsonProperty("cpf_cnpj")]
        public string CpfOrCnpj { get; set; }

        /// <summary>
        /// Endereços de E-mail para cópia separados por vírgula.
        /// </summary>
        [JsonProperty("cc_emails")]
        public string WithCopyEmails { get; set; }

        /// <summary>
        /// Anotações Gerais do Cliente
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        ///  Variáveis Personalizadas
        /// </summary>
        [JsonProperty("custom_variables")]
        public List<CustomVariables> CustomVariables { get; set; }

    }
}
