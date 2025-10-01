using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RaayPoll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
        {
            var poll = await _pollService.GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return NotFound();

            return Ok(poll.Adapt<Poll>());
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken)
        {
            //TypeAdapterConfig config = new();
            //config.NewConfig<PollRequest, Poll>()
            //    .Map(e => e.Description, s => s.Note);

            //var validationResult = await _validator.ValidateAsync(pollRequest);

            var createdPoll = await _pollService.AddAsync(pollRequest.Adapt<Poll>(), cancellationToken);

            await _pollService.CommitAsync(cancellationToken);
            return CreatedAtAction(nameof(GetAsync), new { id = createdPoll.Id }, createdPoll);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, pollRequest.Adapt<Poll>(), cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpPut("{id}/UpdateToggle")]
        public async Task<IActionResult> UpdateToggleAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateToggleAsync(id, cancellationToken);

            if (!result)
                return NotFound();

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }
    }
}
