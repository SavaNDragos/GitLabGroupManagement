using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace ManageGroups
{
    internal class CommandLineOptions
    {
        [Option('f', "fileLocation", Required = true,
            HelpText = "Location for the file that contains our group and policy definition.")]
        public string FileContentLocation { get; set; }

        [Option('g', "gitAddress", Required = false,
            HelpText = "Addresss of the gitlab instance we want to work with.")]
        public string GitAddress { get; set; }

        [Option('t', "privatetoken", Required = false,
            HelpText = "Value of the private token we need to do operations on gitlab.")]
        public string PrivateToken { get; set; }

        [Option('s', "skipDelete", Required = false, DefaultValue = false,
    HelpText = "State if we are going to delete user permissions.")]
        public bool SkipDelete { get; set; }
    }
}
