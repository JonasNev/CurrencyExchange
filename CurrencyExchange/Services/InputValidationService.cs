using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;
using InputConstants = CurrencyExchange.Constants.InputCommand;

namespace CurrencyExchange.Services;

public class InputValidationService : IInputValidationService
{
    private readonly List<string> _errors = [];

    public ValidationResult Validate(string input)
    {
        _errors.Clear();

        if (string.IsNullOrWhiteSpace(input))
        {
            _errors.Add("Input cannot be empty.");

            return new ValidationResult
            {
                Errors = _errors,
                IsValid = false
            };
        }

        var commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        ValidateCommandStructure(commandParts);

        if (commandParts.Length >= 2)
        {
            ValidateCurrencyPairFormat(commandParts[1]);
        }

        if (commandParts.Length >= 3)
        {
            ValidateAmountFormat(commandParts[2]);
        }

        return new ValidationResult
        {
            Errors = _errors,
            IsValid = _errors.Count == 0
        };
    }

    private void ValidateCommandStructure(string[] commandParts)
    {
        if (commandParts.Length != InputConstants.ExpectedPartsCount)
        {
            _errors.Add("Invalid command format. Please use: Exchange CURRENCY1/CURRENCY2 AMOUNT.");
        }
        else if (!string.Equals(commandParts[0], InputConstants.Exchange, StringComparison.OrdinalIgnoreCase))
        {
            _errors.Add("Invalid command. Use 'Exchange' as the command.");
        }
    }

    private void ValidateCurrencyPairFormat(string currencyPairInput)
    {
        var currencyPairParts = currencyPairInput.Split(InputConstants.CurrencyPairSeparator);

        if (currencyPairParts.Length != 2)
        {
            _errors.Add("Invalid currency pair format. Please use: CURRENCY1/CURRENCY2.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(currencyPairParts[0]))
            {
                _errors.Add("Main currency cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(currencyPairParts[1]))
            {
                _errors.Add("Incoming currency cannot be empty.");
            }
        }
    }

    private void ValidateAmountFormat(string amountInput)
    {
        if (!decimal.TryParse(amountInput, out var amount))
        {
            _errors.Add("Invalid amount. Please enter a valid number.");
        }
        else if (amount < 0)
        {
            _errors.Add("Amount cannot be negative.");
        }
    }
}