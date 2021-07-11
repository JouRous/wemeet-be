using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repsitotries
{
	public class UserSettingRepository : IUserSettingRepository
	{
		public readonly AppDbContext _context;
		private readonly IMapper _mapper;
		public UserSettingRepository(
			AppDbContext context,
			IMapper mapper
		)
		{
			_mapper = mapper;
			_context = context;
		}

		public async Task ConfigSettingAsync(SettingDTO dto, int userId)
		{
			var config = _mapper.Map<UserSetting>(dto);

			var e = _context.Settings.FirstOrDefault(x => x.User.Id == userId);

			if (e == null)
			{
				config.User = _context.Users.Find(userId);
				_context.Settings.Add(config);
			}
			else
			{
				e.UpdatedAt = DateTime.Now;
				if (dto.StartFormatTime != null) e.StartFormatTime = (int)dto.StartFormatTime;
				if (dto.EndFormatTime != null) e.EndFormatTime = (int)dto.EndFormatTime;
				if (dto.NotifyBeforeMeeting != 0) e.NotifyBeforeMeeting = (int)dto.NotifyBeforeMeeting;
				_context.Settings.Update(e);
			}

			await _context.SaveChangesAsync();
		}

		public async Task<SettingDTO> GetSetting(string UserId)
		{
			try
			{
				int x = Int32.Parse(UserId);
				var e = await _context.Settings.Where(c => c.User.Id == x).ProjectTo<SettingDTO>(_mapper.ConfigurationProvider)
														.FirstOrDefaultAsync();
				if (e == null) return default;
				return e;
			}
			catch (System.Exception ex)
			{
				return default;
			}
		}




	}
}