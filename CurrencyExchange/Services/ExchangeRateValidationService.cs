using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchange.Services;

public class ExchangeRateValidationService : IExchangeRateValidationService
{
    private readonly List<string> _errors = [];

    public ValidationResult Validate(CurrencyPair currencyPair, ExchangeRate mainCurrencyExchangeRate, ExchangeRate incomingCurrencyExchangeRate)
    {
        _errors.Clear();

        if (mainCurrencyExchangeRate == null && incomingCurrencyExchangeRate == null)
        {
            _errors.Add($"No exchange rate available. Neither {currencyPair.MainCurrency} nor {currencyPair.IncomingCurrency} exists.");

            return new ValidationResult
            {
                IsValid = false,
                Errors = _errors,
            };
        }

        if (mainCurrencyExchangeRate == null)
        {
            _errors.Add($"Currency {currencyPair.MainCurrency} does not exist.");
        }
        else if (incomingCurrencyExchangeRate == null)
        {
            _errors.Add($"Currency {currencyPair.IncomingCurrency} does not exist.");
        }

        return new ValidationResult
        {
            IsValid = _errors.Count == 0,
            Errors = _errors,
        };
    }
}