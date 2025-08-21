namespace CurrencyExchange.Models;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
    public string ErrorMessage => string.Join(" ", Errors);
}