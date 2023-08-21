using CurrencyExchange.Models;

namespace CurrencyExchange.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IList<ExchangeRate> _exchangeRates = new List<ExchangeRate>
        {
            new ExchangeRate(new CurrencyPair("DKK", "DKK"), 1.0M),
            new ExchangeRate(new CurrencyPair("DKK", "EUR"), 1 / 7.43944M),
            new ExchangeRate(new CurrencyPair("DKK", "USD"), 1 / 6.6311M),
            new ExchangeRate(new CurrencyPair("DKK", "GBP"), 1 / 8.5285M),
            new ExchangeRate(new CurrencyPair("DKK", "SEK"), 1 / 0.7610M),
            new ExchangeRate(new CurrencyPair("DKK", "NOK"), 1 / 0.7840M),
            new ExchangeRate(new CurrencyPair("DKK", "CHF"), 1 / 6.8358M),
            new ExchangeRate(new CurrencyPair("DKK", "JPY"), 1 / 0.05974M)
        };

        public ExchangeRate GetExchangeRate(CurrencyPair currencyPair)
        {
            var mainToDkk = _exchangeRates.FirstOrDefault(x =>
                string.Equals(x.CurrencyPair.IncomingCurrency, currencyPair.MainCurrency, StringComparison.OrdinalIgnoreCase));

            var dkkToIncoming = _exchangeRates.FirstOrDefault(x =>
                string.Equals(x.CurrencyPair.IncomingCurrency, currencyPair.IncomingCurrency, StringComparison.OrdinalIgnoreCase));

            if (mainToDkk == null && dkkToIncoming == null)
            {
                throw new InvalidOperationException($"No exchange rate available. Neither {currencyPair.MainCurrency} nor {currencyPair.IncomingCurrency} exists.");
            }

            if (mainToDkk == null || dkkToIncoming == null)
            {
                var missingCurrency = mainToDkk == null ? currencyPair.MainCurrency : currencyPair.IncomingCurrency;
                throw new InvalidOperationException($"Currency {missingCurrency} does not exist.");
            }

            if (currencyPair.MainCurrency == currencyPair.IncomingCurrency)
            {
                return new ExchangeRate(currencyPair, 1.0M);
            }

            var rate = 1 / mainToDkk.Rate * dkkToIncoming.Rate;
            return new ExchangeRate(currencyPair, Math.Round(rate, 4));
        }
    }
}
