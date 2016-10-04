using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitLabGroupManagement.Utils.Types
{
    public class AllPermissions
    {
        public List<UserCollection> UserCollections { get; set; } = new List<UserCollection>();
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<Project> Projects { get; set; } = new List<Project>();


        /// <summary>
        /// Using a file specified it will go and prepare the list of user that shouild be applied on groups and projects.
        /// </summary>
        /// <param name="inFileLocation">Location for the file that contains the information we need to parse.</param>
        public AllPermissions(string inFileLocation)
        {
            //read file 
            var allpermissionsContent = File.ReadAllLines(inFileLocation);
            var currentProcessing = ProcessingStatus.IsCollection;
            var currentName = string.Empty;

            //we parse the file line by line
            foreach (var iterLine in allpermissionsContent)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(iterLine))
                    {
                        if (iterLine.StartsWith("Collection: "))
                        {
                            currentName = iterLine.Replace("Collection: ", "").Replace(" ", "");
                            AddUserCollection(currentName);
                            currentProcessing = ProcessingStatus.IsCollection;
                        }

                        else if (iterLine.StartsWith("Group: "))
                        {
                            currentName = iterLine.Replace("Group: ", "").Replace(" ", "");
                            AddGroup(currentName);
                            currentProcessing = ProcessingStatus.IsGroup;
                        }
                        else if (iterLine.StartsWith("Project: "))
                        {
                            currentName = iterLine.Replace("Project: ", "").Replace(" ", "");
                            AddProject(currentName);
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

                            AddElement(currentName, currentProcessing, iterLine.Replace(" ", ""));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception was detected while going over line {iterLine}");
                    throw ex;
                }
            }

            //take all groups and strip them down to users or collections only
            var collectionsNames = UserCollections.Select(obj => obj.Name.ToLower());
            foreach (var iterUserCollections in UserCollections)
            {
                var tempUsersThatAreCollections = new List<string>();
                foreach (var iterUser in iterUserCollections.Users)
                {
                    if (collectionsNames.Contains(iterUser))
                    {
                        tempUsersThatAreCollections.Add(iterUser);
                    }
                }

                foreach (var tempUsersThatAreCollection in tempUsersThatAreCollections)
                {
                    iterUserCollections.TransformToGroup(tempUsersThatAreCollection,
                            UserCollections.FirstOrDefault(obj => obj.Name.ToLower() == tempUsersThatAreCollection));
                }
            }
        }

        public PermissionRuleListPerGroup GetPermissionRuleListPerGroup(Group inGroup)
        {
            return inGroup.GetPermissionRuleListPerGroup(UserCollections);
        }

        public PermissionRuleListPerGroup GetPermissionRuleListPerGroup(Project inProject)
        {
            return inProject.GetPermissionRuleListPerGroup(UserCollections);
        }

        /// <summary>
        /// Add a new UserCollection item  the the list of UserCollections. 
        /// </summary>
        /// <param name="inName">The name of the new Collection we want to add.</param>
        public void AddUserCollection(string inName)
        {
            if (UserCollections.Exists(obj => obj.Name == inName))
            {
                throw new Exception(
                    $"We have dedected that user collection {inName} was allready defined. Please remove duplicate.");
            }
            UserCollections.Add(new UserCollection(inName));
        }

        public void AddGroup(string inName)
        {
            if (Groups.Exists(obj => obj.Name == inName))
            {
                throw new Exception(
                    $"We have dedected that group {inName} was allready defined. Please remove duplicate.");
            }
            Groups.Add(new Group(inName));
        }

        public void AddProject(string inName)
        {
            if (Projects.Exists(obj => obj.FullName == inName))
            {
                throw new Exception(
                    $"We have dedected that group {inName} was allready defined. Please remove duplicate.");
            }
            Projects.Add(new Project(inName));
        }

        public void AddElement(string inElementName, ProcessingStatus inProcessingStatus, string inLine)
        {
            switch (inProcessingStatus)
            {
                case ProcessingStatus.IsCollection:
                    var tempUserCollection = UserCollections.FirstOrDefault(obj => obj.Name == inElementName);
                    tempUserCollection.Add(inLine);
                    break;
                case ProcessingStatus.IsGroup:
                    var tempGroup = Groups.FirstOrDefault(obj => obj.Name == inElementName);
                    tempGroup.Add(inLine);
                    break;
                case ProcessingStatus.IsProject:
                    var tempProject = Projects.FirstOrDefault(obj => obj.FullName == inElementName);
                    tempProject.Add(inLine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inProcessingStatus), inProcessingStatus, null);
            }
        }
    }
}
