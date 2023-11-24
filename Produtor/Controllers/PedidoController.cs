﻿using Core;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Produtor.Controllers
{
    [ApiController]
    [Route("/Pedido")]
    public class PedidoController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };


            using(var connnection = factory.CreateConnection())
            {
                using (var channel = connnection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "fila",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var message = JsonSerializer.Serialize(new Pedido(1, new Usuario(2, "João", "joao@teste.com")));

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "fila",
                        basicProperties: null,
                        body:body);
                }
            }
            return Ok(new {status = 200});
        }
    }
}
