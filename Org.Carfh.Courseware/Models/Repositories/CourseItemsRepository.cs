using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Cafh.Courseware.Models.Repositories
{
    public class CourseItemsRepository
    {
        private CoursewareDataContext _context;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CourseItemsRepository(ref CoursewareDataContext context)
        {
            _context = context ?? new CoursewareDataContext();
        }

        // CONSTRUCTOR ENDS ------------------------------------------------------------------------------------------------------------- //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseItem"></param>
        /// <returns></returns>
        public CourseItem Post(CourseItem courseItem)
        {
            try
            {
                courseItem.Id = Guid.NewGuid();
                courseItem.Created = DateTime.Now;
                courseItem.LastModified = DateTime.Now;
                courseItem.Revision = 0;
                courseItem.Version = 1;
                _context.CourseItems.InsertOnSubmit(courseItem);
                _context.SubmitChanges();
            } // TRY ENDS
            catch (Exception e)
            {
                courseItem = null;
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return courseItem;
        } // METHOD POST ENDS ------------------------------------------------------------------------------------------------------------- //


        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseItem"></param>
        /// <returns></returns>
        public CourseItem Put(CourseItem courseItem)
        {
            try
            {
                var course1 = courseItem;
                var query = from x in _context.CourseItems
                            where x.Id == course1.Id && x.Course == course1.Course
                            select x;

                foreach (var item in query)
                {
                    item.Content = courseItem.Content;
                    item.Title = courseItem.Title;
                    item.LastModified = DateTime.Now;
                    item.Chapter = courseItem.Chapter;
                    item.Revision++;
                    if (item.Revision > 25)
                    {
                        item.Revision = 0;
                        item.Version++;
                    }
                    _context.SubmitChanges();
                }

            } // TRY ENDS
            catch (Exception e)
            {
                courseItem = null;
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return courseItem;
        } // METHOD PUT ENDS--------------------------------------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CourseItem> Get(Guid courseId)
        {
            var courseItems = new List<CourseItem>();
            try
            {
                var query = (from x in _context.CourseItems
                            where x.Course == courseId
                            select x).OrderByDescending(m=>m.Chapter);
                courseItems.AddRange(query);
            }
            catch (Exception e)
            {
                Trace.Fail(e.Message);
            }
            return courseItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseItem Single(Guid id)
        {
            var teaching = new CourseItem();
            try
            {
                var query = from x in _context.CourseItems
                             where x.Id == id
                             select x;
                foreach (var item in query)
                    teaching = item;
            }
            catch (Exception e)
            {
                teaching = null;
                Trace.Fail(e.Message);
            }
            return teaching;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id)
        {
            try
            {
                var query = from x in _context.CourseItems
                    where x.Id == id
                    select x;
                foreach (var item in query)
                {
                    _context.CourseItems.DeleteOnSubmit(item);
                    _context.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                Trace.Fail(e.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public int NextChapter(Guid course)
        {
            var highestChapter = 0;
            try
            {
                var query = from x in _context.CourseItems
                    where x.Course == course
                    select x;
                foreach (var item in query)
                {
                    if (item.Chapter > highestChapter)
                        highestChapter = item.Chapter;
                } // FOREACH ENDS
            } // TRY END
            catch (Exception e)
            {
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return ++highestChapter;
        }
    }
}
