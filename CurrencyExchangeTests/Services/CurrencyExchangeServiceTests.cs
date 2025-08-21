using FluentAssertions;
using NSubstitute;
using CurrencyExchange.Services;
using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchangeTests.Services;

public class CurrencyExchangeServiceTests
{
    private readonly IInputService _inputService = Substitute.For<IInputService>();
    private readonly IExchangeRateService _exchangeRateService = Substitute.For<IExchangeRateService>();

    private readonly CurrencyExchangeService _currencyExchangeService;

    public CurrencyExchangeServiceTests()
    {
        _currencyExchangeService = new CurrencyExchangeService(_inputService, _exchangeRateService);
    }

    [Fact]
    public void ExchangeCurrency_SpecificValues_ReturnsCorrectCalculation()
    {
        const string input = "Exchange USD/EUR 100";
        var currencyPair = new CurrencyPair("USD", "EUR");
        var consoleCommand = new ConsoleCommand
        {
            CurrencyPair = currencyPair,
            Amount = 100m
        };
        var exchangeRate = new ExchangeRate(currencyPair, 0.85m);

        _inputService.ParseInput(input).Returns(consoleCommand);
        _exchangeRateService.GetExchangeRate(currencyPair).Returns(exchangeRate);

        var actual = _currencyExchangeService.ExchangeCurrency(input);

        actual.Should().Be(85m);
        _inputService.Received(1).ParseInput(input);
        _exchangeRateService.Received(1).GetExchangeRate(currencyPair);
    }
}