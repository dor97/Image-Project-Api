using projectServer.DTOs.Image;
using projectServer.DTOs.Account;
using System.Runtime.CompilerServices;
using projectServer.Models.Image;
using projectServer.Models.Account;

namespace projectServer
{
    public static class Mapper
    {
        public static ImageUploadModel imageUploadDtoToModel(this ImageUploadDto imageUploadDto, UserModel appUser)
        {
            ImageUploadModel imageUploadModel = new ImageUploadModel()
            {
                SampleImage = imageUploadDto.sampleImage,
                UserName = appUser?.UserName ?? "",
                userId = appUser?.Id ?? "",
                date = imageUploadDto.date
            };

            //imageUploadModel.SampleImage = imageUploadDto.sampleImage;

            return imageUploadModel;
        }

        public static ImageDataDto sampleImageModelToImageDataDto(this SampleImageModel sampleImageModel, string Url)
        {
            ImageDataDto imageDataDto = new ImageDataDto()
            {
                id = sampleImageModel.Id,
                ImageUrl = $"{Url}/api/scoreimage/{sampleImageModel.Id}",
                UserName = sampleImageModel.UserName,
                Score = sampleImageModel.Score,
                date = sampleImageModel.date
            };

            return imageDataDto;
        }

        public static SampleImageModel ImageUpdateDtoToSampleImageModel(ImageUpdateDto imageUpdateDto)
        {
            SampleImageModel sampleImageModel = new SampleImageModel()
            {
                Id = imageUpdateDto.Id,
                UserName = imageUpdateDto.UserName,
                Score = imageUpdateDto.Score,
                date = imageUpdateDto.date
            };

            return sampleImageModel;
        }
    }
}
