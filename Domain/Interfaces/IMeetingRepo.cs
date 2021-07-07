using Domain.Entities;
using Domain.Types;
using Domain.DTO;
using Domain.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IMeetingRepo
    {
        void AddOne(MeetingDTO info);
        Task AddOneAsync(Meeting meeting);
        Task<Pagination<MeetingDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery, string f, string s);
        Task<Pagination<MeetingDTO>> GetWaitMeetingByPaginationAsync(PaginationParams paginationParams, string filter, string sort);
        Task<Meeting> GetOneAsync(Guid Id);
        Task Update(Meeting meeting);
        void DeletingOne(int Id);
        void UpdatingOne(MeetingDTO Meeting);
        Task AddUserToMeetingAsync(Guid meetingId, ICollection<int> userIds);
    }
}