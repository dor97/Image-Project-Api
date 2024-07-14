using Microsoft.AspNetCore.Mvc;
using projectServer.DTOs;
using projectServer.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using projectServer.Services.Interfaces;
using projectServer.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace projectServer.Controllers
{
    [ApiController]
    [Route("api/scoreimage")]
    [EnableCors("AllowSpecificOrigin")]
    public class GetSocreController : ControllerBase
    {
        private readonly ILogger<GetSocreController> _logger;
        private readonly IImageService _imageService;
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly IConfiguration _configuration;

        public GetSocreController(ILogger<GetSocreController> logger, IImageService imageService, ApplicationDBContext aplicationDBContext,
            IConfiguration configuration)
        {
            _logger = logger;
            _imageService = imageService;
            _applicationDBContext = aplicationDBContext;
            _configuration = configuration;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult> GetAll([FromRoute] string userName)
        {
            IList<SampleImageModel> sampleImageModels = await _applicationDBContext.ImagesUpload
                .Where(ImageSample => ImageSample.UserName == userName).ToListAsync();


            var results = new List<ImageDataDto>();

            foreach (var sampleImageModel in sampleImageModels)
            {
                ImageDataDto imageDataDto = sampleImageModel.sampleImageModelToImageDataDto(_configuration.GetValue<string>("WebHostUrl"));

                results.Add(imageDataDto);
            }

            return Ok(results);          
        }

        [HttpGet("{userName}/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string userName, [FromRoute] int id)
        {
            SampleImageModel? sampleImageModel = await _applicationDBContext.ImagesUpload.FindAsync(id);

            if(sampleImageModel == null)
            {
                return NotFound();
            }

            FileStream fileStream = _imageService.GetUserFile(sampleImageModel.ImagePath);
           
            return Ok(fileStream);
        }


        [HttpPost]
        public async Task<ActionResult<ImageDataDto>> postImage([FromForm]ImageUploadDto imageUploadDto)
        {
            _logger.LogInformation("In get score for image method");

            if (imageUploadDto == null || imageUploadDto.sampleImage == null || 
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                _logger.LogError("No image sent in form");
                return BadRequest("No image sent in form");
            }

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel();

            try
            {
                float score = await _imageService.GetImageScore(_configuration.GetValue<string>("ScoringApiUrl"), imageUploadModel);

                _logger.LogInformation($"result: {score}");

                imageUploadModel.Score = score;

                SampleImageModel sampleImageModel = await _imageService.saveImage(imageUploadModel);

                await _applicationDBContext.ImagesUpload.AddAsync(sampleImageModel);

                await _applicationDBContext.SaveChangesAsync();

                ImageDataDto imageDataDto = sampleImageModel.sampleImageModelToImageDataDto(_configuration.GetValue<string>("WebHostUrl"));

                return Ok(imageDataDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in getting image score from model: {ex.Message}");

                return StatusCode(500, "Sorry: there is a problem with the server!");
            }
        }

        [HttpPost("/NoSave")]
        public async Task<ActionResult<ImageDataDto>> getSocreForImage([FromForm]ImageUploadDto imageUploadDto)
        {
            _logger.LogInformation("In get score for image method");

            if (imageUploadDto == null || imageUploadDto.sampleImage == null ||
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                _logger.LogError("No image sent in form");
                return BadRequest("No image sent in form");
            }

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel();

            try
            {
                float score = await _imageService.GetImageScore(_configuration.GetValue<string>("ScoringApiUrl"), imageUploadModel);

                _logger.LogInformation($"result: {score}");

                imageUploadModel.Score = score;

                return Ok(score);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getting image score from model: {ex.Message}");

                return StatusCode(500, "Sorry: there is a problem with the server!");
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> upadte([FromRoute] int id, [FromForm] ImageUpdateDto imageUpdateDto)
        {
            SampleImageModel sampleImageModel = await _applicationDBContext.ImagesUpload.FirstOrDefaultAsync(x => x.Id == id);

            if (sampleImageModel == null)
            {
                return NotFound();
            }

            //sampleImageModel.UserName = imageUpdateDto.UserName;
            sampleImageModel.date = imageUpdateDto.date;
            sampleImageModel.Score = imageUpdateDto.Score;

            await _applicationDBContext.SaveChangesAsync();

            return Ok(sampleImageModel.sampleImageModelToImageDataDto(_configuration.GetValue<string>("WebHostUrl")));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> delete([FromRoute] int id)
        {
            SampleImageModel sampleImageModel = await _applicationDBContext.ImagesUpload.FirstOrDefaultAsync(x => x.Id == id);

            if (sampleImageModel == null)
            {
                return NotFound();
            }

            _applicationDBContext.ImagesUpload.Remove(sampleImageModel);

            await _applicationDBContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
