using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.App.Interfaces;
using WebRepo.DAL.Entities;
using WebRepo.Infra;

namespace WebRepo.App.Services
{
    public class VirtualDirectoryService : IVirtualDirectoryService
    {
        private readonly IRepository<FileBlob> _filesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<VirtualDirectory> _virtualDirectoryRepository;

        public VirtualDirectoryService(IRepository<FileBlob> filesRepository, IRepository<User> userRepository, IRepository<VirtualDirectory> virtualDirectoryRepository)
        {
            _filesRepository = filesRepository;
            _userRepository = userRepository;
            _virtualDirectoryRepository = virtualDirectoryRepository;
        }

        public async Task<List<VirtualDirectory>> Get()
        {
            return _virtualDirectoryRepository.Get().Where(x => x.Active).ToList();
        }

        public async Task<List<VirtualDirectory>> GetByUser(string userEmail, int idCurrentFolder)
        {
            if (idCurrentFolder != 0)
            {
                return _virtualDirectoryRepository.Get().Where(x => x.User.Email == userEmail && x.ParentDirectory == idCurrentFolder && x.Active == true).ToList();
            }

            return _virtualDirectoryRepository.Get().Where(x => x.User.Email == userEmail && x.ParentDirectory == null && x.Active == true).ToList();
        }

        public async Task<int> GetParentFolder(string userEmail, int idCurrentFolder)
        {
            var currentFolder = await _virtualDirectoryRepository.Get().Where(x => x.Id == idCurrentFolder).SingleOrDefaultAsync();

            if (idCurrentFolder == 0 || currentFolder.ParentDirectory == null) return 0;

            var parentFolder = await _virtualDirectoryRepository.Get().Where(x => x.Id == currentFolder.ParentDirectory).SingleOrDefaultAsync();

            return parentFolder.Id;
        }

        public async Task<VirtualDirectory> AddFolder(string userEmail, int idCurrentFolder, string filename)
        {
            VirtualDirectory newFolder = new VirtualDirectory();
            User user = await _userRepository.Get().Where(x => x.Email == userEmail && x.Active).SingleOrDefaultAsync();

            if (user == null)
                return null;

            newFolder.User = user;

            var currentDirectory = await _virtualDirectoryRepository.Get().Where(x => x.Id == idCurrentFolder).SingleOrDefaultAsync();

            if (currentDirectory == null) newFolder.ParentDirectory = null;
            else newFolder.ParentDirectory = idCurrentFolder;

            newFolder.Active = true;
            newFolder.CreatedDate = DateTime.Now;
            newFolder.UpdatedDate = DateTime.Now;
            newFolder.CreatedBy = user.Id;
            newFolder.Name = filename;

            _virtualDirectoryRepository.Insert(newFolder);
            _virtualDirectoryRepository.Save();

            return await _virtualDirectoryRepository.Get().OrderBy(x => x.CreatedDate).LastOrDefaultAsync();
        }

        public IEnumerable<VirtualDirectory> GetAllByUser(string userEmail)
        {
            return _virtualDirectoryRepository.Get().Where(x => x.Active == true && x.User.Email == userEmail).AsEnumerable();
        }

        public async Task<VirtualDirectory> PatchFolder(int idCurrentDirectory, string name)
        {
            var folder = await _virtualDirectoryRepository.Get().Where(x => x.Id == idCurrentDirectory).SingleOrDefaultAsync();

            if (_virtualDirectoryRepository.Exists(idCurrentDirectory) == false)
                return null;

            folder.Name = name;
            folder.UpdatedDate = DateTime.Now;
            folder.UpdatedBy = folder.User.Id;

            _virtualDirectoryRepository.Update(folder);
            _virtualDirectoryRepository.Save();

            return folder;
        }
    }
}
