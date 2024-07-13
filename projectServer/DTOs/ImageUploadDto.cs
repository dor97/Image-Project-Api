﻿using System.ComponentModel.DataAnnotations;

namespace projectServer.DTOs
{
    public class ImageUploadDto
    {
        [Required]
        public IFormFile sampleImage { get; set; }
        public string? UserName { get; set; } = "All";
        public DateTime date { get; set; }
    }
}
