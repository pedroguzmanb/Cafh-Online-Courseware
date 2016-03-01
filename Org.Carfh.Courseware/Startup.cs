using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Org.Cafh.Courseware.Startup))]
namespace Org.Cafh.Courseware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
