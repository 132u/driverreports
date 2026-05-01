using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DriverReports.WebApi.Controllers
{
    [ApiController]
    [Route("api/files")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            // папка пользователя
            var userFolder = Path.Combine(uploadsRoot, userId);

            // 👉 ВОТ ЭТО ВАЖНО
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var fullPath = Path.Combine(userFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { url = $"/uploads/{userId}/{fileName}" });
        }
    }
}
