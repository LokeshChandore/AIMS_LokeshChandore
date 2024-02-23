using MessagingAPI.BusinessModels;
using MessagingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MessagingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IRabbitMQService _rabitMQProducer;

        public MessagingController(IRabbitMQService rabitMQProducer) { 
            _rabitMQProducer = rabitMQProducer;
        }

        /// <summary>
        /// this will all the products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProducts")]
        public List<Product> GetProducts()
        {
            return _rabitMQProducer.GetProducts();
        }

        /// <summary>
        /// this will provide the product for matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProduct/{id}")]
        public ProductPriceDetail GetProduct(int id) 
        {
            return _rabitMQProducer.GetProduct(id);
        }
    }
}
