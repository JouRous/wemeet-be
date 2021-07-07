using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries
{
    public class GetMeetingQueryHandler : IRequestHandler<GetMeetingQuery, MeetingDTO>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IMapper _mapper;

        public GetMeetingQueryHandler(IMeetingRepo meetingRepo, IMapper mapper)
        {
            _meetingRepo = meetingRepo;
            _mapper = mapper;
        }

        public async Task<MeetingDTO> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            var meeting = await _meetingRepo.GetOneAsync(request.Id);

            return _mapper.Map<MeetingDTO>(meeting);
        }
    }
}