using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace RaayPoll.API.Services.IServices
{
    public interface IPollService
    {
        Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Result<Poll>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken = default);
        Task<Result> UpdateToggleAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<int>> CommitAsync(CancellationToken cancellationToken = default);
    }
}
