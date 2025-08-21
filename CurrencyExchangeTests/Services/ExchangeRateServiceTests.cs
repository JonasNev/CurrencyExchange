using AutoFixture.Xunit2;
using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;
using CurrencyExchange.Services;
using FluentAssertions;
using NSubstitute;

namespace CurrencyExchangeTests.Services;

public class ExchangeRateServiceTests
{
    private readonly IExchangeRateValidationService _exchangeRateValidationService = Substitute.For<IExchangeRateValidationService>();
    private readonly IExchangeRateRepository _exchangeRateRepository = Substitute.For<IExchangeRateRepository>();

    private readonly ExchangeRateService _exchangeRateService;

    public ExchangeRateServiceTests()
    {
        _exchangeRateService = new ExchangeRateService(_exchangeRateValidationService, _exchangeRateRepository);
    }

    [Theory]
    [AutoData]
    public void GetExchangeRate_ValidCurrencyPair_ReturnsCalculatedRate(
        string mainCurrency,
        string incomingCurrency,
        decimal mainRate,
        decimal incomingRate)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);
        var mainExchangeRate = new ExchangeRate(new CurrencyPair(mainCurrency, "DKK"), mainRate);
        var incomingExchangeRate = new ExchangeRate(new CurrencyPair(incomingCurrency, "DKK"), incomingRate);
        var validResult = new ValidationResult { IsValid = true, Errors = [] };
        var expectedRate = Math.Round(1 / mainRate * incomingRate, 4);

        _exchangeRateRepository.GetExchangeRateForCurrency(mainCurrency).Returns(mainExchangeRate);
        _exchangeRateRepository.GetExchangeRateForCurrency(incomingCurrency).Returns(incomingExchangeRate);
        _exchangeRateValidationService.Validate(currencyPair, mainExchangeRate, incomingExchangeRate).Returns(validResult);

        var result = _exchangeRateService.GetExchangeRate(currencyPair);

        result.Rate.Should().Be(expectedRate);
    }

    [Theory]
    [AutoData]
    public void GetExchangeRate_SameCurrency_ReturnsDefaultRate(string currency)
    {
        var currencyPair = new CurrencyPair(currency, currency);

        var result = _exchangeRateService.GetExchangeRate(currencyPair);

        result.Rate.Should().Be(1.0m);
        result.CurrencyPair.Should().Be(currencyPair);
    }

    [Theory]
    [AutoData]
    public void GetExchangeRate_ValidationFails_ThrowsInvalidOperationException(
        string mainCurrency,
        string incomingCurrency,
        string errorMessage)
    {
        var currencyPair = new CurrencyPair(mainCurrency, incomingCurrency);
        var invalidResult = new ValidationResult { IsValid = false, Errors = [errorMessage] };

        _exchangeRateValidationService.Validate(currencyPair, null, null).Returns(invalidResult);

        _exchangeRateService.Invoking(x => x.GetExchangeRate(currencyPair))
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(errorMessage);
    }

    [Theory]
    [AutoData]
    public void GetSupportedCurrencies_CallsRepository_ReturnsRepositoryResult(string[] currencies)
    {
        _exchangeRateRepository.GetSupportedCurrencies().Returns(currencies);

        var result = _exchangeRateService.GetSupportedCurrencies();

        result.Should().BeEquivalentTo(currencies);
        _exchangeRateRepository.Received(1).GetSupportedCurrencies();
    }
}