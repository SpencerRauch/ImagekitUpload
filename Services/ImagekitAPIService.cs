using Imagekit.Sdk;

namespace ImagekitUpload.Services;

public interface IImagekitAPIService
{
    Task<string> UploadImageAsync(IFormFile imageFile);
}

public class ImagekitAPIService : IImagekitAPIService
{
    private readonly ImagekitClient _imagekit;
    private readonly IConfiguration _configuration;
    private readonly string _privateKey;

    public ImagekitAPIService(IConfiguration configuration)
    {
        _configuration = configuration;
        _privateKey = _configuration["ImagekitPrivateKey"]!;
        _imagekit = new ImagekitClient(
            "public_DveYTxBb+bcc2DsyJjbeX5Fo6VI=", // TODO change to your public key
            _privateKey,
            "https://ik.imagekit.io/spencergr" // TODO change to your endpoint
        );
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Invalid image file.");
        }

        string base64Image = await ConvertToBase64Async(imageFile);
        string fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
        FileCreateRequest request = new()
        {
            file = base64Image,
            fileName = fileName,
            folder = "pet_pics", // ? optional Imagekit folder name (if you have one)
        };

        Result response = await _imagekit.UploadAsync(request);

        if (response.HttpStatusCode != 200)
        {
            throw new Exception("Image upload failed.");
        }

        return response.url;
    }

    public async Task<string> ConvertToBase64Async(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file.");
        }

        using MemoryStream memoryStream = new();
        await file.CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        return Convert.ToBase64String(fileBytes);
    }
}
