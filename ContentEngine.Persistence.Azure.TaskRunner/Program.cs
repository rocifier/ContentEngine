using Microsoft.Extensions.DependencyInjection;
using ContentEngine.Persistence.Azure.TaskRunner.Implementation;
using Microsoft.Extensions.Configuration;
using System;

namespace ContentEngine.Persistence.Azure.TaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder();
            if (environmentName == "Development")
            {
                builder.AddUserSecrets<Program>();
            }
            IConfigurationRoot Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddSingleton<IServiceBusSubscriber, ServiceBusSubscriber>()
                .AddSingleton(Configuration)
                .BuildServiceProvider();
            serviceProvider.GetService<IServiceBusSubscriber>().BeginBackgroundProcessing();

        }
    }
}