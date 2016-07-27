using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GitLabGroupManagement.Utils;
using GitLabGroupManagement.Utils.Types;

namespace GitLabGroupManagement.ConfigExtract
{
    public static  class ExtractData
    {
        public static AllPermissions GetAllPermissions(string inFileLocation)
        {
            var allPermissions = new AllPermissions();

            //read file 
            var allpermissionsContent = File.ReadAllLines(inFileLocation);
            var currentProcessing = ProcessingStatus.IsCollection;
            var currentName = string.Empty;

            foreach (var iterLine in allpermissionsContent)
            {
                if (!string.IsNullOrWhiteSpace(iterLine))
                {
                    if (iterLine.StartsWith("Collection: "))
                    {
                        currentName = iterLine.Replace("Collection: ", "").Replace(" ", "");
                        allPermissions.AddUserCollection(currentName);
                        currentProcessing = ProcessingStatus.IsCollection;
                    }

                    else if (iterLine.StartsWith("Group: "))
                    {
                        currentName = iterLine.Replace("Group: ", "").Replace(" ", "");
                        allPermissions.AddGroup(currentName);
                        currentProcessing = ProcessingStatus.IsGroup;
                    }
                    else if (iterLine.StartsWith("Project: "))
                    {
                        currentName = iterLine.Replace("Project: ", "").Replace(" ", "");
                        allPermissions.AddProject(currentName);
                        currentProcessing = ProcessingStatus.IsProject;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(iterLine)) continue;

                        if (currentName == string.Empty)
                        {
                            Console.WriteLine(
                                "Please keep the file clean. Remove any extra line from the start of the file till a collection/group/project appears.");
                        }

                        allPermissions.AddElement(currentName, currentProcessing, iterLine.Replace(" ", ""));
                    }
                }
            }

            //take all groups and strip them down to users only
            var collectionsNames = allPermissions.UserCollections.Select(obj => obj.Name);
            foreach (var iterUserCollections in allPermissions.UserCollections)
            {
                
                foreach (var iterUser in iterUserCollections.Users)
                {
                    if (collectionsNames.Contains(iterUser))
                    {
                        iterUserCollections.TransformToGroup(iterUser,
                            allPermissions.UserCollections.FirstOrDefault(obj => obj.Name == iterUser));
                    }
                }
            }


            return allPermissions;
        }
    }
}
