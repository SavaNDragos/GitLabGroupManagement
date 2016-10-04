using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GitLabManagement
{
    public class GroupMemberOperation
    {
        public int member_id { get; set; }
        public string member { get; set; }
        public GroupMemberOperationType OperationType { get; set; } = GroupMemberOperationType.Add;
        public int AccessLevel { get; set; } = 10;

        public Method HttpMethod
        {
            get
            {
                switch (OperationType)
                {
                    case GroupMemberOperationType.Add:
                        return Method.POST;
                    case GroupMemberOperationType.Remove:
                        return Method.DELETE;
                    case GroupMemberOperationType.Update:
                        return Method.PUT;
                    default:
                        throw new Exception("Something went wrong trying to get the http method we need to use.");
                }
            }
        }




    }
}
