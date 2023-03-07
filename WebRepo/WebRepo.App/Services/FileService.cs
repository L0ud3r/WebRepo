using Microsoft.AspNetCore.Http;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using System.Security.Claims;

namespace WebRepo.App.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileBlob> _filesRepository;
        private readonly IRepository<User> _userRepository;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IRepository<FileBlob> filesRepository, IRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _filesRepository = filesRepository;
            _userRepository = userRepository;
            //_httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FileBlob>> Get()
        {
            return _filesRepository.Get().Where(x => x.Active == true).ToList();
        }

        public async Task<FileBlob> GetFileByIdentifier(string fileIdentifier)
        {
            return _filesRepository.Get().Where(x => x.FileIdentifier == fileIdentifier && x.Active == true).SingleOrDefault();
        }

        public async Task<List<FileBlob>> GetByUser(string userEmail)
        {
            return _filesRepository.Get().Where(x => x.User.Email == userEmail && x.Active == true).ToList();
        }

        public async Task<List<FileBlob>> GetByFavourites(int idUser)
        {
            return _filesRepository.Get().Where(x => x.User.Id == idUser && x.isFavourite == true && x.Active == true).ToList();
        }

        public async Task<FileBlob> PostFile(string fileIdentifier, string exactpath, string userEmail, IFormFile file)
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

                _filesRepository.Insert(newFile);
                _filesRepository.Save();

                return newFile;
            }
            catch(Exception ex)
            {
                return null;
            }
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
    }
}