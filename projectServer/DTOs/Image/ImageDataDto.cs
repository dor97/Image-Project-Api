using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace projectServer.DTOs.Image
{
    public class ImageDataDto
    {
        public int id {  get; set; }
        public string ImageUrl { get; set; }
        public FileStream? SampleImage { get; set; }
        public string UserName { get; set; } = "All";
        public float Score { get; set; } = -1;
        public DateTime date { get; set; }
    }
}
