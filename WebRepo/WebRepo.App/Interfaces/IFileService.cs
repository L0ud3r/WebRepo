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
        Task<List<FileBlob>> Paginate(string userEmail, string filename, string filetype);
        Task<List<FileBlob>> DeleteFile(string userEmail, int idFile);
        Task<List<FileBlob>> RecoverFile(string userEmail, int idFile);
        Task<List<FileBlob>> GetDeletedFiles(string userEmail);
    }
}
