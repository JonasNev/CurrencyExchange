using FluentAssertions;
using NSubstitute;
using AutoFixture.Xunit2;
using CurrencyExchange.Services;
using CurrencyExchange.Interfaces;
using CurrencyExchange.Models;

namespace CurrencyExchangeTests.Services;

public class InputServiceTests
{
    private readonly IInputValidationService _inputValidationService = Substitute.For<IInputValidationService>();

    private readonly InputService _inputService;

    public InputServiceTests()
    {
        _inputService = new InputService(_inputValidationService);
    }

    [Fact]
    public void ParseInput_ValidInput_ReturnsConsoleCommand()
    {
        const string input = "Exchange USD/EUR 100";
        var validResult = new ValidationResult { IsValid = true, Errors = [] };

        _inputValidationService.Validate(input).Returns(validResult);

        var result = _inputService.ParseInput(input);

        result.CurrencyPair.MainCurrency.Should().Be("USD");
        result.CurrencyPair.IncomingCurrency.Should().Be("EUR");
        result.Amount.Should().Be(100m);
        _inputValidationService.Received(1).Validate(input);
    }

    [Theory]
    [AutoData]
    public void ParseInput_InvalidInput_ThrowsArgumentException(
        string input,
        string errorMessage)
    {
        var invalidResult = new ValidationResult
        {
            IsValid = false,
            Errors = [errorMessage],
        };

        _inputValidationService.Validate(input).Returns(invalidResult);

        _inputService.Invoking(x => x.ParseInput(input))
            .Should()
            .Throw<ArgumentException>()
            .WithMessage(errorMessage);
    }
}