using System.Collections.Generic;

namespace GitLabGroupManagement.Utils.Types
{
    public class Project
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public List<PermissionRule> Permissions { get; set; } = new List<PermissionRule>();

        public Project(string inNameWithGroup)
        {
            var splitedNameFromGroup = inNameWithGroup.Split(new[] {'\\'});
            Name = splitedNameFromGroup[0];
            GroupName = splitedNameFromGroup[1];
            FullName = inNameWithGroup;
        }

        public void Add(string inLine)
        {
            Permissions.Add(new PermissionRule(inLine));
        }

        public PermissionRuleListPerGroup GetPermissionRuleListPerGroup(List<UserCollection> inUserCollections)
        {
            return new PermissionRuleListPerGroup(Permissions, inUserCollections);
        }
    }
}
