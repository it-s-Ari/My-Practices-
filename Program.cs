using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;
namespace CryptoBotV2
{
    class Program
    {


        class CryptoBot : IDisposable
        {
            private bool disposed = false;

            // Implement IDisposable.
            public void Dispose()
            {
                disposed = true;
                GC.SuppressFinalize(this);
            }
            bool auth = false;
            string key;
            string secret;
            string passphrase;
            string baseURL = "https://api-futures.kucoin.com"; // Base URL for "Futures" contracts
            Dictionary<string, dynamic> authHeaders = new Dictionary<string, dynamic>
            {
                {"KC-API-KEY", ""},
                {"KC-API-SIGN", "" },
                {"KC-API-TIMESTAMP", "" },
                {"KC-API-PASSPHRASE", "" },
                {"KC-API-KEY-VERSION","3" }
                

            };
            string body = ""; // in json format
            string method = "GET";
            private long timestamp;
            public CryptoBot()
            {
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            }
            async private Task UpdateTimeStamp()
            {
               timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
               
            }
            async Task SendRequest(HttpClient headers, string url)
            {
                HttpResponseMessage response = await headers.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    auth = true;
                }
                else
                {
                    Console.WriteLine("Authentication failed.");
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
            async Task Auth()
            {
                if (auth)
                {
                    Console.WriteLine("Already authenticated. Different user? Please close and reopen the program.");
                }
                else
                {


                    Console.WriteLine("Key: ");
                    key = Console.ReadLine();
                    authHeaders["KC-API-KEY"] = key;
                    Console.WriteLine("Secret: ");
                    secret = Console.ReadLine();
                    Console.WriteLine("Passphrase: ");
                    passphrase = Console.ReadLine();
                    await UpdateTimeStamp();
                    byte[] secretInByte = Encoding.UTF8.GetBytes(secret);
                    byte[] data = Encoding.UTF8.GetBytes(timestamp + method + "https://api.kucoin.com/api/v1/accounts" + body);
                    using (var hmac = new HMACSHA256(secretInByte))
                    {
                        byte[] hmacBytes = hmac.ComputeHash(data);
                        authHeaders["KC-API-SIGN"] = Convert.ToBase64String(hmacBytes);
                    }
                    using (var hmac = new HMACSHA256(secretInByte))
                    {
                        byte[] hmacBytes = hmac.ComputeHash(data);
                        authHeaders["KC-API-PASSPHRASE"] = Convert.ToBase64String(hmacBytes);
                    }
                    authHeaders["KC-API-TIMESTAMP"] = timestamp;
                    var headers = new HttpClient();
                    foreach (var header in authHeaders)
                    {
                        headers.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    await SendRequest(headers, "https://api.kucoin.com/api/v1/accounts");
                }
            }
            async Task Long (string contract, double amount)
            {

            }
            async Task Short (string contract, double amount)
            {

            }
            async Task Fetch (string symbol)
            {

            }
        }
        static void Main(string[] args)
        {
        }
    }
}
