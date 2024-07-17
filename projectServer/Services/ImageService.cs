using projectServer.Models.Image;
using projectServer.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace projectServer.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<SampleImageModel> saveImage(ImageUploadModel imageUploadModel)
        {
            if (imageUploadModel.SampleImage != null && imageUploadModel.SampleImage.Length > 0)
            {
                // Ensure the user's uploads directory exists
                var userFolder = Path.Combine(_environment.ContentRootPath, "uploads", imageUploadModel.UserName); //_environment.WebRootPath
                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                // Create a unique file name to avoid overwriting
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageUploadModel.SampleImage.FileName;
                var filePath = Path.Combine(userFolder, uniqueFileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageUploadModel.SampleImage.CopyToAsync(stream);
                }

                SampleImageModel sampleImageModel = new SampleImageModel()
                {
                    ImagePath = filePath,
                    UserName = imageUploadModel.UserName,
                    UserId = imageUploadModel.userId,
                    Score = imageUploadModel.Score,
                    date = imageUploadModel.date
                };
                return sampleImageModel;
            }

            throw new ArgumentException("Invalid image");
        }

        public FileStream GetUserFile(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //var filePath = Path.Combine(_environment.WebRootPath, "uploads", userName, fileName);

            /*
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var mimeType = GetMimeType(filePath);

            return (fileBytes, mimeType, Path.GetFileName(filePath));

            */
        }

        public string GetMimeType(string fileName)
        {            
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };
        }

        public async Task<float> GetImageScore(string ScoreingApiUrl, ImageUploadModel imageUploadModel)
        {
            using (HttpClient client = new HttpClient())
            {

                string url = $"{ScoreingApiUrl}/test";

                using (var content = new MultipartFormDataContent())
                {
                    using (StreamContent imageContent = new StreamContent(imageUploadModel.SampleImage.OpenReadStream()))
                    {
                        // Add the stream content to the multipart content with a specified form field name
                        content.Add(imageContent, "file", imageUploadModel.SampleImage.FileName);

                        HttpResponseMessage response = await client.PostAsync(url, content);

                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (float.TryParse(responseBody, out float score))
                        {
                            return (float)Math.Round(score, 2);
                        }
                        else
                        {
                            throw new ArgumentException("Model returned invalid answer");
                        }
                    }                    
                }
            }
        }
    }
}
