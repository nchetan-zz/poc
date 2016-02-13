using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ResourceInMvc.Startup))]
namespace ResourceInMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
