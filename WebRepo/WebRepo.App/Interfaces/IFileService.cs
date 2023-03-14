using Microsoft.AspNetCore.Http;
using WebRepo.DAL.Entities;

namespace WebRepo.App.Interfaces
{
    public interface IFileService
    {
        Task<List<FileBlob>> Get();
        Task<FileBlob> GetById(int id);
        Task<FileBlob> GetFileByIdentifier(string fileIdentifier);
        Task<List<FileBlob>> GetByUser(string userEmail, int idCurrentFolder);
        IEnumerable<FileBlob> GetAllByUser(string userEmail);
        Task<List<FileBlob>> GetByFavourites(string userEmail);
        Task<FileBlob> PostFile(string fileIdentifier, string exactpath, string userEmail, IFormFile file, int idCurrentFolder);
        Task<FileBlob> PatchFile(int fileId, string fileName);
        Task<FileBlob> DeleteRecoverFile(int fileId, string fileName);
        Task<bool> AddRemoveFavourites(int id);
        Task<List<FileBlob>> GetDeletedFiles(string userEmail);
        IEnumerable<FileBlob> GetByFavouritesEnum(string userEmail);
    }
}
