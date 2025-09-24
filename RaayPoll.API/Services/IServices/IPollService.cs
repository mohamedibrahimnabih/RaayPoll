namespace RaayPoll.API.Services.IServices
{
    public interface IPollService
    {
        IEnumerable<Poll> GetAll();
        Poll? GetById(int id);
        Poll Add(Poll poll);
        bool Update(int id, Poll poll);
        bool Delete(int id);
        int Commit();
    }
}
