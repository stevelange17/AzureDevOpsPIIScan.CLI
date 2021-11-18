using AzureDevOpsPIIScan.Lib.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsPIIScan.Lib
{
    class WitScanner
    {
        const bool _loadfromFile = false;


        public static async Task Scan(string orgUrl, string project, string personalAccessToken, string returnFormat, bool checkHealth)
        {
            VssConnection connection = new VssConnection(new Uri(orgUrl), new VssBasicCredential(string.Empty, personalAccessToken));

            // Create instance of WorkItemTrackingHttpClient using VssConnection
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            List<WitDefinition> witList;

            
            if (_loadfromFile)
            {
                witList = LoadWitListFromFile();
            }
            else
            {
                Console.WriteLine("Getting work item types and their text-based fields (this may take a couple minutes)..");
                witList = GetWorkItemTypes(witClient, project);

                // Save to Json file (for future use, possibly)
                SaveWitList(witList);
            }

            CheckResultSummary summary = new CheckResultSummary();

            foreach (WitDefinition wd in witList)
            {
                IList<WorkItem> items = await ProcessWitForProject(witClient, project, wd);
                foreach (WorkItem item in items)
                {
                    
                }
            }
        }

        

        private static async Task<IList<WorkItem>> ProcessWitForProject(WorkItemTrackingHttpClient witClient, string project, WitDefinition wd)
        {
            Console.WriteLine($"Processing work items of type '{wd.Name}'..");
            var workItems = await QueryWorkItemsByWitAsync(witClient, project, wd).ConfigureAwait(true);

            Console.WriteLine(".. Query Results: {0} items found", workItems.Count);

            return workItems;
        }

      
        public static async Task<IList<WorkItem>> QueryWorkItemsByWitAsync(WorkItemTrackingHttpClient witClient, string project, WitDefinition wd, bool batchByDate = false)
        {
            // create a wiql object and build our query
            var wiql = new Wiql();
            if (batchByDate)
            {
                // Build a WIQL for last 5 days, then iterate from there
                wiql = new Wiql()
                {
                    // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                    Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [System.TeamProject] = '" + project + "' " +
                        "And [Work Item Type] = '" + wd.Name + "' " +
                        "And [Changed] = '" + wd.Name + "' " +
                        "Order By [Id] Asc",
                };
            }
            else
            {
                wiql = new Wiql()
                {
                    // NOTE: Even if other columns are specified, only the ID & URL are available in the WorkItemReference
                    Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [System.TeamProject] = '" + project + "' " +
                        "And [Work Item Type] = '" + wd.Name + "' " +
                        "Order By [Id] Asc",
                };
            }
            
           
            // execute the query to get the list of work items in the results
            var result = await witClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
            var ids = result.WorkItems.Select(item => item.Id).ToArray();

            // some error handling
            if (ids.Length == 0)
            {
                return Array.Empty<WorkItem>();
            }

            // build a list of the fields we want to see
            var fields = new[] { "System.Id", "System.WorkItemType", "System.Title" };
            foreach (WitFieldDefinition field in wd.Fields)
            {
                fields.Append(field.FieldReferenceName);
            }

            // get work items for the ids found in query
            return await witClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);

        }

        #region WIT Definition Extraction
        private static List<WitDefinition> LoadWitListFromFile()
        {
            string jsonString = System.IO.File.ReadAllText("witlist.json");
           
            List<WitDefinition> list = JsonConvert.DeserializeObject<List<WitDefinition>>(jsonString);
            return list;
        }

        private static void SaveWitList(List<WitDefinition> witList)
        {
            Console.WriteLine("Saving WIT list to file..");
            string jsonString = JsonConvert.SerializeObject(witList);
            System.IO.File.WriteAllText("witlist.json", jsonString);
        }

        private static List<WitDefinition> GetWorkItemTypes(WorkItemTrackingHttpClient witClient, string project)
        {
            List<WitDefinition> list = new List<WitDefinition>();

            List<WorkItemType> witTypes = witClient.GetWorkItemTypesAsync(project).Result;

            foreach (WorkItemType item in witTypes)
            {
                Console.Write($"..{item.Name}\n\t");
                WitDefinition wd = new WitDefinition
                {
                    Name = item.Name,
                    ReferenceName = item.ReferenceName
                };
                foreach (var itemField in item.Fields)
                {
                    WorkItemField fld = witClient.GetFieldAsync(itemField.Name).Result;
                    if ((fld.Type == FieldType.Html || fld.Type == FieldType.PlainText
                        || fld.Type == FieldType.String) && !fld.IsPicklist && !fld.IsIdentity &&
                        !fld.IsDeleted && !fld.ReadOnly && !fld.Name.Contains("Iteration") && !fld.Name.Contains("Area") &&
                            !fld.Name.Contains("Team Project") && !fld.Name.Contains("Tags"))
                    {
                        Console.Write($"{fld.Name}, ");
                        wd.Fields.Add(new WitFieldDefinition
                        {
                            FieldName = fld.Name,
                            FieldReferenceName = fld.ReferenceName
                        });
                    }
                }
                Console.WriteLine("");
                list.Add(wd);

            }

            return list;

        }

        #endregion

    }
}
