using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.App.Services
{
    public class FileServices
    {
        public static async Task<List<FileBlob>> Get(IRepository<FileBlob> _filesRepository)
        {
            return _filesRepository.Get().Where(x => x.Active == true).ToList();
        }

        public static async Task<FileBlob> GetFileByIdentifier(IRepository<FileBlob> _filesRepository, string fileIdentifier)
        {
            return _filesRepository.Get().Where(x => x.FileIdentifier == fileIdentifier && x.Active == true).SingleOrDefault();
        }

        public static async Task<List<FileBlob>> GetByUser(IRepository<FileBlob> _filesRepository, int idUser)
        {
            return _filesRepository.Get().Where(x => x.User.Id == idUser && x.Active == true).ToList();
        }

        public static async Task<List<FileBlob>> GetByFavourites(IRepository<FileBlob> _filesRepository, int idUser)
        {
            return _filesRepository.Get().Where(x => x.User.Id == idUser && x.isFavourite == true && x.Active == true).ToList();
        }

        public static async Task<FileBlob> PostFile(IRepository<FileBlob> _filesRepository, IRepository<User> _userRepository, string fileIdentifier, string exactpath, IFormFile file)
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
    
        public static async Task<bool> AddRemoveFavourites(IRepository<FileBlob> _filesRepository, int id)
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