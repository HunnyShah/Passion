using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Passion.Startup))]
namespace Passion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
