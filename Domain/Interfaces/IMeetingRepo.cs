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
        Task AddOneAsync(Meeting meeting);
        Task<Pagination<MeetingDTO>> GetAllByPaginationAsync(PaginationParams paginationQuery, string f, string s);
        Task<Pagination<MeetingDTO>> GetWaitMeetingByPaginationAsync(PaginationParams paginationParams, string filter, string sort);
        Task<Meeting> GetOneAsync(Guid Id);
        Task<Pagination<MeetingDTO>> GetAllAsync(Query<MeetingFilterModel> meetingQuery);
        Task Update(Meeting meeting);
        Task DeleteOneAsync(Meeting meeting);
        Task AddUserToMeetingAsync(Guid meetingId, ICollection<Guid> userIds);
        Task AddTagToMeeting(Guid meetingId, ICollection<Guid> tagIds);
        Task AddFileToMeeting(Guid meetingId, Guid fileId);
        Task AddTeams(Guid meetingId, ICollection<Guid> teamIds);
        Task<IEnumerable<AppUser>> GetUserInMeeting(Guid meetingId);
    }
}