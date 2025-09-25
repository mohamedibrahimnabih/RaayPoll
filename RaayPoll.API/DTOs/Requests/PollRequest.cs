namespace RaayPoll.API.DTOs.Requests
{
    public record PollRequest(
        /*[CustomLength(5, 10)]*/ string Name, 
        string? Description,
        bool IsPublished,
        [NotPastDate] DateTime StartAt,
        [EndDateAfter(nameof(StartAt))] DateTime EndAt);

    //public class PollRequest
    //{
    //    public string Name { get; set; } = string.Empty;
    //    public string? Description { get; set; }

    //    //public static explicit operator Poll(PollRequest request)
    //    //{
    //    //    return new Poll()
    //    //    {
    //    //        Name = request.Name,
    //    //        Description = request.Description,
    //    //    };
    //    //}
    //}

    //public static class PollRequestExtension
    //{
    //    public static Poll GetPoll(this PollRequest request)
    //    {
    //        return new Poll()
    //        {
    //            Name = request.Name,
    //            Description = request.Description,
    //        };
    //    }
    //}
}
