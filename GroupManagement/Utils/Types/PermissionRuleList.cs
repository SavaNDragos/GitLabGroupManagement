using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitLabGroupManagement.Utils.Types
{
    public class PermissionRuleList
    {
        public int AccessLevel { get; set; } = 10;
        public List<string> Users { get; set; } = new List<string>();

        public PermissionRuleList(int inAccessLevel)
        {
            AccessLevel = inAccessLevel;
        }

        public void Add(string inName)
        {
            if (!Users.Any(obj => obj == inName))
            {
                Users.Add(inName.ToLower());
            }
        }

        public void Add(List<string> inList)
        {
            foreach (var iterUser in inList)
            {
                Add(iterUser);
            }
        }

        public bool IsUserPresent(string inName)
        {
            return Users.Any(obj => obj == inName);
        }

        public List<string> UsersThatArePresent(List<string> inUsers)
        {
            var forReturnUsers = new List<string>();
            foreach (var inUser in inUsers)
            {
                if (Users.Contains(inUser.ToLower()))
                {
                    forReturnUsers.Add(inUser);
                }
            }
            return forReturnUsers;
        }
    }


    public class PermissionRuleListPerGroup
    {
        //we use this order to work with the major to the trivial one
        public static int[] AccessLevels = {50, 40, 30, 20, 10};
        public List<PermissionRuleList> PermissionRuleLists { get; set; }
        /// <summary>
        /// For quick access of users that we have
        /// </summary>
        public List<string> Users { get; set; } = new List<string>();

        public void InitializeList()
        {
            PermissionRuleLists = new List<PermissionRuleList>
            {
                new PermissionRuleList(10),
                new PermissionRuleList(20),
                new PermissionRuleList(30),
                new PermissionRuleList(40),
                new PermissionRuleList(50)
            };
        }

        public int GetAccessLevelForUser(string inUser)
        {
            foreach (var accessLevel in AccessLevels)
            {
                if (PermissionRuleLists.FirstOrDefault(obj => obj.AccessLevel == accessLevel).IsUserPresent(inUser))
                {
                    return accessLevel;
                }
            }
            throw new Exception(string.Format("We had an issue trying to find user {0} in the permissionRuleLists",
                inUser));
        }

        public PermissionRuleListPerGroup(List<PermissionRule> inPermissionRules, List<UserCollection> inUserCollections)
        {
            InitializeList();

            //take all groups and strip them down to users only
            var collectionsNames = inUserCollections.Select(obj => obj.Name);
            List<int> passedAccessLevels = new List<int>();

            foreach (var iterAccessLevel in AccessLevels)
            {
                passedAccessLevels.Add(iterAccessLevel);
                foreach (var iterPermissionRule in inPermissionRules)
                {
                    if (iterPermissionRule.AccessLevel == iterAccessLevel)
                    {
                        var tempUsersInvestigated = new List<string>();
                        if (collectionsNames.Any(obj => obj.ToUpper() == iterPermissionRule.Target.ToUpper()))
                        {
                            var tempCollection = inUserCollections.FirstOrDefault(
                                obj => obj.Name.ToUpper() == iterPermissionRule.Target.ToUpper());
                            if (tempCollection == null)
                            {
                                throw new Exception(
                                    string.Format("There was an issue trying to retrieve userCollection {0} ",
                                        iterPermissionRule.Target));
                            }
                            tempUsersInvestigated.AddRange(tempCollection.GetAllUsers(new List<string>()));
                        }
                        else
                        {
                            tempUsersInvestigated.Add(iterPermissionRule.Target);
                        }

                        //see if it was used before
                        foreach (var iterpassedAccessLevel in passedAccessLevels)
                        {
                            var tempPermissionRuleList =
                                PermissionRuleLists.FirstOrDefault(obj => obj.AccessLevel == iterpassedAccessLevel);
                            var tempUsersPresent = tempPermissionRuleList.UsersThatArePresent(tempUsersInvestigated);
                            //remove those users that are allready discovered
                            tempUsersInvestigated.RemoveAll(obj => tempUsersPresent.Contains(obj));
                        }
                        var tempPermissionCurrent =
                            PermissionRuleLists.FirstOrDefault(obj => obj.AccessLevel == iterAccessLevel);
                        tempPermissionCurrent.Add(tempUsersInvestigated);
                    }
                }
            }

            foreach (var accessLevel in AccessLevels)
            {
                Users.AddRange(PermissionRuleLists.FirstOrDefault(obj => obj.AccessLevel == accessLevel).Users);
            }
        }
    }
}
