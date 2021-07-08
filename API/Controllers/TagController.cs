using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Utils;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TagController : BaseApiController
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var tag = await _tagRepository.GetAll();
            var data = _mapper.Map<List<TagDTO>>(tag);

            var response = new ResponseBuilder<IEnumerable<TagDTO>>()
                                .AddData(data)
                                .Build();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Tag tag)
        {
            await _tagRepository.Create(tag);

            return Ok(new ResponseBuilder<uint>().AddMessage("Tag has beed created").Build());
        }
    }
}