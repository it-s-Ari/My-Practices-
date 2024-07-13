using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

/*

This async program gets input from a user for a group of crypto contracts (e.g., NOTUSDT), then fetches prices from two 
    exchanges, Binance and KuCoin, then shows the price from each and the average. It is async in order to maximize aaccuracy
    by avoiding lags.
cons: no strong error handling. This can be improved.

*/


namespace Crypto_Bot
{
    class Program
    {
       class Crypto
        {
            public List<string> symbols = new List<string>();
            public string this[int index]
            {
                get { return symbols[index]; }

                set { symbols.Add(value); }
            }
            public async Task FetchPrice(string symbol)
            {
                try
                {
                    var tasks = new List<Task<decimal>>
                        {
                      FetchFromKuCoin(symbol),
                      FetchFromBinance(symbol)
                         };
                    var prices = await Task.WhenAll(tasks);
                    var priceList = prices.ToList();
                    Console.WriteLine($"{symbol} price fetched from two sources. Average price: {priceList.Average()}");
                    Console.WriteLine($"\tBinance Price: {priceList[1]}\n\tKuCoin Price: {priceList[0]}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Could not Fetch");
                    Console.WriteLine(e.Message);
                }
            }

            private async Task<decimal> FetchFromBinance(string symbol)
            {
                string url = $"https://api.binance.com/api/v3/ticker/price?symbol={symbol}"; // e.g. symbol "BNBUSDT"
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        JsonElement root = document.RootElement;
                        
                        return decimal.Parse(root.GetProperty("price").GetString().Replace('.', ','));
                    }
                }
                else
                {
                    Console.WriteLine("Could not fetch from Binance.");
                    Console.WriteLine(response.StatusCode);
                    return 0;
                }
            }
            private async Task<decimal> FetchFromKuCoin(string symbol)
            {
                string fixedSymbol = symbol.Substring(0, symbol.Length - 4) + '-' + symbol.Substring(symbol.Length - 4);
                string url = $"https://api.kucoin.com/api/v1/market/orderbook/level1?symbol={fixedSymbol}"; // e.g. symbol "NOT-USDT"
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(body))
                    {
                        JsonElement root = document.RootElement;
                       
                        return decimal.Parse(root.GetProperty("data").GetProperty("price").GetString().Replace('.', ','));
                    }
                }
                else
                {
                    Console.WriteLine("Could not fetch from KuCoin.");
                    Console.WriteLine(response.StatusCode);
                    return 0;        
                }
            }
        }       
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the name of the symbols:");
            Crypto cryptos = new Crypto();
            int index = 0;
            var symbols = Console.ReadLine().ToUpper().Split().ToList();
            foreach (string symbol in symbols)
            {
                cryptos[index] = symbol;
                index++;
                await cryptos.FetchPrice(symbol);
            }
           Console.Read();
        }
    }
}
