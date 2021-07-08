using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFileRepository
    {
        Task Create(FileEntity file);
        Task<FileEntity> GetFile(Guid id);
        Task Delete(FileEntity file);
    }
}