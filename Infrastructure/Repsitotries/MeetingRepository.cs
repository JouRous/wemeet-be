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

        public async Task<Meeting> GetOneAsync(Guid Id)
        {
            return await _context.Meetings
                            .Include(m => m.Room)
                            .Include(m => m.MeetingTags)
                            .ThenInclude(mt => mt.Tag)
                            .Include(m => m.ParticipantMeetings)
                            .ThenInclude(pm => pm.Participant)
                            .FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task AddOneAsync(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task<Pagination<MeetingDTO>> GetAllByPaginationAsync(
                        PaginationParams paginationParams, string filter, string sort)
        {
            var stat = _context.Meetings.Where(t => t.Name.Contains(filter))
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

            var res = await PaginationService.GetPagination<MeetingDTO>(query, paginationParams.number, paginationParams.size);

            return res;
        }
        public async Task<Pagination<MeetingDTO>> GetWaitMeetingByPaginationAsync(
                        PaginationParams paginationParams, string filter, string sort)
        {
            var stat = _context.Meetings.Where(t => t.Name.Contains(filter) && t.Status == StatusMeeting.Waiting)
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

            var res = await PaginationService.GetPagination<MeetingDTO>(query, paginationParams.number, paginationParams.size);

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
            return await PaginationService.GetPagination<MeetingDTO>(query, paginationParams.number, paginationParams.size);

        }


        public async Task Update(Meeting meeting)
        {
            _context.Entry(meeting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void DeletingOne(int Id)
        {
            var entity = _context.Meetings.Find(Id);
            _context.Meetings.Remove(entity);
        }

        public async Task AddUserToMeetingAsync(Guid meetingId, ICollection<int> userIds)
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
    }
}