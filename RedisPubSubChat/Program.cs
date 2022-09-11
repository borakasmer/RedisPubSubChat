using ElasticCore;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RedisPubSubChat
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (RedisClient client = new RedisClient())
            using (ElasticCoreService<ChatModel> elastic = new ElasticCoreService<ChatModel>())
            {
                //Get Chat History From Elastic
               var chats = elastic.SearchChatLog(5);
                Console.WriteLine("TOP 5 Message History:");
                Console.WriteLine("".PadRight(60, '*'));
                foreach (var chat in chats)
                {
                    Console.WriteLine($"-{chat.From}({chat.PostDate}): {chat.Message}");
                }
                Console.WriteLine("".PadRight(60, '*'));
                Console.WriteLine();              
                //---------------------------
                var pubSub = client.GetSubscriber();
                bool isStay = true;
                while (isStay)
                {
                    await pubSub.SubscribeAsync(client.Channel2, (cannel, message) =>
                    {
                        Console.WriteLine(Environment.NewLine + "Elie: " + message);

                        Console.Write("Write Message : ");
                        var message2 = Console.ReadLine();
                        isStay = message2.ToLower() != "exit" ? true : false;
                        pubSub.PublishAsync(client.Channel, message2, StackExchange.Redis.CommandFlags.FireAndForget);
                        ChatModel chatModel = new ChatModel() { From = "Elie", To = "Joe", Message = message2, PostDate = DateTime.Now };
                        elastic.CheckExistsAndInsertLog(chatModel, ConfigurationManager.AppSettings["ElasticIndexName"]);
                    });

                    Console.Write("Write Message : ");
                    var message = Console.ReadLine();
                    isStay = message.ToLower() != "exit" ? true : false;
                    await pubSub.PublishAsync(client.Channel, message, StackExchange.Redis.CommandFlags.FireAndForget);
                    ChatModel chatModel = new ChatModel() { From = "Elie", To = "Joe", Message = message, PostDate = DateTime.Now };
                    elastic.CheckExistsAndInsertLog(chatModel, ConfigurationManager.AppSettings["ElasticIndexName"]);
                }
            }
        }
    }
}
