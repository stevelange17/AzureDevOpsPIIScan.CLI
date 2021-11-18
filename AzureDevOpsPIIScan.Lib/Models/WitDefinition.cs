using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevOpsPIIScan.Lib.Models
{
    class WitDefinition
    {
        public string Name { get; set; }
        public string ReferenceName { get; set; }
        public List<WitFieldDefinition> Fields { get; set; }


        public WitDefinition()
        {
            Fields = new List<WitFieldDefinition>();
        }
    }
}
