using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Org.Cafh.Courseware.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Name { set; get; }
        public string Lastname { set; get; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // APPLICATION DB CONTEXT                                                                                                               //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<ApplicationRole> Roles { get; set; }

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // CLASS CONSTRUCTOR                                                                                                                //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        ///     Constructor
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        } // CONSTRUCTOR METHOD ENDS ------------------------------------------------------------------------------------------------------ //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // CREATE                                                                                                                           //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        } // METHOD CREATE ENDS ----------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // ON MODEL CREATING                                                                                                                //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            } // IF ENDS

            base.OnModelCreating(modelBuilder);

            //Defining the keys and relations
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");
            modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");
        } // METHOD ON MODEL CREATING ENDS ------------------------------------------------------------------------------------------------ //



        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD ROLE EXISTS                                                                                                               //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RoleExists(ApplicationRoleManager roleManager, string name)
        {
            return roleManager.RoleExists(name);
        } // METHOD ROLE EXISTS ENDS ------------------------------------------------------------------------------------------------------ //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD CREATE ROLE                                                                                                               //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CreateRole(ApplicationRoleManager roleManager, string name, string description = "")
        {
            var idResult = roleManager.Create(new ApplicationRole(name, description));
            return idResult.Succeeded;
        } // METHOD REATE ROLE ENDS ------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD ADD USER TO ROLE                                                                                                          //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool AddUserToRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            var idResult = userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        } // METHOD ADD USER TO ROLE ENDS ------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD CLEAR USER ROLES                                                                                                          //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userId"></param>
        public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        {
            var user = userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.UserRoles);
            foreach (var role in currentRoles.Cast<ApplicationUserRole>())
            {
                userManager.RemoveFromRole(userId, role.Role.Name);
            } // FOREACH ENDS
        } // METHOD CLEAR USER ROLES ENDS ------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD REMOVE FROM ROLE                                                                                                          //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            userManager.RemoveFromRole(userId, roleName);
        } // METHOD REMOVE FROM ROLE ENDS ------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD DELETE ROLE                                                                                                               //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="roleId"></param>
        public void DeleteRole(ApplicationDbContext context, ApplicationUserManager userManager, string roleId)
        {
            var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
            var role = context.Roles.Find(roleId);
            foreach (var user in roleUsers)
            {
                RemoveFromRole(userManager, user.Id, role.Name);
            } // FOREACH ENDS
            context.Roles.Remove(role);
            context.SaveChanges();
        } // METHOD DELETE ROLE ENDS ------------------------------------------------------------------------------------------------------ //

        public bool Seed(ApplicationDbContext context)
        {
            bool success = true;
#if DEBUG


            ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = CreateRole(_roleManager, "Admin", "Global Access");
            if (!success == true) return success;

            success = CreateRole(_roleManager, "ContentCreator", "Puede crear contnido nuevo");
            if (!success == true) return success;

            success = CreateRole(_roleManager, "ContentAdmin", "Administrador y moderador de contenido");
            if (!success) return success;

            // Create my debug (testing) objects here

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            ApplicationUser user = new ApplicationUser();
            PasswordHasher passwordHasher = new PasswordHasher();

            user.UserName = "admin@freedomgrids.com";
            user.Email = "admin@freedomgrids.com";
            user.Name = "Administrator";
            user.Lastname = "Freedom Grids";


            IdentityResult result = userManager.Create(user, "Wstinol123.");

            success = AddUserToRole(userManager, user.Id, "Admin");
            if (!success) return success;

            success = AddUserToRole(userManager, user.Id, "ContentCreator");
            if (!success) return success;

            success = AddUserToRole(userManager, user.Id, "ContentAdmin");
            if (!success) return success;


#endif
            return success;
        }

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // CLASS DROP CREAT ALWAYS INITIALIZER                                                                                              //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        public class DropCreateAlwaysInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

    } // METHOD APPLICATION DB CONTEXT ---------------------------------------------------------------------------------------------------- //
}