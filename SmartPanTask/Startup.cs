using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartPanTask.Startup))]
namespace SmartPanTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
