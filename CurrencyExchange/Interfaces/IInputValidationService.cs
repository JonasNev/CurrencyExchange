using CurrencyExchange.Models;

namespace CurrencyExchange.Interfaces;

public interface IInputValidationService
{
    ValidationResult Validate(string input);
}