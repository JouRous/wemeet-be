using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Enums;
using API.DTO;
using API.Entities;
using API.Models;
using API.Types;
using API.Services;

namespace API.Repsitotries
{
	public class MeetingRepository
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

			var creator = _context.Users.Where(user => user.UserName == dto.Creator.Email).FirstOrDefault();
			if (creator != null) newMeeting.Creator = creator;
			var team = _context.Teams.Find(dto.Team.id);
			if (team != null) newMeeting.Team = team;
			var room = _context.Rooms.Find(dto.Room.Id);
			if (room != null) newMeeting.Room = room;
			foreach (var user in dto.UserInMeeting)
			{
				var u = _context.Users.Where(o => o.UserName == user.Email).FirstOrDefault();
				if (u != null) newMeeting.UsersInMeeting.Add(u);
			}
			var conflict = _context.Meetings.Find(dto.ConflictWith.Id);
			if (conflict != null) newMeeting.ConflictWith = conflict;
			if (dto.Description != null) newMeeting.Description = dto.Description;
			if (dto.Note != null) newMeeting.Note = dto.Note;
			if (dto.StartTime != null) newMeeting.StartTime = (DateTime)dto.StartTime;
			if (dto.EndTime != null) newMeeting.EndTime = (DateTime)dto.EndTime;
			if (dto.Priority != null) newMeeting.Priority = (PriorityMeeting)dto.Priority;
			if (dto.Status != null) newMeeting.Status = (StatusMeeting)dto.Status;
			if (dto.Target != null) newMeeting.Target = dto.Target;
			if (dto.Method != null) newMeeting.Method = (MethodMeeting)dto.Method;

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

		public void AddOne(MeetingDTO meeting)
		{
			var meet = MappingFromDTO(new Meeting(), meeting);
			_context.Meetings.Add(meet);
		}

		public async Task<Pagination<MeetingDTO>> GetAllByPaginationAsync(
						PaginationParams paginationParams, string filter, string sort)
		{
			var stat = _context.Meetings.Where(t => t.Name.Contains(filter));
			switch (sort)
			{
				case "created_at":
					stat = stat.OrderBy(t => t.CreatedAt);
					break;
				case "-created_at":
					stat = stat.OrderByDescending(t => t.CreatedAt);
					break;
			}
			var queries = stat.ToList();

			var formater = new List<MeetingDTO>();

			foreach (var item in queries)
			{
				var dto = ExportDTO(item, new MeetingDTO());
				dto.ConflictWith = ExportDTO(item.ConflictWith, new MeetingDTO());

				formater.Add(dto);
			}

			var page = await PaginationService.GetPagination<MeetingDTO>(formater.AsQueryable(), paginationParams.pageNumber, paginationParams.pageSize);

			return page;
		}

		public async Task<MeetingDTO> GetOneAsync(int Id)
		{
			var result = await _context.Meetings.Where(room => room.Id == Id)
									.SingleOrDefaultAsync();
			return ExportDTO(result, new MeetingDTO());
		}

		public void UpdatingOne(MeetingDTO data)
		{
			var entity = _context.Meetings.Find(data.Id);

			if (data != null)
			{
				entity = MappingFromDTO(entity, data);
			}

			_context.Meetings.Update(entity);

		}

		public void DeletingOne(int Id)
		{
			var entity = _context.Meetings.Find(Id);
			_context.Meetings.Remove(entity);

		}
	}
}