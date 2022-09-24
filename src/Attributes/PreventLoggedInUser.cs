using System;

namespace UsersAuthExample.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PreventLoggedInUser : Attribute
    {
        public bool FetchFromRoutes { get; set; }
    }
}
