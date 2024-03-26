namespace ECommerce.Services
{
    public interface IPaypalService
    {
        string CreatePayment(decimal amount);
        string ExecutePayment(string paymentID, string payerID);
    }
}
