namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.SRP.Good;
internal static class CurrencyUtils
{
    internal static async Task<decimal> ConvertCurrencyAsync(decimal price, Currency from, Currency to)
    {
        decimal rate = 1M;

        if (from != to)
        {
            HttpClient client = new HttpClient();
            string url = $"http://localhost:5000/api/exchange-rate?from={from.ToString().ToLower()}&to={to.ToString().ToLower()}/";
            var response = await client.GetAsync(url);
            rate = Convert.ToDecimal(await response.Content.ReadAsStringAsync());
        }

        return price * rate;
    }
}