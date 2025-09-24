namespace RaayPoll.API.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;

        public IEnumerable<Poll> GetAll() => _context.Polls;
        public Poll? GetById(int id)
        {
            var poll = _context.Polls.Find(id);
            return poll;
        }
        public Poll Add(Poll poll)
        {
            _context.Add(poll);
            return poll;
        }
        public bool Update(int id, Poll poll)
        {
            var pollInDB = GetById(id);

            if (pollInDB is null)
                return false;

            pollInDB.Name = poll.Name;
            pollInDB.Description = poll.Description;
            return true;
        }
        public bool Delete(int id)
        {
            var poll = GetById(id);

            if (poll is null)
                return false;

            _context.Remove(poll);
            return true;
        }
        public int Commit()
        {
            var affectedRows = _context.SaveChanges();
            return affectedRows;
        }
    }
}
