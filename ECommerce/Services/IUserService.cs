using ECommerce.Models;

namespace ECommerce.Services
{
    public interface IUserService
    {
        public void CreateUserSession(User user);
        public void UpdateUserSession(User user);
        public User GetUserSession();
        public void ClearUserSession();
    }
}
