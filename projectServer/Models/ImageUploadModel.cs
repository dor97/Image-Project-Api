using System.ComponentModel.DataAnnotations;

namespace projectServer.Models
{
    public class ImageUploadModel
    {
        [Required]
        public IFormFile? sampleImage { get; set; }
    }
}
