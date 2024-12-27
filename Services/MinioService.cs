using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using NPOI.XWPF.UserModel; // Để đọc file Word
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text;
using System.Drawing; // Để xử lý hình ảnh
using iTextSharp.text.pdf; // Để xử lý PDF
using iTextSharp.text;
using iTextSharp.text.pdf.parser; // Để xử lý PDF
using Minio.DataModel;
using Minio.DataModel.Args;

public class MinIOService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public MinIOService(IConfiguration configuration)
    {
        var endpoint = configuration["MinIO:Endpoint"];
        var accessKey = configuration["MinIO:AccessKey"];
        var secretKey = configuration["MinIO:SecretKey"];

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true // Yêu cầu khi kết nối với MinIO
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, config);
    }

    // Phương thức upload file
    public async Task<bool> UploadFileAsync(Stream fileStream, string fileName, string bucketName)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = "application/octet-stream" // hoặc kiểu phù hợp
            };

            var response = await _s3Client.PutObjectAsync(putRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi upload file: {ex.Message}");
            return false;
        }
    }
    public string GetPreSignedUrl(string bucketName, string fileName, int expiryDurationInSeconds = 3600)
    {
        try
        {
            // Correct method to generate pre-signed URL in AWS SDK
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddSeconds(expiryDurationInSeconds)
            };

            // Generate the URL
            var url = _s3Client.GetPreSignedURL(request);
            return url;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating pre-signed URL: {ex.Message}", ex);
        }
    }


    // Phương thức lấy file
    public async Task<Stream> GetFileAsync(string bucketName, string fileKey)
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        try
        {
            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream; // Trả về stream của file
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy file: {ex.Message}");
            return null; // Hoặc ném ra ngoại lệ nếu cần
        }
    }
    //xóa file
    // Phương thức xóa file với ghi nhật ký và phản hồi chi tiết

    public bool IsValidBucketName(string bucketName)
    {
        // Kiểm tra xem tên bucket có hợp lệ không
        return !string.IsNullOrWhiteSpace(bucketName) &&
               bucketName.Length >= 3 &&
               bucketName.Length <= 63 &&
               !bucketName.Contains(" ") &&
               !bucketName.StartsWith("-") &&
               !bucketName.EndsWith("-") &&
               !bucketName.Contains("--");
    }

    public async Task<bool> IsBucketExistsAsync(string bucketName)
    {
        var buckets = await ListBucketsAsync();
        return buckets.Contains(bucketName);
    }


    // Xóa file với log chi tiết
    public async Task<bool> DeleteFileAsync(string bucketName, string fileKey)
    {
        if (!IsValidBucketName(bucketName))
        {
            Console.WriteLine($"Tên bucket '{bucketName}' không hợp lệ.");
            return false;
        }

        if (!await IsBucketExistsAsync(bucketName))
        {
            Console.WriteLine($"Bucket '{bucketName}' không tồn tại.");
            return false;
        }

        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            Console.WriteLine($"Đã xóa file {fileKey} từ bucket {bucketName}");
            return true;
        }
        catch (AmazonS3Exception s3Ex)
        {
            Console.WriteLine($"Lỗi AWS S3 khi xóa file '{fileKey}' từ bucket '{bucketName}'. " +
                               $"Mã lỗi: {s3Ex.ErrorCode}, Thông điệp: {s3Ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi không xác định khi xóa file '{fileKey}' từ bucket '{bucketName}'. " +
                               $"Thông điệp: {ex.Message}");
            return false;
        }
    }







    // lấy danh sách file
    public async Task<List<string>> ListFilesAsync()
    {
        var files = new List<string>();
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            foreach (var entry in response.S3Objects)
            {
                files.Add(entry.Key); // Lấy tên file
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching files: {ex.Message}");
        }

        return files;
    }

    // Phương thức liệt kê tất cả bucket
    public async Task<List<string>> ListBucketsAsync()
    {
        var buckets = new List<string>();
        var response = await _s3Client.ListBucketsAsync();
        foreach (var bucket in response.Buckets)
        {
            buckets.Add(bucket.BucketName);
        }
        return buckets;
    }

    // Phương thức lấy danh sách file từ một bucket cụ thể
    public async Task<List<string>> ListFilesInBucketAsync(string bucketName)
    {
        var files = new List<string>();
        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            files.AddRange(response.S3Objects.Select(o => o.Key));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi lấy file từ bucket {bucketName}: {ex.Message}");
        }

        return files;
    }
   

    // Phương thức chuyển đổi file Word sang HTML
    public async Task<string> ConvertWordToHtmlAsync(string bucketName, string fileKey)
    {
        using (var stream = await GetFileAsync(bucketName, fileKey))
        {
            if (stream == null)
                return null;

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html><body>");

            using (var document = new XWPFDocument(stream))
            {
                foreach (var paragraph in document.Paragraphs)
                {
                    htmlBuilder.Append($"<p>{paragraph.ParagraphText}</p>");
                }
            }

            htmlBuilder.Append("</body></html>");
            return htmlBuilder.ToString();
        }
    }

    // Phương thức đọc nội dung file TXT
    public async Task<string> ReadTextFileAsync(string bucketName, string fileKey)
    {
        using (var stream = await GetFileAsync(bucketName, fileKey))
        {
            if (stream == null)
                return null;

            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }

    // Phương thức chuyển đổi file PDF sang HTML
    public async Task<string> ConvertPdfToHtmlAsync(string bucketName, string fileKey)
    {
        using (var stream = await GetFileAsync(bucketName, fileKey))
        {
            if (stream == null)
                return null;

            // Sử dụng iTextSharp để đọc PDF
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html><body>");

            using (var pdfReader = new PdfReader(stream))
            {
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    var text = PdfTextExtractor.GetTextFromPage(pdfReader, i);
                    htmlBuilder.Append($"<p>{text}</p>");
                }
            }

            htmlBuilder.Append("</body></html>");
            return htmlBuilder.ToString();
        }
    }

    // Phương thức lấy ảnh PNG
    public async Task<System.Drawing.Image> GetPngImageAsync(string bucketName, string fileKey)
    {
        using (var stream = await GetFileAsync(bucketName, fileKey))
        {
            if (stream == null)
                return null;

            return System.Drawing.Image.FromStream(stream);
        }
    }

    // Các phương thức khác...
}
