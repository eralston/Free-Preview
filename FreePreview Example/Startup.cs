using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FreePreview_Example.Startup))]
namespace FreePreview_Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
