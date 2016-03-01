using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Org.Carfh.Courseware.Startup))]
namespace Org.Carfh.Courseware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
