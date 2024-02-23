using EasyNetQ;
using MessagingAPI.BusinessModels;

namespace MessagingAPI.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private IBus messageBus;
        public static List<Product> Products { get; set; } // static property this hold the products for whole application
        public static List<BusinessModels.PriceReduction> PriceReductions { get; set; } // static property this hold the reductions for whole application
        public RabbitMQService() 
        {
            messageBus = RabbitHutch.CreateBus("host=localhost", s => s.EnableSystemTextJson());
            ReceiveProductMessage();
            ReceiveReductions();
        }

        /// <summary>
        /// this receives the products from the queue.
        /// </summary>
        private void ReceiveProductMessage()
        {
            List<Product> products = new List<Product>();
            messageBus.SendReceive.Receive<List<MessagingAPI.BusinessModels.Product>>("myQueue", msg =>
            {
                if (msg.ToList() != null && msg.ToList().Count > 0)
                {
                    Products = msg.ToList();
                }
            });
        }

        /// <summary>
        /// this exposes the products 
        /// </summary>
        /// <returns></returns>
        public List<Product> GetProducts()
        {
            return Products;
        }

        /// <summary>
        /// this receives the price reductions for the queue
        /// </summary>
        public void ReceiveReductions() 
        {
            List<BusinessModels.PriceReduction> reductions = new List<BusinessModels.PriceReduction>();
            messageBus.SendReceive.Receive<List<MessagingAPI.BusinessModels.PriceReduction>>("PriceReductionQ", msg =>
            {
                if (msg.ToList() != null && msg.ToList().Count > 0)
                {
                    PriceReductions = msg.ToList();
                }
            });
        }

        /// <summary>
        /// this prepares the prodcut info with reduction for the matching id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductPriceDetail GetProduct(int id)
        {
            ProductPriceDetail productPriceDetail = new ProductPriceDetail();
            var reduction = PriceReductions?.FirstOrDefault(x => x.DayOfWeek == ((int)DateTime.Today.DayOfWeek));
            var product = Products?.FirstOrDefault(x => x.Id == id);
            if (product != null && reduction != null)
            {
                productPriceDetail.Id = product.Id;
                productPriceDetail.Name = product.Name;
                productPriceDetail.EntryDate = product.EntryDate;
                productPriceDetail.PriceWithReduction = (double)(product.Price - reduction.Reduction);
            }
            return productPriceDetail;
        }
    }
}
