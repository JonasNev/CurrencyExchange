using FluentAssertions;
using CurrencyExchange.Services;

namespace CurrencyExchangeTests.Services;

public class InputValidationServiceTests
{
    private readonly InputValidationService _inputValidationService = new();

    [Theory]
    [InlineData("Exchange USD/EUR 100")]
    [InlineData("exchange gbp/jpy 50.25")]
    [InlineData("EXCHANGE CAD/AUD 0")]
    public void Validate_ValidInput_ReturnsValidResult(string input)
    {
        var result = _inputValidationService.Validate(input);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "Input cannot be empty.")]
    [InlineData("Exchange USD/EUR", "Invalid command format. Please use: Exchange CURRENCY1/CURRENCY2 AMOUNT.")]
    [InlineData("Buy USD/EUR 100", "Invalid command. Use 'Exchange' as the command.")]
    [InlineData("Exchange USD 100", "Invalid currency pair format. Please use: CURRENCY1/CURRENCY2.")]
    [InlineData("Exchange /EUR 100", "Main currency cannot be empty.")]
    [InlineData("Exchange USD/ 100", "Incoming currency cannot be empty.")]
    [InlineData("Exchange USD/EUR abc", "Invalid amount. Please enter a valid number.")]
    [InlineData("Exchange USD/EUR -100", "Amount cannot be negative.")]
    public void Validate_InvalidInput_ReturnsSpecificError(
        string input,
        string expectedError)
    {
        var result = _inputValidationService.Validate(input);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(expectedError);
    }
}