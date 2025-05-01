
using Microsoft.AspNetCore.Mvc;
using ocrkyc.Models; // ✅ Correct namespace
using System.IO;
using System.Threading.Tasks;

namespace ocrkyc.Controllers
{
    public class AadhaarController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile aadhaarImage)
        {
            if (aadhaarImage == null || aadhaarImage.Length == 0)
                return View();

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsDir);
            var filePath = Path.Combine(uploadsDir, aadhaarImage.FileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await aadhaarImage.CopyToAsync(stream);
            }

            // Show feedback that the image is being processed
            ViewBag.ProcessingMessage = "Processing the image, please wait...";

            var extractedData = AadhaarOCR.ExtractText(filePath);
            ViewBag.OCRResult = extractedData;

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
