using LoginAuth.Services;
using LoginAuth.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LoginAuth.Registration
{
    public class Registration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient(typeof(IAuthenticateService), typeof(AuthenticateService));
        }
    }
}
