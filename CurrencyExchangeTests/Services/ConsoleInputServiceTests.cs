using CurrencyExchange.Services;

namespace CurrencyExchangeTests.Services
{
    public class ConsoleInputServiceTests
    {
        private readonly ConsoleInputService _inputService;

        public ConsoleInputServiceTests()
        {
            _inputService = new ConsoleInputService();
        }

        [Fact]
        public void MapInputToConsoleCommand_ValidInput_ReturnsConsoleCommand()
        {
            var input = "Exchange EUR/DKK 100";
            var expectedAmount = 100M;

            var result = _inputService.ParseExchangeCommand(input);

            Assert.Equal(expectedAmount, result.Amount);
        }

        [Fact]
        public void MapInputToConsoleCommand_InvalidCommandFormat_ThrowsArgumentException()
        {
            var input = "InvalidCommand EUR/DKK 100";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }

        [Fact]
        public void MapInputToConsoleCommand_InvalidCurrencyPairFormat_ThrowsArgumentException()
        {
            var input = "Exchange InvalidCurrencyPair 100";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }

        [Fact]
        public void MapInputToConsoleCommand_InvalidAmountFormat_ThrowsArgumentException()
        {
            var input = "Exchange EUR/DKK InvalidAmount";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }

        [Fact]
        public void MapInputToConsoleCommand_FewerParts_ThrowsArgumentException()
        {
            var input = "Exchange EUR/DKK";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }

        [Fact]
        public void MapInputToConsoleCommand_MoreParts_ThrowsArgumentException()
        {
            var input = "Exchange EUR/DKK 100 ExtraPart";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }

        [Fact]
        public void MapInputToConsoleCommand_IncorrectCurrencyPairDelimiter_ThrowsArgumentException()
        {
            var input = "Exchange EUR_DKK 100";

            Assert.Throws<ArgumentException>(() => _inputService.ParseExchangeCommand(input));
        }
    }
}
