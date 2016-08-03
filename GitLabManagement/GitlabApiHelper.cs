using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitLabGroupManagement.Utils.Types;
using RestSharp;
using RestSharp.Deserializers;

namespace GitLabManagement
{
    public class GitlabApiHelper
    {
        public string GitLabAddress { get; set; }
        public string PrivateToken { get; set; }
        private string GitLabApi => $"{GitLabAddress}/{"api/v3"}";
        
        private IRestResponse ExecuteQuery(string inSpace, Dictionary<string, string> inSegments, RestSharp.Method inMethod)
        {
            try
            {
                var client = new RestClient(GitLabApi);

                var request = new RestRequest(inSpace, inMethod);
                request.AddParameter("private_token", PrivateToken);
                foreach (var inSegment in inSegments)
                {
                    request.AddUrlSegment(inSegment.Key, inSegment.Value);
                }

                var response = client.Execute(request);
                return response;
            }
            catch (Exception)
            {

                throw new Exception("An exception was determine trying to obtain ");
            }
        }

        private List<GitLabUsers> GetAllUsers()
        {
            var returnListOfUsers = new List<GitLabUsers>();
            int pageNumber = 0;
            bool ContinueSearching = true;

            while (ContinueSearching)
            {
                var tempParameters = new Dictionary<string, string>
                {
                    {"per_page", "100"},
                    {"page", pageNumber.ToString()}
                };

                var result = ExecuteQuery("users", tempParameters, Method.GET);
                var tempDeserializedResponse  =  new JsonDeserializer().Deserialize<RootGitLabUsers>(result).Property1.ToList();
                if (!tempDeserializedResponse.Any())
                {
                    ContinueSearching = false;
                    continue;
                }
                returnListOfUsers.AddRange(tempDeserializedResponse);
                pageNumber++;
            }

            return returnListOfUsers;
        }

        public GitLabGroup GetProjectGroupId(string inGroupName)
        {
            var returnGroupId = 0;
            var tempParameters = new Dictionary<string, string> {{"search", inGroupName}};
            var result = ExecuteQuery("groups", tempParameters, Method.GET);

            //deserialize response
            var groupList = new JsonDeserializer().Deserialize<RootGitLabGroup>(result).Property1.ToList();
            var group = groupList.FirstOrDefault(obj => obj.name.ToUpper() == inGroupName.ToUpper());
            if (group == null)
            {
                throw new Exception(string.Format("Group {0} was not found on gitlab server {1}", inGroupName,
                    GitLabAddress));
            }
            return group;
        }

        public List<UserPerGroup> GetAllUsersForGroup(int inGroupId)
        {
            var returnListOfUsers = new List<UserPerGroup>();
            int pageNumber = 0;
            bool ContinueSearching = true;

            while (ContinueSearching)
            {
                var tempParameters = new Dictionary<string, string>
                {
                    {"per_page", "100"},
                    {"page", pageNumber.ToString()}
                };

                var result = ExecuteQuery($"groups/{inGroupId}/members", tempParameters, Method.GET);

                //deserialize response
                var tempDeserializedResponse =
                    new JsonDeserializer().Deserialize<RootUserPerGroup>(result).Property1.ToList();
                if (!tempDeserializedResponse.Any())
                {
                    ContinueSearching = false;
                    continue;
                }
                returnListOfUsers.AddRange(tempDeserializedResponse);
                pageNumber++;
            }
            return returnListOfUsers;
        }

        private int GetUserId(List<GitLabUsers> inLabUsers ,string inUserName)
        {
            try
            {
                return inLabUsers.FirstOrDefault(obj => obj.username.ToUpper() == inUserName.ToUpper()).id;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        "User {0} could not be found in the list of users available on the gitlab instance {1}",
                        inUserName, GitLabAddress));
            }
        }

        public void UpdateGroup(string inGroupName, PermissionRuleListPerGroup inListWeNeedToApply)
        {
            //get group
            var tempGroup = GetProjectGroupId(inGroupName);
            var allUsers = GetAllUsersForGroup(tempGroup.id);

            //get all users
            var tempAllUsersOnServer = GetAllUsers();

            //create list of operations we need to complete
            var Operations = new List<GroupMemberOperation>();

            //first; get users we need to delete
            foreach (var userPerGroup in allUsers)
            {
                if (!inListWeNeedToApply.Users.Contains(userPerGroup.username.ToLower()))
                {
                    Operations.Add(new GroupMemberOperation()
                    {
                        member_id = userPerGroup.id,
                        OperationType = GroupMemberOperationType.Remove,
                    });
                }
            }

            //second; get users we need to add
            foreach (var user in inListWeNeedToApply.Users)
            {
                if (!allUsers.Any(obj => obj.username.ToLower() == user))
                {
                    Operations.Add(new GroupMemberOperation()
                    {
                        member_id = GetUserId(tempAllUsersOnServer, user),
                        OperationType = GroupMemberOperationType.Add,
                        AccessLevel = inListWeNeedToApply.GetAccessLevelForUser(user),
                    });
                }
            }

            //third; update rights for some users
            foreach (var userPerGroup in allUsers)
            {
                if (inListWeNeedToApply.Users.Contains(userPerGroup.username.ToLower()))
                {
                    if (userPerGroup.access_level !=
                        inListWeNeedToApply.GetAccessLevelForUser(userPerGroup.username.ToLower()))
                    {
                        Operations.Add(new GroupMemberOperation()
                        {
                            member_id = userPerGroup.id,
                            OperationType = GroupMemberOperationType.Update,
                            AccessLevel = inListWeNeedToApply.GetAccessLevelForUser(userPerGroup.username.ToLower()),
                        });
                    }
                }
            }

            //complete operations
            foreach (var groupMemberOperation in Operations)
            {
                var tempParameters = new Dictionary<string, string>
                {
                    {"user_id", groupMemberOperation.member_id.ToString()}
                };
                if (groupMemberOperation.OperationType != GroupMemberOperationType.Remove)
                {
                    tempParameters.Add("access_level", groupMemberOperation.AccessLevel.ToString());
                }
                var result = ExecuteQuery($"groups/{tempGroup.id}/members", tempParameters, groupMemberOperation.HttpMethod);
            }
        }
    }
}
