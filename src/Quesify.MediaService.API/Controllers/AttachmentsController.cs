using Microsoft.AspNetCore.Mvc;
using Quesify.MediaService.API.Models;
using Quesify.SharedKernel.AspNetCore.Controllers;
using Quesify.SharedKernel.Storage;

namespace Quesify.MediaService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AttachmentsController : BaseController
{
    private readonly IStorageService _storageService;

    public AttachmentsController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(UploadFileResponse), 201)]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var fileModel = new FileModel()
        {
            File = file,
            Path = "wwwroot/attachments"
        };

        var result = await _storageService.UploadAsync(fileModel);
        
        return CreatedResponse(null, new UploadFileResponse()
        {
            Path = $"attachments/{result.Name}",
            Size = file.Length
        });
    }
}
