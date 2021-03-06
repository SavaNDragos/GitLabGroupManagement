﻿using System;
using System.Linq;
using GitLabGroupManagement.Utils.Types;
using GitLabManagement;

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
                var gitLabApiHelper = new GitlabApiHelper()
                {
                    GitLabAddress = options.GitAddress,
                    PrivateToken = options.PrivateToken,
                };

                //pass on each group and apply changes
                foreach (var iterGroup in allpermisions.Groups)
                {
                    Console.WriteLine($"Looking over group {iterGroup.Name} to apply permissions.");
                    gitLabApiHelper.UpdateGroup(iterGroup, allpermisions,options.SkipDelete);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has been dedected with error message {ex.Message}.");
                throw ex;
            }
        }
    }
}
