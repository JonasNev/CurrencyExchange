using FluentAssertions;
using AutoFixture.Xunit2;
using CurrencyExchange.Services;
using CurrencyExchange.Models;

namespace CurrencyExchangeTests.Services;

public class ExchangeRateValidationServiceTests
{
    private readonly ExchangeRateValidationService _exchangeRateValidationService = new();

    [Theory]
    [AutoData]
    public void Validate_BothExchangeRatesExist_ReturnsValidResult(
        string mainCurrency,
        string incomingCurrency,
        decimal mainRate,
        decimal incomingRate)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);
        var mainExchangeRate = new ExchangeRate(new CurrencyPair(mainCurrency, "DKK"), mainRate);
        var incomingExchangeRate = new ExchangeRate(new CurrencyPair(incomingCurrency, "DKK"), incomingRate);

        var result = _exchangeRateValidationService.Validate(currencyPair, mainExchangeRate, incomingExchangeRate);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("USD", "EUR", "No exchange rate available. Neither USD nor EUR exists.")]
    public void Validate_BothExchangeRatesAreNull_ReturnsSpecificError(string mainCurrency, string incomingCurrency, string expectedError)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);

        var result = _exchangeRateValidationService.Validate(currencyPair, null, null);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(expectedError);
    }

    [Theory]
    [InlineData("USD", "EUR", "Currency USD does not exist.")]
    [InlineData("GBP", "JPY", "Currency GBP does not exist.")]
    [InlineData("CAD", "AUD", "Currency CAD does not exist.")]
    public void Validate_MainCurrencyRateIsNull_ReturnsSpecificError(string mainCurrency, string incomingCurrency, string expectedError)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);
        var incomingExchangeRate = new ExchangeRate(new CurrencyPair(incomingCurrency, incomingCurrency), 7.5m);

        var result = _exchangeRateValidationService.Validate(currencyPair, null, incomingExchangeRate);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(expectedError);
    }

    [Theory]
    [InlineData("USD", "EUR", "Currency EUR does not exist.")]
    [InlineData("GBP", "JPY", "Currency JPY does not exist.")]
    [InlineData("CAD", "AUD", "Currency AUD does not exist.")]
    public void Validate_IncomingCurrencyRateIsNull_ReturnsSpecificError(string mainCurrency, string incomingCurrency, string expectedError)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);
        var mainExchangeRate = new ExchangeRate(new CurrencyPair(mainCurrency, "DKK"), 6.8m);

        var result = _exchangeRateValidationService.Validate(currencyPair, mainExchangeRate, null);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(expectedError);
    }
}