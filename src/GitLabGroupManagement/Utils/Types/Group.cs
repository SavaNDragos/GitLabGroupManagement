using System.Collections.Generic;

namespace GitLabGroupManagement.Utils.Types
{
    public class Group
    {
        public string Name { get; set; }
        public List<PermissionRule> Permissions { get; set; } = new List<PermissionRule>();

        public Group(string inName)
        {
            Name = inName;
        }

        public void Add(string inLine)
        {
            Permissions.Add(new PermissionRule(inLine));
        }
    }
}
