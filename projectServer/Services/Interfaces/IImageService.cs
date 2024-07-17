using projectServer.Models.Image;

namespace projectServer.Services.Interfaces
{
    public interface IImageService
    {
        Task<SampleImageModel> saveImage(ImageUploadModel imageUploadModel);
        FileStream GetUserFile(string filePath);
        Task<float> GetImageScore(string ScoreingApiUrl, ImageUploadModel imageUploadModel);
    }
}
