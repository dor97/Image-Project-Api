using System.ComponentModel.DataAnnotations;

namespace projectServer.Models
{
    public class ImageUploadModel
    {
        [Required]
        public IFormFile SampleImage { get; set; }

        public ImageUploadModel(IFormFile sampleImage)
        {
            SampleImage = sampleImage;
        }
    }
}
