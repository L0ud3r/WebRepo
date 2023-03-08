using Microsoft.AspNetCore.Http;
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
            if(idCurrentFolder != 0)
            {
                return _virtualDirectoryRepository.Get().Where(x => x.User.Email == userEmail && x.ParentDirectory.Id == idCurrentFolder && x.Active == true).ToList();
            }

            return _virtualDirectoryRepository.Get().Where(x => x.User.Email == userEmail && x.ParentDirectory == null && x.Active == true).ToList();
        }
    }
}
