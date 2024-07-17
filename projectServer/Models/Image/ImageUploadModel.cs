using System.ComponentModel.DataAnnotations;

namespace projectServer.Models.Image
{
    public class ImageUploadModel
    {
        [Required]
        public IFormFile SampleImage { get; set; }
        public string UserName { get; set; } = "All";
        public string userId { get; set; }
        public float Score { get; set; } = -1;
        public DateTime date { get; set; }
    }
}
