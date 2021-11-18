using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsPIIScan.Lib.Models
{
    class CheckResult
    {
        public string WorkItemField { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public CheckResult()
        {
            this.SubCategory = "none";
        }
    }

   
}
