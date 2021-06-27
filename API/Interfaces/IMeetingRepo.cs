using API.Entities;
using API.Types;
using API.DTO;
using API.Models;
using System.Threading.Tasks;
using System;

namespace API.Interfaces
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