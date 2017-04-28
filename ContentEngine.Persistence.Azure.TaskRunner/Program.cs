using Microsoft.Extensions.DependencyInjection;
using ContentEngine.Persistence.Azure.TaskRunner.Implementation;

namespace ContentEngine.Persistence.Azure.TaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.AddSingleton<IServiceBusSubscriber, ServiceBusSubscriber>().BuildServiceProvider();
            serviceProvider.GetService<IServiceBusSubscriber>().BeginBackgroundProcessing();
        }
    }
}