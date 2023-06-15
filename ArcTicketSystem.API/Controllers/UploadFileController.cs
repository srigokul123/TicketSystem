using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArcTicketSystem.API.Controllers
{
    [ApiController]
    public class UploadFileController : ControllerBase
    {

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            string activationUrl = "";
            try
            {
                var extenstion = "." + file.FileName.Split(".")[file.FileName.Split(".").Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extenstion;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                activationUrl = $"{baseUrl}/Upload/Files/" + filename;

            }
            catch (Exception ex)
            {

            }
            return activationUrl;
            ;
        }
    }
}

