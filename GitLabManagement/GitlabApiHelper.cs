using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GitLabManagement
{
    public class GitlabApiHelper
    {
        public string GitLabAddress { get; set; }
        public string PrivateToken { get; set; }
        private string GitLabApi => $"{GitLabAddress}/{"api/v3"}";

        private string ExecuteQuery(string inSpace, Dictionary<string, string> inSegments, RestSharp.Method inMethod)
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
                return response.Content;
            }
            catch (Exception)
            {

                throw new Exception("An exception was determine trying to obtain ");
            }
        }

        public void GetProjectGroupId(string inGroupName)
        {
            var tempParameters = new Dictionary<string, string> {{"search", inGroupName}};
            var result = ExecuteQuery("groups", tempParameters, Method.GET);
        }
    }
}
