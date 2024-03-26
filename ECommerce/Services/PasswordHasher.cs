namespace ECommerce.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateHashedPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string providedPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedPassword);
        }
    }
}
