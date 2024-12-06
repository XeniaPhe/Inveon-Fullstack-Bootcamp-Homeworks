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
    internal Task<decimal> GetPriceInCurrencyAsync(Currency currency)
    {
        return conversionService.ConvertCurrencyAsync(Price, this.Currency, currency);
    }

    //Alternatively, a delegate could also be used:
    internal Task<decimal> GetPriceInCurrencyAsync(Currency currency, Func<Currency, Currency, decimal, Task<decimal>> currencyConverter)
    {
        return currencyConverter(this.Currency, currency, Price);
    }

    //Even a custom/named delegate could be used for clarity:
    internal delegate Task<decimal> CurrencyConverter (decimal price, Currency from, Currency to);
    internal Task<decimal> GetPriceInCurrencyAsync(Currency currency, CurrencyConverter currencyConverter)
    {
        return currencyConverter(Price, this.Currency, currency);
    }

    //Another approach is to use a static utility method. However this would make testing harder for more complex types.
    //This method is also not as flexible as the other more functional or object oriented approaches since you can't swap out
    //the implementations as easily.
    internal Task<decimal> getPriceInCurrencyAsync(Currency currency)
    {
        return CurrencyUtils.ConvertCurrencyAsync(Price, this.Currency, currency);
    }
}