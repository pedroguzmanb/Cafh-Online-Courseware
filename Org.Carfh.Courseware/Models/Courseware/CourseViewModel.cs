using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lang;
using Org.Carfh.Courseware.Models.Repositories;

namespace Org.Carfh.Courseware.Models.Courseware
{
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS COURSE VIEW MODEL                                                                                                              //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class CourseViewModel
    {
        public Guid Id { set; get; }

        [Required]
        [Display(ResourceType = typeof (Global), Name = "Language")]
        public Guid Language { set; get; }

        [Required]
        [Display(ResourceType = typeof (Global), Name = "Title")]
        public string Title { set; get; }

        [Required]
        [Display(ResourceType = typeof (Global), Name = "Description")]
        public string Description { set; get; }

        public static CourseViewModel Transform(Course item)
        {
            return new CourseViewModel
            {
                Id = item.Id,
                Description = item.Description, 
                Language = item.Language, 
                Title = item.Title
            };
        }

        public static Course Transform(CourseViewModel item)
        {
            return new Course
            {
                Id = item.Id,
                Description = item.Description,
                Language = item.Language,
                Title = item.Title
            };
        }

    } // CLASS COURSE VIEW MODEL ENDS ----------------------------------------------------------------------------------------------------- //
}
