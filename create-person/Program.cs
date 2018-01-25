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
            Console.WriteLine("Enter an ID for the group you wish to create a person in :");
            string personGroupId = Console.ReadLine();
            Console.WriteLine("Enter Person Name");
            string personName = Console.ReadLine();
            Console.WriteLine("Enter Person Description");
            string description = Console.ReadLine();
            MakeRequest(personGroupId, personName, description);

            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }


        static async void MakeRequest(string personGroupId, string  personName, string description)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons";
            Console.WriteLine(uri);            
            string json = "{\"name\":\""+ personName+"\", \"userData\":\""+description+"\"}";
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);
            Console.WriteLine("Response status: " + response.StatusCode);
        }
    }
}