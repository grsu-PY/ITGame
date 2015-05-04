using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ITGame.WebApp.DataManager.Startup))]
namespace ITGame.WebApp.DataManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
