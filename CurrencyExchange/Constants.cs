namespace CurrencyExchange;

public static class Constants
{
    public static class Program
    {
        public const string EndText = "end";
    }

    public static class InputCommand
    {
        public const string Exchange = "Exchange";
        public const int ExpectedPartsCount = 3;
        public const char CurrencyPairSeparator = '/';
    }

    public static class ExchangeRate
    {
        public const decimal DefaultRate = 1.0M;
        public const int RatePrecision = 4;
    }
}