using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repsitotries
{
	public class UserSettingRepository
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

		public async Task ConfigSettingAsync(UserSetting config)
		{
			_context.Settings.Add(config);

			await _context.SaveChangesAsync();
		}


	}
}