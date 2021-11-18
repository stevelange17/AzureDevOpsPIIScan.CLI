using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsPIIScan.Lib.Models
{
    class CheckResultSummary
    {
        public string WorkItemId { get; set; }
        public string WorkItemTitle { get; set; }
        public List<CheckResult> CheckResults { get; set; }

        public bool HasHealthResults { get; set; }

        public List<CheckHealthResult> CheckHealthResults { get; set; }

        public CheckResultSummary()
        {
            CheckResults = new List<CheckResult>();
            CheckHealthResults = new List<CheckHealthResult>();
            HasHealthResults = false;

        }

    }
}
