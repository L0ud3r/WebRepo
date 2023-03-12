using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using WebRepo.Models;

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

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedFiles()
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var userFiles = await _fileService.GetDeletedFiles(userEmail);

            if (userFiles.Count <= 0)
                return new JsonResult(true) { StatusCode = 204, Value = "You don't have any files!" };

            return new JsonResult(userFiles);
        }

        [HttpPost("paginate")]
        public async Task<IActionResult> PaginateFiles([FromBody] FileSearchViewModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

            var allUserFiles = _fileService.GetAllByUser(model.UserEmail);

            var fileName = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filename");

            if (fileName != null && !string.IsNullOrEmpty(fileName.FieldValue))
            {
                allUserFiles = allUserFiles.Where(x => x.FileName.ToUpper().StartsWith(fileName.FieldValue.ToUpper()));
            }

            var fileType = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filetype");

            if (fileType != null && !string.IsNullOrEmpty(fileType.FieldValue))
            {
                allUserFiles = allUserFiles.Where(x => x.ContentType.ToUpper().StartsWith(fileType.FieldValue.ToUpper()));
            }
            
            var result = allUserFiles.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileIdentifier,
                x.PathAPI,
                x.ContentType,
                x.ContentLength,
                x.isFavourite,
                x.VirtualDirectory,
                x.Active,
                x.CreatedBy,
                x.CreatedDate,
                x.UpdatedDate,
                x.UpdatedBy
            }).ToList();

            var _count = allUserFiles.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                //To fix
                totalNotFiltered = _count
            });
        }

        
        [HttpPost("paginatefavourites")]
        public async Task<IActionResult> PaginateFavouriteFiles([FromBody] FileSearchViewModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

            var allUserFiles = _fileService.GetByFavouritesEnum(model.UserEmail);

            var fileName = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filename");

            if (fileName != null && !string.IsNullOrEmpty(fileName.FieldValue))
            {
                allUserFiles = allUserFiles.Where(x => x.FileName.ToUpper().StartsWith(fileName.FieldValue.ToUpper()));
            }

            var fileType = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filetype");

            if (fileType != null && !string.IsNullOrEmpty(fileType.FieldValue))
            {
                allUserFiles = allUserFiles.Where(x => x.ContentType.ToUpper().StartsWith(fileType.FieldValue.ToUpper()));
            }

            var result = allUserFiles.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Id,
                x.FileName,
                x.FileIdentifier,
                x.PathAPI,
                x.ContentType,
                x.ContentLength,
                x.isFavourite,
                x.VirtualDirectory,
                x.Active,
                x.CreatedBy,
                x.CreatedDate,
                x.UpdatedDate,
                x.UpdatedBy
            }).ToList();

            var _count = allUserFiles.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                //To fix
                totalNotFiltered = _count
            });
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
        [Route("uploadfile/{idCurrentFolder}")]
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

        [HttpPatch]
        public async Task<IActionResult> PatchFile(FileViewModel file)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var edittedFile = await _fileService.PatchFile(file.Id, file.FileName);

            if (edittedFile == null)
                return new JsonResult(true) { StatusCode = 404, Value = "Couldn't change file" };

            return new JsonResult(edittedFile);
        }

        [HttpPatch("addremovefavourites")]
        public async Task<IActionResult> AddRemoveFavourites([FromBody] int id)
        {
            var result = await _fileService.AddRemoveFavourites(id);

            if (result == false)
                return new JsonResult(result) { StatusCode = 404, Value = result };

            return new JsonResult(result) { StatusCode = 200, Value = result };
        }

        [HttpPatch("removerecover")]
        public async Task<IActionResult> DeleteRecoverFile([FromBody] FileViewModel file)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var edittedFile = await _fileService.DeleteRecoverFile(file.Id, file.FileName);

            if (edittedFile == null)
                return new JsonResult(true) { StatusCode = 404, Value = "Couldn't change file" };

            return new JsonResult(edittedFile);
        }
    }
}
