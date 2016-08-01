using System;
using System.Linq;
using GitLabGroupManagement.Utils.Types;

namespace ManageGroups
{
    class Program
    {
        private static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                throw new Exception("Incorect usage of command line parameters. Please review.");
            }

            try
            {
                var allpermisions = new AllPermissions(options.FileContentLocation);
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
