using System.ComponentModel.DataAnnotations;

namespace projectServer.DTOs
{
    public class ImageUploadDto
    {
        [Required]
        public IFormFile? sampleImage { get; set; }
    }
}
