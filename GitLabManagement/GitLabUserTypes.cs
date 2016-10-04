using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLabManagement
{

    public class RootGitLabUsers
    {
        public GitLabUsers[] Property1 { get; set; }
    }

    public class GitLabUsers
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string avatar_url { get; set; }
        public string web_url { get; set; }
        public DateTime created_at { get; set; }
        public bool is_admin { get; set; }
        public object bio { get; set; }
        public object location { get; set; }
        public string skype { get; set; }
        public string linkedin { get; set; }
        public string twitter { get; set; }
        public string website_url { get; set; }
        public DateTime? last_sign_in_at { get; set; }
        public DateTime confirmed_at { get; set; }
        public int theme_id { get; set; }
        public int color_scheme_id { get; set; }
        public int projects_limit { get; set; }
        public DateTime current_sign_in_at { get; set; }
        public Identity[] identities { get; set; }
        public bool can_create_group { get; set; }
        public bool can_create_project { get; set; }
        public bool two_factor_enabled { get; set; }
        public bool external { get; set; }
    }

    public class Identity
    {
        public string provider { get; set; }
        public string extern_uid { get; set; }
    }


    public class RootGitLabGroup
    {
        public GitLabGroup[] Property1 { get; set; }
    }

    public class GitLabGroup
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string description { get; set; }
        public int visibility_level { get; set; }
        public string avatar_url { get; set; }
        public string web_url { get; set; }
    }


    public class RootUserPerGroup
    {
        public UserPerGroup[] Property1 { get; set; }
    }

    public class UserPerGroup
    {
        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public DateTime created_at { get; set; }
        public int access_level { get; set; }
    }


}
