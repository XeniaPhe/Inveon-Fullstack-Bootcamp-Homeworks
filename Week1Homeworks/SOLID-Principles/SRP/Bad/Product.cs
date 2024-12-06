using System.Text;

namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.SRP.Bad;

/*
 * Why does this class violate the Single Responsibility Principle?
 * 
 * Aspect 1- Separation Of Concerns:
 *      The Product class does two separate things: It houses product-related logic along with currency rate conversion logic.
 *      A class should ideally have only one reason to change, and this class might now have to be changed for two different reasons.
 *      Currency conversion logic doesn't belong in this class as other classes may also need to do currency conversion,
 *      which would lead to duplicate code. Therefore currency conversion should ideally be handled in a service or static utility class/method
 *      depending on the situation. Even if this was the only class that needed currency conversion, the logic still might change.
 *      Later on, it could be that some currencies are disallowed in certain scenarios or some other logic about buying/selling
 *      rates of currencies might have to be added. Even if we made sure that this is the only class that needs currency conversion
 *      and that the conversion logic itself will never change, it would still violate the SRP principle due to separation of concerns.
 *      It's almost always a good practice to separate different concerns.
 *      (PS: This may have sounded a little dogmatic but that's just an example to convey the idea. )
 *      
 * Aspect 2- Depending on External APIs:
 *      Ignoring the separation of concerns, this class still violates the SRP principle because it directly depends on an external API.
 *      Libraries, web services, methods, or classes that are maintained outside your or your team's control are considered external APIs.
 *      Since this class depends on this currency API, it now has to change everytime the API is changed. Url of the endpoints,
 *      expected parameter type or names in the urls, response bodies might all change. These changes would break this class and would have
 *      to be reflected in it. So, this class now has 3 reasons to change:
 *          1-Product-related logic might change
 *          2-Currency conversion logic might change
 *          3-Currency rate API might change
 *      
 *      Ideally, there should be a facade class that is dedicated to handling the passing of the requests and parsing of the responses
 *      of this external API. This class would provide a common and simple interface to retrieve conversion rates between currencies.
 *      Since this class'es sole responsibility is to handle the communication between the program and the external API, it would be
 *      fine even when the API is changed. Because this facade class isolates the external API from the rest of the system,
 *      changes in the API would only require modifications to the facade class, leaving the rest of the application unaffected.
 */

internal class Product
{
    internal string ProductCode { get; init; }
    internal string ProductName { get; init; }
    internal Currency Currency { get; init; }
    internal decimal Price { get; init; }


    internal Product(string productCode, string productName, Currency currency, decimal price)
    {
        this.ProductCode = productCode;
        this.ProductName = productName;
        this.Currency = currency;
        this.Price = price;
    }

    internal async Task<decimal> GetPriceInCurrency(Currency currency)
    {
        decimal rate = 1M;

        if (this.Currency != currency)
        {
            HttpClient client = new HttpClient();
            string url = $"http://localhost:5000/api/exchange-rate?from={this.Currency.ToString().ToLower()}&to={currency.ToString().ToLower()}/";
            var response = await client.GetAsync(url);
            rate = Convert.ToDecimal(await response.Content.ReadAsStringAsync());
        }

        return Price * rate;
    }
}