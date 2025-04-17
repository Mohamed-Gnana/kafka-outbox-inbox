using Microsoft.Extensions.Configuration;

namespace Infra.Broker.Kafka
{
    public static class Extensions
    {
        public static KafkaConfiguration GetKafkaConfigurations(this IConfiguration configuration)
        {
            return configuration.GetSection("Kafka").Get<KafkaConfiguration>() ?? new();
        }

        public static string GetMessageTopic(this Type type)
        {
            return type.Name.Replace("Message", "").ToLower() + "-topic";
        }
    }
}
