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
            Console.WriteLine("Enter an ID for the group do you want to list Persons:");

            string personGroupId = Console.ReadLine();
            MakeRequest(personGroupId);

            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }


        static async void MakeRequest(string personGroupId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons";
            Console.WriteLine(uri);            

            string response = await client.GetStringAsync(uri);
            Console.WriteLine(response);
        }
    }
}