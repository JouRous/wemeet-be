using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using AutoMapper;
using API.Repsitotries;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public ITeamRepository TeamRepository => new TeamRepository(_context, _mapper);
        public IBuildingRepository BuildingRepository => new BuildingRepository(_context, _mapper);
        public IRoomRepository RoomRepository => new RoomRepository(_context, _mapper);
        public INotificationRepo NotificationRepository => new NotificationRepository(_context, _mapper);
        public IMeetingRepo MeetingRepository => new MeetingRepository(_context, _mapper);


        public UnitOfWork(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Complete()
        {
            var numChanges = await _context.SaveChangesAsync();

            return numChanges > 0;
        }
    }
}