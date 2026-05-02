using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace DriverReports.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var uploadPath = Path.Combine("wwwroot", "uploads", userId);
            // папка пользователя
            var userFolder = Path.Combine(uploadPath, userId);

            // 👉 ВОТ ЭТО ВАЖНО
            if (!Directory.Exists(userFolder))
            {   
                Directory.CreateDirectory(userFolder);
            }

            var urls = new List<string>();

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                urls.Add($"/uploads/{userId}/{fileName}");
            }

            return Ok(new { urls });
            //return Ok(new { url = $"/uploads/{userId}/{fileName}" });
        }
    }
}
