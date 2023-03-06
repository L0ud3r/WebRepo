using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {

        private readonly IRepository<FileBlob> _filesRepository;
        private readonly IRepository<User> _userRepository;

        public FileController(IRepository<FileBlob> filesRepository, IRepository<User> userRepository)
        {
            _filesRepository = filesRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(_filesRepository.Get().Where(x => x.Active == true).AsQueryable());
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetbyUser(int idUser)
        {
            return new JsonResult(_filesRepository.Get().AsQueryable().Where(x => x.User.Id == idUser && x.Active == true));
        }

        [HttpGet("{idUser}")]
        public async Task<IActionResult> GetbyFavourites(int idUser)
        {
            return new JsonResult(_filesRepository.Get().AsQueryable().Where(x => x.User.Id == idUser && x.isFavourite == true && x.Active == true));
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
            string filename = string.Empty;

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
                
                /** ALTERAR O ID TENDO EM CONTA O USER AUTENTICADO **/
                
                newFile.User = _userRepository.Get().Where(x => x.Id == 1).SingleOrDefault();
                newFile.FileName = filename;
                newFile.PathAPI = exactpath;
                newFile.ContentLength = file.Length;
                newFile.ContentType = file.ContentType;
                newFile.Active = true;
                newFile.UpdatedDate = DateTime.Now;
                newFile.CreatedDate = DateTime.Now;
                newFile.CreatedBy = 0;
                newFile.isFavourite = false;

                _filesRepository.Insert(newFile);
                _filesRepository.Save();

                return new JsonResult(newFile);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebRepo.DAL\\Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }

        [HttpPatch("addremovefavourites/{id}")]
        public async Task<IActionResult> AddRemoveFavourites(int id)
        {
            try
            {
                var file = _filesRepository.Get().Where(x => x.Id == id && x.Active == true).SingleOrDefault();
                
                if (file != null) 
                {
                    file.isFavourite = !file.isFavourite;
                    _filesRepository.Update(file);
                    _filesRepository.Save();

                    return new JsonResult(true) { StatusCode = 200, Value = "Success on status change!" };
                }

                return new JsonResult(false) { StatusCode = 400, Value = "Error on modifying file" };
            }
            catch(Exception ex) 
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
