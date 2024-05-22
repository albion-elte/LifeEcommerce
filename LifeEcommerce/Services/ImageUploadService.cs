using Amazon.S3;
using Amazon.S3.Model;
using LifeEcommerce.Helpers.Models;
using LifeEcommerce.Models;
using Newtonsoft.Json;

namespace LifeEcommerce.Services
{
    public class ImageUploadService
    {
        public async Task<string> UploadPicture(IFormFile file, IConfiguration configuration)
        {
            #region Imgur
            //// Replace with your actual Imgur API key
            //const string ImgurApiKey = "d64aa6e883b4821";

            //var imageUrl = UploadToImgur(imagePath, ImgurApiKey);

            //return imageUrl.Result;

            #endregion

            var uploadPicture = await UploadToBlob(file, configuration);

            //var blobConfiguration = configuration.GetSection(nameof(BlobConfiguration)).Get<BlobConfiguration>();


            var imageUrl = $"https://gjirafatechiam.eu-1.cdn77-storage.com/LIFE/{file.FileName}";

            return imageUrl;
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

        public async Task<PutObjectResponse> UploadToBlob(IFormFile file, IConfiguration configuration)
        {
            var blobConfiguration = configuration.GetSection(nameof(BlobConfiguration)).Get<BlobConfiguration>();

            var config = new AmazonS3Config()
            {
                ServiceURL = blobConfiguration.ServiceUrl
            };

            var s3client = new AmazonS3Client(blobConfiguration.AccessKey, blobConfiguration.SecretKey, config);

            var keyName = $"{blobConfiguration.DefaultFolder}{file.FileName}";

            var fileStream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = blobConfiguration.BucketName,
                Key = keyName,
                InputStream = fileStream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            return await s3client.PutObjectAsync(request);
        }
    }
}

