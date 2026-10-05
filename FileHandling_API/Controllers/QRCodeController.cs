using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace Day21_FileHandling_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodeController : ControllerBase
    {
        // GET: api/QRCode/generate
        [HttpGet("generate")]
        public IActionResult GenerateQRCode(
            int userId,
            string name,
            string email)
        {
            // Data to put inside QR code
            string userData =
                $"UserId: {userId}\n" +
                $"Name: {name}\n" +
                $"Email: {email}";

            // Create QR Code
            using var qrGenerator = new QRCodeGenerator();

            using QRCodeData qrCodeData =
                qrGenerator.CreateQrCode(
                    userData,
                    QRCodeGenerator.ECCLevel.Q
                );

            using var qrCode =
                new PngByteQRCode(qrCodeData);

            byte[] qrCodeBytes =
                qrCode.GetGraphic(20);

            return File(
                qrCodeBytes,
                "image/png",
                "UserQRCode.png"
            );
        }
    }
}