using LifeEcommerce.Models;
using Newtonsoft.Json;

namespace LifeEcommerce.Services
{
    public static class ImageUploadService
    {

        public static string UploadPicture(string imagePath)
        {
            // Replace with your actual Imgur API key
            const string ImgurApiKey = "YOUR_API_KEY";

            var imageUrl = UploadToImgur(imagePath, ImgurApiKey);

            return imageUrl.Result;
        }

        public static async Task<string> UploadToImgur(string imagePath, string apiKey)
        {
            using (var client = new HttpClient())
            {

                var authHeaderValue = "Client-ID {d64aa6e883b4821}";
                client.DefaultRequestHeaders.Add("Authorization", authHeaderValue);

                byte[] imageData;
                using (FileStream fileStream = File.OpenRead(imagePath))
                {
                    imageData = new byte[fileStream.Length];
                    await fileStream.ReadAsync(imageData, 0, imageData.Length);
                }

                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(imageData), "image", "image.jpg");

                var response = await client.PostAsync("https://api.imgur.com/3/image", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<Root>(responseContent);

                return responseModel.data.link;
            }
        }
    }
}

