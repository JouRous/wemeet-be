using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Enums;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using Domain.Types;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Utils;

namespace Infrastructure.Repositories
{
    public class MeetingRepository : IMeetingRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public MeetingRepository(AppDbContext app, IMapper map)
        {
            _context = app;
            _mapper = map;
        }

        public async Task<MeetingDTO> GetOneAsync(Guid Id)
        {
            return await _context.Meetings
                            .Include(m => m.Room)
                            .ThenInclude(r => r.Building)
                            .Include(m => m.MeetingTags)
                            .ThenInclude(mt => mt.Tag)
                            .Include(m => m.ParticipantMeetings)
                            .ThenInclude(pm => pm.Participant)
                            .Include(m => m.MeetingFiles)
                            .ThenInclude(mf => mf.FileEntity)
                            .Include(m => m.MeetingTeams)
                            .ThenInclude(mt => mt.Team)
                            .ThenInclude(t => t.Leader)
                            .ProjectTo<MeetingDTO>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<Meeting> GetMeetingEntity(Guid Id)
        {
            return await _context.Meetings.FindAsync(Id);
        }

        public async Task AddOneAsync(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task<Pagination<MeetingBaseDTO>> GetWaitingMeetingAsync(Query<MeetingFilterModel> meetingQuery)
        {
            var filter = meetingQuery.filter;
            var paginationParams = meetingQuery.paginationParams;
            var sort = meetingQuery.sort;

            var stat = _context.Meetings
                        .Where(t => t.Status == StatusMeeting.Waiting)
                        .Where(m => m.Name.Contains(filter.Name))
                        .Include(m => m.MeetingTeams)
                        .ThenInclude(mt => mt.Team)
                        .ThenInclude(t => t.Leader)
                        .ProjectTo<MeetingBaseDTO>(_mapper.ConfigurationProvider);

            if (filter.Team != Guid.Empty)
            {
                stat = stat.Where(m => m.Teams.First().Id == filter.Team);
            }

            if (filter.Room != Guid.Empty)
            {
                stat = stat.Where(m => m.Room.Id == filter.Room);
            }

            if (!string.IsNullOrEmpty(filter.Creator))
            {
                var normalizeFilterName = StringHelper.RemoveAccentedString(filter.Creator);
                stat = stat.Where(m => m.Creator.UnsignedName.Contains(normalizeFilterName));
            }

            switch (sort)
            {
                case "created_at":
                    stat = stat.OrderBy(t => t.CreatedAt);
                    break;
                case "-created_at":
                    stat = stat.OrderByDescending(t => t.CreatedAt);
                    break;
            }
            var query = stat.AsQueryable();

            var res = await PaginationService.GetPagination<MeetingBaseDTO>(query, paginationParams.number, paginationParams.size);

            return res;
        }

        public async Task<Pagination<MeetingDTO>> GetAllAsync(Query<MeetingFilterModel> meetingQuery)
        {
            var _filter = meetingQuery.filter;
            var paginationParams = meetingQuery.paginationParams;
            var sort = meetingQuery.sort;

            var stat = _context.Meetings
                        .ProjectTo<MeetingDTO>(_mapper.ConfigurationProvider);

            switch (sort)
            {
                case "created_at":
                    stat = stat.OrderBy(t => t.CreatedAt);
                    break;
                case "-created_at":
                    stat = stat.OrderByDescending(t => t.CreatedAt);
                    break;
            }
            var query = stat.AsQueryable();
            return await PaginationService
                            .GetPagination<MeetingDTO>(query,
                                                       paginationParams.number,
                                                       paginationParams.size);

        }


        public async Task Update(Meeting meeting)
        {
            _context.Entry(meeting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToMeetingAsync(Guid meetingId, ICollection<Guid> userIds)
        {
            if (userIds.Count < 0) return;

            var meeting = await _context.Meetings.FindAsync(meetingId);

            if (meeting == null) return;

            _context.ParticipantMeeting.RemoveRange(
                _context.ParticipantMeeting.Where(pm => pm.MeetingId == meeting.Id).ToList()
            );

            foreach (var userId in userIds)
            {
                meeting.ParticipantMeetings.Add(new ParticipantMeeting
                {
                    MeetingId = meeting.Id,
                    ParticipantId = userId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOneAsync(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task AddTagToMeeting(Guid meetingId, ICollection<Guid> tagIds)
        {
            _context.MeetingTag.RemoveRange(
                _context.MeetingTag.Where(mt => mt.MeetingId == meetingId)
            );

            foreach (var tagId in tagIds)
            {
                _context.MeetingTag.Add(new MeetingTag
                {
                    MeetingId = meetingId,
                    TagId = tagId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddFileToMeeting(Guid meetingId, Guid fileId)
        {
            _context.MeetingFile.Add(new MeetingFile
            {
                FileEntityId = fileId,
                MeetingId = meetingId
            });

            await _context.SaveChangesAsync();
        }

        public async Task AddTeams(Guid meetingId, ICollection<Guid> teamIds)
        {
            _context.MeetingTeam.RemoveRange(
                _context.MeetingTeam.Where(mt => mt.MeetingId == meetingId).ToList()
            );

            foreach (var teamId in teamIds)
            {
                _context.MeetingTeam.Add(new MeetingTeam
                {
                    MeetingId = meetingId,
                    TeamId = teamId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUserInMeeting(Guid meetingId)
        {
            var user = await _context.Meetings
                                .Where(m => m.Id == meetingId)
                                .Include(m => m.ParticipantMeetings)
                                .ThenInclude(pm => pm.Participant)
                                .Select(m => m.ParticipantMeetings.Select(pm => pm.Participant))
                                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<Pagination<MeetingDTO>> GetAllByTeamAsync(Guid TeamId, Query<MeetingFilterModel> meetingQuery)
        {
            var _filter = meetingQuery.filter;
            var paginationParams = meetingQuery.paginationParams;
            var sort = meetingQuery.sort;

            var stat = _context.Meetings
                        .Where(m => m.MeetingTeams.Any(mt => mt.TeamId == TeamId))
                        .ProjectTo<MeetingDTO>(_mapper.ConfigurationProvider);

            if (_filter.Role.Equals(UserRoles.STAFF))
            {
                stat = stat.Where(m => m.Status == StatusMeeting.Accepted);
            }

            if (_filter.Role.Equals(UserRoles.LEAD))
            {
                stat = stat.Where(m => m.Status == StatusMeeting.Accepted || m.Status == StatusMeeting.Waiting);
            }

            switch (sort)
            {
                case "created_at":
                    stat = stat.OrderBy(t => t.CreatedAt);
                    break;
                case "-created_at":
                    stat = stat.OrderByDescending(t => t.CreatedAt);
                    break;
            }
            var query = stat.AsQueryable();
            return await PaginationService
                            .GetPagination<MeetingDTO>(query,
                                                       paginationParams.number,
                                                       paginationParams.size);
        }

        public async Task<IEnumerable<MeetingBase>> GetMeetingByTimeAndRoom(Guid roomId, DateTime timestart, DateTime timeend)
        {
            var m = await _context.Meetings
                   .Where(m => m.RoomId == roomId)
                   .Where(
                       m => (
                           (timestart <= m.StartTime && timeend <= m.EndTime && timeend >= m.StartTime) ||
                           (timestart <= m.StartTime && timeend >= m.EndTime) ||
                           (timestart >= m.StartTime && timeend <= m.EndTime) ||
                           (timestart >= m.StartTime && timeend >= m.EndTime && timestart <= m.EndTime)
                       )
                   )
                   .ProjectTo<MeetingBase>(_mapper.ConfigurationProvider)
                   .ToListAsync();

            return m;
        }

        public async Task<IEnumerable<MeetingBase>> GetMeetingByRoomAndDate(Guid roomId, DateTime date, int dayNumber)
        {
            return await _context.Meetings.Where(
                m => m.RoomId == roomId && m.StartTime >= date && m.StartTime <= date.AddDays(dayNumber)
            ).ProjectTo<MeetingBase>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

    }
}