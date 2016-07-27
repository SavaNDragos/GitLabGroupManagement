using System;
using System.Collections.Generic;
using System.Linq;

namespace GitLabGroupManagement.Utils.Types
{
    public class UserCollection
    {
        public string Name { get; set; }
        public List<string> Users { get; set; } = new List<string>();
        public List<UserCollection> Collections = new List<UserCollection>();

        public List<string> GetAllUsers()
        {
            var retrunUsers = new List<string>();
            var allUsers = Collections.Select(obj => obj.GetAllUsers());
            foreach (var allUser in allUsers)
            {
                retrunUsers.AddRange(allUser);
            }
            retrunUsers.AddRange(Users);
            return retrunUsers;
        }

        public UserCollection(string inName)
        {
            Name = inName;
        }

        public void Add(string inUser)
        {
            if (Users.Contains(inUser))
            {
                Console.WriteLine("We have dedected that user {0} was allready added to the group {1}.", inUser, Name);
            }
            else
            {
                Users.Add(inUser);
            }
        }

        public void TransformToGroup(string inName, UserCollection inUserCollection)
        {
            if (inUserCollection.Name == Name)
            {
                throw new Exception(
                    string.Format(
                        "An error was dedected when trying to transform user {0} to a group as part of user collection {1}. There is not allowed to have a user collection have a self reference. ",
                        inName, Name));
            }
            if (!Users.Remove(inName))
            {
                throw new Exception(
                    string.Format(
                        "An error was dedected when trying to transform user {0} to a group as part of user collection {1}. The user could not be removed.",
                        inName, Name));
            }
            Collections.Add(inUserCollection);
        }
    }
}
