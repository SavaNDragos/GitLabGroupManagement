using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabGroupManagement.ConfigExtract;

namespace GitLabGroupManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var allpermisions = ExtractData.GetAllPermissions(@"D:\exPermissions.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has been dedected.");
                Console.WriteLine(ex.InnerException);
                
            }
        }
    }
}
