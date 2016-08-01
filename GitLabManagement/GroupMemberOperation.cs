using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLabManagement
{
    public class GroupMemberOperation
    {
        public string member { get; set; }
        public GroupMemberOperationType OperationType { get; set; } = GroupMemberOperationType.Add;
        public int AccessLevel { get; set; } = 10;
    }
}
