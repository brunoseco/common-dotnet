using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Nfe.Layout
{
    internal class FieldConfig
    {
        public int Length { get; set; }
        public char PaddingChar { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; } 
    }
}
