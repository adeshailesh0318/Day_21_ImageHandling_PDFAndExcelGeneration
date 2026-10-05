using FileHandling_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileHandling_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        private static List<ImageFile> imageFiles = new List<ImageFile>();

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        // POST: api/File/upload-image
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select an image.");
            }

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(
                    "Only JPG, JPEG and PNG files are allowed."
                );
            }

            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "uploads"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName =
                Guid.NewGuid().ToString() + extension;

            var filePath = Path.Combine(
                uploadsFolder,
                uniqueFileName
            );

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Create ID
            var imageFile = new ImageFile
            {
                Id = imageFiles.Count + 1,
                OriginalFileName = file.FileName,
                StoredFileName = uniqueFileName
            };

            imageFiles.Add(imageFile);

            return Ok(new
            {
                Message = "Image uploaded successfully",
                Id = imageFile.Id,
                OriginalFileName = imageFile.OriginalFileName,
                StoredFileName = imageFile.StoredFileName
            });
        }


        // GET: api/File/download-image/{id}
        [HttpGet("download-image/{id}")]
        public IActionResult DownloadImage(int id)
        {
            // Find image by ID
            var imageFile = imageFiles
                .FirstOrDefault(x => x.Id == id);

            if (imageFile == null)
            {
                return NotFound(
                    "Image not found for this ID."
                );
            }

            // File path
            var filePath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "uploads",
                imageFile.StoredFileName
            );

            // Check file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(
                    "Image file does not exist."
                );
            }

            // Read file
            var fileBytes =
                System.IO.File.ReadAllBytes(filePath);

            // Content type
            var contentType =
                GetContentType(filePath);

            // Return/download file
            return File(
                fileBytes,
                contentType,
                imageFile.OriginalFileName
            );
        }


        // Get content type
        private string GetContentType(string fileName)
        {
            var extension =
                Path.GetExtension(fileName)
                    .ToLowerInvariant();

            return extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
}