using Microsoft.AspNetCore.Mvc;
using projectServer.DTOs.Image;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using projectServer.Services.Interfaces;
using projectServer.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using api.Extensions;
using Microsoft.AspNetCore.Identity;
using projectServer.Models.Image;
using projectServer.Models.Account;

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
        private readonly UserManager<UserModel> _userManager;

        public GetSocreController(ILogger<GetSocreController> logger, IImageService imageService, ApplicationDBContext aplicationDBContext,
            IConfiguration configuration, UserManager<UserModel> userManager)
        {
            _logger = logger;
            _imageService = imageService;
            _applicationDBContext = aplicationDBContext;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            string username = User.GetUserName();
            UserModel appUser = await _userManager.FindByNameAsync(username);
            IList<SampleImageModel> sampleImageModels = await _applicationDBContext.ImagesUpload
                .Where(u => u.UserId == appUser.Id).ToListAsync();            
            //IList<SampleImageModel> sampleImageModels = await _applicationDBContext.ImagesUpload
            //    .Where(ImageSample => ImageSample.UserName == userName).ToListAsync();


            var results = new List<ImageDataDto>();

            foreach (var sampleImageModel in sampleImageModels)
            {
                ImageDataDto imageDataDto = sampleImageModel.sampleImageModelToImageDataDto(_configuration.GetValue<string>("WebHostUrl"));

                results.Add(imageDataDto);
            }

            return Ok(results);          
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            SampleImageModel? sampleImageModel = await _applicationDBContext.ImagesUpload.FindAsync(id);

            if (sampleImageModel == null)
            {
                return NotFound();
            }

            FileStream fileStream = _imageService.GetUserFile(sampleImageModel.ImagePath);
           
            return Ok(fileStream);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ImageDataDto>> postImage([FromForm]ImageUploadDto imageUploadDto)
        {
            _logger.LogInformation("In get score for image method");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (imageUploadDto == null || imageUploadDto.sampleImage == null || 
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                _logger.LogError("No image sent in form");
                return BadRequest("No image sent in form");
            }

            string username = User.GetUserName();
            UserModel appUser = await _userManager.FindByNameAsync(username);

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel(appUser);

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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (imageUploadDto == null || imageUploadDto.sampleImage == null ||
                !imageUploadDto.sampleImage.Headers.ContentType.ToString().Contains("image"))
            {
                _logger.LogError("No image sent in form");
                return BadRequest("No image sent in form");
            }

            ImageUploadModel imageUploadModel = imageUploadDto.imageUploadDtoToModel(null);

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
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> upadte([FromRoute] int id, [FromForm] ImageUpdateDto imageUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SampleImageModel sampleImageModel = await _applicationDBContext.ImagesUpload.FirstOrDefaultAsync(x => x.Id == id);

            if (sampleImageModel == null)
            {
                return NotFound();
            }

            string username = User.GetUserName();
            UserModel appUser = await _userManager.FindByNameAsync(username);

            if(appUser.Id != sampleImageModel.UserId)
            {
                return Unauthorized();
            }

            //sampleImageModel.UserName = imageUpdateDto.UserName;
            sampleImageModel.date = imageUpdateDto.date;
            sampleImageModel.Score = imageUpdateDto.Score;

            await _applicationDBContext.SaveChangesAsync();

            return Ok(sampleImageModel.sampleImageModelToImageDataDto(_configuration.GetValue<string>("WebHostUrl")));
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
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
