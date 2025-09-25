using Microsoft.EntityFrameworkCore;
using RaayPoll.API.Models;
using System.Threading.Tasks;

namespace RaayPoll.API.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
        public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);
            return poll;
        }

        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(poll, cancellationToken);
            return poll;
        }
        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
        {
            var pollInDB = await GetByIdAsync(id, cancellationToken);

            if (pollInDB is null)
                return false;

            pollInDB.Name = poll.Name;
            pollInDB.Description = poll.Description;
            return true;
        }

        public async Task<bool> UpdateToggleAsync(int id, CancellationToken cancellationToken = default)
        {
            var pollInDB = await GetByIdAsync(id, cancellationToken);

            if (pollInDB is null)
                return false;

            pollInDB.IsPublished = !pollInDB.IsPublished;
            return true;
        }
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return false;

            _context.Remove(poll);
            return true;
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows;
        }

        
    }
}
