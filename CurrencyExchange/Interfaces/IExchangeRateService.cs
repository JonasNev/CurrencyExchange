using CurrencyExchange.Models;

namespace CurrencyExchange.Interfaces;

public interface IExchangeRateService
{
    ExchangeRate GetExchangeRate(CurrencyPair currency);

    IEnumerable<string> GetSupportedCurrencies();
}