using Domain.Entities;
using Domain.Types;
using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;
using System;

namespace Domain.Interfaces
{
    public interface IMeetingRepo
    {
        void AddOne(MeetingDTO info);
        Task<Pagination<MeetingDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery, string f, string s);
        Task<Pagination<MeetingDTO>> GetWaitMeetingByPaginationAsync(PaginationParams paginationParams, string filter, string sort);
        MeetingDTO GetOneAsync(int Id);
        void DeletingOne(int Id);
        void UpdatingOne(MeetingDTO Meeting);
    }
}