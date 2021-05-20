using CompanyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        public CompanyController()
        {

        }

        [HttpPost]
        [SwaggerOperation(Summary = "Add New CompanyName To The RabbitMQ.", Description = "<h2>Trigger Go Microservice</h2> <br> When You Add new Company to the Queue, It will insert to the SQLDB and Get Latest 10 Articles from borakasmer.com")]
        public bool InsertCompany(Company companyModel)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "127.0.0.1",
                    Password = "guest",
                    Port = 5672,
                    VirtualHost = "/",
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "company",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var companyData = JsonConvert.SerializeObject(companyModel);
                    var body = Encoding.UTF8.GetBytes(companyData);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "company",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"{companyModel.CompanyName} is Send to the queue");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
