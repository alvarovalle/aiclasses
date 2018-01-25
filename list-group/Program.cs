using System;
using System.Net.Http.Headers;
using System.Net.Http;

namespace CSHttpClientSample
{
    static class Program
    {
        const string subscriptionKey = "< put your subscription ley here >";
        static void Main()
        {
            Console.WriteLine("Listing every Person Group In your Account:");
            MakeRequest();
            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups";
            string response = await client.GetStringAsync(uri);
            Console.WriteLine(response);
        }
    }
}   