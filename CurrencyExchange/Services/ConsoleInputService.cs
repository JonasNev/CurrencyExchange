using CurrencyExchange.Models;

namespace CurrencyExchange.Services
{
    public class ConsoleInputService
    {
        public ConsoleCommand ParseExchangeCommand(string input)
        {
            var commandParts = input.Split(' ');

            if (commandParts.Length != 3 || !string.Equals(commandParts[0], "exchange", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid command. Please use the format: Exchange CURRENCY1/CURRENCY2 AMOUNT");
            }

            var currencyPairInput = commandParts[1].Split('/');
            if (currencyPairInput.Length != 2)
            {
                throw new ArgumentException("Invalid currency pair. Please use the format: CURRENCY1/CURRENCY2");
            }

            var currencyPair = new CurrencyPair(currencyPairInput[0], currencyPairInput[1]);

            if (!decimal.TryParse(commandParts[2], out var amount))
            {
                throw new ArgumentException("Invalid amount. Please enter a valid number.");
            }

            return new ConsoleCommand
            {
                CurrencyPair = currencyPair,
                Amount = amount
            };
        }
    }
}
