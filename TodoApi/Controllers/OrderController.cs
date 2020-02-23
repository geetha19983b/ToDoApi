using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly ProducerConfig producerConfig;

        private readonly ConsumerConfig consumerconfig;
        public OrderController(ProducerConfig producerConfig, ConsumerConfig consumerConfig)
        {
            this.producerConfig = producerConfig;
            this.consumerconfig = consumerConfig;

        }
        // POST api/values
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(this.producerConfig, "orderrequests");
            await producer.writeMessage(serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }
        [HttpGet]
        [Route("ProcessOrder")]
        public async Task<ActionResult> ProcessMessage()
        {
            var consumerHelper = new ConsumerWrapper(consumerconfig, "orderrequests");
            List<string> lstorderRequests = consumerHelper.testMessage();
            if (lstorderRequests.Any())
            {
                foreach (var orderRequest in lstorderRequests)
                {
                    //Deserilaize 
                    OrderRequest order = JsonConvert.DeserializeObject<OrderRequest>(orderRequest);

                    //TODO:: Process Order
                    Console.WriteLine($"Info: OrderHandler => Processing the order for {order.productname}");
                    order.status = OrderStatus.COMPLETED;


                    //Write to ReadyToShip Queue

                    //var producerWrapper = new ProducerWrapper(producerConfig, "readytoship");
                    //await producerWrapper.writeMessage(JsonConvert.SerializeObject(order));
                }

                return Ok(lstorderRequests);
            }
            return Ok("No messages to process");

        }

    }


    public class OrderRequest
    {
        public int id { get; set; }
        public string productname { get; set; }
        public int quantity { get; set; }

        public OrderStatus status { get; set; }
    }
    public enum OrderStatus
    {
        IN_PROGRESS,
        COMPLETED,
        REJECTED
    }
}