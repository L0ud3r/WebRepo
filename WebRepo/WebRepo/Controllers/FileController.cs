using Microsoft.AspNetCore.Mvc;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.Controllers
{
    [ApiController]
    public class FileController : Controller
    {

        private IRepository<FileBlob> _filesRepository;

        public FileController(IRepository<FileBlob> filesRepository)
        {
            _filesRepository = filesRepository;
        }

        [HttpPost]
        [Route("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationtoken)
        {
            var result = await WriteFile(file);
            return Ok(result);
        }

        private async Task<IActionResult> WriteFile(IFormFile file)
        {
            string filename = "";

            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                FileBlob newFile = new FileBlob();

                newFile.FileName = filename;
                newFile.PathAPI = exactpath;
                newFile.ContentLength = 1;
                newFile.ContentType = file.ContentType;
                newFile.Active = true;
                newFile.UpdatedDate = DateTime.Now;
                newFile.CreatedDate = DateTime.Now;
                newFile.CreatedBy = 0;

                _filesRepository.Insert(newFile);
                _filesRepository.Save();

                return new JsonResult(newFile);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
