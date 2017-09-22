using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Products.BackEnd.Startup))]
namespace Products.BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
