using System;
using System.Collections.Generic;
using System.Linq;

namespace GitLabGroupManagement.Utils.Types
{
    public class AllPermissions
    {
        public List<UserCollection> UserCollections { get; set; } = new List<UserCollection>();
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<Project> Projects { get; set; } = new List<Project>();

        public AllPermissions()
        {
            
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
