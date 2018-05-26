using Common.Domain.Payment;
using Common.Payment.Entity;
using Common.Payment.Lib;
using Common.Payment.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Payment.Models;

namespace Common.Payment
{
    public class Pagamento : IPagamento
    {
        public async Task<Fatura> CriarFatura(Fatura model)
        {
            using (var apiInvoice = new Invoice())
            {
                var invoiceItems = this.ConfiguraItensDaFatura(model);
                var irm = this.ConfiguraFaturaInicial(model, invoiceItems);

                this.ConfiguraMultaJuros(model, irm);
                this.ConfiguraDescontoAteVencimento(model, invoiceItems, irm);
                this.ConfiguraTipoPagamento(model, irm);
                this.ConfiguraPagador(model, irm);

                var invoice = await apiInvoice.CreateAsync(irm).ConfigureAwait(false);

                return new Fatura(invoice.Email,
                    Convert.ToDateTime(invoice.DueDate),
                    invoice.Items.Select(_ => new FaturaItem
                    {
                        Id = _.ID,
                        Descricao = _.Description,
                        Valor = _.PriceCents / 100,
                        Quantidade = _.Quantity,
                    }).ToList()) { Id = invoice.ID };
            };
        }

        private void ConfiguraPagador(Fatura model, InvoiceRequestMessage irm)
        {
            if (model.Pagador != null)
            {
                irm.Payer = new PayerModel
                {
                    CpfOrCnpj = model.Pagador.CPF_CNPJ,
                    Name = model.Pagador.Nome,
                    PhonePrefix = model.Pagador.TelefonePrefixo,
                    Phone = model.Pagador.Telefone,
                    Email = model.Pagador.Email,
                    Address = new AddressModel
                    {
                        Street = model.Pagador.Rua,
                        Number = model.Pagador.Numero,
                        City = model.Pagador.Cidade,
                        State = model.Pagador.UF,
                        Country = model.Pagador.Pais,
                        ZipCode = model.Pagador.CEP,
                    }
                };
            }
        }

        private List<Item> ConfiguraItensDaFatura(Fatura model)
        {
            var invoiceItems = new List<Item>();
            foreach (var item in model.FaturaItems)
            {
                invoiceItems.Add(new Item
                {
                    Description = item.Descricao,
                    PriceCents = (int)(item.Valor * 100),
                    Quantity = item.Quantidade
                });
            }

            return invoiceItems;
        }

        private void ConfiguraTipoPagamento(Fatura model, InvoiceRequestMessage irm)
        {
            if (model.TipoPagamento == ETipoPagamento.BOLETO)
                irm.PaymentMethod = Constants.PaymentMethod.BANK_SLIP;
            else if (model.TipoPagamento == ETipoPagamento.TODOS)
                irm.PaymentMethod = Constants.PaymentMethod.BANK_SLIP;
            else
                irm.PaymentMethod = Constants.PaymentMethod.ALL;
        }

        private void ConfiguraDescontoAteVencimento(Fatura model, List<Item> invoiceItems, InvoiceRequestMessage irm)
        {
            if (model.TemDescontoAteVencimento)
            {
                irm.EarlyPaymentDiscount = model.TemDescontoAteVencimento;
                irm.EarlyPaymentDiscountDays = 0;
                var _valor = invoiceItems.Sum(_ => _.PriceCents * _.Quantity) / 100M;
                irm.EarlyPaymentDiscountPercent = (model.ValorDescontoAteVencimento / _valor * 100);
            }
        }

        private InvoiceRequestMessage ConfiguraFaturaInicial(Fatura model, List<Item> invoiceItems)
        {
            return new InvoiceRequestMessage(model.Email, model.DataVencimento, invoiceItems.ToArray());
        }

        private void ConfiguraMultaJuros(Fatura model, InvoiceRequestMessage irm)
        {
            if (model.ValorMulta > 0)
            {
                irm.EnableLateFine = true;
                irm.LatePaymentFine = model.ValorMulta.ToString();
            }

            if (model.CobrarJuros)
                irm.EnableProportionalDailyTax = true;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
