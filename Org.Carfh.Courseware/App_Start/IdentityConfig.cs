using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Org.Carfh.Courseware.Models;
using Org.Carfh.Courseware.Properties;
using SendGrid;
using Twilio;

namespace Org.Carfh.Courseware
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return ConfigSendGridAsync(message);
        } // METHOD SEND ASYNC ENDS ------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD CONFIG SEND GRID ASYNC                                                                                                    //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// Configures an email client for SEND GRID and sends the email using the REST API
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ConfigSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "noreply@cafh.org", "CAFH Administrator");

            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                       Settings.Default.SendGridUID,
                       Settings.Default.SendGridSecret
                       );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        } // METHJOD CONFIG SEND GRID ASYNC ENDS ------------------------------------------------------------------------------------------ //
    }

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // CLASS SMS SERVICE                                                                                                                    //
    // ------------------------------------------------------------------------------------------------------------------------------------ // 
    /// <summary>
    /// Configures an SMS provider that can be used to send SMS Two-Factor Authentication
    /// </summary>
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var twilioLive = new TwilioRestClient(
                Properties.Settings.Default.TwilioSId,
                Properties.Settings.Default.TwilioToken
            );
            TwilioRestClient twilio = null;
            twilio = twilioLive;
            var result = twilio.SendMessage(
                Properties.Settings.Default.TwilioFromNumber,
                message.Destination, "[GATORADE-CR]:" + message.Body);
            // Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            Trace.TraceInformation(result.Status);
            // Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);
        }
    } // CLASS SMS SERVICE ENDS ----------------------------------------------------------------------------------------------------------- //

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------------ //
    // APPLICATION ROLE MANAGER                                                                                                             //
    // ------------------------------------------------------------------------------------------------------------------------------------ //
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(
            IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }
        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
        }
    } // CLASS APPLICATION ROLE MANAER ENDS ----------------------------------------------------------------------------------------------- //

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
