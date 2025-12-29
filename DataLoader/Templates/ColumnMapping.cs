using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader.Templates
{
    public class ColumnMapping
    {
        public string ParamName { get; set; }
        public string Name { get; set; }
        public string XPath { get; set; }
        public string SqlDataType { get; set; }
        public string DefaultValue { get; set; }
    }
}