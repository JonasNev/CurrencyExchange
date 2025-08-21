using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchange.Services;

public class ExchangeRateService(
    IExchangeRateValidationService exchangeRateValidationService,
    IExchangeRateRepository exchangeRateRepository) : IExchangeRateService
{
    public ExchangeRate GetExchangeRate(CurrencyPair currencyPair)
    {
        if (IsSameCurrencyPair(currencyPair))
        {
            return new ExchangeRate(currencyPair, Constants.ExchangeRate.DefaultRate);
        }

        var dkkToMain = exchangeRateRepository.GetExchangeRateForCurrency(currencyPair.MainCurrency);

        var dkkToIncoming = exchangeRateRepository.GetExchangeRateForCurrency(currencyPair.IncomingCurrency);

        var validationResult = exchangeRateValidationService.Validate(currencyPair, dkkToMain, dkkToIncoming);

        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }

        var rate = 1 / dkkToMain.Rate * dkkToIncoming.Rate;
        return new ExchangeRate(currencyPair, Math.Round(rate, Constants.ExchangeRate.RatePrecision));
    }

    public IEnumerable<string> GetSupportedCurrencies()
    {
        return exchangeRateRepository.GetSupportedCurrencies();
    }

    private static bool IsSameCurrencyPair(CurrencyPair currencyPair)
    {
        return string.Equals(currencyPair.MainCurrency, currencyPair.IncomingCurrency, StringComparison.OrdinalIgnoreCase);
    }
}