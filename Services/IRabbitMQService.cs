using MessagingAPI.BusinessModels;

namespace MessagingAPI.Services
{
    public interface IRabbitMQService
    {
        public List<Product> GetProducts();
        public ProductPriceDetail GetProduct(int id);

    }
}
