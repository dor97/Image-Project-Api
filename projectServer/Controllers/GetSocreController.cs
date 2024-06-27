using Microsoft.AspNetCore.Mvc;
using projectServer.DTOs;
using projectServer.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

namespace projectServer.Controllers
{
    [ApiController]
    [Route("api/getsocre")]
    public class GetSocreController : ControllerBase
    {
        private readonly ILogger<GetSocreController> _logger;

        public GetSocreController(ILogger<GetSocreController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> getSocreForImage(ImageUploadDto imageUploadDto)
        {
            _logger.LogInformation("In get score for image method");

            if (imageUploadDto == null || imageUploadDto.sampleImage == null || 
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                _logger.LogError("No image sent in form");
                return BadRequest("No image sent in form");
            }

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel();

            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestVersion = new Version(1, 1);

                string url = "http://super-sicret-project:8000/test"; //localhost   /   super-sicret-project

                using (var content = new MultipartFormDataContent())
                {
                    StreamContent imageContent = new StreamContent(imageUploadModel.sampleImage.OpenReadStream());
                    //imageContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue();

                    // Add the stream content to the multipart content with a specified form field name
                    content.Add(imageContent, "file", imageUploadModel.sampleImage.FileName);

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"result: {responseBody}");

                    return Ok(responseBody);
                }
            }
        }
    }
}
