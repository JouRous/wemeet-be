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
            return await _context.Meetings.FindAsync(Id);
        }
        public void AddOne(MeetingDTO meeting)
        {
            // var meet = MappingFromDTO(new Meeting(), meeting);
            // _context.Meetings.Add(meet);
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


        public void UpdatingOne(MeetingDTO data)
        {
            // var entity = _context.Meetings.Where(X => X.Id == data.Id)
            // .ProjectTo<Meeting>(_mapper.ConfigurationProvider).SingleOrDefault();

            // if (data != null)
            // {
            //     entity = MappingFromDTO(entity, data);
            // }

            // _context.Meetings.Update(entity);

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

            meeting.ParticipantMeetings.Clear();
            await _context.SaveChangesAsync();
            // foreach (var userId in userIds)
            // {
            //     meeting.ParticipantMeetings.Add(new ParticipantMeeting
            //     {
            //         MeetingId = meeting.Id,
            //         ParticipantId = userId
            //     });
            // }
            // _context.Meetings.Update(meeting);

            // await _context.SaveChangesAsync();
        }
    }
}