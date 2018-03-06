using System;
using System.Web.UI;
using Microsoft.VisualBasic;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.041.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <Author>
    /// Felipe Silveira - Transis Informática
    /// </Author>
    internal class Banco_Banrisul : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _primDigito;
        private int _segDigito;

        /// <author>
        /// Classe responsavel em criar os campos do Banco Banrisul.
        /// </author>
        internal Banco_Banrisul()
        {
            this.Codigo = 041;
            this.Digito = "8";
            this.Nome = "Banco Banrisul";
        }

        #region Boleto
        public override void ValidaBoleto(Boleto boleto)
        {
            boleto.ContaBancaria = boleto.Cedente.ContaBancaria;

            //Formata o tamanho do número da agência
            if (boleto.Cedente.ContaBancaria.Conta.Length < 4)
                throw new Exception("Número da agência inválido");

            //Formata o tamanho do número da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Formata o tamanho do número de nosso número
            if (boleto.NossoNumero.Length < 8)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);
            else if (boleto.NossoNumero.Length > 8)
                throw new NotSupportedException("Para o banco Banrisul, o nosso número deve ter 08 posições e 02 dígitos verificadores (calculados automaticamente).");

            boleto.NossoNumero = CalcularNCNossoNumero(boleto.NossoNumero);

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            FormataNossoNumero(boleto);
        }

        private string CalcularNCNossoNumero(String nossoNumero)
        {
            int dv1 = Mod10Banri(nossoNumero);
            int dv1e2 = Mod11Banri(nossoNumero, dv1); // O módulo 11 sempre devolve os dois Dígitos, pois, as vezes o dígito calculado no mod10 será incrementado em 1
            return nossoNumero + dv1e2.ToString("00");
        }

        private string CalcularNCCodBarras(String seq)
        {
            int dv1 = Mod10Banri(seq);
            int dv2 = Mod11Banri(seq, dv1);// O módulo 11 sempre devolve os dois Dígitos, pois, as vezes, o dígito calculado no mod10 será incrementado em 1
            return dv2.ToString("00");
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.NossoNumero.Length == 10)
            {
                boleto.NossoNumero = boleto.NossoNumero.Substring(0, 8) + "-" + boleto.NossoNumero.Substring(8, 2);
            }
            else
            {
                throw new Exception("Erro ao tentar formatar nosso número, verifique o tamanho do campo");
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função do fomata número do documento não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1 CCCCC.CCNNNd2 NNNNN.041XXd3 V FFFF9999999999
            //OU 
            //041M2.1AAAd1 ACCCCC.CCNNd2 NNNNN.N40XXd3 V FFFF9999999999
            //(Isso depende da constante que usar) no caso de cima "041" no de baixo "40" antes do "XX"

            string Campo1 = string.Empty;
            string Campo2 = string.Empty;
            string Campo3 = string.Empty;
            string Campo4 = string.Empty;
            string Campo5 = string.Empty;

            string Cedente = boleto.Cedente.Codigo.Substring(4); //Os quatro primeiros digitos do código do cedente é sempre a agência
            string NossoNumero = boleto.NossoNumero.Substring(0, 8);

            //Campo 1
            string M = boleto.Moeda.ToString();
            string AAA = boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3);
            string Metade1 = "041" + M + "2";
            string Metade2 = "1" + AAA;
            string d1 = Mod10Banri(Metade1 + Metade2).ToString();
            Campo1 = Metade1 + "." + Metade2 + d1;

            //Campo 2
            Metade1 = string.Empty;
            Metade2 = string.Empty;
            Metade1 = Cedente.Substring(0, 5);
            //Metade2 = Cedente.Substring(5, 2) + NossoNumero.Substring(0, 2);
            Metade2 = Cedente.Substring(5, 2) + NossoNumero.Substring(0, 3);
            string d2 = Mod10Banri(Metade1 + Metade2).ToString();
            Campo2 = Metade1 + "." + Metade2 + d2;

            //Campo 3
            Metade1 = string.Empty;
            Metade2 = string.Empty;
            string XX = _primDigito.ToString() + _segDigito.ToString();
            //Metade1 = NossoNumero.Substring(2, 5);
            Metade1 = NossoNumero.Substring(3, 5);
            //Metade2 = NossoNumero.Substring(7, 1) + "041" + XX;
            Metade2 = "041" + XX;
            string d3 = Mod10Banri(Metade1 + Metade2).ToString();
            Campo3 = Metade1 + "." + Metade2 + d3;

            //Campo 4
            Campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            //Campo 5
            string fatorVenc = FatorVencimento(boleto).ToString("0000");
            string valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valor = Utils.FormatCode(valor, 10);
            Campo5 = fatorVenc + valor;

            boleto.CodigoBarra.LinhaDigitavel = Campo1 + "  " + Campo2 + "  " + Campo3 + "  " + Campo4 + "  " + Campo5;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            string campo1 = string.Empty;
            string campo2 = string.Empty;
            string campoLivre = string.Empty;
            campo1 = "041" + boleto.Moeda.ToString();
            int dacCodBarras;
            string fatorVenc = FatorVencimento(boleto).ToString("0000");
            string valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
            valor = Utils.FormatCode(valor, 10);
            campo2 = fatorVenc + valor;

            string nossoNumero = boleto.NossoNumero.Replace(".", "").Replace("-", "");
            nossoNumero = nossoNumero.Substring(0, 8);
            //campoLivre = "21" + boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3) + boleto.Cedente.ContaBancaria.Conta + nossoNumero + "041";
            string codCedente = boleto.Cedente.Codigo.Substring(4, 7);// Os quatro primeiros digitos do código do cedente é sempre a agência
            campoLivre = "21" + boleto.Cedente.ContaBancaria.Agencia.Substring(1, 3) + codCedente + nossoNumero + "041";
            string ncCodBarra = CalcularNCCodBarras(campoLivre);
            Int32.TryParse(ncCodBarra.Substring(0, 1), out _primDigito);
            Int32.TryParse(ncCodBarra.Substring(1, 1), out _segDigito);
            campoLivre = campoLivre + ncCodBarra;

            dacCodBarras = Mod11Peso2a9Banri(campo1 + campo2 + campoLivre);

            boleto.CodigoBarra.Codigo = campo1 + dacCodBarras.ToString() + campo2 + campoLivre;
        }

        private int Mod10Banri(string seq)
        {
            /* (N1*1-9) + (N2*2-9) + (N3*1-9) + (N4*2-9) + (N5*1-9) + (N6*2-9) + (N7*1-9) + (N8*2-9)
             * Observação:
             * a) a subtração do 9 somente será feita se o produto obtido da multiplicação individual for maior do que 9. 
             * b) quando o somatório for menor que 10, o resto da divisão por 10 será o próprio somatório. 
             * c) quando o resto for 0, o primeiro DV é igual a 0.
             */
            int soma = 0, resto, dv1, peso = 2, n, result;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                result = n * peso > 9 ? (n * peso) - 9 : n * peso;
                soma += result;
                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            if (soma < 10)
                resto = soma;
            else
                resto = soma % 10;
            dv1 = resto == 0 ? 0 : 10 - resto;
            return dv1;
        }

        private int Mod11Banri(string seq, int dv1)
        {
            /* Obter somatório (peso de 2 a 7), sempre da direita para a esquerda (N1*4)+(N2*3)+(N3*2)+(N4*7)+(N5*6)+(N6*5)+(N7*4)+(N8*3)+(N9*2)
             * Caso o somatório obtido seja menor que "11", considerar como resto da divisão o próprio somatório.
             * Caso o ''resto'' obtido no cálculo do módulo ''11'' seja igual a ''1'', considera-se o DV inválido. 
             * Soma-se, então, "1" ao DV obtido do módulo "10" e refaz-se o cálculo do módulo 11 . 
             * Se o dígito obtido pelo módulo 10 era igual a "9", considera-se então (9+1=10) DV inválido. 
             * Neste caso, o DV do módulo "10" automaticamente será igual a "0" e procede-se assim novo cálculo pelo módulo "11". 
             * Caso o ''resto'' obtido no cálculo do módulo "11" seja ''0'', o segundo ''NC'' será igual ao próprio ''resto''
             */
            int peso = 2, mult, sum = 0, rest, dv2, b = 7, n;
            seq += dv1.ToString();
            bool dvInvalido;
            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                mult = n * peso;
                sum += mult;
                if (peso < b)
                    peso++;
                else
                    peso = 2;
            }
            seq = seq.Substring(0, seq.Length - 1);
            rest = sum < 11 ? sum : sum % 11;
            if (rest == 1)
                dvInvalido = true;
            else
                dvInvalido = false;

            if (dvInvalido)
            {
                int novoDv1 = dv1 == 9 ? 0 : dv1 + 1;
                dv2 = Mod11Banri(seq, novoDv1);
            }
            else
            {
                dv2 = rest == 0 ? 0 : 11 - rest;
            }
            if (!dvInvalido)
            {
                string digitos = dv1.ToString() + dv2;
                return Convert.ToInt32(digitos);
            }
            else
            {
                return dv2;
            }
        }

        private int Mod11BaseIndef(string seq, int b)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2;


            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(seq, i, 1)) * p);  // s = s + (Convert.ToInt32(seq.Mid(i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            d = 11 - (s % 11);


            if ((d > 9) || (d == 0) || (d == 1))
                d = 1;

            return d;
        }

        private int Mod11Peso2a9Banri(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             * n - Numero (string convertida)
             */

            int d, r, s = 0, p = 2, b = 9, n;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));

                s = s + (n * p);

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            r = s % 11;

            if (r == 0 || r == 1 || r > 9)
                d = 1;
            else
                d = 11 - r;

            return d;
        }

        private int CalculaSoma(string Numero)
        {
            int mult;
            int x;
            int y;
            int resul;
            int resto;
            int soma;
            soma = 0;
            mult = 2;
            int I = Numero.Length;
            //para começar o cálculo pelo nº final (sempre começa multiplicando por 2)
            for (x = 1; x <= Numero.Length; x++)
            {
                if (Codigo == 41)
                {
                    //Banrisul só vai até 7
                    if (mult == 8)
                        mult = 2;
                }
                else
                {
                    if (mult == 10)
                        mult = 2;
                }
                y = Convert.ToInt32(Strings.Mid(Numero, I, 1));
                resul = y * mult;
                soma = soma + resul;
                mult = mult + 1;
                I = I - 1;
            }
            if (Codigo == 41 | Codigo == 33 | Codigo == 353)
            {
                return soma;
                // calcula no retorno pois tem umas exceções
            }
            else
            {
                resto = soma % 11;
                if (resto == 0)
                    resto = 1;
                return resto;
            }
        }
        #endregion

        #region Métodos de geração do arquivo de remessa CNAB400

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //            
            switch (tipoArquivo)
            {
                case TipoArquivo.CNAB400:
                    vRetorno = ValidarRemessaCNAB400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                string _header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        _header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.CNAB400:
                        _header = GerarHeaderRemessaCNAB400(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.CNAB400:
                        _detalhe = GerarDetalheRemessaCNAB400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                string _trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.CNAB240:
                        _trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.CNAB400:
                        _trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return _trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }
        #endregion

        #region Métodos de geração do arquivo de remessa CNAB240

        #region HEADER

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                string header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.CNAB240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public string GerarHeaderRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia ?? " ";
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? " " : cedente.ContaBancaria.DigitoConta[0].ToString();
                var agEConta = ConfiguraDigitoAgenciaConta(cedente);

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                                      //Codigo do banco                           001-003
                header += "0000";                                                                                       //Lote de serviço                           004-007
                header += "0";                                                                                          //Tipo de registro                          008-008
                header += Utils.FormatCode("", " ", 9);                                                                 //Resevado (uso Banco)                      009-017
                header += cedente.CPFCNPJ.Length == 11 ? "1" : "2";                                                     //Tipo inscrição empresa                    018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 14, true);                                             //Nº inscrição empresa                      019-032
                header += Utils.FormatCode(cedente.Codigo, "0", 20, true);                                              //Codigo transmissão adquirido pelo banco   033-052 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);                                //Agencia                                   055-059
                header += Utils.FormatCode(dvAgencia, "0", 1);                                                          //DV Agência do cedente                     060-061                     
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);                                 //Numero da conta corrente do cedente       061-072       
                header += Utils.FormatCode(dvConta, "0", 1);                                                            //DV Numero da conta corrente do cedente    073-073
                header += Utils.FormatCode(agEConta, "0", 1);                                                           //Dígito verificador da Ag / Conta          074-074
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);                       //Nome do cedente                           074-103
                header += Utils.FormatCode("BANRISUL", " ", 30);                                                        //Nome do Banco                             104-133
                header += Utils.FormatCode("", " ", 10);                                                                //Resevado (uso Banco)                      134-143
                header += "1";                                                                                          //Código remessa = 1                        144-144
                header += DateTime.Now.ToString("ddMMyyyy");                                                            //Data geração do arquivo DDMMAAAA          145-152
                header += Utils.FormatCode("", " ", 6);                                                                 //Resevado (uso Banco)                      153-158
                header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 6, 6, '0', 0, true, true, true);       //Nº sequencial do arquivo                  159-164
                header += "040";                                                                                        //Nº da versão do layout                    165-167
                header += Utils.FormatCode("", " ", 74);                                                                //Resevado (uso Banco)                      168-240

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private static string ConfiguraDigitoAgenciaConta(Cedente cedente)
        {
            var agEConta = " ";
            if (cedente.ContaBancaria.DigitoConta != null)
            {
                if (cedente.ContaBancaria.DigitoConta.Length > 1)
                    agEConta = cedente.ContaBancaria.DigitoConta[1].ToString();
                else
                    agEConta = cedente.ContaBancaria.DigitoConta[0].ToString();
            }

            return agEConta;
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var dvAgencia = cedente.ContaBancaria.DigitoAgencia ?? " ";
                var dvConta = cedente.ContaBancaria.DigitoConta == null ? " " : cedente.ContaBancaria.DigitoConta[0].ToString();
                var agEConta = ConfiguraDigitoAgenciaConta(cedente);

                string header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                  //Codigo do Banco                               001-003
                header += "0001";                                                                   //Numero lote Remessa                           004-007
                header += "1";                                                                      //Tipo de registro                              008-008
                header += "R";                                                                      //Tipo de operacao (R - Remessa)                009-009
                header += "01";                                                                     //Tipo de serviço - 01 (Cobrança)               010-011
                header += "00";                                                                     //Resevado (uso Banco)                          012-013
                header += "020";                                                                    //Nº versão layout do lote                      014-016
                header += " ";                                                                      //Resevado (uso Banco)                          017-017
                header += cedente.CPFCNPJ.Length == 11 ? "1" : "2";                                 //Tipo inscrição empresa 1 = CPF, 2 = CNPJ      018-018
                header += Utils.FormatCode(cedente.CPFCNPJ, "0", 15, true);                         //Nº inscrição empresa                          019-032
                header += Utils.FormatCode(cedente.Codigo, "0", 20, true);                          //Resevado (uso Banco)                          034-053
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);            //Agencia                                       054-058
                header += Utils.FormatCode(dvAgencia, "0", 1);                                      //DV Agência do cedente                         059-059                     
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);             //Numero da conta corrente do cedente           060-071       
                header += Utils.FormatCode(dvConta, "0", 1);                                        //DV Numero da conta corrente do cedente        072-072
                header += Utils.FormatCode(agEConta, "0", 1);                                       //Dígito verificador da Ag / Conta              073-073
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);   //Nome do cedente                               074-103
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 1                                    104-143
                header += Utils.FormatCode("", " ", 40);                                            //Mensagem 2                                    144-183
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 8, true);          //Número remessa                                184-191
                header += DateTime.Now.ToString("ddMMyyyy");                                        //Data de gravação da remessa  DDMMAAAA         192-199   
                header += Utils.FormatCode("", " ", 41);                                            //Resevado (uso Banco)                          200-240
                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion

        #region DETALHE
        public string GerarDetalheRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string codigoTransmissao)
        {
            try
            {
                var dvAgencia = boleto.Cedente.ContaBancaria.DigitoAgencia ?? " ";
                var dvConta = boleto.Cedente.ContaBancaria.DigitoConta == null ? " " : boleto.Cedente.ContaBancaria.DigitoConta[0].ToString();
                var agEConta = ConfiguraDigitoAgenciaConta(boleto.Cedente);

                string _segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                                                             //1-3
                _segmentoP += "00013";                                                                                                      //4-8
                _segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);                             //8-8
                _segmentoP += "P ";                                                                                                         //14-12
                _segmentoP += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);                               //16-17
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);                  //18-22
                _segmentoP += Utils.FitStringLength(dvAgencia, 1, 1, '0', 0, true, true, true);                                             //23-23
                _segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 12, 12, '0', 0, true, true, true);                  //24-35
                _segmentoP += Utils.FitStringLength(dvConta, 1, 1, '0', 0, true, true, true);                                               //36-36
                _segmentoP += Utils.FitStringLength(agEConta, 1, 1, '0', 0, true, true, true);                                              //37-37
                _segmentoP += Utils.FitStringLength(boleto.NossoNumero.Replace("-", ""), 20, 20, '0', 0, true, true, true);                 //38-57
                _segmentoP += boleto.Carteira[1].ToString();                                                                                //58-58
                _segmentoP += "1"; //Cobrança registrada                                                                                    //59-59
                _segmentoP += "1"; //Tipo de documento 1=Tradicional 2=Escritural                                                           //60-60
                _segmentoP += "2 ";                                                                                                         //61-62
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);                             //63-77
                _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);           //78-85
                _segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);//86-100
                _segmentoP += "00000 ";                                                                                                     //101-106
                _segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo.ToString(), 2, 2, '0', 0, true, true, true);             //107-108
                _segmentoP += "N";                                                                                                          //109-109
                _segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);            //110-117
                #region Juros e Desconto

                var valorJuros = 0M;
                if (boleto.PercJurosMora > Convert.ToDecimal(0.00))
                    valorJuros = Math.Round(boleto.ValorBoleto * (boleto.PercJurosMora / 100), 2);

                if (valorJuros > 0)
                {
                    _segmentoP += "1";                                                                                                      //118-118
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);       //119-126
                    var valorJurosArredondado = valorJuros.ToString("0.00");
                    _segmentoP += Utils.FitStringLength(valorJurosArredondado.Replace(",", ""), 15, 15, '0', 0, true, true, true);          //127-141
                }
                else if (boleto.JurosPermanente)
                {
                    _segmentoP += "1";                                                                                                      //118-118
                    _segmentoP += Utils.FormatCode("", "0", 8);                                                                             //119-126
                    _segmentoP += Utils.FormatCode("", "0", 15);                                                                            //127-141
                }
                else
                {
                    _segmentoP += "3";                                                                                                      //118-118
                    _segmentoP += Utils.FormatCode("", "0", 8);                                                                             //119-126
                    _segmentoP += Utils.FormatCode("", "0", 15);                                                                            //127-141
                }

                if (boleto.ValorDesconto > 0)
                {
                    _segmentoP += "1";                                                                                                      //142-142
                    _segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);       //143-150
                    _segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ToString("0.00").Replace(",", ""), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    _segmentoP += "0";                                                                                                      //142-142
                    _segmentoP += Utils.FormatCode("", "0", 8);                                                                             //143-150
                    _segmentoP += Utils.FormatCode("", "0", 15);                                                                            //151-165
                }
                #endregion

                _segmentoP += Utils.FormatCode(boleto.IOF.ToString().Replace(",", "").Replace(".", ""), "0", 15);                           //166-180
                _segmentoP += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 15);                    //181-195
                _segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);                             //196-220

                _segmentoP += "3";                                                                                                          //221-221
                _segmentoP += "00";                                                                                                         //222-223
                _segmentoP += "3";                                                                                                          //224-224
                _segmentoP += "000";                                                                                                        //225-227
                _segmentoP += "09";                                                                                                         //228-229
                _segmentoP += "           ";                                                                                                //229-240

                _segmentoP = Utils.SubstituiCaracteresEspeciais(_segmentoP);

                return _segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoQ;

                _segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoQ += "00013";
                _segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoQ += "Q ";
                _segmentoQ += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);

                if (boleto.Sacado.CPFCNPJ.Length <= 11)
                    _segmentoQ += "1";
                else
                    _segmentoQ += "2";

                _segmentoQ += Utils.FitStringLength(boleto.Sacado.CPFCNPJ, 15, 15, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.CEP, 8, 8, ' ', 0, true, true, false).ToUpper(); ;
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.UF, 2, 2, ' ', 0, true, true, false).ToUpper();
                _segmentoQ += Utils.FitStringLength("", 16, 16, '0', 0, true, true, true);
                _segmentoQ += Utils.FitStringLength("", 40, 40, ' ', 0, true, true, true);
                _segmentoQ += "000000000000";
                _segmentoQ += Utils.FitStringLength("", 19, 19, ' ', 0, true, true, true);

                _segmentoQ = Utils.SubstituiCaracteresEspeciais(_segmentoQ);

                return _segmentoQ;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _brancos151 = new string(' ', 151);

                string _segmentoR;

                _segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoR += "00013";
                _segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoR += "R ";
                _segmentoR += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                // Desconto 2
                _segmentoR += "000000000000000000000000"; //24 zeros
                // Reservado (uso do Banco)
                _segmentoR += "                        "; //24 brancos
                #region Multa
                if (boleto.ValorMulta > 0)
                {
                    _segmentoR += "1";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FitStringLength(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), 15, 15, '0', 0, true, true, true); // Valor/Percentual a Ser Aplicado
                }
                else if (boleto.PercMulta > 0)
                {
                    _segmentoR += "2";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += boleto.DataVencimento.ToString("ddMMyyyy");                               // Data da Multa 
                    _segmentoR += Utils.FitStringLength(boleto.PercMulta.ToString().Replace(",", "").Replace(".", ""), 15, 15, '0', 0, true, true, true); // Valor/Percentual a Ser Aplicado
                }
                else
                {
                    _segmentoR += "0";                                                                      // Código da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa 
                    _segmentoR += Utils.FormatCode("", "0", 8);                                             // Data da Multa 
                    _segmentoR += Utils.FormatCode("", "0", 15);                                            // Valor/Percentual a Ser Aplicado
                }
                #endregion
                _segmentoR += _brancos151;

                _segmentoR = Utils.SubstituiCaracteresEspeciais(_segmentoR);

                return _segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                string _segmentoS;

                _segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                _segmentoS += "00013";
                _segmentoS += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                _segmentoS += "S ";
                _segmentoS += Utils.FitStringLength(boleto.CodigoOcorrencia, 2, 2, '0', 0, true, true, true);
                _segmentoS += "2";

                if (boleto.Instrucoes.Count > 0)
                {
                    var instrucoes = boleto.Instrucoes[0].Descricao.Replace("<br/>", " ");
                    _segmentoS += Utils.FitStringLength(instrucoes, 200, 200, ' ', 0, true, true, false);
                }
                else
                    _segmentoS += Utils.FitStringLength("", 200, 200, ' ', 0, true, true, false);

                _segmentoS += Utils.FitStringLength("", 22, 22, ' ', 0, true, true, false);

                _segmentoS = Utils.SubstituiCaracteresEspeciais(_segmentoS);

                return _segmentoS;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }
        #endregion

        #region TRAILER
        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += Utils.FormatCode("0001", "0", 4, true);
                trailer += "5";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);
                trailer += Utils.FormatCode("", " ", 217);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                trailer += "9999";
                trailer += "9";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += "000001";
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);
                trailer += Utils.FormatCode("", " ", 211);
                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        #endregion

        #endregion

        #region CNAB 400 - sidneiklein
        public bool ValidarRemessaCNAB400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            bool vRetorno = true;
            string vMsg = string.Empty;
            //
            #region Pré Validações
            if (banco == null)
            {
                vMsg += String.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += String.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += String.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }
            #endregion
            //
            foreach (Boleto boleto in boletos)
            {
                #region Validação de cada boleto
                if (boleto.Remessa == null)
                {
                    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    #region Validações da Remessa que deverão estar preenchidas quando BANRISUL
                    //Comentado porque ainda está fixado em 01
                    //if (String.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                    //{
                    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código de Ocorrência!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    if (String.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    {
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else if (boleto.Remessa.TipoDocumento.Equals("06") && !String.IsNullOrEmpty(boleto.NossoNumero))
                    {
                        //Para o "Remessa.TipoDocumento = "06", não poderá ter NossoNumero Gerado!
                        vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Não pode existir NossoNumero para o Tipo Documento '06 - cobrança escritural'!", Environment.NewLine);
                        vRetorno = false;
                    }

                    //Para o Tipo
                    #endregion
                }
                #endregion
            }
            //
            mensagem = vMsg;
            return vRetorno;
        }
        public string GerarHeaderRemessaCNAB400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 010, 0, "01REMESSA", ' ')); //001-009
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0010, 016, 0, "", ' ')); //010-026
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0027, 013, 0, cedente.Codigo, ' ')); //027-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0040, 007, 0, "", ' ')); //040-046
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' ')); //047-076
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0077, 011, 0, "041BANRISUL", ' ')); //077-087
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0088, 007, 0, "", ' ')); //088-094
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' ')); //095-100
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0101, 009, 0, "", ' ')); //101-109
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0110, 004, 0, "", ' ')); //110-113
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0114, 001, 0, "", ' ')); //114-114
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0115, 001, 0, "", ' ')); //115-115
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0116, 001, 0, "", ' ')); //116/116
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, "", ' ')); //117-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 268, 0, "", ' ')); //126-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, "000001", ' ')); //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _header = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }
        public string GerarDetalheRemessaCNAB400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Variáveis Locais a serem Implementadas em nível de Config do Boleto...
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ bANRISUL
                //
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                //
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "1", ' '));                                       //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 016, 0, string.Empty, ' '));                              //002-017
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0018, 013, 0, boleto.Cedente.Codigo, ' '));                     //018-030 (sidnei.klein 22/11/2013: No Banrisul, o Código do Cedente não é a concatenação de Número da Conta com o Dígito Verificador.)
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0031, 007, 0, string.Empty, ' '));                              //031-037
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0038, 025, 0, boleto.NumeroDocumento, ' '));                    //038-062
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0063, 010, 0, boleto.NossoNumero, '0'));                        //063-072
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0073, 032, 0, string.Empty, ' '));                              //073-104
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0105, 003, 0, string.Empty, ' '));                              //105-107
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0108, 001, 0, "1", ' '));                                       //108-108   //COBRANÇA SIMPLES
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110   //REMESSA
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                    //111-120   
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139   //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0140, 003, 0, "041", ' '));                                     //140-142
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0143, 005, 0, string.Empty, ' '));                              //143-147
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0148, 002, 0, boleto.Remessa.TipoDocumento, ' '));              //148-149
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediDataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                  //151-156
                //
                #region Instruções
                string vQtdeDiasCodigo_9_ou_15 = "0";
                //
                string vInstrucao1 = string.Empty;
                string vInstrucao2 = string.Empty;
                switch (boleto.Instrucoes.Count)
                {
                    case 1:
                        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                        vInstrucao2 = string.Empty;
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[0].QuantidadeDias.ToString();
                        //
                        break;
                    case 2:
                        vInstrucao1 += boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[0].QuantidadeDias.ToString();
                        //
                        vInstrucao2 += boleto.Instrucoes[1].Codigo.ToString().PadLeft(2, '0');
                        //valida se é código 9 ou 15, para adicionar os dias na posição 370-371
                        if (boleto.Instrucoes[1].Codigo == 9 || boleto.Instrucoes[1].Codigo == 15)
                            vQtdeDiasCodigo_9_ou_15 = boleto.Instrucoes[1].QuantidadeDias.ToString();
                        //
                        break;
                }
                #endregion
                //
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0157, 002, 0, vInstrucao1, ' '));                               //157-158
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0159, 002, 0, vInstrucao2, ' '));                               //159-160
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0161, 001, 0, "0", ' '));                                       //161-161

                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0162, 012, 2, boleto.JurosMora, '0'));                          //162-173
                #region DataDesconto
                string vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0193, 013, 2, boleto.IOF, '0'));                                //193-205
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218
                #region Regra Tipo de Inscrição Sacado
                string vCpfCnpjSac = "99";
                if (boleto.Sacado.CPFCNPJ.Length.Equals(11)) vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CPFCNPJ.Length.Equals(14)) vCpfCnpjSac = "02"; //Cnpj é sempre 14;
                #endregion
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0219, 002, 0, vCpfCnpjSac, '0'));                               //219-220
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CPFCNPJ, '0'));                     //221-234
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0235, 035, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-269
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0270, 005, 0, string.Empty, ' '));                              //270-274
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0315, 007, 0, string.Empty, ' '));                              //315-321
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0322, 003, 0, 0, '0'));                                         //322-324
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0325, 002, 0, 0, '0'));                                         //325-326
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.CEP, '0'));                //327-334
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.UF.ToUpper(), ' '));       //350-351
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0352, 004, 1, 0, '0'));                                         //352-355
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0356, 001, 0, string.Empty, ' '));                              //356-356
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0357, 013, 2, 0, '0'));                                         //357-369
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0370, 002, 0, vQtdeDiasCodigo_9_ou_15, '0'));                   //370-371
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0372, 023, 0, string.Empty, ' '));                              //372-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400
                //

                //
                reg.CodificarLinha();
                //
                string _detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
                //
                return _detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }
        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                TRegistroEDI reg = new TRegistroEDI();
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 026, 0, string.Empty, ' '));   //002-027
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0028, 013, 2, vltitulostotal, '0')); //027-039
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0041, 354, 0, string.Empty, ' '));   //040-394
                reg.CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediNumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
                //
                reg.CodificarLinha();
                //
                string vLinha = reg.LinhaRegistro;
                string _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                TRegistroEDI_Banrisul_Retorno reg = new TRegistroEDI_Banrisul_Retorno();
                //
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                DetalheRetorno detalhe = new DetalheRetorno(registro);
                //
                //detalhe. = Constante1;
                detalhe.CodigoInscricao = Utils.ToInt32(reg.TipoInscricao);
                detalhe.NumeroInscricao = reg.CpfCnpj;
                //detalhe.Agencia = Utils.ToInt32(reg.CodigoCedente.Substring(0, 3));
                //detalhe.Conta = Utils.ToInt32(reg.CodigoCedente.Substring(4, 7));
                //detalhe.DACConta = Utils.ToInt32(reg.CodigoCedente.Substring(36, 1));

                //detalhe. = reg.EspecieCobrancaRegistrada;
                //detalhe. = reg.Branco1;
                detalhe.NumeroControle = reg.IdentificacaoTituloCedente;
                detalhe.IdentificacaoTitulo = reg.IdentificacaoTituloBanco_NossoNumero;
                //detalhe. = reg.IdentificacaoTituloBanco_NossoNumeroOpcional;
                //detalhe. = reg.NumeroContratoBLU;
                //detalhe. = reg.Brancos2;
                //detalhe. = reg.TipoCarteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.CodigoOcorrencia);
                //
                int dataOcorrencia = Utils.ToInt32(reg.DataOcorrenciaBanco);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                //
                detalhe.NumeroDocumento = reg.SeuNumero;
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV
                //
                int dataVencimento = Utils.ToInt32(reg.DataVencimentoTitulo);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                //
                decimal valorTitulo = Convert.ToInt64(reg.ValorTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoCobrador);
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.CodigoAgenciaCobradora);
                //
                //detalhe.Especie = reg.TipoDocumento; //Verificar Espécie de Documentos...
                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa)
                decimal valorDespesa = Convert.ToUInt64(reg.ValorDespesasCobranca);
                detalhe.ValorDespesa = valorDespesa / 100;
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                decimal valorOutrasDespesas = Convert.ToUInt64(reg.OutrasDespesas);
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                //detalhe. = reg.Zeros1;
                //detalhe. = reg.ValorAvista;
                //detalhe. = reg.SituacaoIOF;
                //detalhe. = reg.Zeros2;

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.ValorAbatimento_DeflacaoConcedido);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.ValorDescontoConcedido);
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorPago);
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.ValorJuros);
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(reg.ValorOutrosRecebimentos);
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //detalhe. = reg.Brancos3;
                int dataCredito = Utils.ToInt32(reg.DataCreditoConta);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //detalhe. = reg.Brancos4;
                detalhe.OrigemPagamento = reg.PagamentoDinheiro_Cheque;
                //detalhe. = reg.Brancos5;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //detalhe. = reg.Brancos6;
                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequenciaRegistro);
                //
                //
                #region NAO RETORNADOS PELO BANRISUL
                detalhe.IOF = 0;
                //Motivos das Rejeições para os Códigos de Ocorrência
                detalhe.MotivosRejeicao = string.Empty;
                //Número do Cartório
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;
                //Nome do Sacado
                detalhe.NomeSacado = "";
                #endregion

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }


        #endregion
    }
}