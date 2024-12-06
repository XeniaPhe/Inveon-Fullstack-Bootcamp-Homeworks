using System.Text;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.SRP.Good;

internal class Product
{
    private ICurrencyConversionService conversionService;

    internal string ProductCode { get; init; }
    internal string ProductName { get; init; }
    internal Currency Currency { get; init; }
    internal decimal Price { get; init; }


    internal Product(string productCode, string productName, Currency currency, decimal price, ICurrencyConversionService conversionService)
    {
        this.ProductCode = productCode;
        this.ProductName = productName;
        this.Currency = currency;
        this.Price = price;
        this.conversionService = conversionService;
    }

    //There is no need to await here, we can simply delegate the resulting task to the caller method.
    internal Task<decimal> GetPriceInCurrency(Currency currency)
    {
        return conversionService.ConvertCurrencyAsync(Price, this.Currency, currency);
    }

    //Alternatively, a delegate could also be used:
    internal decimal GetPriceInCurrency(Currency currency, Func<Currency, Currency, decimal, decimal> currencyConverter)
    {
        return currencyConverter(this.Currency, currency, Price);
    }
}