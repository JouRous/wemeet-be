using API.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Enums;
using API.Entities;
using API.Models;
using API.Services;
using API.Types;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
namespace API.Repsitotries
{
	public class NotificationRepository
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		public NotificationRepository(AppDbContext app, IMapper map)
		{
			_context = app;
			_mapper = map;
		}

		public void AddOne(Notification message)
		{
			_context.Notifications.Add(message);
		}

		public async Task<Pagination<NotificationMessageDTO>> GetMessagesPagiantionAsync(PaginationParams paginationQuery)
		{
			var data = _context.Notifications.ProjectTo<NotificationMessageDTO>(_mapper.ConfigurationProvider)
								.OrderByDescending(e => e.CreatedAt)
																.AsQueryable();
			var res = await PaginationService.GetPagination<NotificationMessageDTO>(data, paginationQuery.currentPage, paginationQuery.pageSize);
			return res;
		}

		public async Task<Pagination<NotificationMessageDTO>> GetMessagesUnreadPaginationAsync(PaginationParams paginationQuery)
		{
			var data = _context.Notifications.Where(notify => notify.IsRead == false)
						.OrderByDescending(e => e.CreatedAt)
						.ProjectTo<NotificationMessageDTO>(_mapper.ConfigurationProvider)
						.AsQueryable();
			var res = await PaginationService.GetPagination<NotificationMessageDTO>(data, paginationQuery.currentPage, paginationQuery.pageSize);
			return res;
		}

		public void MarkReadNotification(string Id)
		{
			var entity = _context.Notifications.Find(Id);
			entity.IsRead = true;
			_context.Notifications.Update(entity);
			_context.SaveChanges();
		}


	}
}