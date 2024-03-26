using ECommerce.Models;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Orders;

namespace ECommerce.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void CreateUserSession(User user)
        {
            SaveUserToSession(user);
        }

        public User GetUserSession()
        {
           return GetUserFromHttpSession();
        }

        public void UpdateUserSession(User user)
        {
            var sessionUser = GetUserFromHttpSession();
            if (sessionUser != null)
            {
                sessionUser = user;
                SaveUserToSession(sessionUser);
            }
        }
        private void SaveUserToSession(User user)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                var currentJson = JsonConvert.SerializeObject(user);
                httpContext.Session.SetString("CurrentUser", currentJson);
            }
        }
        private User GetUserFromHttpSession()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Retrieve or create the cart from session
                var currentUserJson = httpContext.Session.GetString("CurrentUser");
                var user = string.IsNullOrEmpty(currentUserJson) ? new User() : JsonConvert.DeserializeObject<User>(currentUserJson);
                return user;
            }

            // Return a default or empty cart if HttpContext is null
            return new User();
        }

        public void ClearUserSession()
        {
            SaveUserToSession(new User());
        }
    }
}
