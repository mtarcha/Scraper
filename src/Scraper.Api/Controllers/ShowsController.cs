using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scraper.Application.Queries;
using System.Threading.Tasks;

namespace Scraper.Api.Controllers
{
    [ApiController]
    [Route("api/shows")]
    public sealed class ShowsController : Controller
    {
        private readonly IMediator _mediator;

        public ShowsController(IMediator mediator)
        {
            _mediator = mediator;
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

            return Ok(result);
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
