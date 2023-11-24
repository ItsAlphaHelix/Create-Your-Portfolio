namespace Portfolio.API.Services
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Portfolio.API.Services.Contracts;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        public CloudinaryService(
            IConfiguration configuration)
        {
            Account account = new Account(
                configuration["CloudinarySettings:Name"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:SecretKey"]);

            cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageToCloudinaryAsync(IFormFile file, int heigth, int width, string publicId)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId,
                    Transformation = new Transformation()
                    .Height(heigth).Width(width)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task DeleteImageAsync(List<string> projectPublicIds)
        {
            foreach (var publicId in projectPublicIds)
            {
                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image
                };

                try
                {
                    // Perform the image deletion
                    var result = await cloudinary.DestroyAsync(deletionParams);

                    // Check the result status
                    if (result.Result == "ok")
                    {
                        Console.WriteLine($"Image with public ID '{publicId}' deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete image. Error: {result.Error.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
