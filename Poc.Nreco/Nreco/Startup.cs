using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nreco.Startup))]
namespace Nreco
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
