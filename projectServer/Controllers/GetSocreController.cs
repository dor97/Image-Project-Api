﻿using Microsoft.AspNetCore.Mvc;
using projectServer.DTOs;
using projectServer.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace projectServer.Controllers
{
    [ApiController]
    [Route("api/getsocre")]
    public class GetSocreController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> getSocreForImage(ImageUploadDto imageUploadDto)
        {
            if (imageUploadDto == null || imageUploadDto.sampleImage == null || 
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                return BadRequest("No image sent in form");
            }

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel();

            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestVersion = new Version(1, 1);

                string url = "http://localhost:8000/test"; //localhost   /   super-sicret-project

                using (var content = new MultipartFormDataContent())
                {
                    StreamContent imageContent = new StreamContent(imageUploadModel.sampleImage.OpenReadStream());
                    //imageContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue();

                    // Add the stream content to the multipart content with a specified form field name
                    content.Add(imageContent, "file", imageUploadModel.sampleImage.FileName);

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    return Ok(responseBody);
                }
            }
        }
    }
}