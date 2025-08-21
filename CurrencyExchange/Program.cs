using CurrencyExchange.Interfaces;
using CurrencyExchange.Repositories;
using CurrencyExchange.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var currencyExchangeService = serviceProvider.GetRequiredService<ICurrencyExchangeService>();
            var exchangeRateService = serviceProvider.GetRequiredService<IExchangeRateService>();

            Console.WriteLine("=== Currency Exchange Tool ===");
            Console.WriteLine("Enter commands like: Exchange EUR/USD 100");

            var supportedCurrencies = string.Join(", ", exchangeRateService.GetSupportedCurrencies());
            Console.WriteLine($"Supported currencies: {supportedCurrencies}");
            do
            {
                Console.WriteLine($"Enter command or type '{Constants.Program.EndText}' to exit: ");
                var input = Console.ReadLine();

                if (input?.Equals(Constants.Program.EndText, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    break;
                }

                try
                {
                    var exchangedAmount = currencyExchangeService.ExchangeCurrency(input);

                    Console.WriteLine($"{exchangedAmount}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            } while (true);

            Console.WriteLine("Goodbye!");
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<IExchangeRateValidationService, ExchangeRateValidationService>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddTransient<IInputValidationService, InputValidationService>();
            services.AddTransient<IInputService, InputService>();
            services.AddTransient<ICurrencyExchangeService, CurrencyExchangeService>();
        }
    }
}