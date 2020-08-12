using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Api.StartupOwin))]

namespace Api
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
