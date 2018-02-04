using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSHttpClientSample
{
    static class Program
    {
        const string subscriptionKey = "<your token goes here>";
                const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";
        static void Menu(){
            Console.WriteLine("\n\n\n1-For List Persons\n\n\n2-Load Photos into the cloud\n\n\n3-To Train\n\n\n4-Check Photo\n\n\n5-Forget Faces");
            string resp = Console.ReadLine();
            if(resp == "1"){
                ListPersonRequest("jazz");
            }
            if(resp == "2"){
                LoadPhotoRequest("jazz");
            }
            if(resp == "3"){
                TrainRequest("jazz");
            }            
            if(resp == "4"){
                Console.WriteLine("\n\n\nType Image Path\n\n\n");
                string image_path = Console.ReadLine();
                MakeAnalysisRequest("jazz", image_path);
            }
            if(resp == "5"){
                ClearPersistedFacesRequest("jazz");
            }
        }
        static void Main()
        {
            Menu();

            Console.WriteLine("\n\n\nWait for the result below, then hit ENTER to exit...\n\n\n");
            Console.ReadLine();
        }


        static async void ClearPersistedFacesRequest(string personGroupId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons";
            Console.WriteLine(uri);            

            string response = await client.GetStringAsync(uri);

            List<PersonModel> players = JsonConvert.DeserializeObject<List<PersonModel>>(response);

            foreach(PersonModel player in players){

                Console.WriteLine("Deleting "+player.name+" Faces"); 
                foreach(string  persistedFaceId in player.persistedFaceIds){
                  DeletePersistedFaceId(personGroupId, player.personId, persistedFaceId);
                }
                Console.WriteLine("-----------------------------");
            }
        }


        static async void ListPersonRequest(string personGroupId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons";

            string response = await client.GetStringAsync(uri);

            List<PersonModel> players = JsonConvert.DeserializeObject<List<PersonModel>>(response);

            foreach(PersonModel player in players){
                Console.WriteLine(player.name);
                Console.WriteLine(player.personId);
                Console.WriteLine("-----------------------------");
            }
            Console.WriteLine("Raw Data");
            Console.WriteLine("-----------------------------");
            Console.WriteLine(response);
        }

        static async void TrainRequest(string personGroupId){
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/train";
            HttpContent content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            Console.WriteLine("Response status: " + response.StatusCode);
        }
        //Change this method so you can read from your own sources
        static async void LoadPhotoRequest(string personGroupId){
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons";
            Console.WriteLine(uri);            

            string response = await client.GetStringAsync(uri);
            List<PersonModel> players = JsonConvert.DeserializeObject<List<PersonModel>>(response);

            foreach(PersonModel player in players){
                Console.WriteLine(player.name);
                Console.WriteLine(player.personId);
                for(int x =1;x<=3;x++){
                   string urlImage = "http://alvarovalle.com/images/faces/"+player.name+"-0"+x.ToString()+".jpg";//Change this 
                   Console.WriteLine("Loading Image From URL : "+urlImage);
                   LoadPhotoRequestCore(personGroupId, player.personId, urlImage);
                   Console.WriteLine("-----------------------------");
                }
                
            }

        }


        static async void DeletePersistedFaceId(string personGroupId, string personId, string persistedFaceId)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons/"+personId+"/persistedFaces/"+persistedFaceId;
            //Console.WriteLine(uri);            
            HttpResponseMessage response = await client.DeleteAsync(uri);

            Console.WriteLine("Response status: " + response.StatusCode);
        }

        static async void LoadPhotoRequestCore(string personGroupId, string personId, string urlImage)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/persongroups/" + personGroupId + "/persons/"+personId+"/persistedFaces";
            //Console.WriteLine(uri);            
            string json = "{\"url\":\""+urlImage+"\"}";
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);

            Console.WriteLine("Response status: " + response.StatusCode);
        }

        static async void MakeAnalysisRequest(string personGroupId, string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                List<DetectModel> players = JsonConvert.DeserializeObject<List<DetectModel>>(contentString);
                
                //Console.WriteLine(contentString);
                if(contentString != "[]"){
                  CheckPhotoRequest(personGroupId,players[0].faceId);
                }else{
                  Console.WriteLine("Image is not well formed");
                }

            }
        }

        static async void CheckPhotoRequest(string personGroupId, string personId)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            string uri = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/identify";
            string json = "{\"personGroupId\":\""+personGroupId+"\",\"faceIds\":[\""+personId+"\"   ],\"maxNumOfCandidatesReturned\":2,\"confidenceThreshold\": 0.5}";
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(uri, content);
            string contentString = await response.Content.ReadAsStringAsync(); //right!
            List<CheckPhotoModel> players = JsonConvert.DeserializeObject<List<CheckPhotoModel>>(contentString);
          //  Console.WriteLine(contentString);
            Console.WriteLine("PersonId Best Candidate : " + players[0].candidates[0].personId);
            //Console.WriteLine("PersonId Best Candidate : " + players[0].candidates.Count);
            ListPersonRequest(personGroupId);
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

    }
}