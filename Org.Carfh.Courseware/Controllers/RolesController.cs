using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Org.Cafh.Courseware.Models;

namespace Org.Cafh.Courseware.Controllers
{
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS ROLES CONTROLLER                                                                                                               //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class RolesController : Controller
    {

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // CLASS PRIVATE ATTRIBUTES                                                                                                         //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;

        public RolesController()
        {
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
        }

        public RolesController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
        }

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION INDEX                                                                                                                     //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var rolesList = _db.Roles.ToList().Select(item => new RoleViewModel(item)).ToList();
            return View(rolesList);
        } // METHOD INDEX ENDS ------------------------------------------------------------------------------------------------------------ //

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION CREATE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult Create(string message = "")
        {
            ViewBag.Message = message;
            return View();
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION CREATE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create([Bind(Include =
            "RoleName,Description")]RoleViewModel model)
        {
            string message = "El rol especificado ya existe en el sistema";
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(model.RoleName, model.Description);
                if (_db.RoleExists(_roleManager, model.RoleName))
                {
                    return View(message);
                }
                else
                {
                    _db.CreateRole(_roleManager, model.RoleName, model.Description);
                    return RedirectToAction("Index", "Roles");
                }
            }
            return View();
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // EDIT                                                                                                                             //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            // It's actually the Role.Name tucked into the id param:
            var role = _db.Roles.First(r => r.Name == id);
            var roleModel = new EditRoleViewModel(role);
            return View(roleModel);
        } // METHOD EDIT ENDS ------------------------------------------------------------------------------------------------------------- //


        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION EDIT                                                                                                                      //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "RoleName,OriginalRoleName,Description")] EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = _db.Roles.First(r => r.Name == model.OriginalRoleName);
                role.Name = model.RoleName;
                role.Description = model.Description;
                _db.Entry(role).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            } // IF ENDS
            return View(model);
        } // METHOD EIT ENDS -------------------------------------------------------------------------------------------------------------- //

        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION DELETE                                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _db.Roles.First(r => r.Name == id);
            var model = new RoleViewModel(role);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(model);
        } // METHOD DELETE ENDS ----------------------------------------------------------------------------------------------------------- //


        //--------------------------------------------------------------------------------------------------------------------------------- //
        // ACTION DELETE POST                                                                                                               //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var role = _db.Roles.First(r => r.Name == id);
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _db.DeleteRole(_db, userManager, role.Id);
            return RedirectToAction("Index");
        } // METHOD DELETE CONFIRMED ENDS ------------------------------------------------------------------------------------------------- //
    }
}