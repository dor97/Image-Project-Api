using System.ComponentModel.DataAnnotations;

namespace projectServer.DTOs.Image
{
    public class ImageUpdateDto
    {
        [Required]
        public DateTime date { get; set; } = DateTime.Now;
        [Required]
        [Range(0, 1)]
        public float Score { get; set; } = -1;
    }
}
