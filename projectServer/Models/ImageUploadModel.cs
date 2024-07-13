using System.ComponentModel.DataAnnotations;

namespace projectServer.Models
{
    public class ImageUploadModel
    {
        [Required]
        public IFormFile SampleImage { get; set; }
        public string UserName { get; set; } = "All";
        public float Score { get; set; } = -1;
        public DateTime date { get; set; }
    }
}
