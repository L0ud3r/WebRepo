using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.Controllers
{
    public class FileCdnController : Controller
    {
        private IRepository<FileCdn> _fileRepository;

        public FileCdnController(IRepository<FileCdn> fileRepository)
        {
            _fileRepository = fileRepository;
        }

        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadFile(IFormFile file, [FromServices] ICdnService cdnService)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("Please select a file to upload.");
        //    }

        //    // Save the file to disk or to the database
        //    // Here is an example of how to save the file to disk:
        //    var filePath = Path.Combine(Path.GetTempPath(), file.FileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    // Upload the file to the CDN
        //    var fileUrl = await cdnService.UploadFileAsync(filePath, file.FileName);

        //    // Create a new FileBlob entity and save the file URL and CDN ID
        //    var fileEntity = new FileCdn
        //    {
        //        FileName = file.FileName,
        //        Size = file.Length,
        //        ContentType = file.ContentType,
        //        Id = fileUrl.CdnId,
        //        CdnUrl = fileUrl.Url
        //    };

        //    _fileRepository.Insert(fileEntity);
        //    _fileRepository.Save();

        //    return Ok("FileBlob uploaded successfully.");
        //}

        //[HttpGet("download")]
        //public async Task<IActionResult> DownloadFile(int fileId, [FromServices] ICdnService cdnService)
        //{
        //    // Retrieve the FileBlob entity from the database
        //    var fileEntity = _fileRepository.Get().FirstOrDefault(f => f.Id == fileId && f.Active == true);

        //    if (fileEntity == null)
        //    {
        //        return NotFound("FileBlob not found.");
        //    }

        //    // Get the CDN URL for the file
        //    var cdnUrl = await cdnService.GetFileUrlAsync(fileEntity.Id);

        //    // Return the file as a FileResult, which will prompt the user to download the file
        //    return Redirect(cdnUrl);
        //}
    }
}
