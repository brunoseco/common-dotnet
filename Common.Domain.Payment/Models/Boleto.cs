using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Payment.Models
{
    public class Boleto
    {
        public string LinhaDigitavel { get; set; }
        public string DadosCodigoBarras { get; set; }
        public string CodigoBarras { get; set; }
    }
    
        
}
