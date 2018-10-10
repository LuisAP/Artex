using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Artex.Startup))]
namespace Artex
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
