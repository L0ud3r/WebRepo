using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Default;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.App.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<FileBlob> _filesRepository;
        private readonly IRepository<User> _userRepository;

        public FileService(IRepository<FileBlob> filesRepository, IRepository<User> userRepository)
        {
            _filesRepository = filesRepository;
            _userRepository = userRepository;
        }

        public async Task<List<FileBlob>> Get()
        {
            return _filesRepository.Get().Where(x => x.Active == true).ToList();
        }

        public async Task<FileBlob> GetFileByIdentifier(string fileIdentifier)
        {
            return _filesRepository.Get().Where(x => x.FileIdentifier == fileIdentifier && x.Active == true).SingleOrDefault();
        }

        public async Task<List<FileBlob>> GetByUser(int idUser)
        {
            return _filesRepository.Get().Where(x => x.User.Id == idUser && x.Active == true).ToList();
        }

        public async Task<List<FileBlob>> GetByFavourites(int idUser)
        {
            return _filesRepository.Get().Where(x => x.User.Id == idUser && x.isFavourite == true && x.Active == true).ToList();
        }

        public async Task<FileBlob> PostFile(string fileIdentifier, string exactpath, IFormFile file)
        {
            try
            {
                FileBlob newFile = new FileBlob();

                /** ALTERAR O ID TENDO EM CONTA O USER AUTENTICADO **/

                newFile.User = _userRepository.Get().Where(x => x.Id == 1).SingleOrDefault();
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