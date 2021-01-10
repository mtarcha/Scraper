using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scraper.Api.ViewModels;
using Scraper.Application.Queries;

namespace Scraper.Api.Controllers
{
    [ApiController]
    [Route("shows")]
    public class ShowsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ShowsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "skip")] int skip = 0,
            [FromQuery(Name = "take")] int take = 10)
        {
            if (take > 500)
                return BadRequest("Api can return 500 or less tv shows per call.");

            var query = new GetShowsQuery
            {
                Skip = skip,
                Take = take
            };

            var result = await _mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<Show>>(result));
        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCount()
        {
            var query = new GetShowsCountQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
