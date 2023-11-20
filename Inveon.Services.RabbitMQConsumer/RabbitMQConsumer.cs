using Inveon.Services.RabbitMQConsumer.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Inveon.Services.RabbitMQConsumer
{
    public class RabbitMQConsumer : BackgroundService
    {
        private IConnection conn;
        private readonly IModel channel;

        public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                };
                conn = factory.CreateConnection();
                channel = conn.CreateModel();
                channel.QueueDeclare(queue: "checkoutqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, e) =>
            {
                var body = e.Body;
                var json = System.Text.Encoding.UTF8.GetString(body.ToArray());
                MailContentDTO content = JsonConvert.DeserializeObject<MailContentDTO>(json);
                _ = SendEmailAsync(content);
            };
            try
            {
                channel.BasicConsume("checkoutqueue", false, consumer);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        public string GetMailContent(MailContentDTO mailContentDTO)
        {
            string content =
                "Alışverişiniz için teşekkür ederiz :)" + "\n" +
                "\t ~Fatura Bilgileri~" + "\n" +
            "---------------------------------------------------" + "\n" +
            $@"Tarih: {mailContentDTO.PickupDateTime.ToString("MM/dd/yyyy h:mm tt")}" + "\n" +
            $@"Ad: {mailContentDTO.FirstName}" + "\n" +
            $@"Soyad: {mailContentDTO.LastName}" + "\n" +
            $@"Ödeme Methodu: {mailContentDTO.CardNumber}" + "\n";
            if(mailContentDTO.CouponCode != null){
                content += $@"Kullanılan Kupon Kodu: {mailContentDTO.CouponCode}" + "\n";
            }
            content += $@"Toplam: {Math.Round(mailContentDTO.OrderTotal, 2)}" + "\n" +
            "---------------------------------------------------";
            return content;
        }

        public async Task SendEmailAsync(MailContentDTO mailContentDTO)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(SD.sender_name, SD.sender_email));
            // Because I've used dummy data when creating a buyer object now, I have to update the receiver's mail address
            mailContentDTO.Email = SD.receiver_email;
            message.To.Add(MailboxAddress.Parse(mailContentDTO.Email));
            string content = GetMailContent(mailContentDTO);
            message.Subject = SD.subject;

            message.Body = new TextPart("plain") { Text = content };

            var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(SD.sender_email, SD.password);
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

    }
}
