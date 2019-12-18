using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NationalParksHiking.Startup))]
namespace NationalParksHiking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
