using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using RaayPoll.API.Models;

namespace RaayPoll.API.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default) => Result.Ok((await _context.Polls.AsNoTracking().ToListAsync(cancellationToken)).Adapt<IEnumerable<PollResponse>>());

        public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Fail($"Poll with id {id} not found");

            return Result.Ok(poll.Adapt<PollResponse>());
        }

        public async Task<Result<Poll>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.AddAsync(pollRequest.Adapt<Poll>(), cancellationToken);
            return Result.Ok(poll.Entity);
        }
        public async Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default)
        {
            var pollInDB = await _context.Polls.FindAsync(id, cancellationToken);

            if (pollInDB is null)
                return Result.Fail($"Poll with id {id} not found");

            pollInDB.Name = pollRequest.Name;
            pollInDB.Description = pollRequest.Description;

            return Result.Ok();
        }

        public async Task<Result> UpdateToggleAsync(int id, CancellationToken cancellationToken = default)
        {
            var pollInDB = await _context.Polls.FindAsync(id, cancellationToken);

            if (pollInDB is null)
                return Result.Fail($"Poll with id {id} not found");

            pollInDB.IsPublished = !pollInDB.IsPublished;

            return Result.Ok();
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);

            if (poll is null)
                return Result.Fail($"Poll with id {id} not found");

            _context.Remove(poll);

            return Result.Ok();
        }
        public async Task<Result<int>> CommitAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok(affectedRows);
        }
    }
}
