using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace ConstructionAPI.Services
{
    public class FirebaseStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucketName = _configuration["Firebase:StorageBucket"];
            var credentialsPath = _configuration["Firebase:CredentialsPath"];

            // Kimlik doğrulama dosyasının yolunu ortam değişkeni olarak ayarla
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
    
                _storageClient = StorageClient.Create();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || !FileExtensions.IsImage(file))
            {
                throw new ArgumentException("File cannot be null or empty.", nameof(file));
            }

            var objectName = Path.GetRandomFileName();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream);
                }
            }
            catch (Exception ex)
            {
                // Hatanın detaylarını loglama veya yazdırma
                Console.Error.WriteLine($"Dosya yükleme hatası: {ex.Message}");
                throw;
            }

            var url = $"https://storage.googleapis.com/{_bucketName}/{objectName}";
            return url;
        }

    }
}
