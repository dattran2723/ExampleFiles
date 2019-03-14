using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var fileName = file.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", fileName);
            
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Content("file uploaded");
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Download(string fileName)
        {
            if (fileName == null)
                return Content("FileName not present!");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", fileName);
            var memory = new MemoryStream();

            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }
    }
}