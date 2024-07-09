using projectServer.DTOs;
using projectServer.Models;
using System.Runtime.CompilerServices;

namespace projectServer
{
    public static class Mapper
    {
        public static ImageUploadModel imageUploadDtoToModel(this ImageUploadDto imageUploadDto)
        {
            ImageUploadModel imageUploadModel = new ImageUploadModel(imageUploadDto.sampleImage);

            //imageUploadModel.SampleImage = imageUploadDto.sampleImage;

            return imageUploadModel;
        }
    }
}
