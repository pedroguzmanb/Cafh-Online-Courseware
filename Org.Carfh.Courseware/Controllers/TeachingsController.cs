using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lang;
using Org.Carfh.Courseware.Models.Courseware;
using Org.Carfh.Courseware.Models.Repositories;

namespace Org.Carfh.Courseware.Controllers
{
    public class TeachingsController : Controller
    {

        private static CoursewareDataContext _context = new CoursewareDataContext();
        private static readonly LanguagesRepository _langs = new LanguagesRepository(ref _context);
        private static readonly CoursesRepository _courses = new CoursesRepository(ref _context);
        private static readonly  CourseItemsRepository _teachings = new CourseItemsRepository(ref _context);


        // GET: Teachings
        public ActionResult Index(Guid course)
        {
            var crs = _courses.Single(course);
            ViewBag.Lang = crs.Language;
            ViewBag.Course = course;
            var teachings = _teachings.Get(course).Select(CourseItemViewModel.Transform).ToList();
            teachings.Reverse();
            return View(teachings);
        }


        public ActionResult Read(Guid id)
        {
            var teaching = CourseItemViewModel.Transform(_teachings.Single(id));
            return View(teaching);
        }

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION CREATE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public ActionResult Create(Guid course)
        {
            var teaching = new CourseItemViewModel
            {
                Course = course,
                Id = Guid.NewGuid(),
                Chapter = _teachings.NextChapter(course)
            };
            return View(teaching);
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION CREATE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseItemViewModel model)
        {
            ActionResult result = View(model);
            if (ModelState.IsValid)
            {
                var teaching = _teachings.Post(CourseItemViewModel.Transform(model));
                if (teaching != null)
                {
                    result =  RedirectToAction("Read", new {id = teaching.Id});
                } // IF ENDS
                else
                {
                    ModelState.AddModelError("", Global.ErrorProcessingRequest);
                    result = View(model);
                } // ELSE ENDS
            } // IF ENDS
            else
            {
                result = View(model);
            }
            return result;
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION DELETE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            return RedirectToAction("Index");
        }

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION EDIT                                                                                                                      //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var teaching = CourseItemViewModel.Transform(_teachings.Single(id));
            return View(teaching);
        }


        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION CREATE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseItemViewModel model)
        {
            ActionResult result = View(model);
            if (ModelState.IsValid)
            {
                var teaching = _teachings.Put(CourseItemViewModel.Transform(model));
                if (teaching != null)
                {
                    result = RedirectToAction("Read", new { id = teaching.Id });
                } // IF ENDS
                else
                {
                    ModelState.AddModelError("", Global.ErrorProcessingRequest);
                    result = View(model);
                } // ELSE ENDS
            } // IF ENDS
            else
            {
                result = View(model);
            }
            return result;
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //


    }
}