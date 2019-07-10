using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookingCalendar.Startup))]
namespace BookingCalendar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
