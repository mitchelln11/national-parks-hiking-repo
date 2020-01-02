using Microsoft.Owin;
using Owin;
using NationalParksHiking.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

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
