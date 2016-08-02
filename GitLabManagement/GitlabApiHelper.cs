using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var tempParameters = new Dictionary<string, string>();
            var result = ExecuteQuery("users", tempParameters, Method.GET);
            var response2 = new JsonDeserializer().Deserialize<RootGitLabUsers>(result);
            return response2.Property1.ToList();
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
            //GET / groups /:id / members
            var tempParameters = new Dictionary<string, string>();
            var result = ExecuteQuery($"groups/{inGroupId}/members", tempParameters, Method.GET);

            //deserialize response
            return new JsonDeserializer().Deserialize<RootUserPerGroup>(result).Property1.ToList();

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

        public void UpdateGroup(string inGroupName)
        {
            //get group
            var tempGroup = GetProjectGroupId(inGroupName);
            var allUsers = GetAllUsersForGroup(tempGroup.id);

            //get all users
            var tempAllUsersOnServer = GetAllUsers();

            //foreach (var groupMemberOperation in newRights)
            //{
            //    var tempParameters = new Dictionary<string, string>();
            //    tempParameters.Add("user_id", GetUserId(tempAllUsersOnServer, groupMemberOperation.member).ToString());
            //    if (groupMemberOperation.OperationType != GroupMemberOperationType.Remove)
            //    {
            //        tempParameters.Add("access_level", groupMemberOperation.AccessLevel.ToString());
            //    }
            //}
        }
    }
}
