using FileManagment.API.DTOs.UploadedFile;
using FileManagment.API.ServicesServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService _fileService= fileService;
        [HttpPost("upload")]
		public async Task<IActionResult> Upload([FromForm]UploadFileRequest request,CancellationToken cancellationToken)
        {
            var fileId = await _fileService.UploadAsync(request.File, cancellationToken);
			//To reach to file by URL===> https://localhost:7097/api/Files/download/01959792-f2c0-7e28-8f14-00dbac1afc3b this link is in file header (Location)
			return CreatedAtAction(nameof(Download), new {id=fileId},null);
        }
        [HttpPost("upload-many")]
		public async Task<IActionResult> UploadMany([FromForm] UploadManyFilesRequest request,CancellationToken cancellationToken)
        {
            var filesIds = await _fileService.UploadManyAsync(request.Files, cancellationToken);
            return Ok(filesIds);
        }
        [HttpPost("upload-image")]
		public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request,CancellationToken cancellationToken)
        {
            await _fileService.UploadImageAsync(request.Image, cancellationToken);
            return Created();
        }
        [HttpGet("download/{id}")]
		public async Task<IActionResult> Download([FromRoute]Guid id,CancellationToken cancellationToken)
        {
            var(fileContent, ContentType, fileName) =await _fileService.Download(id, cancellationToken);
            return fileContent is [] ? NotFound() : File(fileContent, ContentType, fileName);
		}
        [HttpGet("stream/{id}")]
		public async Task<IActionResult> Stream([FromRoute]Guid id,CancellationToken cancellationToken)
        {
            var(fileStream, ContentType, fileName) =await _fileService.Stream(id, cancellationToken);
            return fileStream is null ? NotFound() : File(fileStream, ContentType, fileName,enableRangeProcessing:true);
		}
    }
}
