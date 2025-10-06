using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RaayPoll.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.GetByIdAsync(id, cancellationToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    }
                );
            }

            return Ok(result.Value);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(PollRequest pollRequest, CancellationToken cancellationToken)
        {
            //TypeAdapterConfig config = new();
            //config.NewConfig<PollRequest, Poll>()
            //    .Map(e => e.Description, s => s.Note);

            //var validationResult = await _validator.ValidateAsync(pollRequest);

            var result = await _pollService.AddAsync(pollRequest, cancellationToken);
            await _pollService.CommitAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value.Adapt<PollResponse>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest pollRequest, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateAsync(id, pollRequest, cancellationToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    }
                );
            }

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.DeleteAsync(id, cancellationToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    }
                );
            }

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }

        [HttpPut("{id}/UpdateToggle")]
        public async Task<IActionResult> UpdateToggle(int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.UpdateToggleAsync(id, cancellationToken);

            if (result.IsFailed)
            {
                var errors = result.Errors.Select(e => e.Message).ToArray();

                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    extensions: new Dictionary<string, object?>
                    {
                        ["errors"] = errors
                    }
                );
            }

            await _pollService.CommitAsync(cancellationToken);
            return NoContent();
        }
    }
}
