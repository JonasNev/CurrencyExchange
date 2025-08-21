using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchange.Services;

public class InputService(IInputValidationService inputValidationService) : IInputService
{
    public ConsoleCommand ParseInput(string input)
    {
        var validationResult = inputValidationService.Validate(input);

        return validationResult.IsValid ? CreateConsoleCommand(input) : throw new ArgumentException(validationResult.ErrorMessage);
    }

    private static ConsoleCommand CreateConsoleCommand(string input)
    {
        var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currencyPairInput = commandParts[1].Split(Constants.InputCommand.CurrencyPairSeparator);
        var currencyPair = new CurrencyPair(currencyPairInput[0], currencyPairInput[1]);
        var amount = decimal.Parse(commandParts[2]);

        return new ConsoleCommand
        {
            CurrencyPair = currencyPair,
            Amount = amount
        };
    }
}