using Microsoft.AspNetCore.Http;
using WebRepo.DAL.Entities;

namespace WebRepo.App.Interfaces
{
    public interface IFileService
    {
        Task<List<FileBlob>> Get();
        Task<FileBlob> GetFileByIdentifier(string fileIdentifier);
        Task<List<FileBlob>> GetByUser(string userEmail, int idCurrentFolder);
        Task<List<FileBlob>> GetByFavourites(string userEmail);
        Task<FileBlob> PostFile(string fileIdentifier, string exactpath, string userEmail, IFormFile file, int idCurrentFolder);
        Task<bool> AddRemoveFavourites(int id);
    }
}
