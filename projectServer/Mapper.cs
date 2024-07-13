using projectServer.DTOs;
using projectServer.Models;
using System.Runtime.CompilerServices;

namespace projectServer
{
    public static class Mapper
    {
        public static ImageUploadModel imageUploadDtoToModel(this ImageUploadDto imageUploadDto)
        {
            ImageUploadModel imageUploadModel = new ImageUploadModel()
            {
                SampleImage = imageUploadDto.sampleImage,
                UserName = imageUploadDto.UserName,
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
                ImageUrl = $"{Url}/api/scoreimage/{sampleImageModel.UserName}/{sampleImageModel.Id}",
                UserName = sampleImageModel.UserName,
                Score = sampleImageModel.Score,
                date = sampleImageModel.date
            };

            return imageDataDto;
        }
    }
}
