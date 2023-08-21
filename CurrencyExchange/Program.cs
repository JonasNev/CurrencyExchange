using CurrencyExchange.Services;

namespace CurrencyExchange
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exchangeRateService = new ExchangeRateService();
            var currencyConverter = new CurrencyConversionService(exchangeRateService);
            var inputService = new ConsoleInputService();

            do
            {
                Console.WriteLine($"Enter command (e.g., Exchange EUR/DKK 1) or type '{Constants.Program.EndText}' to exit: ");
                var input = Console.ReadLine();
                if (input?.Equals(Constants.Program.EndText, StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    break;
                }

                try
                {
                    var consoleCommand = inputService.ParseExchangeCommand(input!);
                    var exchangedAmount = currencyConverter.Convert(consoleCommand.CurrencyPair, consoleCommand.Amount);
                    Console.WriteLine($"{exchangedAmount}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);
        }
    }
}