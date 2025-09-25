using Microsoft.EntityFrameworkCore;

namespace RaayPoll.API.Services.IServices
{
    public interface IPollService
    {
        Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default);
        Task<bool> UpdateToggleAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
