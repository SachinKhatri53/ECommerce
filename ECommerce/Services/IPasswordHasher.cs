namespace ECommerce.Services
{
    public interface IPasswordHasher
    {
        public string GenerateHashedPassword(string password);
        public bool VerifyPassword(string providedPassword, string storedPassword);
    }
}
