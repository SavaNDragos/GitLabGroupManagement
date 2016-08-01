using GitLabGroupManagement.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageGroups
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var allpermisions = new AllPermissions(@"D:\exPermissions.txt");

                var result = allpermisions.GetPermissionRuleListPerGroup(allpermisions.Groups.First());
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has been dedected.");
                Console.WriteLine(ex.InnerException);

            }
        }
    }
}
