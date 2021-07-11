using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Features.Queries
{
    public class GetAllMeetingQueryHandler : IRequestHandler<GetAllMeetingQuery, Pagination<MeetingDTO>>
    {
        private readonly IMeetingRepo _meetingRepo;
        private readonly IMapper _mapper;

        public GetAllMeetingQueryHandler(IMeetingRepo meetingRepo, IMapper mapper)
        {
            _meetingRepo = meetingRepo;
            _mapper = mapper;
        }

        public async Task<Pagination<MeetingDTO>> Handle(GetAllMeetingQuery request, CancellationToken cancellationToken)
        {
            var meetings = await _meetingRepo.GetAllAsync(request.query);

            return meetings;
        }
    }
}