using System.Collections.Generic;

namespace AzureDevOpsPIIScan.Lib.Models
{
    public class HealthcareRelation
    {
        public string RelationType { get; set; }
        public List<RelationRole> RelationRoles { get; set; }

        public HealthcareRelation()
        {
            this.RelationRoles = new List<RelationRole>();
        }

    }
}