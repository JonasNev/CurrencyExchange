using CurrencyExchange.Models;

namespace CurrencyExchange.Interfaces;

public interface IInputService
{
    ConsoleCommand ParseInput(string input);
}