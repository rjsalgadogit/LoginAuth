using LoginAuth.Models;

namespace LoginAuth.Services.Interfaces
{
    public interface IAuthenticateService
    {
        public UserModel Authenticate(LoginViewModel model);
        public string Generate(UserModel user);
    }
}
