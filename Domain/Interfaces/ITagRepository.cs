using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> GetTag(Guid Id);
        Task Create(Tag tag);
        Task Update(Tag tag);
        Task Delete(Tag tag);
    }
}