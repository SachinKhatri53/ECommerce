using ECommerce.Data;
using PayPalCheckoutSdk.Orders;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;

        public ProductService(ECommerceContext context)
        {
            _context = context;
        }
        public string GetProductNameById(int productId)
        {
            var product = _context.Product.FirstOrDefault(p => p.ProductId == productId);

            return product?.ProductName ?? "Product Not Found";
        }
    }
}
