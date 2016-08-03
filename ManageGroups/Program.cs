using System;
using System.Linq;
using GitLabGroupManagement.Utils.Types;
using GitLabManagement;

namespace ManageGroups
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                throw new Exception("Incorect usage of command line parameters. Please review.");
            }

            //for debug testing 
            //options.GitAddress = "";
            //options.PrivateToken = "";
            //options.FileContentLocation = "D:\\filetest.txt";

            try
            {
                var allpermisions = new AllPermissions(options.FileContentLocation);
                var gitLabApiHelper = new GitlabApiHelper()
                {
                    GitLabAddress = options.GitAddress,
                    PrivateToken = options.PrivateToken,
                };

                //pass on each group and apply changes
                foreach (var iterGroup in allpermisions.Groups)
                {
                    gitLabApiHelper.UpdateGroup(iterGroup, allpermisions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has been dedected.");
                Console.WriteLine(ex.InnerException);
            }
        }
    }
}
