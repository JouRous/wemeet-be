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
        Task<Pagination<MeetingBaseDTO>> GetWaitingMeetingAsync(Query<MeetingFilterModel> meetingQuery);
        Task<MeetingDTO> GetOneAsync(Guid Id);
        Task<Meeting> GetMeetingEntity(Guid Id);
        Task<Pagination<MeetingDTO>> GetAllAsync(Query<MeetingFilterModel> meetingQuery);
        Task<Pagination<MeetingDTO>> GetAllByTeamAsync(Guid TeamId, Query<MeetingFilterModel> meetingQuery);
        Task<IEnumerable<Meeting>> GetMeetingByTime(DateTime timestart, DateTime timeend);
        Task Update(Meeting meeting);
        Task DeleteOneAsync(Meeting meeting);
        Task AddUserToMeetingAsync(Guid meetingId, ICollection<Guid> userIds);
        Task AddTagToMeeting(Guid meetingId, ICollection<Guid> tagIds);
        Task AddFileToMeeting(Guid meetingId, Guid fileId);
        Task AddTeams(Guid meetingId, ICollection<Guid> teamIds);
        Task<IEnumerable<AppUser>> GetUserInMeeting(Guid meetingId);
    }
}