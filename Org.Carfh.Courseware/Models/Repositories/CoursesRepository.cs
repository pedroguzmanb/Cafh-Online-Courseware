using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Cafh.Courseware.Models.Repositories
{

    

    public class CoursesRepository
    {
        private CoursewareDataContext _context;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CoursesRepository(ref CoursewareDataContext context)
        {
            _context = context ?? new CoursewareDataContext();
        }

        // CONSTRUCTOR ENDS ------------------------------------------------------------------------------------------------------------- //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course Post(Course course)
        {
            try
            {
                course.Id = new Guid();
                course.Created = DateTime.Now;
                _context.Courses.InsertOnSubmit(course);
                _context.SubmitChanges();
            } // TRY ENDS
            catch (Exception e)
            {
                course = null;
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return course;
        } // METHOD POST ENDS ------------------------------------------------------------------------------------------------------------- //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course Put(Course course)
        {
            try
            {
                var course1 = course;
                var query = from x in _context.Courses
                    where x.Id == course1.Id
                    select x;
                foreach (var item in query)
                {
                    item.Description = course.Description;
                    item.Language = course.Language;
                    item.Title = course.Title;
                    _context.SubmitChanges();
                }
                
            } // TRY ENDS
            catch (Exception e)
            {
                course = null;
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return course;
        } // METHOD PUT ENDS--------------------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Course> Get(Guid language)
        {
            var courses = new List<Course>();
            try
            {
                var query = from x in _context.Courses
                            where x.Language == language
                    select x;
                foreach(var item in query)
                    courses.Add(item);
            }
            catch (Exception e)
            {
                Trace.Fail(e.Message);
            }
            return courses;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course Single(Guid id)
        {
            var courses = new Course();
            try
            {
                var query = from x in _context.Courses
                            where x.Id == id
                            select x;
                foreach (var item in query)
                    courses = item;
            }
            catch (Exception e)
            {
                courses = null;
                Trace.Fail(e.Message);
            }
            return courses;
        }

    }
}
