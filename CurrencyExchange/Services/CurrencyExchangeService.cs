using CurrencyExchange.Interfaces;

namespace CurrencyExchange.Services;

public class CurrencyExchangeService(
    IInputService inputService,
    IExchangeRateService exchangeRateService) : ICurrencyExchangeService
{
    public decimal ExchangeCurrency(string input)
    {
        var parsedInput = inputService.ParseInput(input);

        var exchangeRate = exchangeRateService.GetExchangeRate(parsedInput.CurrencyPair);

        return parsedInput.Amount * exchangeRate.Rate;
    }
}