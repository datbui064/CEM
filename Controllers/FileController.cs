using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using System.IO;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;

    public FileController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    
    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string bucketName, [FromQuery] string fileName)
    {
        try
        {
            // Lấy file từ MinIO
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            using var response = await _s3Client.GetObjectAsync(request);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);

            // Trả file dưới dạng FileContentResult
            return File(memoryStream.ToArray(), response.Headers["Content-Type"], fileName);
        }
        catch (AmazonS3Exception ex)
        {
            return BadRequest(new { Message = $"Lỗi khi tải file: {ex.Message}" });
        }
    }
}
