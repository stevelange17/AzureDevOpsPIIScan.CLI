using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureDevOpsPIIScan.Lib;

namespace AzureDevOpsPIIScan.CLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string orgUrl = "https://dev.azure.com/<ORG_NAME>";
            string projectName = "<PROJECT_NAME>";
            string personalToken = "<PAT>";


            Console.Clear();
            Console.WriteLine("===============================================================================");
            Console.WriteLine("This simple app will do the following:");
            Console.WriteLine($"\tConnect to Azure DevOps at '{orgUrl}' using a Personal Access Token");
            Console.WriteLine("\tFor each project, get all work item types in use");
            Console.WriteLine("\tGet all work items and their text-based fields (that could contain PII/PHI)");
            //Console.WriteLine("\tUse Azure Text Analytics to identify potential PII/PHI");
            Console.WriteLine("===============================================================================\n\n");

            Console.WriteLine("Press ENTER to get started.");
            Console.ReadLine();

            List<string> projects = new List<string>();
            projects.Add(projectName); // This is currently ignored. Scanner tried to loop through every accessible project 

            await Scanner.RunWorkItemScan(orgUrl, projects, personalToken, "json", false);

            Console.WriteLine("Done! Press ENTER to quit.");
            Console.ReadLine();
        }
    }
}
