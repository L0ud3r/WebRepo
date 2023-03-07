using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.App.Interfaces
{
    public interface IFileService
    {
        Task<List<FileBlob>> Get();
        Task<FileBlob> GetFileByIdentifier(string fileIdentifier);
        Task<List<FileBlob>> GetByUser(string userEmail);
        Task<List<FileBlob>> GetByFavourites(int idUser);
        Task<FileBlob> PostFile(string fileIdentifier, string exactpath, string userEmail, IFormFile file);
        Task<bool> AddRemoveFavourites(int id);
    }
}
