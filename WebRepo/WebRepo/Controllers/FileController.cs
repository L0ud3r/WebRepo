using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {

        //private readonly IRepository<FileBlob> _filesRepository;
        //private readonly IRepository<User> _userRepository;
        private readonly IFileService _fileService;

        public FileController(IRepository<FileBlob> filesRepository, IRepository<User> userRepository, IFileService fileService)
        {
            //_filesRepository = filesRepository;
            //_userRepository = userRepository;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var files = await _fileService.Get();

            if (files.Count <= 0)
                return new JsonResult(false) { StatusCode = 404, Value = "There are no files on the database" };

            return new JsonResult(true) { StatusCode = 200, Value = files };
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetbyUser(int idCurrentFolder)
        {
            string userEmail = "";

            if(User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var userFiles = await _fileService.GetByUser(userEmail, idCurrentFolder);

            if (userFiles.Count <= 0)
                return new JsonResult(true) { StatusCode = 204, Value = "You don't have any files!" };

            return new JsonResult(userFiles);
        }

        [HttpGet("favourites")]
        public async Task<IActionResult> GetbyFavourites()
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var userFavouriteFiles = await _fileService.GetByFavourites(userEmail);

            if (userFavouriteFiles.Count <= 0)
                return new JsonResult(true) { StatusCode = 204, Value = "You don't have any favourite files!" };

            return new JsonResult(userFavouriteFiles);
        }

        [HttpPost]
        [Route("uploadfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file, int idCurrentFolder)
        {
            var result = await WriteFile(file, idCurrentFolder);

            return Ok(result);
        }

        private async Task<IActionResult> WriteFile(IFormFile file, int idCurrentFolder)
        {
            string fileIdentifier = string.Empty;
            string userEmail = "";

            try
            {
                if (User.Identity.IsAuthenticated)
                    userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                else
                    return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileIdentifier = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files", fileIdentifier);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newFile = await _fileService.PostFile(fileIdentifier, filepath, userEmail, file, idCurrentFolder);

                if(newFile == null)
                    return new JsonResult(false) { StatusCode = 400, Value = "Error uploading file" };

                return new JsonResult(true) { StatusCode = 200, Value = newFile };
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
                contenttype = "application/octet-stream";

            var file = await _fileService.GetFileByIdentifier(filename);

            if (file == null)
                return new JsonResult(false) { StatusCode = 404, Value = "Couldn't download file" };

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, file.FileName);
        }

        [HttpPatch("addremovefavourites")]
        public async Task<IActionResult> AddRemoveFavourites([FromBody]int id)
        {
            var result = await _fileService.AddRemoveFavourites(id);

            if (result == false)
                return new JsonResult(result) { StatusCode = 404, Value = result };

            return new JsonResult(result) { StatusCode = 200, Value = result };
        }
    }
}
