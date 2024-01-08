using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;


namespace ePizzaHub.Services.Implementations
{
    public class AuthService : IAuthService
    {
        IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public bool CreateUser(User user, string role)
        {
            return _userRepository.CreateUser(user, role);
        }

        public UserModel ValidateUser(string email, string password)
        {
            UserModel model = _userRepository.ValidateUser(email, password);
            return model;
        }
    }
}
