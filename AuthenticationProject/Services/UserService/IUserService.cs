using AuthenticationProject.Services.Utils;

namespace AuthenticationProject.Services.UserService
{
    public interface IUserService
    {
        Task<ResponseData<User>> Register(UserRequest request);
        Task<ResponseData<string>> Login(UserLogin request);
        
    }
}
