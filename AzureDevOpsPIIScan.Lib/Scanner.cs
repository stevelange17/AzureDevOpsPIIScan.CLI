using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;
namespace AzureDevOpsPIIScan.Lib
{
    public static class Scanner
    {
        const string analyticsEndpoint = "<ANALYTICS_SERVICE_ENDPOINT>";
        const string analyticsKey = "<ANALYTICS_SERVICE_KEY>";

        public static async Task<string> RunWorkItemScan(string orgUrl, List<string> projectList, string personalAccessToken, string returnFormat, bool checkHealth)
        {
            Console.WriteLine("Connecting to Azure DevOps..");
            VssConnection connection = new VssConnection(new Uri(orgUrl), 
                new VssBasicCredential(string.Empty, personalAccessToken));

            string result = "none";

            if (projectList.Count == 0)
            {
                Console.WriteLine("Processing all accessible projects..");
                ProjectHttpClient projectClient = connection.GetClient<ProjectHttpClient>();
                IEnumerable<TeamProjectReference> projects = projectClient.GetProjects().Result;
                foreach (var project in projects)
                {
                    projectList.Add(project.Name);
                }
            }

            foreach (string project in projectList)
            {
                Console.WriteLine($"-- {project}: Beginning work item scan..");
                await WitScanner.Scan(orgUrl, project, personalAccessToken, returnFormat, checkHealth);
            }
            

            return result;
        }

    }
}
