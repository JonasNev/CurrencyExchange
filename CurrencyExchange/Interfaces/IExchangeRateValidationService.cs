using CurrencyExchange.Models;

namespace CurrencyExchange.Interfaces;

public interface IExchangeRateValidationService
{
    ValidationResult Validate(CurrencyPair currencyPair, ExchangeRate mainCurrencyExchangeRate, ExchangeRate incomingCurrencyExchangeRate);
}