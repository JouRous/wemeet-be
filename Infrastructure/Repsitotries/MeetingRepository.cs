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

        private Meeting MappingFromDTO(Meeting newMeeting, MeetingDTO dto)
        {

            if (dto.Creator != null)
            {
                var creator = _context.Users.Where(user => (user.Id == dto.Creator.Id)).FirstOrDefault();
                if (creator != null) newMeeting.Creator = creator;
            }
            if (dto.Team != null)
            {
                var team = _context.Teams.Find(dto.Team.Id);
                if (team != null) newMeeting.Team = team;
            }
            if (dto.Room != null)
            {
                var room = _context.Rooms.Find(dto.Room.Id);
                if (room != null) newMeeting.Room = room;
            }
            newMeeting.UsersInMeeting = new List<AppUser>();
            foreach (var user in dto.UserInMeeting)
            {
                var u = _context.Users.Where(o => o.Id == user.Id).FirstOrDefault();
                if (u != null) newMeeting.UsersInMeeting.Add(u);
            }
            if (dto.ConflictWith != null)
            {
                var conflict = _context.Meetings.Find(dto.ConflictWith.Id);
                if (conflict != null) newMeeting.ConflictWith = conflict;
            }
            if (dto.Description != null) newMeeting.Description = dto.Description;
            if (dto.Name != null) newMeeting.Name = dto.Name;
            if (dto.Note != null) newMeeting.Note = dto.Note;
            if (dto.StartTime != null) newMeeting.StartTime = (DateTime)dto.StartTime;
            if (dto.EndTime != null) newMeeting.EndTime = (DateTime)dto.EndTime;
            if (dto.Target != null) newMeeting.Target = dto.Target;
            newMeeting.Priority = (PriorityMeeting)dto.Priority;
            newMeeting.Status = (StatusMeeting)dto.Status;
            newMeeting.Method = (MethodMeeting)dto.Method;

            return newMeeting;
        }

        private MeetingDTO ExportDTO(Meeting dto, MeetingDTO newMeeting)
        {
            if (dto == null) return null;
            var creator = _mapper.Map<UserDTO>(dto.Creator);
            if (creator != null) newMeeting.Creator = creator;
            var team = _mapper.Map<TeamDTO>(dto.Team);
            if (team != null) newMeeting.Team = team;
            var room = _mapper.Map<RoomDTO>(dto.Room);
            if (room != null) newMeeting.Room = room;
            foreach (var user in dto.UsersInMeeting)
            {
                var u = _mapper.Map<UserDTO>(user);
                if (u != null) newMeeting.UserInMeeting.Add(u);
            }
            newMeeting.Description = dto.Description;
            newMeeting.Note = dto.Note;
            newMeeting.StartTime = (DateTime)dto.StartTime;
            newMeeting.EndTime = (DateTime)dto.EndTime;
            newMeeting.Priority = (PriorityMeeting)dto.Priority;
            newMeeting.Status = (StatusMeeting)dto.Status;
            newMeeting.Target = dto.Target;
            newMeeting.Method = (MethodMeeting)dto.Method;

            newMeeting.ConflictWith = null;
            return newMeeting;
        }

        public MeetingDTO GetOneAsync(int Id)
        {
            // var e = _context.Meetings.Where(met => met.Id == Id)
            //                 .ProjectTo<MeetingDTO>(_mapper.ConfigurationProvider)
            //                 .SingleOrDefault();

            // return e;
            throw new Exception();
        }
        public void AddOne(MeetingDTO meeting)
        {
            var meet = MappingFromDTO(new Meeting(), meeting);
            _context.Meetings.Add(meet);
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

        public void DeletingOne(int Id)
        {
            var entity = _context.Meetings.Find(Id);
            _context.Meetings.Remove(entity);

        }
    }
}