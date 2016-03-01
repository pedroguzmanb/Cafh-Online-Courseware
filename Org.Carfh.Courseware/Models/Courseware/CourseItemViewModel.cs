using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lang;
using Org.Carfh.Courseware.Models.Repositories;

namespace Org.Carfh.Courseware.Models.Courseware
{

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS COURSEWARE ITEM VIEW MODEL                                                                                                     //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class CourseItemViewModel
    {
        public Guid Id { set; get; }

        [Required]
        [Display(ResourceType = typeof (Global), Name = "Course")]
        public Guid Course { set; get; }

        [Display(ResourceType = typeof (Global), Name = "Created")]
        public DateTime Created { set; get; }

        [Display(ResourceType = typeof (Global), Name = "Version")]
        public int? Version { set; get; }

        [Display(ResourceType = typeof (Global), Name = "Revision")]
        public int? Revision { set; get; }
        public int Chapter { set; get; }

        [Display(ResourceType = typeof(Global), Name = "LastModified")]
        public DateTime LastModified { set; get; }

        [Required]
        [Display(ResourceType = typeof (Global), Name = "Title")]
        public string Title { set; get; }

        [Required]
        [AllowHtml]

        public string Content { set; get; }


        public static CourseItemViewModel Transform(CourseItem item)
        {
            return new CourseItemViewModel
            {
                Id = item.Id,Chapter = item.Chapter,
                Content = item.Content,
                Course = item.Course,
                Created = item.Created,
                LastModified = item.LastModified,
                Revision = item.Revision,
                Title = item.Title,
                Version = item.Version 
            };
        }

        public static CourseItem Transform(CourseItemViewModel item)
        {
            return new CourseItem
            {
                Id = item.Id,
                Chapter = item.Chapter,
                Content = item.Content,
                Course = item.Course,
                Created = item.Created,
                LastModified = item.LastModified,
                Revision = item.Revision,
                Title = item.Title,
                Version = item.Version
            };
        }

    } // CLASS COURSE ITEM VIEW MODEL ENDS ------------------------------------------------------------------------------------------------ //
}
