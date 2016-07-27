﻿using System;

namespace GitLabGroupManagement.Utils.Types
{
    public class PermissionRule
    {
        public string Target { get; set; }
        public int AccessLevel { get; set; } = 10;

        public PermissionRule(string inTarget)
        {
            if (!inTarget.Contains(":"))
            {
                Target = inTarget;
            }
            else
            {
                var tempTargetWithAccessLevel = inTarget.Split(new[] {':'});
                Target = tempTargetWithAccessLevel[0];
                try
                {
                    AccessLevel = Convert.ToInt32(tempTargetWithAccessLevel[1]);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("The access value should be a numerical value. The value we processed was: {0}", inTarget));
                }
            }
        }

        public PermissionRule(string inTarget, int inAccessLevel)
        {
            Target = inTarget;
            AccessLevel = inAccessLevel;
        }
    }
}
