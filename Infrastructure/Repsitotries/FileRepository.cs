using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(FileEntity file)
        {
            _context.FileEntities.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(FileEntity file)
        {
            _context.FileEntities.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<FileEntity> GetFile(Guid id)
        {
            return await _context.FileEntities.FindAsync(id);
        }
    }
}