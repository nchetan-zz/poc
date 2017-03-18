using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CloudWebRole1.Startup))]
namespace CloudWebRole1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
