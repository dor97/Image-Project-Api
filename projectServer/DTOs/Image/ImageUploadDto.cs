using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace projectServer.DTOs.Image
{
    public class ImageUploadDto
    {
        [Required]
        public IFormFile sampleImage { get; set; }
        //public string? UserName { get; set; } = "All";
        [Required]
        public DateTime date { get; set; } = DateTime.Now;
    }
}
