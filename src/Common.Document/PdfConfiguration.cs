using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Document
{
    public class PdfConfiguration
    {
        public bool landscape { get; set; }
        public string htmlStringWithPageNumbers { get; set; }
        public bool? useWebScale { get; set; }
        public int? height { get; set; }
        public int? width { get; set; }
        public bool? boletoCarne { get; set; }
    }
}
