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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return NotFound();

            return Ok(poll.Adapt<Poll>());
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(PollRequest pollRequest, CancellationToken cancellationToken)
        {
            //TypeAdapterConfig config = new();
            //config.NewConfig<PollRequest, Poll>()
            //    .Map(e => e.Description, s => s.Note);

            //var validationResult = await _validator.ValidateAsync(pollRequest);

            var createdPoll = await _pollService.AddAsync(pollRequest.Adapt<Poll>(), cancellationToken);

            await _pollService.CommitAsync(cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = createdPoll.Id }, createdPoll);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest pollRequest, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, pollRequest.Adapt<Poll>(), cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpPut("{id}/UpdateToggle")]
        public async Task<IActionResult> UpdateToggle(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateToggleAsync(id, cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }
    }
}
