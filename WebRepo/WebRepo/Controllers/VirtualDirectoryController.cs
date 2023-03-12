using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebRepo.App.Interfaces;
using WebRepo.App.Services;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using WebRepo.Models;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirtualDirectoryController : Controller
    {
        private readonly IVirtualDirectoryService _virtualDirectoryService;

        public VirtualDirectoryController(IVirtualDirectoryService virtualDirectoryService)
        {
            _virtualDirectoryService = virtualDirectoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var folders = await _virtualDirectoryService.Get();

            if (folders.Count <= 0)
                return new JsonResult(false) { StatusCode = 404, Value = "There are no folders on the database" };

            return new JsonResult(true) { StatusCode = 200, Value = folders };
        }

        [HttpGet("folders")]
        public async Task<IActionResult> GetbyUser(int idCurrentFolder)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var userFolders = await _virtualDirectoryService.GetByUser(userEmail, idCurrentFolder);

            if (userFolders.Count <= 0)
                return new JsonResult(false) { StatusCode = 404, Value = "You don't have any folders!" };

            return new JsonResult(true) { StatusCode = 200, Value = userFolders };
        }

        [HttpGet("getparent")]
        public async Task<IActionResult> GetParentFolder(int idCurrentFolder)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var parentFolderId = await _virtualDirectoryService.GetParentFolder(userEmail, idCurrentFolder);

            return new JsonResult(true) { StatusCode = 200, Value = parentFolderId };
        }

        [HttpPost]
        public async Task<IActionResult> AddFolder(NewFolderViewModel folder)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var newFolder = await _virtualDirectoryService.AddFolder(userEmail, folder.IdCurrentDirectory, folder.Name);

            return new JsonResult(true) { StatusCode = 200, Value = newFolder };
        }

        [HttpPost("paginate")]
        public async Task<IActionResult> PaginateFolders([FromBody] FileSearchViewModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

            var allUserFolders = _virtualDirectoryService.GetAllByUser(model.UserEmail);

            var fileName = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filename");
            var fileType = model.SearchParameter.FirstOrDefault(x => x.FieldName == "Filetype");

            if (fileType.FieldValue == "Folder" || fileType.FieldValue == "")
            {
                allUserFolders = allUserFolders.Where(x => x.Name.ToUpper().StartsWith(fileName.FieldValue.ToUpper()));
            }
            else
            {
                allUserFolders = Enumerable.Empty<VirtualDirectory>();
            }

            var result = allUserFolders.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Id,
                x.Name,
                x.Active,
                x.CreatedDate,
                x.CreatedBy,
                x.UpdatedBy,
                x.UpdatedDate,
                x.ParentDirectory
            }).ToList();

            var _count = allUserFolders.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                //To fix
                totalNotFiltered = _count
            });
        }


        [HttpPatch]
        public async Task<IActionResult> PatchFolder([FromBody] NewFolderViewModel folder)
        {
            string userEmail = "";

            if (User.Identity.IsAuthenticated)
                userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            else
                return new JsonResult(false) { StatusCode = 401, Value = "User not authenticated" };

            var edditedFolder = await _virtualDirectoryService.PatchFolder(folder.IdCurrentDirectory, folder.Name);

            return new JsonResult(true) { StatusCode = 200, Value = edditedFolder };
        }

    }
}
