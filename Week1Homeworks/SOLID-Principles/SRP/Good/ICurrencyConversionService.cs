namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.SRP.Good;

internal interface ICurrencyConversionService
{
    Task<decimal> ConvertCurrencyAsync(decimal price, Currency from, Currency to);
}