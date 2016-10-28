using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GardenationApp.Startup))]
namespace GardenationApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
