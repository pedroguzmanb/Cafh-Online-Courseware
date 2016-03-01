using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Org.Carfh.Courseware.Models
{
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS APPLICATION ROLE                                                                                                               //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name, string description)
            : base(name)
        {
            Description = description;
        }
        public virtual string Description { get; set; }
    } // CLASS APPPLIATION ROLE ENDS ------------------------------------------------------------------------------------------------------ //


    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS APPLICATION USER ROLE                                                                                                          //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole() : base() { }
        public ApplicationRole Role { get; set; }
    } // CLASS APPLICATION USER ROLE ENDS ------------------------------------------------------------------------------------------------- //
}
