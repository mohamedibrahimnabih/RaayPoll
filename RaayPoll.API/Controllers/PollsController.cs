using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RaayPoll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;
        private readonly IValidator<PollRequest> _validator;

        public PollsController(IPollService pollService, IValidator<PollRequest> validator)
        {
            _pollService = pollService;
            _validator = validator;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var polls = _pollService.GetAll();
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var poll = _pollService.GetById(id);

            if (poll is null)
                return NotFound();

            return Ok(poll.Adapt<Poll>());
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(PollRequest pollRequest)
        {
            //TypeAdapterConfig config = new();
            //config.NewConfig<PollRequest, Poll>()
            //    .Map(e => e.Description, s => s.Note);

            //var validationResult = await _validator.ValidateAsync(pollRequest);

            var createdPoll = _pollService.Add(pollRequest.Adapt<Poll>());

            _pollService.Commit();
            return CreatedAtAction(nameof(Get), new { id = createdPoll.Id }, createdPoll);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, PollRequest pollRequest)
        {
            var result = _pollService.Update(id, pollRequest.Adapt<Poll>());

            if (!result)
                return NotFound();

            _pollService.Commit();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _pollService.Delete(id);

            if (!result)
                return NotFound();

            _pollService.Commit();
            return NoContent();
        }
    }
}
