using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Entities;

namespace WebRepo.App.Interfaces
{
    public interface IVirtualDirectoryService
    {
        Task<List<VirtualDirectory>> Get();
        Task<List<VirtualDirectory>> GetByUser(string userEmail, int idCurrentFolder);
    }
}
