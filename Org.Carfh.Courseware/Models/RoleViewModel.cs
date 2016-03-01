using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Cafh.Courseware.Models
{
    public class RoleDto
    {
        public string Role { set; get; }
    }

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS ROLE VIEW MODEL                                                                                                                //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class RoleViewModel
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public RoleViewModel() { }
        public RoleViewModel(ApplicationRole role)
        {
            RoleName = role.Name;
            Description = role.Description;
        }
    } // CLASS ROLE VIEW MODEL ------------------------------------------------------------------------------------------------------------ //

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS SELECT ROLE EDITOR VIEW MODEL                                                                                                  //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }
        // Update this to accept an argument of type ApplicationRole:
        public SelectRoleEditorViewModel(ApplicationRole role)
        {
            RoleName = role.Name;
            // Assign the new Descrption property:
            Description = role.Description;
        }
        public bool Selected { get; set; }
        [Required]
        public string RoleName { get; set; }
        // Add the new Description property:
        public string Description { get; set; }
    } // CLASS SELECT ROLE EDITOR VIEW MODEL ENDS ----------------------------------------------------------------------------------------- //


    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // EDIT ROLE VIEW MODEL                                                                                                                 //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class EditRoleViewModel
    {
        public string OriginalRoleName { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public EditRoleViewModel() { }
        public EditRoleViewModel(ApplicationRole role)
        {
            OriginalRoleName = role.Name;
            RoleName = role.Name;
            Description = role.Description;
        }
    } // CLASS EDIT ROLE VIEW MODEL ------------------------------------------------------------------------------------------------------- //

}
