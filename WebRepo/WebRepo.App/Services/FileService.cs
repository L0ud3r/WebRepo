using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.App.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileBlob> _filesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<VirtualDirectory> _virtualDirectoryRepository;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IRepository<FileBlob> filesRepository, IRepository<User> userRepository, IRepository<VirtualDirectory> virtualDirecotryRepository)
        {
            _filesRepository = filesRepository;
            _userRepository = userRepository;
            _virtualDirectoryRepository = virtualDirecotryRepository;
            //_httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FileBlob>> Get()
        {
            return _filesRepository.Get().Where(x => x.Active == true).ToList();
        }

        public async Task<FileBlob> GetById(int id)
        {
            return _filesRepository.GetByID(id);
        }

        public IEnumerable<FileBlob> GetAllByUser(string userEmail)
        {
            return _filesRepository.Get().Where(x => x.Active == true && x.User.Email == userEmail).AsEnumerable();
        }

        public async Task<FileBlob> GetFileByIdentifier(string fileIdentifier)
        {
            return _filesRepository.Get().Where(x => x.FileIdentifier == fileIdentifier && x.Active == true).SingleOrDefault();
        }

        public async Task<List<FileBlob>> GetByUser(string userEmail, int idCurrentFolder)
        {
            if (idCurrentFolder != 0)
            {
                return _filesRepository.Get().Where(x => x.User.Email == userEmail && x.VirtualDirectory.Id == idCurrentFolder && x.Active == true).ToList();
            }

            return _filesRepository.Get().Where(x => x.User.Email == userEmail && x.VirtualDirectory == null && x.Active == true).ToList();

        }

        public async Task<List<FileBlob>> GetByFavourites(string userEmail)
        {
            return _filesRepository.Get().Where(x => x.User.Email == userEmail && x.isFavourite == true && x.Active == true).ToList();
        }

        public IEnumerable<FileBlob> GetByFavouritesEnum(string userEmail)
        {
            return _filesRepository.Get().Where(x => x.User.Email == userEmail && x.isFavourite == true && x.Active == true).AsEnumerable();
        }

        public async Task<FileBlob> PostFile(string fileIdentifier, string exactpath, string userEmail, IFormFile file, int idCurrentFolder)
        {
            try
            {
                FileBlob newFile = new FileBlob();

                newFile.User = _userRepository.Get().Where(x => x.Email == userEmail).SingleOrDefault();

                newFile.FileIdentifier = fileIdentifier;
                newFile.FileName = file.FileName;
                newFile.PathAPI = exactpath;
                newFile.ContentLength = file.Length;
                newFile.ContentType = file.ContentType;
                newFile.Active = true;
                newFile.UpdatedDate = DateTime.Now;
                newFile.CreatedDate = DateTime.Now;
                newFile.CreatedBy = 0;
                newFile.isFavourite = false;

                if (idCurrentFolder != 0)
                    newFile.VirtualDirectory = await _virtualDirectoryRepository.Get().Where(x => x.Id == idCurrentFolder).SingleOrDefaultAsync();
                else
                    newFile.VirtualDirectory = null;

                _filesRepository.Insert(newFile);
                _filesRepository.Save();

                return newFile;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileBlob> PatchFile(int fileId, string fileName)
        {
            var file = await _filesRepository.Get().Where(x => x.Id == fileId).SingleOrDefaultAsync();
            var extension = string.Empty;

            if (_filesRepository.Exists(fileId) == false)
                return null;

            if (file.ContentType == "text/plain")
                extension = ".txt";
            else if (file.ContentType == "text/html")
                extension = ".html";
            else if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg")
                extension = ".jpeg";
            else if (file.ContentType == "image/png")
                extension = ".png";
            else if (file.ContentType == "image/gif")
                extension = ".gif";
            else if (file.ContentType == "image/bmp")
                extension = ".bmp";
            else if (file.ContentType == "image/svg+xml")
                extension = ".svg";
            else if (file.ContentType == "audio/wav" || file.ContentType == "audio/x-wav")
                extension = ".wav";
            else if (file.ContentType == "audio/mpeg")
                extension = ".mp3";
            else if (file.ContentType == "audio/x-ms-wma")
                extension = ".wma";
            else if (file.ContentType == "application/json")
                extension = ".json";
            else if (file.ContentType == "application/xml")
                extension = ".xml";
            else if (file.ContentType == "application/pdf")
                extension = ".pdf";
            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                extension = ".docx";
            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                extension = ".xlsx";
            else if (file.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                extension = ".pptx";
            else if (file.ContentType == "application/vnd.ms-powerpoint")
                extension = ".ppt";
            else if (file.ContentType == "video/mp4")
                extension = ".mp4";
            else if (file.ContentType == "video/x-msvideo")
                extension = ".avi";
            else if (file.ContentType == "video/quicktime")
                extension = ".mov";
            else if (file.ContentType == "video/x-ms-wmv")
                extension = ".wmv";
            else if (file.ContentType == "video/x-matroska")
                extension = ".mkv";
            else if (file.ContentType == "video/x-flv")
                extension = ".flv";

            file.FileName = fileName+extension;

            _filesRepository.Update(file);
            _filesRepository.Save();

            return file;
        }

        public async Task<FileBlob> DeleteRecoverFile(int fileId, string fileName)
        {
            var file = await _filesRepository.Get().Where(x => x.Id == fileId).SingleOrDefaultAsync();

            if (_filesRepository.Exists(fileId) == false)
                return null;

            file.Active = !file.Active;

            _filesRepository.Update(file);
            _filesRepository.Save();

            return file;
        }

        public async Task<bool> AddRemoveFavourites(int id)
        {
            try
            {
                var file = _filesRepository.Get().Where(x => x.Id == id && x.Active == true).SingleOrDefault();

                if (file != null)
                {
                    file.isFavourite = !file.isFavourite;
                    _filesRepository.Update(file);
                    _filesRepository.Save();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<FileBlob>> GetDeletedFiles(string userEmail)
        {
            return _filesRepository.Get().Where(x => x.Active == false && x.User.Email == userEmail).ToList();
        }
    }
}