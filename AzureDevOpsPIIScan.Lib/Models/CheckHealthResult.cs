using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsPIIScan.Lib.Models
{
    class CheckHealthResult
    {
        public string WorkItemField { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string NormalizedText { get; set; }
        public List<HealthDataSource> HealthDataSources { get; set; }
        public string AssertionAssociation { get; set; }
        public string AssertionCertainty { get; set; }
        public string AssertionConditionality { get; set; }

        public List<HealthcareRelation> HealthcareRelations { get; set; }


        public CheckHealthResult()
        {
            this.HealthDataSources = new List<HealthDataSource>();
            this.HealthcareRelations = new List<HealthcareRelation>();
        }
    }


}
