using Amazon.S3;
using Amazon.S3.Model;

namespace EnglishCentralManagement.Services
{
    public class CloudflareR2Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _publicUrl;

        public CloudflareR2Service(IConfiguration config)
        {
            _bucketName = config["Cloudflare:BucketName"];
            _publicUrl = config["Cloudflare:PublicUrl"];

            var s3Config = new AmazonS3Config
            {
                ServiceURL = $"https://{config["Cloudflare:AccountId"]}.r2.cloudflarestorage.com",
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(
                config["Cloudflare:AccessKey"],
                config["Cloudflare:SecretKey"],
                s3Config
            );
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "images")
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using var stream = file.OpenReadStream();

            await _s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{folder}/{fileName}",
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            });

            return $"{_publicUrl}/{folder}/{fileName}";
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var key = uri.AbsolutePath.TrimStart('/');

            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            });
        }
    }
}
