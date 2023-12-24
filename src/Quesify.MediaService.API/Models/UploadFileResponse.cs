namespace Quesify.MediaService.API.Models;

public class UploadFileResponse
{
    public string Path { get; set; }

    public long Size { get; set; }

    public UploadFileResponse()
    {
        Path = null!;
    }
}
