using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MongoPlaces.Web.Startup))]
namespace MongoPlaces.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
